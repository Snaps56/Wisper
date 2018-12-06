using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shrine : MonoBehaviour {

	// Particles to indicate player is near shrine
	public GameObject activationParticles;
	// Parts of the shrine that change material
	public GameObject shrinePart1;
	public GameObject shrinePart2;
	// Materials representing the clean shrine
	public Material cleanShrine1;
	public Material cleanShrine2;
	// Materials representing the dirty shrine
	private Material dirtyShrine1;
	private Material dirtyShrine2;

	// Player object to check if near
	public GameObject player;
	// Shrine clean condition
	private bool isClean;
	// Generated orbs for player collection and deposit
	private GameObject orbInstance;
	private GameObject orbDepositInstance;
	// The bases of the orbs
	public GameObject orb;
	public GameObject orbDeposit;
	// Array holding the different particles that play when player is near shrine
	public Component[] coloredParticles;
	// Here to check if player is using a certain ability
	public GameObject playerAbilities;

    private PlayerMovement playerMovement;

	private PersistantStateData persistantStateData;

	public float depositRate;
	private float nextDeposit;
	private float orbDepositsInTransit;

    [Header("Cutscene Objects")]
	// Camera that is used for cutscene
    public Camera cutsceneCamera;
	// Primary player camera
    public Camera mainCamera;
	// Rain for the cutscene
    public GameObject rain;
	// Lightning for the cutscene
    public GameObject light;
    public GameObject windPowerUI;

    [Header("Clean Stuff")]
	// Condition for when the shrine is in the process of being cleaned
	public bool gettingCleaned;
	// Numerical representation for shrine clean process
	public float cleanProgress;
	public float cleanThreshold;
	public float cleanTick;

	// Use this for initialization
	void Start () {
		// Dirty shrine look is the default materials
		dirtyShrine1 = shrinePart1.GetComponent<MeshRenderer> ().material;
		dirtyShrine2 = shrinePart2.GetComponent<MeshRenderer> ().material;

		// Initialize clean progress to false
		gettingCleaned = false;
		//isClean = false;
		//isClean = GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"];

		// Initialize clean progress to 0
		cleanProgress = 0;

		// Gather the activation particles and put them in an array
		coloredParticles = activationParticles.GetComponentsInChildren<ParticleSystem> ();

		persistantStateData = GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ();

		nextDeposit = 0.0f;
		orbDepositsInTransit = 0;
	}

	// Update is called once per frame
	void Update () {

		// Check that the player is near the shrine
		if (player.GetComponentInChildren<InteractableCollision>().nearShrine) {
			
			// Start playing the particles
			foreach (ParticleSystem partPlay in coloredParticles) {
				//Debug.Log (partPlay.name);
				if(!partPlay.isPlaying) {
					partPlay.Play ();
				}
			}
			// Check that the player is holding down the lift(clean) button
			gettingCleaned = playerAbilities.GetComponent<ObjectLift>().GetIsLiftingObjects();
			// Is the shrine is not clean and the player is lifting, start cleaning the shrine
			if (!(bool)persistantStateData.stateConditions ["ShrineIsClean"]) {
				// Change the material of the shrine over time
				if (gettingCleaned && cleanProgress < cleanThreshold) {
					cleanProgress += cleanTick;
					shrinePart1.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine1, cleanShrine1, cleanProgress);
					//shrinePart2.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine2, cleanShrine2, cleanProgress);
				}
				// Once clean, drop the orb rewards and signal the PersistantStateData to change
				if (cleanProgress >= cleanThreshold) {
					//isClean = true;
					persistantStateData.stateConditions ["ShrineIsClean"] = true;
					persistantStateData.updateCount++;
					this.GetComponent<SpawnOrbs> ().DropOrbs ();
				}
			}
			// If the user is near the shrine after cleaning it, they can press a button to deposit an orb
			if ((Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("XBOX_Button_X")) 
				&& (bool)persistantStateData.stateConditions ["ShrineIsClean"] /*&& (bool)persistantStateData.stateConditions["OrbDepositInProgress"]*/) {
				// Make sure the player has orbs to deposit
				if (player.GetComponent<OrbCount> ().GetOrbCount () > 0) {
					orbDepositsInTransit = player.GetComponent<OrbCount> ().GetOrbCount ();
					// Function that changes orb count
					DepositOrbs (orbDepositsInTransit);

					// Create an instance of an orb specifically for the deposit sequence
					//orbDepositInstance = Instantiate (orbDeposit, player.transform.position + new Vector3 (0, 0f, 0), Quaternion.identity);
					//orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
				}
				if (orbDepositsInTransit == 0) {
					//persistantStateData.stateConditions ["OrbDepositInProgress"] = false;

					// After depositing orbs, play a cutscene of the storm
					/*
                	player.GetComponent<PlayerMovement>().ToggleMovement();
                	//Deactivate main camera
                	mainCamera.gameObject.SetActive(false);
                	//Activate Cutscene Camera
                	cutsceneCamera.gameObject.SetActive(true);
                	//Find the UI Element for the Wind Power
                	windPowerUI.SetActive(false);
                	//Activate the rain particle system
                	rain.SetActive(true);
                	//Change the directional light to be dimmer
                	light.GetComponent<Light>().color = Color.black;
                	//Play the animation for the camera
                	cutsceneCamera.GetComponent<Animation>().Play("Deposit");
                	*/
				}
            }
		// If the player is not near the shrine, don't play particles
		} else {
			foreach (ParticleSystem partStop in coloredParticles) {
				if(partStop.isPlaying) {
					partStop.Stop ();
				}
			}
		}

		// If the player is returning to the shrine after cleaning it, set the shrine materials to the clean ones
		if ((bool)persistantStateData.stateConditions ["ShrineIsClean"]) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			//shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}

	// Function to basically decrement player orb count
	void DepositOrbs (float depositCount) {
		/*
		while (player.GetComponent<OrbCount> ().GetOrbCount () > 0) {
			player.GetComponent<OrbCount> ().SetOrbCount (player.GetComponent<OrbCount> ().GetOrbCount () - 1);
			// Create an instance of an orb specifically for the deposit sequence
			orbDepositInstance = Instantiate (orbDeposit, player.transform.position + new Vector3 (0, 0f, 0), Quaternion.identity);
			orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
		}
		*/
		player.GetComponent<OrbCount> ().SetOrbCount (0);
		for (int oc = 0; oc < depositCount; oc++) {
			//if (Time.time > nextDeposit) {
				// Create an instance of an orb specifically for the deposit sequence
			Vector3 spawnPosition = Random.onUnitSphere * (1f ) + player.transform.position;
			orbDepositInstance = Instantiate (orbDeposit, spawnPosition, Quaternion.identity);
			orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
			//orbDepositInstance.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 5);
				//Thread.Sleep (2000);
			//}
		}
	}

	// If an orb is being deposited, destroy it once it reaches the shrine
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("OrbDeposit")) {
			orbDepositsInTransit--;
			Destroy (other.gameObject);
		}
	}
}