using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour {

    private Text floatingText;
    private Queue<string> sentences;
    private bool sentenceDisplayInProgress;
    private bool clearingInProgress;

    // Use this for initialization
    void Start () {
        sentences = new Queue<string>();
        floatingText = this.gameObject.GetComponentInChildren<Text>();
        sentenceDisplayInProgress = false;
        clearingInProgress = false;
	}
	
    public void StartFloatingText (Dialogue floatingText)
    {
        // Debug.Log("Recieved floating text " + floatingText.dialogueName);
        sentences.Clear();
        //floatingText = speaker.GetComponentInChildren<Text>();
        foreach(string sentence in floatingText.sentences)
        {
            sentences.Enqueue(sentence);
            // Debug.Log("Enqued: " + sentence);
        }
        DisplayNextSentence(2);
    }

    // Returns 0 when floating text is over
    public int DisplayNextSentence(int timeDelay, float charDelay = 0.066F)
    {
        if(sentenceDisplayInProgress)
        {
            return 1;
        }
        else
        {
            if (sentences.Count == 0)
            {
                // Debug.Log("End of floating text, not displaying sentence");
                EndFloatingText();
                return 0;
            }
            else
            {
                sentenceDisplayInProgress = true;
                string sentence = sentences.Dequeue();
                // Debug.Log("Running corutine with sentence: " + sentence);
                StartCoroutine(DisplaySentence(sentence, timeDelay, charDelay));
                return 1;
            }
        }
    }

    IEnumerator DisplaySentence(string sentence, int timeDelay, float charDelay)
    {
        // Debug.Log("In coroutine displaySentence");
        floatingText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            floatingText.text += letter;
            yield return new WaitForSeconds(charDelay); //Wait charDelay seconds.
        }
        yield return new WaitForSeconds(timeDelay);
        sentenceDisplayInProgress = false;
    }

    // End floating text normally as a result of text completion or moving out of distance
    public void EndFloatingText()
    {
        // Debug.Log("Conversation over");
        if(clearingInProgress)  // If this process has already been initiated, do nothing and let corutine finish
        {
            // Debug.Log("Clear already initiated");
        }
        else
        {
            // If the clear process was called the first time, set status bool and run corutine
            clearingInProgress = true;
            StartCoroutine(ClearFloatingText());

        }
        
    }

    // Clears out the queue and text box
    IEnumerator ClearFloatingText()
    {
        // Debug.Log("Clearing the floating text");
        sentences.Clear();  // Clears out queue
        while (sentenceDisplayInProgress)   // Wait until any in progress text has finished displaying
        {
            yield return null;
        }
        yield return new WaitForSeconds(1); // Leave text on screen 1 second afterward before wiping it
        floatingText.text = "";
        this.gameObject.GetComponent<FloatingTextTrigger>().deactivateFloatingText(); // Informs the trigger that there is no longer an active.
        // Debug.Log("reseting clear status");
        clearingInProgress = false; // Reset status bool after job is completed
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
