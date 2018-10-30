﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Holds a set of dialogue. Includes:
 * A number of sentences of dialogue for display
 * Referential information to this (name and ID)
 * Enabled bool to mark as usable by the game
 * Condition for when to enable or disable
 * 
 * //////////// Warning ////////////
 * Modifying this script in any meaningful way will reset the dialogue settings in the Inspector, deleting any dialogues written.
 * Once implemented, use the feature to save dialogues to a hard copy before changing. Then the dialogues can be reloaded to the editor.
*/
[System.Serializable]
public class Dialogue{

    public string dialogueName; // Used to find element by name
    private int dialogueID; // Used to find element by ID

    public bool enabled = false;    // Set to mark this dialogue as enabled.

    public List<StringBoolPairs> enableConditions; // The conditions and what they should be set to for enabling this dialogue. Should be <string condition, bool desiredState>
    
    private bool active = false;    // May not be necessary

    [TextArea(1,3)]
    public string[] sentences;

    public int getID()
    {
        return dialogueID;
    }

    public void setID(int dialogueID)
    {
        this.dialogueID = dialogueID;
    }

    public bool getActive()
    {
        return active;
    }

    public void setActive(bool state)
    {
        this.active = state;
    }
}
