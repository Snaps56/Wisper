using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogues : MonoBehaviour {

    public float defaultCharDelay = 0.066F;
    public List<Dialogue> dialogues;
    private bool inDialogueRange;
    

    private void Start()
    {
        inDialogueRange = false;
    }

    public bool GetInDialogueRange()
    {
        return inDialogueRange;
    }

    public void SetInDialogueRange(bool state)
    {
        inDialogueRange = state;
    }
}
