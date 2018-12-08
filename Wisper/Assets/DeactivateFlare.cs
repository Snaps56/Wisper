﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateFlare : MonoBehaviour {

    public LensFlare lensFlare;
    public GameObject player;
    private PersistantStateData PSDScript;

	// Use this for initialization
	void Start () {
        PSDScript = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
	}
	
	// Update is called once per frame
	void Update () {
        lensFlare.brightness = Vector3.Distance(lensFlare.transform.position, player.transform.position) / 50;
        Debug.Log("Distance: " + Vector3.Distance(lensFlare.transform.position, player.transform.position));
        Debug.Log("Brightness: " + Vector3.Distance(lensFlare.transform.position, player.transform.position) / 50);
        if ((bool)PSDScript.stateConditions["ShrineFirstConversation"])
        {
            //Turn off the flare
            this.gameObject.SetActive(false);
        }
	}
}
