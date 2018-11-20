using System.Collections;
using System.Collections.Generic;
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

    [Header("Cutscene Objects")]
	// Camera that is used for cutscene
    public Camera cutsceneCamera;
	// Primary player camera
    public Camera mainCamera;
	// Rain for the cutscene
    public GameObject rain;
	// Lightning for the cutscene
    public GameObject light;

    [Header("Clean Stuff")]
	// Condition for when the shrine is in the process of being cleaned
	public bool gettingCleaned;
	// Numerical representation for shrine clean process
	public float cleanProgress;

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
			if (!(bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
				// Change the material of the shrine over time
				if (gettingCleaned && cleanProgress < 1.0f) {
					cleanProgress += 0.1f;
					shrinePart1.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine1, cleanShrine1, cleanProgress);
					shrinePart2.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine2, cleanShrine2, cleanProgress);
				}
				// Once clean, drop the orb rewards and signal the PersistantStateData to change
				if (cleanProgress >= 1.0f) {
					//isClean = true;
					GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"] = true;
					GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().updateCount++;
					this.GetComponent<SpawnOrbs> ().DropOrbs ();
				}
			}
			// If the user is near the shrine after cleaning it, they can press a button to deposit an orb
			if (Input.GetKeyDown (KeyCode.L) && (bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
				// Make sure the player has orbs to deposit
				if (player.GetComponent<OrbCount> ().GetOrbCount () > 0) {
					// Function that changes orb count
					DepositOrbs ();
					// Create an instance of an orb specifically for the deposit sequence
					orbDepositInstance = Instantiate (orbDeposit, player.transform.position + new Vector3 (0, 0f, 0), Quaternion.identity);
					orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
				}
				// After depositing orbs, play a cutscene of the storm
                /*
                if (cutsceneCamera.gameObject.activeSelf == false)
                {
                    mainCamera.gameObject.SetActive(false);
                    cutsceneCamera.gameObject.SetActive(true);
                    GameObject.Find("WindPowerBG").SetActive(false);
                    rain.SetActive(true);
                    light.GetComponent<Light>().color = Color.black;
                    cutsceneCamera.GetComponent<Animation>().Play();
                    //GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
                    if (!cutsceneCamera.GetComponent<Animation>().isPlaying)
                    {
                        mainCamera.gameObject.SetActive(true);
                        cutsceneCamera.gameObject.SetActive(false);
                    }
                }
                */
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
		if ((bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}

	// Function to basically decrement player orb count
	void DepositOrbs () {
		if (player.GetComponent<OrbCount> ().GetOrbCount () > 0) {
			player.GetComponent<OrbCount> ().SetOrbCount (player.GetComponent<OrbCount> ().GetOrbCount () - 1);
		}
	}

	// If an orb is being deposited, destroy it once it reaches the shrine
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("OrbDeposit")) {
			Destroy (other.gameObject);
		}
	}
}