using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XInputDotNetPure;

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

    // Vibration variables
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

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
    public GameObject orbExplode;
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
    public GameObject directionalLight1;
    public GameObject directionalLight2;
    public GameObject windPowerUI;
    public AudioSource lighting;
    public AudioSource rainSound;

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
    private bool Triggered = false;


    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        // Dirty shrine look is the default materials
        dirtyMaterialInner = shrineMeshInner.GetComponent<MeshRenderer> ().material;
		dirtyMaterialMain = shrineMeshMain.GetComponent<MeshRenderer> ().material;

		// Initialize clean progress to false
		gettingCleaned = false;
		
        if(!(bool)persistantStateData.stateConditions["ShrineIsClean"])
        {
            // Initialize clean progress to 0
            cleanProgress = 0;
            firstTime = false;
        }
		else
        {
            cleanProgress = cleanThreshold * 0.02f;
            firstTime = true;
            shrineMeshInner.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialInner, cleanMaterialInner, cleanProgress);
            shrineMeshMain.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialMain, cleanMaterialMain, cleanProgress);
        }

		// Gather the activation particles and put them in an array
		coloredParticles = activationParticles.GetComponentsInChildren<ParticleSystem> ();

		

		nextDeposit = 0.0f;
		orbDepositsInTransit = 0;

		
		playerOrb = player.GetComponentInChildren<OrbCount> ().GetOrbCount ();
        playerMovement = player.GetComponent<PlayerMovement>();
		orbLimit = 10;
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    // Update is called once per frame
    void Update () {

        Debug.Log("Shrine is Clean: " + (bool)persistantStateData.stateConditions["ShrineIsClean"] + ", Orb Deposit In Progress: " + (bool)persistantStateData.stateConditions["OrbDepositInProgress"]);
        // Check that the player is near the shrine
        //if (player.GetComponentInChildren<InteractableCollision>().withinDialogueRange) {
        if (true) {
			// If the user is near the shrine after cleaning it, they can press a button to deposit an orb
			if (/*(Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("XBOX_Button_X")) && */
				(bool)persistantStateData.stateConditions ["ShrineIsClean"] && (bool)persistantStateData.stateConditions["OrbDepositInProgress"]) {
                playerOrb = player.GetComponentInChildren<OrbCount>().GetOrbCount();
                Debug.Log("Shrine is clean and orb deposit should be in progress, therefore deposit!");
                // Make sure the player has orbs to deposit
                if (playerOrb > 0) {
                    Debug.Log("Player has orbs to deposit");
					orbDepositsInTransit = playerOrb;
					// Function that changes orb count
					DepositOrbs (orbDepositsInTransit);

					// Create an instance of an orb specifically for the deposit sequence
					//orbDepositInstance = Instantiate (orbDeposit, player.transform.position + new Vector3 (0, 0f, 0), Quaternion.identity);
					//orbDepositInstance.GetComponent<OrbSequence> ().setDestination (this.gameObject, "shrine");
				}
				if (orbDepositsInTransit == 0) {
					
					persistantStateData.ChangeStateConditions ("OrbDepositInProgress", false);
                    playerMovement.RemoveFollowTarget();

					Debug.Log ("Deposit Complete");
                    lighting.Play();
                    rainSound.Play();
                    Debug.Log("Triggered: " + Triggered);
                    if (!Triggered )
                    {
                        Debug.Log("Triggered: " + Triggered);
                        Debug.Log("WE'RE IN");
                        // After depositing orbs, play a cutscene of the storm
                        Debug.Log("Shrine script disable player movement");
                        player.GetComponent<PlayerMovement>().DisableMovement();
                        //Deactivate main camera
                        mainCamera.gameObject.SetActive(false);
                        //Activate Cutscene Camera
                        cutsceneCamera.gameObject.SetActive(true);
                        //Find the UI Element for the Wind Power
                        windPowerUI.SetActive(false);
                        //Activate the rain particle system
                        rain.SetActive(true);
                        //Change the directional light to be dimmer
                        /*
                        directionalLight1.GetComponent<Light>().color = Color.black;
                        directionalLight2.GetComponent<Light>().color = Color.black;
                        */
                        //Play the animation for the camera
                        cutsceneCamera.GetComponent<Animation>().Play("Deposit");
                        Triggered = true;
                    }

                }
            }
        }
        // Tethers player to shrine during orb turn in so that the turn in process can be completed.
        if ((bool)persistantStateData.stateConditions["OrbDepositInProgress"])
        {
            playerMovement.SetFollowTarget(gameObject, 3, 5);
        }
        /*
        if (Input.GetKey(KeyCode.M))
        {
            PersistantStateData.persistantStateData.ChangeStateConditions("DemoEnd", true);
        }
        */
    }

    // todo move this up!!!!!!!
    private float totalOrbsDeposited;

	// Function to basically decrement player orb count
	void DepositOrbs (float depositCount) {
        // have a private local variable that tracks total orbs deposited, set PSD variable equal to this variable
        totalOrbsDeposited += player.GetComponentInChildren<OrbCount>().GetOrbCount();

        PersistantStateData.persistantStateData.ChangeStateConditions("TotalOrbsDeposited", totalOrbsDeposited);

        // check if total orbs deposited is greather or equal to total orb count
        if ((float) PersistantStateData.persistantStateData.stateConditions["TotalOrbsDeposited"] >= (float)PersistantStateData.persistantStateData.stateConditions["TotalOrbCount"])
        {
            PersistantStateData.persistantStateData.ChangeStateConditions("AllTasksDone", true);
        }

        player.GetComponentInChildren<OrbCount> ().SetOrbCount (0);
        Debug.Log("Orb getting deposited");
        PersistantStateData.persistantStateData.ChangeStateConditions("HasNoOrbs", true);
        for (int oc = 0; oc < depositCount; oc++)
        {
			orbDepositInstance = Instantiate (orbDeposit, player.transform.position, Quaternion.identity);	
		}
	}
    public void destroyOrbDeposit (GameObject orbDeposit) {
        Debug.Log("Orb getting destroyed");
        Instantiate(orbExplode, orbDeposit.transform.position, Quaternion.identity);
        Destroy(orbDeposit.gameObject);
		orbDepositsInTransit--;
	}
}