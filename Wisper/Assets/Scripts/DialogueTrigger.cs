using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public List<Dialogue> dialogues;
    private int activeDialogue = -1;

    public void TriggerDialogue()
    {
        if(dialogues.Count > 0)
        {
            //TODO Pull the correct dialogue from the list and run start dialogue
            // Currently just uses the first dialogue of the list

            // If there is not an active dialogue, start one
            // Else trigger next sentece, which has a 2 second delay before next one is displayed
            if(activeDialogue < 0)
            {
                activeDialogue = 0;
                dialogues[0].setActive(true);
                FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
            }
            else
            {
                //Debug.Log("Calling DisplayNextSentence");
                if(this.gameObject.GetComponent<DialogueManager>().DisplayNextSentence(2) == 0)
                {
                    deactivateDialogue();
                }
            }
            
        }
    }

    private void deactivateDialogue()
    {
        Debug.Log("Deactivating dialogue");
        dialogues[activeDialogue].setActive(false);
        activeDialogue = -1;
    }

    private void Update()
    {
        if ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < 10)
        {
            TriggerDialogue();
        }
        else if (activeDialogue >= 0)   //If a dialogue was active and player moved out of range, end dialogue
        {
            if (dialogues[activeDialogue].getActive())
            {
                deactivateDialogue();
                this.gameObject.GetComponent<DialogueManager>().EndDialogue();
            }
        }
    }
}
