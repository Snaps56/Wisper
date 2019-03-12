using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleToggle : MonoBehaviour {
    public GameObject swirlParticles;
    public GameObject rasenganParticles;

    bool doSwirlParticles = true;
	// Use this for initialization
	void Start ()
    {
        if (doSwirlParticles)
        {
            swirlParticles.SetActive(true);
            rasenganParticles.SetActive(false);
        }
        else
        {
            swirlParticles.SetActive(false);
            rasenganParticles.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (doSwirlParticles)
            {
                doSwirlParticles = false;
                swirlParticles.SetActive(true);
                rasenganParticles.SetActive(false);
            }
            else
            {
                doSwirlParticles = true;
                swirlParticles.SetActive(false);
                rasenganParticles.SetActive(true);
            }
        }
	}
}
