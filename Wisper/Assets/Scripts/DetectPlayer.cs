using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour {

	public GameObject activationParticles;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Player> ().nearShrine) {
			activationParticles.SetActive (true);
		} else {
			activationParticles.SetActive (false);
		}
	}
}
