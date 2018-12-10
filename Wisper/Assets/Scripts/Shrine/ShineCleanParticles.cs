using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineCleanParticles : MonoBehaviour {

    private bool isCleaning;
    private bool hasCleaned;
    private PersistantStateData PSDScript;
    private Shrine shrine;
    private ParticleSystem cleaningParticles;

    // Use this for initialization
    void Start ()
    {
        PSDScript = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        cleaningParticles = GetComponent<ParticleSystem>();
        shrine = GetComponentInParent<Shrine>();
        isCleaning = shrine.gettingCleaned;
        hasCleaned = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        hasCleaned = (bool)PSDScript.stateConditions["ShrineIsClean"];
        isCleaning = shrine.gettingCleaned;
        //Debug.Log("is cleaning: " + isCleaning + ", hasCleaned : " + hasCleaned);
        if (!hasCleaned && isCleaning)
        {
            if(!cleaningParticles.isPlaying)
             {
                cleaningParticles.Play();
            }
            //Debug.Log("I'm cleaning!");
        }
        else
        {
            if (cleaningParticles.isPlaying)
            {
                cleaningParticles.Stop();
            }
        }
	}
}
