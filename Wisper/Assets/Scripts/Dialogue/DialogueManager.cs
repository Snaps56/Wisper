using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  Note that npc and dialogueTrigger are mechanically synonymous in this class. An NPC with something to say will have a DialogueTrigger (prefab) as a child, which contains a
 *      collider and NPCDialogues components. The DT prefab is used for detecting if the npc is in range and holding all data needed for the dialogue, so from this script's
 *      perspective, it is the npc. However, sometimes this prefab is leveraged in ways intended to affect the DT itself and not so much the npc, and thus the term
 *      DialogueTrigger is used to indicate this. 
 *      EX: A function intended to move the DT but not the NPC it is attached to has its parameter called dialogueTrigger, whereas
 *      a function which  updates which Dialogues are enabled for an NPC uses the parameter name npc.
 */
public class DialogueManager : MonoBehaviour {
    public static DialogueManager dialogueManager;

    // UI elements
    private GameObject dialogueBox;         // The GameObject containing the dialogue box text elements
    private Text dialogueText;              // Text field of dialogue box
    private Text dialogueName;              // Name field of dialogue box
    private GameObject optionPanels;        // The gameobject that is a parent of the object panels.

    // Display time values & references
    private GameObject activeNPC;           // Reference to NPC with active dialogue
    private Dialogue activeDialogue;        // Reference to the active dialogue
    private Queue<string> sentences;        // Queue of sentences to display
    private int sentenceIndex = 0;          // Index for number of sentence from queue currently being displayed.
    private float charDelay;                // Delay between adding chars to display. Controls the "speed of speech." Minimum delay is 1/framerate seconds for charDelay, which corresponds to the "fastest talking speed."
    

    private Option activeOption;            // Reference to the active option
    private Choice activeChoice;            // Reference to the choice currently selected.
    private int activeChoiceIndex = 0;          // Index of active choice within list of display choices
    private List<Choice> displayChoices = new List<Choice>();    // List of choices to display on screen
    private List<int> activeOptionPanels = new List<int>();   // List of which option panels are displayed by their number (1, 2, 3, 4)
    private int selectedOptionPanelIndex = 0;  // Index of which display panel is currently selected
    

    // Locks and controls
    private bool sentenceDisplayInProgress; // A lock used with subroutines that update UI text elements
    private bool dialogueBoxActive;         // A lock for preventing floating text or other UI elements from displaying during dialogue box use. Can be referenced for other things which need to be locked when dialogue box is active.
    private bool skipText = false;          // Switch to fast forward text on player click
    private bool optionActive = false;
    private bool optionChangeOnCooldown = false;
    
    private bool displayWhenDoneLock = false;   // A lock used to ensure only one call to the display next sentence when done coroutine is running.
    // References to entities in scene
    private GameObject player;                                      // Reference to the player
    private List<GameObject> inRangeNPC = new List<GameObject>();   // List of all DialogueTriggers in range of player collider. Stored here to avoid repetitive find operations each frame.
    private GameObject nearestNPC;                                  // Reference to the closest NPC DialogueTrigger with NPCDialogues
    
    // References to persistant state data
    private GameObject persistantStateData;         // A reference stored here to prevent multiple find calls.

    // Update info
    public int persistantStateDataUpdateCount = 0; // Compared to the corresponding value in persistantStateData to determine if the dialogues need to check for updates

    void Awake()
    {
        // Ensures there will only ever be 1 dialogue manager in a scene. (Singleton pattern).
        if (dialogueManager == null)
        {
            DontDestroyOnLoad(gameObject);
            dialogueManager = this;
        }
        else if (dialogueManager != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization. Some references to other gameobject will need to be initialized in Update.
    void Start ()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");

        optionPanels = GameObject.FindGameObjectWithTag("OptionPanels");
        /*if(dialogueBox != null)
        {
            // Debug.Log("Found dialogueBox");
        }*/
        foreach(Text textField in dialogueBox.GetComponentsInChildren<Text>())
        {
            if(textField.gameObject.name == "Speaker")
            {
                dialogueName = textField;
            }
            else if(textField.gameObject.name == "Dialogue")
            {
                dialogueText = textField;
            }
            /*else
            {
                Debug.Log("Unexpected text field found with name: " + textField.name);
            }*/
        }

        sentences = new Queue<string>();
        
        sentenceDisplayInProgress = false;
        dialogueBoxActive = false;

        player = GameObject.FindGameObjectWithTag("Player");
        persistantStateData = GameObject.Find("PersistantStateData");
        HideBox();
    }

    private void Update()
    {
        // Make sure reference to PSD is set (may have been created after DM's start and awake)
        if (persistantStateData == null)
        {
            persistantStateData = GameObject.Find("PersistantStateData");
        }

        // When PSD updates, run an update on all dialogues in the scene.
        if (persistantStateDataUpdateCount != persistantStateData.GetComponent<PersistantStateData>().updateCount)
        {
            Debug.Log("Updating NPCDialogues");
            persistantStateDataUpdateCount = persistantStateData.GetComponent<PersistantStateData>().updateCount;
            foreach (GameObject dt in GameObject.FindGameObjectsWithTag("DialogueTrigger"))
            {
                UpdateDialogues(dt);
                try
                {
                    Dialogue tmpDialogue = GetEnabledDialogue(dt);
                    if (tmpDialogue.forceOnEnable)
                    {
                        TranslateToPlayer(dt);  // If any forceOnEnable Dialogues where enabled, move their DialogueTriggers to the player
                    }
                }
                catch(MissingReferenceException e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        // If any of the dialogues on the player are forceOnEnable, keep the DialogueTrigger on the player each update cycle
        if (inRangeNPC.Count != 0)
        {
            foreach(GameObject dt in inRangeNPC)
            {
                try
                {
                    if (GetEnabledDialogue(dt).forceOnEnable)
                    {
                        TranslateToPlayer(dt);
                    }
                }
                catch(MissingReferenceException e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        // When dialogue is active, respond to input
        if(dialogueBoxActive)
        {
            if ((Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X")) && !activeDialogue.autoPlay)    // Standard dialogue progression behaviour
            {
                if (sentenceDisplayInProgress)
                {
                    skipText = true;
                }
                else
                {
                    DisplayNextSentence();  //No options, so display next sentence;
                }
            }
            else if (optionActive && !sentenceDisplayInProgress)    // Option selection behaviour
            {
                if (Input.GetAxis("XBOX_Thumbstick_L_Y") != 0 || Input.GetAxis("PC_Axis_MovementZ") != 0)
                {
                    Debug.Log("Detected input on forward/backward axis");
                    if(!optionChangeOnCooldown)
                    {
                        Debug.Log("Cooldown is false");
                        optionChangeOnCooldown = true;
                        if(Input.GetAxis("XBOX_Thumbstick_L_Y") > 0 || Input.GetAxis("PC_Axis_MovementZ") > 0)
                        {
                            StartCoroutine(ChoiceScrollCoroutine("up"));
                        }
                        else if(Input.GetAxis("XBOX_Thumbstick_L_Y") < 0 || Input.GetAxis("PC_Axis_MovementZ") < 0)
                        {
                            StartCoroutine(ChoiceScrollCoroutine("down"));
                        }
                    }
                }
                else if(Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X"))
                {
                    foreach (TargetCondition condition in activeChoice.changeConditions)
                    {
                        ChangeCondition(condition); // Apply any condition changes attached to the active choice
                    }
                    foreach (int panelNum in activeOptionPanels)
                    {
                        UpdateOptionPanel(panelNum);    // Deactivates all the option panels
                    }

                    // Reset option variables
                    activeOption = null;
                    activeChoice = null;            // Reference to the choice currently selected.
                    activeChoiceIndex = 0;          // Index of active choice within list of display choices
                    displayChoices.Clear();
                    activeOptionPanels.Clear();
                    optionActive = false;
                    selectedOptionPanelIndex = 0;  // Index of which display panel is currently selected
                    DisplayNextSentence();
                }
            }
            else if (activeDialogue.autoPlay)   // Auto play dialogue behaviour
            {
                if(!displayWhenDoneLock)
                {
                    displayWhenDoneLock = true;
                    StartCoroutine(DisplayNextSentenceWhenDone(activeDialogue.autoPlayDelay));
                }
            }
        }
        else if(inRangeNPC.Count != 0) // When dialogue triggers are in range & dialogue not active, handle input (or force dialogue to start if forceOnEnable set).
        {
            if(!dialogueBoxActive)
            {
                try
                {
                    nearestNPC = GetClosestNPC();
                    //TODO: Display interact button by this npc
                    if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X") || GetEnabledDialogue(nearestNPC).forceOnEnable)
                    {
                        dialogueBoxActive = true;
                        activeNPC = nearestNPC;
                        charDelay = activeNPC.GetComponent<NPCDialogues>().defaultCharDelay;
                        StartDialogue(GetEnabledDialogue(activeNPC)); //Determine dialogue to use, Activate dialogue
                    }
                }
                catch(MissingReferenceException e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }
    }

    // Add an npc as "in range" of player. Called by the InteractableCollision script on player.
    public void AddInRangeNPC(GameObject npc)
    {
        inRangeNPC.Add(npc);
    }

    // Remove npc as "in range" of player. Called by the InteractableCollision script on player
    public void RemoveInRangeNPC(GameObject npc)
    {
        inRangeNPC.Remove(npc);
    }

    // Finds the closest npc to the player that has dialogue. This NPC may not have any enabled however.
    private GameObject GetClosestNPC()
    {
        if (inRangeNPC.Count != 0)
        {
            GameObject closestNPC = inRangeNPC[0];
            float dist = (closestNPC.transform.position - player.transform.position).magnitude;

            foreach (GameObject npc in inRangeNPC)
            {
                float npcDist = (npc.transform.position - player.transform.position).magnitude;
                if (npcDist < dist)
                {
                    closestNPC = npc;
                    dist = npcDist;
                }
            }
            return closestNPC;
        }
        else
        {
            throw new MissingReferenceException("No dialogue NPC's in range of player");
        }
    }

    // Transforms the position of an object to the player. Used for moving dialogue triggers, particularly for "forceOnEnable" dialogues.
    private void TranslateToPlayer(GameObject dialogueTrigger)
    {
        dialogueTrigger.transform.SetPositionAndRotation(player.transform.position, dialogueTrigger.transform.rotation);
    }

    // Updates all dialogues in NPCDialogues component attached to the given game object
    public void UpdateDialogues(GameObject npc)
    {
        // TODO: decide what errors to throw and handle if more than one dialogue would be enabled
        NPCDialogues npcDialogues = npc.GetComponent<NPCDialogues>();
        bool tempCheck = true;
        foreach(Dialogue d in npcDialogues.dialogues)
        {
            foreach (TargetCondition condition in d.enableConditions)
            {
                if (tempCheck) // If the previous conditions have held
                {
                    if(!CheckCondition(condition))
                    {
                        tempCheck = false;  // If this condition does not match the value in persistant state, tempCheck is false
                    }
                }
            }
            if (tempCheck)
            {
                d.enabled = true;   // If all conditions are met, enable dialog
            }
            else
            {
                if(d.enabled)
                {
                    d.enabled = false;
                    if(d.forceOnEnable)
                    {
                        npc.transform.localPosition = new Vector3(0, 0, 0); // If a forceOnEnable was deactivated, move its DialogueTrigger back to the parent's location  
                    }
                }
                
            }
            tempCheck = true;   // Reset tempCheck for next dialogue
        }
    }

    // Checks if a given condition matches the value in persistant state data
    private bool CheckCondition(TargetCondition condition)
    {
        try
        {
            if ((bool)persistantStateData.GetComponent<PersistantStateData>().stateConditions[condition.conditionName] == condition.conditionValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch(System.NullReferenceException e)
        {
            Debug.LogWarning("NullReferenceException generate on CheckCondition for " + condition.conditionName + " on " + 
                ". Here is the exception message: " + e.Message);
            return false;
        }
    }

    // Returns the first enabled dialogue. Take care when setting dialogue conditions to try and only have one enabled for any npc.
    public Dialogue GetEnabledDialogue(GameObject npc)
    {
        foreach(Dialogue d in npc.GetComponent<NPCDialogues>().dialogues)
        {
            if(d.enabled)
            {
                return d;
            }
        }
        throw new MissingReferenceException("No enabled dialogue on " + npc.transform.parent.name);
    }

    // If an option would be active for the current sentence, set it for the active dialogue and initialize choices. Otherwise set as null.
    private void SetActiveOption()
    {
        activeOption = null;
        if(activeDialogue != null)
        {
            if(activeDialogue.options.Count!= 0)
            {
                foreach(Option o in activeDialogue.options)
                {
                    if(o.sentenceIndex == sentenceIndex)
                    {
                        activeOption = o;
                        activeChoice = activeOption.choices[0]; // Set default choice to the first one.
                        activeChoiceIndex = 0;
                        InitializeDisplayedChoices();
                    }
                }
            }
        }
    }

    // Initializes the displayed choices
    private void InitializeDisplayedChoices()
    {
        if(activeOption.choices.Count <= 4)
        {
            foreach(Choice c in activeOption.choices)
            {
                if(activeOption == null)
                {
                    Debug.Log("Active option is null");
                }
                else if(activeOption.choices == null)
                {
                    Debug.Log("Active option choices is null");
                }
                else if(c == null)
                {
                    Debug.Log("Choice c is null");
                }
                
                displayChoices.Add(c);
            }

            if(displayChoices.Count < 3)
            {
                activeOptionPanels.Add(2); activeOptionPanels.Add(3); // Use middle 2 option panels
            }
            else if ( displayChoices.Count < 4)
            {
                activeOptionPanels.Add(2); activeOptionPanels.Add(3); activeOptionPanels.Add(4); // Use last 3 choice panels
            }
            else
            {
                activeOptionPanels.Add(1); activeOptionPanels.Add(2); activeOptionPanels.Add(3); activeOptionPanels.Add(4); // Use all 4 option panels
            }
        }
        else
        {
            for(int i = 0; i < 4; i++)
            {
                displayChoices.Add(activeOption.choices[i]);
            }
            activeOptionPanels.Add(1); activeOptionPanels.Add(2); activeOptionPanels.Add(3); activeOptionPanels.Add(4); // Use all 4 option pannels
        }
        selectedOptionPanelIndex = 0;
    }
    
    IEnumerator ChoiceScrollCoroutine(string direction)
    {
        Debug.Log("inside scroll enumerator");
        if(direction.ToLower() == "up")
        {
            Debug.Log("scrolling up");
            ChoicesScrollUp();
        }
        else if(direction.ToLower() == "down")
        {
            Debug.Log("Scrolling down");
            ChoicesScrollDown();
        }
        ShowChoices();
        yield return new WaitForSeconds(0.4f); // Enforces a tiny cooldown when scrolling between options so player has some precision control;
        optionChangeOnCooldown = false;
    }
    // Call when moving up option list. Will shift dialogue options if more than 4, and wraps to bottom option if the first option was selected.
    private void ChoicesScrollUp()
    {
        if(activeChoiceIndex != 0)  // If the active choice was not the first choice, no "option wrapping" is required
        {
            activeChoiceIndex--;
            activeChoice = activeOption.choices[activeChoiceIndex];
            if(selectedOptionPanelIndex != 0)
            {
                selectedOptionPanelIndex--; // If the top option panel wasn't the selected panel, make the next highest option panel the selected one
            }
            else
            {
                if (activeOption.choices.Count > 4) // If there are more than 4 choices, have the displayed options "scroll up"
                {
                    for (int i = 2; i >= 0; i--)
                    {
                        displayChoices[i + 1] = displayChoices[i];  // Move each choice down in display order
                    }
                    displayChoices[0] = activeChoice; // Set top displayed choice to active choice
                }
            }
        }
        else // Active choice was the first one, wrap to last choice
        {
            activeChoiceIndex = activeOption.choices.Count - 1;         // Set the selected choice to the last one
            activeChoice = activeOption.choices[activeChoiceIndex];     // Set the active choice to one pointed to by index
            selectedOptionPanelIndex = activeOptionPanels.Count - 1;    // Set selected panel to bottom one
            if (activeOption.choices.Count > 4) // If there are more than 4 choices, setup the options to be the last 4
            {
                for (int i = 0; i < 3; i++)
                {
                    displayChoices[i] = activeOption.choices[activeChoiceIndex - (i + 1)]; // Populates top 3 choices with 3 choices above the last one                    
                }
                displayChoices[3] = activeChoice; // Set the last display choice to the active one
            }
        }   
    }

    // Call when moving down option list. Will shift dialogue options if more than 4, and wraps to top option if the last option was selected.
    private void ChoicesScrollDown()
    {
        if (activeChoiceIndex != activeOption.choices.Count - 1)  // If the active choice was not the last choice, no "option wrapping" is required
        {
            activeChoiceIndex++;
            activeChoice = activeOption.choices[activeChoiceIndex];
            if (selectedOptionPanelIndex != activeOptionPanels.Count - 1)
            {
                selectedOptionPanelIndex++; // If the bottom option panel wasn't the selected panel, make the next lowest option panel the selected one
            }
            else
            {
                if (activeOption.choices.Count > 4) // If there are more than 4 choices, have the displayed options "scroll down"
                {
                    for (int i = 0; i < 3; i++)
                    {
                        displayChoices[i] = displayChoices[i+1];  // Move each choice up in display order
                    }
                    displayChoices[3] = activeChoice; // Set bottom displayed choice to active choice
                }
            }
        }
        else // Active choice was the last one, wrap to first choice
        {
            activeChoiceIndex = 0;         // Set the selected choice to the last one
            activeChoice = activeOption.choices[activeChoiceIndex]; 
            selectedOptionPanelIndex = 0;    // Set selected panel to top one
            if (activeOption.choices.Count > 4) // If there are more than 4 choices, setup the options to be the first 4
            {
                for (int i = 3; i > 0; i--)
                {
                    displayChoices[i] = activeOption.choices[(i + 1)]; // Populates bottom 3 choices with 3 choices below the first one                    
                }
                displayChoices[0] = activeChoice; // Set the last display choice to the active one
            }
        }
    }

    private void ShowChoices()
    {
        if(activeOption != null)
        {
            if(!optionActive)
            {
                optionActive = true;    // toggle lock for option active
            }

            int panelIter = 0;
            foreach(Choice c in displayChoices)
            {
                UpdateOptionPanel(activeOptionPanels[panelIter], c);
                panelIter++;
            }
        }
    }

    private void UpdateOptionPanel(int panelNum, Choice c = null)
    {
        GameObject optionPanel = optionPanels.transform.Find("Panel " + panelNum).gameObject;
        if(c == null)
        {
            optionPanel.SetActive(false);   // If no choice argument, deactivate panel
        }
        else
        {
            optionPanel.GetComponentInChildren<Text>().text = c.reply;  // If choice argument passed, update text and activate panel
            
            if(panelNum == activeOptionPanels[selectedOptionPanelIndex])
            {
                Color tmpColor = optionPanel.GetComponent<Image>().color;
                tmpColor.a = 1.0f;
                optionPanel.GetComponent<Image>().color = tmpColor;
            }
            else
            {
                Color tmpColor = optionPanel.GetComponent<Image>().color;
                tmpColor.a = 0.5f;
                optionPanel.GetComponent<Image>().color = tmpColor;
            }
            optionPanel.SetActive(true);
        }
    }

    // Change a target condition in PersistantStateData to the value of changeTarget
    private void ChangeCondition(TargetCondition changeTarget)
    {
        persistantStateData.GetComponent<PersistantStateData>().stateConditions[changeTarget.conditionName] = changeTarget.conditionValue;
        persistantStateData.GetComponent<PersistantStateData>().updateCount++;
    }

    // If condition updates set for active dialgoue, apply them. Also, if active dialogue was a forceOnEnable, reset it's position to the npc (FOE's enable condition(s) should be updated on its completion)
    private void OnEndConditionUpdates()
    {
        if(activeDialogue.conditionChangeOnExit.Count > 0)
        {
            foreach(TargetCondition condition in activeDialogue.conditionChangeOnExit)
            {
                ChangeCondition(condition);
                //persistantStateData.GetComponent<PersistantStateData>().stateConditions[condition.conditionName] = condition.conditionValue;
                //persistantStateData.GetComponent<PersistantStateData>().updateCount++;
            }
            if(activeDialogue.forceOnEnable)
            {
                activeNPC.transform.position= activeNPC.transform.parent.transform.position;
            }
        }

    }

    // Returns the next DialogueSpeedToken for the current sentenceIndex. If no such token exists, returns null.
    private DialogueSpeedToken GetNextSpeedToken(DialogueSpeedToken currentToken)
    {
        if(activeDialogue != null)
        {
            DialogueSpeedToken nextToken = null;
            foreach(DialogueSpeedToken token in activeDialogue.speedControls)
            {
                if (token.sentenceIndex == sentenceIndex)    // Only process tokens for the active sentence. (if none exist, null is returned)
                {
                    if (nextToken == null)
                    {
                        if (currentToken == null)
                        {
                            nextToken = token;  // If currentToken does not exist, any token for this sentence initializes nextToken.
                        }
                        else if (token.charIndex > currentToken.charIndex)
                        {
                            nextToken = token;  // If currentToken existed and a larger token has been found than currentToken, initialize nextToken to this
                        }
                    }
                    else if (currentToken != null)
                    {
                        if (token.charIndex > currentToken.charIndex && token.charIndex < nextToken.charIndex)
                        {
                            nextToken = token;  // If charIndex is less than nextToken, set it as nextToken
                        }  
                    }
                    else if (token.charIndex < nextToken.charIndex)
                    {
                        nextToken = token;  // If charIndex is less than nextToken, set it as nextToken
                    }
                }
            }
            return nextToken;
        }
        else
        {
            throw new MissingReferenceException("No active dialogue found in DialogueManager. Cannot retrieve DialogueSpeedToken");
        }
    }

    void StartDialogue(Dialogue dialogue)
    {
		if (activeNPC.transform.parent.GetComponent<FloatingTextManager> () != null) {
            activeNPC.transform.parent.GetComponent<FloatingTextManager>().disableFloatingText = true; // Disable the floating text for this npc
		}
        
        activeDialogue = dialogue;
        sentences.Clear();
        foreach(Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence.line);
        }
        ShowBox();
        if (activeDialogue.follow)
        {
            player.GetComponent<PlayerMovement>().SetFollowTarget(activeNPC.transform.parent.gameObject, activeNPC.GetComponent<NPCDialogues>().followTetherMinDistance, activeNPC.GetComponent<NPCDialogues>().followTetherStrongDistance);  // Set movement script to follow the "npc" the trigger is attached to.
        }
        
        DisplayNextSentence();
    }

    // Runs DisplaySentence with next sentence from queue. Runs EndDialogue if queue is empty, and does nothing if sentenceDisplayInProgress is true
    public void DisplayNextSentence()
    {
        if (!sentenceDisplayInProgress)
        {
            if (sentences.Count == 0)   // Dialogue is complete, end the dialogue interaction
            {
                EndDialogue();
            }
            else
            {
                sentenceDisplayInProgress = true;   // Flags the sentence display coroutine as being in progress
                SetActiveOption();
                string sentence = sentences.Dequeue();
                StartCoroutine(DisplaySentence(sentence));
            }
        }
    }

    // Used by cutscenes. Fast forwards sentence, then displays the next one, ensuring that the next sentence is displayed when called.
    public void CutsceneDislayNextSentence()
    {
        if (sentenceDisplayInProgress)
        {
            skipText = true;
        }
        DisplayNextSentenceWhenDone();
    }

    // Displays next sentence after the current one has finished without any input. Can set a delay 
    IEnumerator DisplayNextSentenceWhenDone(float delayBetween = 0)
    {
        while(sentenceDisplayInProgress) { yield return new WaitForEndOfFrame(); } // Wait until sentence is fully displayed
        if(delayBetween == 0)
        {
            DisplayNextSentence();
        }
        else
        {
            yield return new WaitForSeconds(delayBetween);
            DisplayNextSentence();
            displayWhenDoneLock = false;
        }
    }

    // Handles displaying of a sentence in the dialogueBox.
    IEnumerator DisplaySentence(string sentence)
    {
        DialogueSpeedToken nextSpeedToken = null;   // A token used to determine when to change the display speed and by how much
        int charIndex = 0;                          // An index to the current char within the sentence

        if (activeDialogue.speedControls.Count > 0)
        {
            nextSpeedToken = GetNextSpeedToken(nextSpeedToken); // Initialize nextSpeedToken if there are any
        }

        dialogueText.text = ""; // Clear text field
        
        // This loop controls the display speed of a sentence.
        foreach (char letter in sentence.ToCharArray())
        {
            if (skipText)   // This value allows the player to override normal display speed and present the entire sentence at once.
            {
                dialogueText.text = sentence;   // Set to display entire sentence immediately.
                while(nextSpeedToken!= null)    // Applies all speed tokens that would have occured had the text not been skipped.
                {
                    charDelay += nextSpeedToken.charDelayChange;
                    nextSpeedToken = GetNextSpeedToken(nextSpeedToken);
                }
                break;                          // Now that full sentence has been display, break out of loop.
            }

            if (nextSpeedToken != null )   // Updates charDelay and speed token, if any (more) exist.
            {
                if (nextSpeedToken.charIndex == charIndex)
                {
                    charDelay += nextSpeedToken.charDelayChange;
                    if (charDelay < 0)
                    {
                        charDelay = 0f; // charDelay cannot go negative.;
                    }
                    nextSpeedToken = GetNextSpeedToken(nextSpeedToken);
                }
            }

            yield return new WaitForSeconds(charDelay); //Wait charDelay seconds between each letter. 
            dialogueText.text += letter;
            charIndex++;
        }

        if (skipText)
        {
            skipText = !skipText;   // Once sentence is display, turn off skip text so it doesn't automatically run on next sentence
        }

        ShowChoices();    // Display choices after sentence has been printed.
        sentenceIndex++;
        sentenceDisplayInProgress = false;
    }

    // Resets and Updates values at completion of dialogue
    public void EndDialogue()
    {
        sentences.Clear();
        dialogueText.text = "";
        HideBox();
		if (activeNPC.transform.parent.GetComponent<FloatingTextManager> () != null) {
            activeNPC.transform.parent.GetComponent<FloatingTextManager> ().disableFloatingText = false;
		}

        player.GetComponent<PlayerMovement>().RemoveFollowTarget(); // Ends following movement of player on target

        OnEndConditionUpdates();

        sentenceIndex = 0;


        activeDialogue = null;
        dialogueBoxActive = false;
        activeNPC = null;
    }

    // Activates the dialogueBox
    public void ShowBox()
    {
        dialogueBox.SetActive(true);
    }

    // Deactivates the dialogueBox
    public void HideBox()
    {
        dialogueBox.SetActive(false);
    }

    public Dialogue GetActiveDialogue()
    {
        return activeDialogue;
    }

    public bool IsDialogueActive()
    {
        return dialogueBoxActive;
    }
}