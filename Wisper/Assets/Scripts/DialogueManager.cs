using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager dialogueManager;

    private Text dialogueText;
    private Text dialogueName;
    private Queue<string> sentences;
    private bool sentenceDisplayInProgress;
    public bool dialogueBoxActive;
    private GameObject dialogueBox;
    
    void Awake()
    {
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
        hideBox();
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
