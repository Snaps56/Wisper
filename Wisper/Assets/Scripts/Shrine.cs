using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shrine : MonoBehaviour {

	// Particles to indicate player is near shrine
	public GameObject activationParticles;
	// Parts of the shrine that change material
	public GameObject shrineMeshMain;
	public GameObject shrineMeshInner;
	// Materials representing the clean shrine
	public Material cleanMaterialMain;
	public Material cleanMaterialInner;
	// Materials representing the dirty shrine
	private Material dirtyMaterialMain;
	private Material dirtyMaterialInner;

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
	private GameObject[] OrbDepositInstanceArray;

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

	private bool firstTime;
	private float playerOrb;
	private float orbLimit;

	// Use this for initialization
	void Start () {
		// Dirty shrine look is the default materials
		dirtyMaterialInner = shrineMeshInner.GetComponent<MeshRenderer> ().material;
		dirtyMaterialMain = shrineMeshMain.GetComponent<MeshRenderer> ().material;

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

		firstTime = false;
		playerOrb = player.GetComponent<OrbCount> ().GetOrbCount ();
		orbLimit = 10;
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
                playerOrb = player.GetComponent<OrbCount>().GetOrbCount();
				// Change the material of the shrine over time
				if (playerOrb >= orbLimit) {;
					if (gettingCleaned && cleanProgress < cleanThreshold * 0.02f) {
                        cleanProgress += cleanTick;
						shrineMeshInner.GetComponent<MeshRenderer> ().material.Lerp (dirtyMaterialInner, cleanMaterialInner, cleanProgress);
						shrineMeshMain.GetComponent<MeshRenderer> ().material.Lerp (dirtyMaterialMain, cleanMaterialMain, cleanProgress);
					}
					// Once clean, drop the orb rewards and signal the PersistantStateData to change
					if (cleanProgress >= cleanThreshold * 0.02f) {
                        //isClean = true;
                        persistantStateData.ChangeStateConditions ("ShrineIsClean", true);
                        persistantStateData.ChangeStateConditions("ShrineFirstTurnIn", true);
                        this.GetComponent<SpawnOrbs> ().DropOrbs ();
					}
				} else if (!firstTime && (bool)persistantStateData.stateConditions["WaitingForCleanAttempt"] && gettingCleaned) {
					firstTime = true;
                    Hashtable tmpHash = new Hashtable();
                    tmpHash.Add("ShrineFirstConversation2Primer", true);
					tmpHash.Add("WaitingForCleanAttempt", false);
                    persistantStateData.ChangeStateConditions(tmpHash);
				}
			}
			// If the user is near the shrine after cleaning it, they can press a button to deposit an orb
			if (/*(Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("XBOX_Button_X")) && */
				(bool)persistantStateData.stateConditions ["ShrineIsClean"] && (bool)persistantStateData.stateConditions["OrbDepositInProgress"]) {
                playerOrb = player.GetComponent<OrbCount>().GetOrbCount();
                // Make sure the player has orbs to deposit
                if (playerOrb > 0) {
					orbDepositsInTransit = playerOrb;
					// Function that changes orb count
					DepositOrbs (orbDepositsInTransit);

					// Create an instance of an orb specifically for the deposit sequence
					//orbDepositInstance = Instantiate (orbDeposit, player.transform.position + new Vector3 (0, 0f, 0), Quaternion.identity);
					//orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
				}
				if (orbDepositsInTransit == 0) {
					if ((bool)persistantStateData.stateConditions ["ShrineFirstTurnIn"]) {
						
					}
					//persistantStateData.stateConditions ["OrbDepositInProgress"] = false;
					persistantStateData.ChangeStateConditions ("OrbDepositInProgress", false);

					Debug.Log ("Deposit Complete");

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
		if ((bool)persistantStateData.stateConditions ["ShrineIsClean"])
        {
            shrineMeshInner.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialInner, cleanMaterialInner, cleanProgress);
            shrineMeshMain.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialMain, cleanMaterialMain, cleanProgress);
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
			Vector3 spawnPosition = Random.onUnitSphere * (1f ) + player.transform.position;
			orbDepositInstance = Instantiate (orbDeposit, spawnPosition, Quaternion.identity);
			//OrbDepositInstanceArray [oc] = orbDepositInstance;
			//orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
		}
	}

	// If an orb is being deposited, destroy it once it reaches the shrine
	/*
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("OrbDeposit")) {
			orbDepositsInTransit--;
			Destroy (other.gameObject);
		}
	}
	*/

	public void destroyOrbDeposit (GameObject orbDeposit) {
		Destroy (orbDeposit.gameObject);
		orbDepositsInTransit--;
	}
}