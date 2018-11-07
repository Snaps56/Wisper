using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogues : MonoBehaviour {

    public List<Dialogue> dialogues;
    private bool inDialogueRange;

    private void Start()
    {
        inDialogueRange = false;
    }

    public bool getInDialogueRange()
    {
        return inDialogueRange;
    }

    public void setInDialogueRange(bool state)
    {
        inDialogueRange = state;
    }
}
