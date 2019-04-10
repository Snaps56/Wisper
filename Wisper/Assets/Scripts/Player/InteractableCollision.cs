using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCollision : MonoBehaviour {

    private GameObject interactText;
    private GameObject dialogueBox;
    public bool nearShrine;
	private DialogueManager dialogueManager;

	// Use this for initialization
	void Start () {
		dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        dialogueBox = GameObject.Find("DialoguePanel");
        interactText = GameObject.Find("InteractText");
    }
    private void OnTriggerStay(Collider other)
    {
        // player finishes talking to shellster, dialoguebox deactivates, but player can stil talk to shellster...
        // button prompt should reactivate but it doesn't since nothing is triggering it to reactivate...
        // since button prompt activates only on trigger enter
        
        // so if other collider is dialogue trigger, dialoguebox is inactive, and npc has dialogues, activate the prompt
        if (other.gameObject.CompareTag("DialogueTrigger") && !dialogueBox.activeInHierarchy)
        {
            Debug.Log("In Dialogue Range");
            NPCDialogues npcDialogues = other.gameObject.GetComponent<NPCDialogues>();
            if (npcDialogues != null)   // If this npc has dialogues
            {
                Debug.Log("Has Dialogue");
                try
                {
                    dialogueManager.GetEnabledDialogue(other.gameObject);
                    Debug.Log("Found some dialogue?");
                    ActivatePrompt();
                }
                catch (MissingReferenceException e)
                {
                    Debug.Log("Couldn't find dialogue?");
                    DeactivatePrompt();
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
	{
        
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = true;
		}

		if (other.gameObject.CompareTag("DialogueTrigger"))
		{
			// make sure reference to DM is set
			if (dialogueManager == null)
			{
				// Debug.Log("null dialogueManager, checking again");
				dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
			}

			NPCDialogues npcDialogues = other.gameObject.GetComponent<NPCDialogues>();
			if (npcDialogues != null)   // If this npc has dialogues
            {
                // set interact text active only when:
                //      -npc has dialogues
                //      -within interact range
                //      -dialogue box is not active

                // Debug.Log("NPC has dialogues");
                if (!npcDialogues.GetInDialogueRange())
                {
                    npcDialogues.SetInDialogueRange(true);  // Flags dialogues attached to npc as in range. Used as a lock to prevent unnecessary updates to dialogue manager.
                    
					// The following update line may no longer be necessary (currently doesn't interfere though, and may be used depending on changes in requirements)
					dialogueManager.UpdateDialogues(other.gameObject); // Updates list of dialogues on this npc, enabling and disabling based on various states
					
					dialogueManager.AddInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
        
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = false;
        }
		if (other.gameObject.CompareTag ("DialogueTrigger"))
        {
            DeactivatePrompt();

            NPCDialogues npcDialogues = other.gameObject.GetComponent<NPCDialogues>();
            if (npcDialogues != null)   // If this npc has dialogues
            {
                // Debug.Log("NPC has dialogues");
                if (npcDialogues.GetInDialogueRange())
                {
                    npcDialogues.SetInDialogueRange(false);  // Flags dialogues attached to npc as in range. Used as a lock to prevent unnecessary updates to dialogue manager.
                    dialogueManager.RemoveInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range

                }
            }
        }
    }
    void ActivatePrompt()
    {
        interactText.SetActive(true);
    }
    void DeactivatePrompt()
    {
        interactText.SetActive(false);
    }
}
