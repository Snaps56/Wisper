using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue{

    public string dialogueName; // Used to find element by name
    private int dialogueID; // Used to find element by ID

    public bool enabled = false;    // Set to mark this dialogue as enabled.

    public List<StringBoolPairs> enableConditions; // The conditions and what they should be set to for enabling this dialogue. Should be <string condition, bool desiredState>
    
    private bool active = false;

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
