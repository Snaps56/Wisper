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




   [Header("UI")]
	public GameObject miniMap;

	// Use this for initialization
	void Start () {
		miniMap.SetActive(true);
		rb = GetComponent<Rigidbody>();


		dialogueManager = GameObject.Find("DialogueManager");
	}

	void OnTriggerEnter(Collider other)
	{


		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = true;
		}



		if (other.gameObject.CompareTag("NPC") || other.gameObject.CompareTag ("Shrine"))
		{
			// Debug.Log("Detecting collision with NPC: " + other.name);
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
					// Debug.Log("Updating " + other.name + " dialogues");
					managerScript.UpdateDialogues(other.gameObject); // Updates list of dialogues on this npc, enabling and disabling based on various states
					// Debug.Log("Adding " + other.name + " to dialogue manager");
					managerScript.AddInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range

				}
			}
		}
	}


	void OnTriggerExit(Collider other) {


		if (other.gameObject.CompareTag ("Shrine")) {
			nearShrine = false;
		}
		if (other.gameObject.CompareTag ("NPC") || other.gameObject.CompareTag("Shrine"))
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
