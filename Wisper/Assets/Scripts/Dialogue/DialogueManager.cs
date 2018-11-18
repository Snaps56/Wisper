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

    // Display time values & references
    private GameObject activeNPC;           // Reference to NPC with active dialogue
    private Dialogue activeDialogue;        // Reference to the active dialogue
    private Queue<string> sentences;        // Queue of sentences to display
    private int sentenceIndex = 0;          // Index for number of sentence from queue currently being displayed.
    private float charDelay;                // Delay between adding chars to display. Controls the "speed of speech." Minimum delay is 1/framerate seconds for charDelay, which corresponds to the "fastest talking speed."

    // Locks and controls
    private bool sentenceDisplayInProgress; // A lock used with subroutines that update UI text elements
    private bool dialogueBoxActive;         // A lock for preventing floating text or other UI elements from displaying during dialogue box use. Can be referenced for other things which need to be locked when dialogue box is active.
    private bool skipText = false;          // Switch to fast forward text on player click
    
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
        /*if(dialogueName == null)
        {
            // Debug.LogError("Dialogue display componenet not found: dialogueName");
        }
        if(dialogueText == null)
        {
            // Debug.LogError("Dialogue display componenet not found: dialogueText");
        }*/

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
            if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X"))
            {
                if (sentenceDisplayInProgress)
                {
                    skipText = true;
                }
                else
                {
                    DisplayNextSentence();
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
                d.enabled = false;
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

    // If condition updates set for active dialgoue, apply them. Also, if active dialogue was a forceOnEnable, reset it's position to the npc (FOE's enable condition(s) should be updated on its completion)
    private void OnEndConditionUpdates()
    {
        if(activeDialogue.conditionChangeOnExit.Count > 0)
        {
            foreach(TargetCondition condition in activeDialogue.conditionChangeOnExit)
            {
                persistantStateData.GetComponent<PersistantStateData>().stateConditions[condition.conditionName] = condition.conditionValue;
                persistantStateData.GetComponent<PersistantStateData>().updateCount++;
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
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        ShowBox();
        player.GetComponent<PlayerMovement>().SetFollowTarget(activeNPC.transform.parent.gameObject);  // Set movement script to follow the "npc" the trigger is attached to.
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
                string sentence = sentences.Dequeue();
                StartCoroutine(DisplaySentence(sentence));
            }
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
}
