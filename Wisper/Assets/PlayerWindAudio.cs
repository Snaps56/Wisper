using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWindAudio : MonoBehaviour {

    public Rigidbody playerrigidbody;
    public AudioSource source;
    private float finalVelocity;

	// Use this for initialization
	void Start () {
        source.volume = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerrigidbody.velocity.magnitude > 1)
        {
            source.volume = 1 - 1 / playerrigidbody.velocity.magnitude;
            source.pitch = 1 - 1 / playerrigidbody.velocity.magnitude;
        }
        else
        {
            Debug.Log("idle");
            source.volume = 0.5f;
            source.pitch = 0.5f;
        }
	}
}
