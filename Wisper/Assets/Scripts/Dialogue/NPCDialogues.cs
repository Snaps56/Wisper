using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogues : MonoBehaviour {

    public float defaultCharDelay = 0.066F;
    public float followTetherMinDistance = 0.5f;
    public float followTetherStrongDistance = 2f;
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
