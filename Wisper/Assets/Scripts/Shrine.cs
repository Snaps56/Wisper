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

	private GameObject player;
	private bool isClean;
	public Component[] coloredParticles;
	public GameObject playerAbilities;

	[Header("Clean Stuff")]
	public bool gettingCleaned;
	public float cleanProgress;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		dirtyShrine1 = shrinePart1.GetComponent<MeshRenderer> ().material;
		dirtyShrine2 = shrinePart2.GetComponent<MeshRenderer> ().material;
		gettingCleaned = false;
		isClean = false;
		cleanProgress = 0;
		coloredParticles = activationParticles.GetComponentsInChildren<ParticleSystem> ();
		//foreach (ParticleSystem part in coloredParticles) {
		//	part.Stop (true);
		//}
		//coloredParticles.Stop (true);
	}

	// Update is called once per frame
	void Update () {

		if (player.GetComponent<PlayerCollision> ().nearShrine) {
			//activationParticles.SetActive (true);
			//transform.Find("Blue Particles").gameObject.GetComponent<ParticleSystem>().Play();
			foreach (ParticleSystem partPlay in coloredParticles) {
				//Debug.Log (partPlay.name);
				if(!partPlay.isPlaying) {
					partPlay.Play ();
				}
			}
			//coloredParticles.Play(true);
			gettingCleaned = playerAbilities.GetComponent<ObjectSwirl>().isSwirling;
			if (!isClean) {
				if (gettingCleaned && cleanProgress < 1.0f) {
					cleanProgress += 0.1f;
					shrinePart1.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine1, cleanShrine1, cleanProgress);
					shrinePart2.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine2, cleanShrine2, cleanProgress);
				}
				if (cleanProgress >= 1.0f) {
					isClean = true;
					this.GetComponent<SpawnOrbs> ().DropOrbs ();
				}
			}
			if (Input.GetKeyDown (KeyCode.L) && isClean) {
				DepositOrbs();
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

		if (isClean) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}

	void DepositOrbs () {
		player.GetComponent<PlayerCollision> ().SetOrbCount (0);
	}
}