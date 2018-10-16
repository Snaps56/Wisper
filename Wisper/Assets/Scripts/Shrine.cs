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
	}

	// Update is called once per frame
	void Update () {
		
		if (player.GetComponent<Player> ().nearShrine) {
			activationParticles.SetActive (true);
			gettingCleaned = playerAbilities.GetComponent<ObjectSwirl>().isSwirling;
			if (gettingCleaned && cleanProgress < 1.0f) {
				cleanProgress += 0.1f;
				shrinePart1.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine1, cleanShrine1, cleanProgress);
				shrinePart2.GetComponent<MeshRenderer> ().material.Lerp (dirtyShrine2, cleanShrine2, cleanProgress);
			}
			if (cleanProgress >= 1.0f) {
				isClean = playerAbilities.GetComponent<ObjectSwirl>().isSwirling;
			}
		} else {
			activationParticles.SetActive (false);
		}

		if (isClean) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}
}
