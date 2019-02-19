using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCollision : MonoBehaviour {

	[Header("Player Controls")]

	public bool nearShrine;

	private Rigidbody rb;
	private float startingSpeed;
	private float originalVAcceleration;

	private float vel;

	private GameObject dialogueManager;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();


		dialogueManager = GameObject.Find("DialogueManager");
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
			// Debug.Log("Left NPC collision: " + other.name);
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
}
