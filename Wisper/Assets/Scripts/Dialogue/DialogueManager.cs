using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager dialogueManager;

    private GameObject dialogueBox; // The GameObject containing the dialogue box text elements
    private Text dialogueText;  // Text field of dialogue box
    private Text dialogueName;  // Name field of dialogue box

    private Dialogue activeDialogue;    // Reference to the active dialogue
    private Queue<string> sentences;    // Queue of sentences to display
    private int sentenceIndex = 0;

    private bool sentenceDisplayInProgress; // A lock used with subroutines that update UI text elements
    private bool dialogueBoxActive; // A lock for preventing floating text or other UI elements from displaying during dialogue box use. Can be referenced for other things which need to be locked when dialogue box is active.

    private bool skipText = false;  // Switch to fast forward text on player click
    private float charDelay;        // Delay between adding chars to display. Controls the "speed of speech"

    private GameObject player;  // The player
    private List<GameObject> inRangeNPC = new List<GameObject>();    // List of all npc's in range of player collider. Stored here to avoid repetitive find operations each frame.
    private GameObject nearestNPC;
    private GameObject activeNPC;
    private GameObject persistantStateData;

    // Update info
    public int persistantStateDataUpdateCount = 0; // Compared to the value in persistantStateData to determine if the dialogues need to check for updates

    void Awake()
    {
        // Ensures there will only ever be 1 dialogue manager in a scene
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

    // Use this for initialization
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
                Dialogue tmpDialogue = GetEnabledDialogue(dt);
                if (tmpDialogue.forceOnEnable)
                {
                    TranslateToPlayer(dt);
                }
            }
        }

        // If any of the dialogues on the player are forceOnEnable, keep the trigger on the player each update cycle
        if (inRangeNPC.Count != 0)
        {
            foreach(GameObject dt in inRangeNPC)
            {
                if(GetEnabledDialogue(dt).forceOnEnable)
                {
                    TranslateToPlayer(dt);
                }
            }
        }

        // When dialogue is active, respond to input
        if(dialogueBoxActive)
        {
            if (Input.GetKeyDown(KeyCode.T))
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
                    Debug.LogError(e.Message);
                }
            }
        }
    }

    // Add an npc as "in range" of player
    public void AddInRangeNPC(GameObject npc)
    {
        inRangeNPC.Add(npc);
    }

    // Remove npc as "in range" of player
    public void RemoveInRangeNPC(GameObject npc)
    {
        inRangeNPC.Remove(npc);
    }

    // Finds the closest npc to the player that has dialogue. (This NPC may not have any enabled however.
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

    // Returns the first enabled dialogue
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

    public int DisplayNextSentence()
    {
        if (sentenceDisplayInProgress)
        {
            return 1;
        }
        else
        {
            if (sentences.Count == 0)   // Dialogue is complete, end the dialogue interaction
            {
                EndDialogue();
                return 0;
            }
            else
            {
                sentenceDisplayInProgress = true;   // Flags the sentence display coroutine as being in progress
                string sentence = sentences.Dequeue();
                StartCoroutine(DisplaySentence(sentence));
                return 1;
            }
        }
    }

    IEnumerator DisplaySentence(string sentence)
    {
        DialogueSpeedToken nextSpeedToken = null;
        int charIndex = 0;

        if (activeDialogue.speedControls.Count > 0)
        {
            nextSpeedToken = GetNextSpeedToken(nextSpeedToken);
        }

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (skipText)
            {
                dialogueText.text = sentence;
                break;
            }

            if (nextSpeedToken != null )   // Updates charDelay and speed token, if any.
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
            skipText = !skipText;
        }

        sentenceIndex++;
        sentenceDisplayInProgress = false;
    }

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

    public void ShowBox()
    {
        dialogueBox.SetActive(true);
    }
    public void HideBox()
    {
        dialogueBox.SetActive(false);
    }
}
