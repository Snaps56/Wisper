using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour {

	[Header("Player Controls")]
	public float speed;
	public Transform camera;
	public Vector3 currentMovementForce;

	public bool nearShrine;

	private float orbMax = 50;
	private float orbCount;
	private Rigidbody rb;
	private float orbIncrementSpeed = 0.1f;
	private float treeSpeed;
	private float treeSlow = 0.7f;
	private float originalSpeed;
	private float startingSpeed;
	private float originalVAcceleration;
	private int treeCount = 0;
	private float vel;
	private GameObject[] pickups;
	private HandleObjects handleObjects;
	private Vector3 positionStamp;
	private float shake;
	private GameObject dialogueManager;

	private float verticalAcceleration = 0.001f;
	private float verticalSpeed = 0;

	[Header("Collision Handeling")]
	public Collider playerCollider;
	private float shakeAmount;
    private Vector3 terrainPosition;

	[Header("UI")]
	public Image windPowerBar;
	public GameObject turnBackText;
	public GameObject miniMap;
	public Text orbCountText;

	// Use this for initialization
	void Start () {
		miniMap.SetActive(true);
		rb = GetComponent<Rigidbody>();
		//rb.drag = 1;
		//rb.angularDrag = 1;
		orbCount = 0;
		startingSpeed = speed;
		originalSpeed = speed;
		originalVAcceleration = verticalAcceleration;
		treeSpeed = treeSlow * speed;
		pickups = GameObject.FindGameObjectsWithTag("PickUp");
		turnBackText.SetActive(false);
		orbCountText.text = orbCount.ToString()+"/" + orbMax;
		shakeAmount = 0.05f;
		shake = 0;
		dialogueManager = GameObject.Find("DialogueManager");
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Terrain"))
        {
            terrainPosition = other.transform.position;
        }
        if (other.gameObject.CompareTag("TutorialMove"))
		{

		}
		else if (other.gameObject.CompareTag("TutorialLook"))
		{

		}
		else if (other.gameObject.CompareTag("TutorialAscend"))
		{

		}
		if (other.gameObject.CompareTag("TutorialLift"))
		{

		}
		float objectDistance = (transform.position - other.transform.position).magnitude;
		if (other.gameObject.CompareTag("Orb") && objectDistance < 2f) {
			Destroy(other.gameObject);
			if (orbCount < orbMax)
			{
				orbCount += 1;
				verticalAcceleration += 0.0001f;
				speed += orbIncrementSpeed;
			}
			windPowerBar.fillAmount = orbCount / orbMax; 
			originalSpeed = speed;
			originalVAcceleration = verticalAcceleration;
			treeSpeed = treeSlow * speed;
			orbCountText.text = orbCount.ToString() + "/" + orbMax;

			//These are causing problems.  I'll need to come back to it ~ Nick
			//GetComponent<ObjectThrow>().throwForce += 15;
			//GetComponent<ObjectLift>().liftCenterStrength += 10;
		}
		if (other.gameObject.CompareTag("Border"))
		{
			//shake = 1;
			positionStamp = this.transform.position;
			//if (speed > preTreeSpeed/2 )
			//{
			//    speed = speed * 0.1f;
			//    verticalAcceleration = 0.001f;
			//}

			//if (speed > originalSpeed/2 )
			//{
			//    speed = speed * 0.1f;
			//    verticalAcceleration = 0.001f;
			//}
			//if (shake > 0)
			//{
			//    this.transform.position = this.transform.position + Random.insideUnitSphere * shakeAmount;
			//}
			//else
			//{
			//    shake -= Time.deltaTime * 0.1f;
			//}
			turnBackText.SetActive(true);
		}
		if (other.gameObject.CompareTag("BorderTele"))
		{
			rb.velocity = new Vector3(0, 0, 0);
			this.transform.position = positionStamp;
		}
		if (other.gameObject.CompareTag ("Tree")) {
			speed = treeSpeed;
			treeCount++;
			Debug.Log ("Speed is reduced to :" + speed);
		}
		//      if (other.gameObject.CompareTag ("PickUp")) {
		//	other.gameObject.GetComponent<HandleObjects>().throwForce = throwPower;
		//}
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
				if (!npcDialogues.getInDialogueRange())
				{

					npcDialogues.setInDialogueRange(true);  // Flags dialogues attached to npc as in range. Used as a lock to prevent unnecessary updates to dialogue manager.

					DialogueManager managerScript = dialogueManager.GetComponent<DialogueManager>();
					// Debug.Log("Updating " + other.name + " dialogues");
					managerScript.UpdateDialogues(other.gameObject); // Updates list of dialogues on this npc, enabling and disabling based on various states
					// Debug.Log("Adding " + other.name + " to dialogue manager");
					managerScript.AddInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range

				}
			}
		}
	}
	//Trigger function activated while collision is being made
	void OnTriggerStay(Collider other)
	{
		shake = 1;
		//Activates when the player enters the border
		if (other.gameObject.CompareTag("Border"))
		{
			//Activate shake
			if (shake > 0)
			{
				this.transform.position = this.transform.position + Random.insideUnitSphere * shakeAmount;
			}
			//Reduce shake
			else
			{
				shake -= Time.deltaTime * 0.1f;
			}
		}



        //If player hits a wall
        if (other.gameObject.CompareTag("Terrain"))
        {
            Vector3 reflectDirection = this.transform.position - terrainPosition;
            this.rb.AddForce(reflectDirection*GetComponent<Player>().speed);
        }
    }

	void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag ("Tree")) {
			treeCount--;
			if (treeCount == 0) {
				speed = originalSpeed;
				Debug.Log ("Speed is back to :" + speed);
			}
		}
		if(other.gameObject.CompareTag("Border"))
		{
			turnBackText.SetActive(false);
			speed = originalSpeed;
			verticalAcceleration = originalVAcceleration;
			shake = 0;
		}
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
				if (npcDialogues.getInDialogueRange())
				{

					npcDialogues.setInDialogueRange(false);  // Flags dialogues attached to npc as in range. Used as a lock to prevent unnecessary updates to dialogue manager.
					DialogueManager managerScript = dialogueManager.GetComponent<DialogueManager>();
					// Debug.Log("Removing " + other.name + " from dialogue manager");
					managerScript.RemoveInRangeNPC(other.gameObject);    // Updates dialogue manager with all npcs in range

				}
			}
		}
	}

	public float GetVerticalAccel(){
		return verticalAcceleration;
	}

	public void SetOrbCount(float newOrbCount) {
		orbCount = newOrbCount;
		windPowerBar.fillAmount = orbCount / orbMax;
		orbCountText.text = orbCount.ToString() + "/" + orbMax;
		//Assuming that the passed in newOrbCount is 0, which it should be
		verticalAcceleration = 0.001f;
		speed = startingSpeed;
		originalSpeed = startingSpeed;
		treeSpeed = treeSlow * startingSpeed;
		Debug.Log ("OrbCount = " + orbCount);
		Debug.Log ("Speed is back to :" + speed);
	}

	public float GetOrbCount(){
		return orbCount;
	}
}
