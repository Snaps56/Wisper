using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleToggle : MonoBehaviour {
    public ParticleSystem swirlParticles;
    public ParticleSystem rasenganParticles;

    bool doSwirlParticles = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (doSwirlParticles)
            {
                doSwirlParticles = false;
            }
            else
            {
                doSwirlParticles = true;
            }
        }

		if (doSwirlParticles)
        {
            if (swirlParticles.isStopped)
            {
                swirlParticles.Play();
            }
            if (rasenganParticles.isPlaying)
            {
                rasenganParticles.Stop();
            }
        }
        else
        {
            if (rasenganParticles.isStopped)
            {
                rasenganParticles.Play();
            }
            if (swirlParticles.isPlaying)
            {
                swirlParticles.Stop();
            }
        }
	}
}
