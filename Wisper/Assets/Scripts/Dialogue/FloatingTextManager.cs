using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour {

    // Text related objects
    private Text floatingTextBox;           // The object which renders its text component to the screen
    private FloatingTexts floatingTexts;    // The component which contains all floating texts for this obj (usually an npc)
    private Dialogue activeFloatingText;    // The Dialogue object, which contains the text to write into the foating text box
    private Dialogue newFloatingText;       // The Dialogue object retrieved after updating the "enabled" Dialogues of floatingTexts. Essentially it is "what should be displayed."
    private Queue<string> sentences;        // The queue buffer which sentences from Dialogue objects are loaded into for display.

    // Locks for controlling asynchronous execution of many functions by update()
    private bool sentenceDisplayInProgress;     // Restricts other functions and coroutines while a sentence is being displayed to the screen
    private bool clearingInProgress;            // Restricts other functions and coroutines while the screen is clearing at completion of dialogue
    public bool updatingFloatingTexts = false; // Restricts other functions while the dialogues are updating.
    public bool disableFloatingText = false;    // Set true to prevent the floating text for the npc this is attached to. Prevents execution of some function calls and removes delays on coroutines.
    private bool killFloatingText = false;      // Functionally same as disableFloatingText, but used internally for temporary purposes such as reseting text after an update.

    // Exterior game objects which need to be referenced
    private GameObject persistantStateData;
    private GameObject player;

    // Update info
    public int persistantStateDataUpdateCount = 0; // Compared to the value in persistantStateData to determine if the dialogues need to check for updates

    // Use this for initialization
    void Start () {
        sentences = new Queue<string>();
        floatingTextBox = this.gameObject.GetComponentInChildren<Text>();
        floatingTexts = this.GetComponent <FloatingTexts>();
        sentenceDisplayInProgress = false;
        clearingInProgress = false;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        if (persistantStateData == null)    // Initialize persistantStateData
        {
            persistantStateData = GameObject.Find("PersistantStateData");
        }
        if (floatingTexts == null)  // Initialize floating text if it did not properly do so in Start
        {
            floatingTexts = this.GetComponent<FloatingTexts>();
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (persistantStateDataUpdateCount != persistantStateData.GetComponent<PersistantStateData>().updateCount && !updatingFloatingTexts)
        {
            Debug.Log("Updating floating texts");
            updatingFloatingTexts = true;
            persistantStateDataUpdateCount = persistantStateData.GetComponent<PersistantStateData>().updateCount;
            UpdateFloatingText();
            updatingFloatingTexts = false;
        }


        if ((transform.position - player.transform.position).magnitude < floatingTexts.range)
        {
            if (!updatingFloatingTexts) // Wait for text to be updated
            {
                if (newFloatingText == null) // If dialogue update was never called, set newDialogue.
                {
                    newFloatingText = getEnabledText();
                }

                if (activeFloatingText != newFloatingText) // If the dialogue finished (reset to null), or has changed since last time (doesn't match newFT after update)
                {
                    if (activeFloatingText != null) // If it changed, kill the current dialogue before proceeding.
                    {
                        killFloatingText = true;
                        EndFloatingText();
                        killFloatingText = false;
                    }

                    activeFloatingText = newFloatingText;
                    StartFloatingText(activeFloatingText);  // Start the new dialogue as active dialogue
                }
                else // Display next sentence
                {
                    DisplayNextSentence(3);
                }
            }
        }
        else if (activeFloatingText != null)   //If a dialogue was active and player moved out of range, end dialogue
        {
            EndFloatingText();
        }
    }

    public void StartFloatingText (Dialogue fT)
    {
        if (!(disableFloatingText || killFloatingText))   // If disableFloatingText is set, the floating text will not be started
        {
            // Debug.Log("Recieved floating text " + floatingText.dialogueName);
            sentences.Clear();
            //floatingText = speaker.GetComponentInChildren<Text>();
            foreach (string sentence in fT.sentences)
            {
                sentences.Enqueue(sentence);
                // Debug.Log("Enqued: " + sentence);
            }
            DisplayNextSentence(2);
        } 
    }

    
    public void DisplayNextSentence(int timeDelay, float charDelay = 0.066F)
    {
        if (!(disableFloatingText || killFloatingText))
        {
            if (!sentenceDisplayInProgress)
            {
                if (sentences.Count == 0)
                {
                    // Debug.Log("End of floating text, not displaying sentence");
                    EndFloatingText();
                }
                else
                {
                    sentenceDisplayInProgress = true;
                    string sentence = sentences.Dequeue();
                    // Debug.Log("Running corutine with sentence: " + sentence);
                    StartCoroutine(DisplaySentence(sentence, timeDelay, charDelay));
                }
            }
        }
        else
        {
            EndFloatingText();  // If floating text is active, but disableFloatingText was turned on, end the floating text.
        }
    }

    IEnumerator DisplaySentence(string sentence, int timeDelay, float charDelay)
    {
        // Debug.Log("In coroutine displaySentence");
        floatingTextBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if ((disableFloatingText || killFloatingText))   // If disableFloatingText is turned on, clear out the text field and exit loop
            {
                floatingTextBox.text = "";
                break;
            }
            floatingTextBox.text += letter;
            yield return new WaitForSeconds(charDelay); //Wait charDelay seconds.
        }
        if (!(disableFloatingText|| killFloatingText)) // Don't bother with delay if the floating text was disabled
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

        if (!(disableFloatingText || killFloatingText))   // Ignore delay if floating text was disabled
        {
            yield return new WaitForSeconds(2f); // Leave text on screen x second afterward before wiping it
        }

        floatingTextBox.text = "";
        activeFloatingText = null; // Nulls the active floating text so that update knows to reset it to "newFloatingText", which may or may not have changed
        clearingInProgress = false; // Reset status bool after job is completed
    }

    private void UpdateFloatingText()
    {
        // TODO: decide what errors to throw and handle if more than one dialogue would be enabled
        bool tempCheck = true;
        foreach (Dialogue d in floatingTexts.floatingTexts)
        {
            foreach (TargetCondition condition in d.enableConditions)
            {
                if (tempCheck) // If the previous conditions have held
                {
                    if (!CheckCondition(condition))
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
        newFloatingText = getEnabledText();
    }

    private bool CheckCondition(TargetCondition condition)
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

    private Dialogue getEnabledText()
    {
        foreach (Dialogue d in floatingTexts.floatingTexts)
        {
            if (d.enabled == true)
            {
                return d;
            }
        }
        throw new MissingReferenceException("No enabled floating texts found for " + this.name);
    }
}
