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
        PSDScript = PersistantStateData.persistantStateData;
        cleaningParticles = GetComponent<ParticleSystem>();
        shrine = GetComponentInParent<Shrine>();
        isCleaning = shrine.gettingCleaned;
        if((bool)PSDScript.stateConditions["ShrineIsClean"])
        {
            hasCleaned = true;
        }
        else
        {
            hasCleaned = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {   
        isCleaning = shrine.gettingCleaned;
        //Debug.Log("is cleaning: " + isCleaning + ", hasCleaned : " + hasCleaned);
        if (!hasCleaned && isCleaning)
        {
            hasCleaned = (bool)PSDScript.stateConditions["ShrineIsClean"];
            if (!cleaningParticles.isPlaying)
             {
                cleaningParticles.Play();
            }
            //Debug.Log("I'm cleaning!");
        }
        else if (cleaningParticles.isPlaying)
        {
            cleaningParticles.Stop();
        }
	}
}
