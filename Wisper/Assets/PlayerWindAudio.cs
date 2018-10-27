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
            if (1 - 1 / playerrigidbody.velocity.magnitude > 0.5)
            {
                source.volume = (1 - 1 / playerrigidbody.velocity.magnitude) * 0.5f;
                source.pitch = (1 - 1 / playerrigidbody.velocity.magnitude) * 2;
            }
        }
        else
        {
            source.volume = 0.25f;
            source.pitch = 1f;
        }
	}
}
