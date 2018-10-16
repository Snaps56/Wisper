using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour {

	public GameObject activationParticles;
	public GameObject shrinePart1;
	public GameObject shrinePart2;
	public Material cleanShrine1;
	public Material cleanShrine2;

	private GameObject player;
	private bool isClean;
	public GameObject playerAbilities;

	private bool gettingBlown;
	private double blowProgress;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		gettingBlown = false;
		isClean = false;
		blowProgress = 0;
	}

	// Update is called once per frame
	void Update () {
		
		if (player.GetComponent<Player> ().nearShrine) {
			activationParticles.SetActive (true);
			gettingBlown = playerAbilities.GetComponent<ObjectThrow>().isThrowingObjects;
			if (gettingBlown) {
				blowProgress += 20;
			}
			if (blowProgress >= 100) {
				isClean = playerAbilities.GetComponent<ObjectThrow>().isThrowingObjects;
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
