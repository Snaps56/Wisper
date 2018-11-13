using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour {

	public GameObject activationParticles;
	public GameObject shrinePart1;
	public GameObject shrinePart2;
	public Material cleanShrine1;
	public Material cleanShrine2;
	private Material dirtyShrine1;
	private Material dirtyShrine2;

	public GameObject player;
	private bool isClean;
	private GameObject orbInstance;
	public GameObject orb;
	public Component[] coloredParticles;
	public GameObject playerAbilities;

    [Header("Cutscene Objects")]
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public GameObject rain;
    public GameObject light;

    [Header("Clean Stuff")]
	public bool gettingCleaned;
	public float cleanProgress;

	// Use this for initialization
	void Start () {
		dirtyShrine1 = shrinePart1.GetComponent<MeshRenderer> ().material;
		dirtyShrine2 = shrinePart2.GetComponent<MeshRenderer> ().material;
		gettingCleaned = false;
		//isClean = false;
		//isClean = GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"];
		cleanProgress = 0;
		coloredParticles = activationParticles.GetComponentsInChildren<ParticleSystem> ();
	}

	// Update is called once per frame
	void Update () {

		if (player.GetComponentInChildren<PlayerCollision>().nearShrine) {
			//activationParticles.SetActive (true);
			//transform.Find("Blue Particles").gameObject.GetComponent<ParticleSystem>().Play();
			foreach (ParticleSystem partPlay in coloredParticles) {
				//Debug.Log (partPlay.name);
				if(!partPlay.isPlaying) {
					partPlay.Play ();
				}
			}
			//coloredParticles.Play(true);
			gettingCleaned = playerAbilities.GetComponent<ObjectLift>().GetIsLiftingObjects();
			if (!(bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
				if (gettingCleaned && cleanProgress < 1.0f) {
					cleanProgress += 0.1f;
					shrinePart1.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine1, cleanShrine1, cleanProgress);
					shrinePart2.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine2, cleanShrine2, cleanProgress);
				}
				if (cleanProgress >= 1.0f) {
					//isClean = true;
					GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"] = true;
					GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().updateCount++;
					this.GetComponent<SpawnOrbs> ().DropOrbs ();
				}
			}
			if (Input.GetKeyDown (KeyCode.L) && (bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
				DepositOrbs();
				orbInstance = Instantiate(orb, player.transform.position + new Vector3 (0, 2f, 0), Quaternion.identity);
				orbInstance.GetComponent<OrbSequence>().setDestination(this.gameObject, "shrine");
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
		} else {
			//activationParticles.SetActive (false);
			foreach (ParticleSystem partStop in coloredParticles) {
				if(partStop.isPlaying) {
					partStop.Stop ();
				}
			}
			//coloredParticles.Stop(true);
		}

		if ((bool)GameObject.Find ("PersistantStateData").GetComponent<PersistantStateData> ().stateConditions ["ShrineIsClean"]) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}

	void DepositOrbs () {
		player.GetComponent<PlayerCollision> ().SetOrbCount (0);
	}
}