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
    

    private GameObject player;  // The player
    private List<GameObject> inRangeNPC;    // List of all npc's in range of player collider. Stored here to avoid repetitive find operations each frame.

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
    void Start () {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        sentences = new Queue<string>();
        foreach(Text textField in GameObject.FindGameObjectWithTag("DialogueBox").GetComponents<Text>())
        {
            if(textField.name == "Name")
            {
                dialogueName = textField;
            }
            else if(textField.name == "Text")
            {
                dialogueText = textField;
            }
            else
            {
                Debug.Log("Unexpected text field found with name: " + textField.name);
            }
        }
        if(dialogueName == null)
        {
            Debug.LogError("Dialogue display componenet not found: dialogueName");
        }
        if(dialogueText == null)
        {
            Debug.LogError("Dialogue display componenet not found: dialogueText");
        }
        sentenceDisplayInProgress = false;
        player = GameObject.FindGameObjectWithTag("Player");
        hideBox();
    }

    private void Update()
    {
        if(inRangeNPC.Count != 0)
        {
            if(!dialogueBoxActive)
            {
                // Find nearest npc and display dialogue interaction button above them
                // If the player inputs this button, toggle dialogueBoxActive and start the dialogue for that npc
            }
            else
            {
                // Dialogue box is active, respond to input as necessary here.
            }
        }
    }

    // Add an npc as in range of player
    public void addInRangeNPC(GameObject npc)
    {
        inRangeNPC.Add(npc);
    }

    // Remove npc as in range of player
    public void removeInRangeNPC(GameObject npc)
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

    void startDialogue(Dialogue dialogue)
    {
        Debug.Log("Recieved dialogue " + dialogue.dialogueName);
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            Debug.Log("Enqued: " + sentence);
        }
        showBox();
        DisplayNextSentence();
    }

    public int DisplayNextSentence(float charDelay = 0.066F )
    {
        if (sentenceDisplayInProgress)
        {
            // Could add a bool switch here to tell corutine to break and display all text at once.
            return 1;
        }
        else
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return 0;
            }
            else
            {
                sentenceDisplayInProgress = true;
                string sentence = sentences.Dequeue();
                StartCoroutine(DisplaySentence(sentence, charDelay));
                return 1;
            }
        }
    }

    IEnumerator DisplaySentence(string sentence, float charDelay)
    {
        Debug.Log("In coroutine displaySentence");
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(charDelay); //Wait 1 frame between each letter.
        }
        sentenceDisplayInProgress = false;
    }

    public void EndDialogue()
    {
        Debug.Log("Conversation over");
        sentences.Clear();
        dialogueText.text = "";
        hideBox();
    }

    public void showBox()
    {
        dialogueBox.SetActive(true);
    }
    public void hideBox()
    {
        dialogueBox.SetActive(false);
    }
}
