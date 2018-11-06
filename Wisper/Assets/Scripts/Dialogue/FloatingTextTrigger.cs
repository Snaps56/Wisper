﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextTrigger : MonoBehaviour
{

    public List<Dialogue> floatingTexts;
    private int activeFloatingText = -1;

    public void TriggerFloatingText()
    {
        if (floatingTexts.Count > 0)
        {
            //TODO Pull the correct dialogue from the list and run start dialogue
            // Currently just uses the first dialogue of the list

            // If there is not an active floating text, start one
            // Else trigger next sentece, which has a 2 second delay before next one is displayed
            if (activeFloatingText < 0)
            {
                activeFloatingText = 0;
                floatingTexts[0].setActive(true);
                FindObjectOfType<FloatingTextManager>().StartFloatingText(floatingTexts[0]);
            }
            else
            {
                this.gameObject.GetComponent<FloatingTextManager>().DisplayNextSentence(3);
            }

        }
    }

    // Usually called by floating text manager after text is cleared
    public void deactivateFloatingText()
    {
        // Debug.Log("Deactivating dialogue");
        if(activeFloatingText >= 0)
        {
            floatingTexts[activeFloatingText].setActive(false);
            activeFloatingText = -1;
        }
    }

    private void Update()
    {
        if ((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < 8)
        {
            TriggerFloatingText();
        }
        else if (activeFloatingText >= 0)   //If a dialogue was active and player moved out of range, end dialogue
        {
            if (floatingTexts[activeFloatingText].getActive())
            {
                deactivateFloatingText();
                this.gameObject.GetComponent<FloatingTextManager>().EndFloatingText();
            }
        }
    }
}