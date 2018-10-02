using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue{

    public string dialogueName; //Used to find element by name
    private int dialogueID; //Used to find element by ID

    public bool enabled;
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
