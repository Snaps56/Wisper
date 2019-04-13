using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCollision : MonoBehaviour {
    
    private GameObject dialogueBox;
    public bool nearShrine;
	private DialogueManager dialogueManager;
    private GameObject interactText;

    private int currentDialogueTriggers = 0;
    public bool withinDialogueRange = true;

	// Use this for initialization
	void Start () {
		dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        dialogueBox = GameObject.Find("MainCanvas").gameObject.transform.Find("DialoguePanel").gameObject;
        interactText = GameObject.Find("MainCanvas").gameObject.transform.Find("InteractText").gameObject;
    }
    private void Update()
    {
        //Debug.Log("Within dialogue range: " + withinDialogueRange + ", number of dialogue triggers: " + currentDialogueTriggers);
        if (currentDialogueTriggers > 0)
        {
            withinDialogueRange = true;
        }
        else
        {
            withinDialogueRange = false;
        }
    }
    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = true;
		}

		if (other.gameObject.CompareTag("DialogueTrigger"))
        {
            currentDialogueTriggers++;
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
                if (currentDialogueTriggers < 2)
                {
                    try
                    {
                        dialogueManager.GetEnabledDialogue(other.gameObject);
                        //Debug.Log("Found some dialogue?");
                        if (!dialogueBox.activeInHierarchy)
                        {
                            ActivatePrompt();
                        }
                    }
                    catch (MissingReferenceException e)
                    {
                        //Debug.Log("Couldn't find dialogue?");
                        DeactivatePrompt();
                    }
                    //ActivatePrompt();
                }

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
            currentDialogueTriggers--;
            if (currentDialogueTriggers < 1)
            {
                DeactivatePrompt();
            }

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
