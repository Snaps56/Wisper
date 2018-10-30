using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour {

    private Text floatingText;
    private Queue<string> sentences;
    private bool sentenceDisplayInProgress;
    private bool clearingInProgress;

    public bool disableFloatingText = false;   // Set true to prevent the floating text for the npc this is attached to.

    // Use this for initialization
    void Start () {
        sentences = new Queue<string>();
        floatingText = this.gameObject.GetComponentInChildren<Text>();
        sentenceDisplayInProgress = false;
        clearingInProgress = false;
	}
	
    public void StartFloatingText (Dialogue floatingText)
    {
        if (!disableFloatingText)   // If disableFloatingText is set, the floating text will not be started
        {
            // Debug.Log("Recieved floating text " + floatingText.dialogueName);
            sentences.Clear();
            //floatingText = speaker.GetComponentInChildren<Text>();
            foreach (string sentence in floatingText.sentences)
            {
                sentences.Enqueue(sentence);
                // Debug.Log("Enqued: " + sentence);
            }
            DisplayNextSentence(2);
        } 
    }

    // Returns 0 when floating text is over
    public int DisplayNextSentence(int timeDelay, float charDelay = 0.066F)
    {
        if (!disableFloatingText)
        {
            if (sentenceDisplayInProgress)
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
        else
        {
            EndFloatingText();  // If floating text is active, but disableFloatingText was turned on, end the floating text.
            return 0;
        }
    }

    IEnumerator DisplaySentence(string sentence, int timeDelay, float charDelay)
    {
        // Debug.Log("In coroutine displaySentence");
        floatingText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (disableFloatingText)   // If disableFloatingText is turned on, clear out the text field and exit loop
            {
                floatingText.text = "";
                break;
            }
            floatingText.text += letter;
            yield return new WaitForSeconds(charDelay); //Wait charDelay seconds.
        }
        if (!disableFloatingText) // Don't bother with delay if the floating text was disabled
        {
            yield return new WaitForSeconds(timeDelay); // Wait timeDelay seconds before allowing next sentence to display
        }
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

        if (!disableFloatingText)   // Ignore delay if floating text was disabled
        {
            yield return new WaitForSeconds(2f); // Leave text on screen x second afterward before wiping it
        }

        floatingText.text = "";
        this.gameObject.GetComponent<FloatingTextTrigger>().deactivateFloatingText(); // Informs the trigger that there is no longer an active.
        // Debug.Log("reseting clear status");
        clearingInProgress = false; // Reset status bool after job is completed
    }

    //When a character triggers a condition that would change dialogue (e.g. completed a task), run this function to enable/disable dialogues.
    public void UpdateDialogues(int conditionID)
    {
        
    }
}
