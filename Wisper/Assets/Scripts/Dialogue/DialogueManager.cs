using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager dialogueManager;

    private GameObject dialogueBox; // The GameObject containing the dialogue box text elements
    private Text dialogueText;  // Text field of dialogue box
    private Text dialogueName;  // Name field of dialogue box

    private Queue<string> sentences;    // Queue of sentences to display

    private bool sentenceDisplayInProgress; // A lock used with subroutines that update UI text elements
    private bool dialogueBoxActive; // A lock for preventing floating text or other UI elements from displaying during dialogue box use. Can be referenced for other things which need to be locked when dialogue box is active.

    private bool skipText = false; // Switch to fast forward text on player click

    private GameObject player;  // The player
    private List<GameObject> inRangeNPC = new List<GameObject>();    // List of all npc's in range of player collider. Stored here to avoid repetitive find operations each frame.
    private GameObject nearestNPC;
    private GameObject persistantStateData;

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
        // Debug.Log("Checking textfields in dialogueBox");
        if(dialogueBox != null)
        {
            // Debug.Log("Found dialogueBox");
        }
        foreach(Text textField in dialogueBox.GetComponentsInChildren<Text>())
        {
            if(textField.gameObject.name == "Speaker")
            {
                // Debug.Log("Found textfield \"Speaker\"");
                dialogueName = textField;
            }
            else if(textField.gameObject.name == "Dialogue")
            {
                // Debug.Log("Found textfield \"Dialogue\"");
                dialogueText = textField;
            }
            else
            {
                // Debug.Log("Unexpected text field found with name: " + textField.name);
            }
        }
        if(dialogueName == null)
        {
            // Debug.LogError("Dialogue display componenet not found: dialogueName");
        }
        if(dialogueText == null)
        {
            // Debug.LogError("Dialogue display componenet not found: dialogueText");
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
        if (persistantStateData == null)
        {
            persistantStateData = GameObject.Find("PersistantStateData");
        }

        // Debug.Log("inRangeNPC count: " + inRangeNPC.Count);
        if(inRangeNPC.Count != 0)
        {
            if(!dialogueBoxActive)
            {
                try
                {
                    nearestNPC = GetClosestNPC();
                    //TODO: Display interact button by this npc
                    // Debug.Log("DialogueManager: Checking for input");
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        // Debug.Log("Detected input");
                        dialogueBoxActive = true;
                        StartDialogue(GetEnabledDialogue(nearestNPC)); //Determine dialogue to use, Activate dialogue
                    }
                }
                catch(MissingReferenceException e)
                {
                    Debug.LogError(e.Message);
                }
            }
            else    // Dialogue box is active, respond to input as necessary here.
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
        }
    }

    // Add an npc as in range of player
    public void AddInRangeNPC(GameObject npc)
    {
        inRangeNPC.Add(npc);
        // Debug.Log(npc.name + " added to inRangeNPC");
    }

    // Remove npc as in range of player
    public void RemoveInRangeNPC(GameObject npc)
    {
        inRangeNPC.Remove(npc);
    }

    // Finds the closest npc to the player that has dialogue.
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

    public void UpdateDialogues(GameObject npc)
    {
        // TODO: decide what errors to throw and handle if more than one dialogue would be enabled
        NPCDialogues npcDialogues = npc.GetComponent<NPCDialogues>();
        bool tempCheck = true;
        foreach(Dialogue d in npcDialogues.dialogues)
        {
            foreach (StringBoolPairs condition in d.enableConditions)
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

    private bool CheckCondition(StringBoolPairs condition)
    {
        string conditionName = condition.String;
        bool conditionValue = condition.Bool;
        if ((bool)persistantStateData.GetComponent<PersistantStateData>().stateConditions[conditionName] == conditionValue)
        {
            return true;
        }
        else
        {
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
        throw new MissingReferenceException("No enabled dialogue on this NPC");
    }

    void StartDialogue(Dialogue dialogue)
    {
        nearestNPC.GetComponent<FloatingTextManager>().disableFloatingText = true; // Disable the floating text for this npc
        // Debug.Log("Recieved dialogue " + dialogue.dialogueName);
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            // Debug.Log("Enqued: " + sentence);
        }
        ShowBox();
        DisplayNextSentence();
    }

    public int DisplayNextSentence(float charDelay = 0.066F )
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
                StartCoroutine(DisplaySentence(sentence, charDelay));
                return 1;
            }
        }
    }

    IEnumerator DisplaySentence(string sentence, float charDelay)
    {
        // Debug.Log("In coroutine displaySentence");
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (skipText)
            {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(charDelay); //Wait charDelay seconds between each letter.
        }
        if (skipText)
        {
            skipText = !skipText;
        }
        sentenceDisplayInProgress = false;
    }

    public void EndDialogue()
    {
        // Debug.Log("Conversation over");
        sentences.Clear();
        dialogueText.text = "";
        HideBox();
        nearestNPC.GetComponent<FloatingTextManager>().disableFloatingText = false;
        dialogueBoxActive = false;
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
