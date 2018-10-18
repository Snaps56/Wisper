﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{

    public List<Dialogue> monologues;
    private int activeMonologue = -1;

    public void TriggerMonologue()
    {
        if (monologues.Count > 0)
        {
            //TODO Pull the correct dialogue from the list and run start dialogue
            // Currently just uses the first dialogue of the list

            // If there is not an active dialogue, start one
            // Else trigger next sentece, which has a 2 second delay before next one is displayed
            if (activeMonologue < 0)
            {
                activeMonologue = 0;
                monologues[0].setActive(true);
                FindObjectOfType<MonologueManager>().StartMonologue(monologues[0]);
            }
            else
            {
                // Should be called by monologue manager after text is cleared to avoid conflicting writes to text box.
                /*if (this.gameObject.GetComponent<MonologueManager>().DisplayNextSentence(2) == 0)
                {
                    deactivateMonologue();
                } */
                this.gameObject.GetComponent<MonologueManager>().DisplayNextSentence(2);
            }

        }
    }

    // Usually called by monologue manager after text is cleared
    public void deactivateMonologue()
    {
        Debug.Log("Deactivating dialogue");
        if(activeMonologue >= 0)
        {
            monologues[activeMonologue].setActive(false);
            activeMonologue = -1;
        }
    }

    private void Update()
    {
        if ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < 8)
        {
            TriggerMonologue();
        }
        else if (activeMonologue >= 0)   //If a dialogue was active and player moved out of range, end dialogue
        {
            if (monologues[activeMonologue].getActive())
            {
                deactivateMonologue();
                this.gameObject.GetComponent<MonologueManager>().EndMonologue();
            }
        }
    }
}