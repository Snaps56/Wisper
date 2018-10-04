using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Text dialogueText;
    private Queue<string> sentences;
    private bool sentenceDisplayInProgress;

	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
        dialogueText = this.gameObject.GetComponentInChildren<Text>();
        sentenceDisplayInProgress = false;
	}
	
    public void StartDialogue (Dialogue dialogue)
    {
        Debug.Log("Recieved dialogue " + dialogue.dialogueName);
        sentences.Clear();
        //dialogueText = speaker.GetComponentInChildren<Text>();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
            Debug.Log("Enqued: " + sentence);
        }
        DisplayNextSentence(2);
    }

    // Returns 0 when dialogue is over
    public int DisplayNextSentence(int timeDelay)
    {
        if(sentenceDisplayInProgress)
        {
            return 1;
        }
        else
        {
            sentenceDisplayInProgress = true;
            if (sentences.Count == 0)
            {
                Debug.Log("End of Dialogue, not displaying sentence");
                EndDialogue();
                return 0;
            }
            string sentence = sentences.Dequeue();
            StartCoroutine(displaySentence(sentence, timeDelay));
            return 1;
        }
    }

    IEnumerator displaySentence(string sentence, int timeDelay)
    {
        Debug.Log("In coroutine displaySentence");
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; //Wait 1 frame between each letter.
        }
        yield return new WaitForSeconds(timeDelay);
        sentenceDisplayInProgress = false;
    }

    public void EndDialogue()
    {
        Debug.Log("Conversation over");
        sentences.Clear();
        dialogueText.text = "";
        sentenceDisplayInProgress = false;
    }

    //When a character triggers a condition that would change dialogue (e.g. completed a task), run this function to enable/disable dialogues.
    public void UpdateDialogues(int conditionID)
    {
        /* Assumptions:
         *  There exists a table A with columns: DialogueName(string), DialogueID(numeric), Enabled(bool), ConditionCount(numeric)
         *  There exists a table B with columns: DialogueID (numeric), ConditionName(string)
         *  There exists a table C with columns: ConditionName(string), CondiionID(numeric), State(bool)
         *  
         *  Table A is generated on loading an area (at some point after C), and deleted upon leaving. Defaults derived from dialogue object.
         *  Table B is generated after A, connecting A to C using conditions named in A.
         *  Table C is generated on game launch, and is persistant for a save file. This may be used outside of dialogues.
        */
        /*
         * Condition is met, this function has been called
         * Find ConditionID in table C.
         * Find all instances of this in table B.
         * For each instance
         *  find the corresponding tuple in A
         *  find all instances of that tuple's DialogueID in table B
         *
         */
        //Having conditions stored in table would allow for retention of dialogue status on reloading characters/areas.
        //
    }
}
