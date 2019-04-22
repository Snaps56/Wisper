using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreNPC : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
        player = PlayerPersistance.player.transform;

    }

    // Update is called once per frame
    void Update () {
		IgnoreCollision ();
	}

	void IgnoreCollision() {
		Physics.IgnoreCollision (player.GetComponent<Collider>(), this.GetComponent<CapsuleCollider>());
	}
}
