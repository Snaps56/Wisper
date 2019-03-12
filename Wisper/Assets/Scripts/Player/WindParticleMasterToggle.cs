using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticleMasterToggle : MonoBehaviour {
    public GameObject type1;
    public GameObject type2;
    public bool doType1 = true;
    
	// Use this for initialization
	void Start ()
    {
        if (doType1)
        {
            type1.SetActive(true);
            type2.SetActive(false);
        }
        else
        {
            type1.SetActive(false);
            type2.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (doType1)
            {
                doType1 = false;
                type1.SetActive(true);
                type2.SetActive(false);
            }
            else
            {
                doType1 = true;
                type1.SetActive(false);
                type2.SetActive(true);
            }
        }
        
	}
}
