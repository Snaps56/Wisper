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
	public GameObject playerBlowing;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		isClean = false;
	}

	// Update is called once per frame
	void Update () {
		
		if (player.GetComponent<Player> ().nearShrine) {
			activationParticles.SetActive (true);
			//isClean = playerBlowing.isThrowingObjects;
		} else {
			activationParticles.SetActive (false);
		}

		if (isClean) {
			shrinePart1.GetComponent<MeshRenderer> ().material = cleanShrine1;
			shrinePart2.GetComponent<MeshRenderer> ().material = cleanShrine2;
		}
	}
}
