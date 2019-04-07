using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCollision : MonoBehaviour {

    private GameObject interactText;
    private GameObject depositText;
    private GameObject dialogueBox;
    public bool nearShrine;

    private bool checkInteract = false;

	private Rigidbody rb;
	private float startingSpeed;
	private float originalVAcceleration;

	private float vel;

    private OrbCount orbCountScript;

	private GameObject dialogueManager;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
        
		dialogueManager = GameObject.Find("DialogueManager");
        dialogueBox = GameObject.Find("DialoguePanel");
        interactText = GameObject.Find("InteractText");
        depositText = GameObject.Find("ShrineDepositText");
        orbCountScript = GameObject.Find("Orb Collection Collider").GetComponent<OrbCount>();
    }
    private void OnTriggerStay(Collider other)
    {
        // player finishes talking to shellster, dialoguebox deactivates, but player can stil talk to shellster...
        // button prompt should reactivate but it doesn't since nothing is triggering it to reactivate...
        // since button prompt activates only on trigger enter
        

        // TODO: GO FORTH PSD IS NOT THE VAR YOU WANT TO USE
        Debug.Log("Orb Count: " + orbCountScript.GetOrbCount() + ", PSD Go Forth: " + (bool)PersistantStateData.persistantStateData.stateConditions["GoForth"] + ", + near shrine: " + nearShrine);
        // so if other collider is dialogue trigger, dialoguebox is inactive, and npc has dialogues, activate the prompt
        if (other.gameObject.CompareTag("DialogueTrigger") && !dialogueBox.activeInHierarchy && !interactText.activeInHierarchy)
        {
            NPCDialogues npcDialogues = other.gameObject.GetComponent<NPCDialogues>();
            if (npcDialogues != null)   // If this npc has dialogues
            {
                ActivatePrompt();
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
				dialogueManager = GameObject.Find("DialogueManager");
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

					DialogueManager managerScript = dialogueManager.GetComponent<DialogueManager>();

					// The following update line may no longer be necessary (currently doesn't interfere though, and may be used depending on changes in requirements)
					managerScript.UpdateDialogues(other.gameObject); // Updates list of dialogues on this npc, enabling and disabling based on various states
					
					managerScript.AddInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range
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
                    DialogueManager managerScript = dialogueManager.GetComponent<DialogueManager>();
                    // Debug.Log("Removing " + other.name + " from dialogue manager");
                    managerScript.RemoveInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range

                }
            }
        }
    }
    void ActivatePrompt()
    {
        // show deposit text instead if player is ready to deposit
        if (nearShrine && (bool)PersistantStateData.persistantStateData.stateConditions["GoForth"] && orbCountScript.GetOrbCount() > 0)
        {
            depositText.SetActive(true);
        }
        else
        {
            // else do normal interact text
            interactText.SetActive(true);
        }
    }
    void DeactivatePrompt()
    {
        interactText.SetActive(false);
        depositText.SetActive(false);
    }
}
