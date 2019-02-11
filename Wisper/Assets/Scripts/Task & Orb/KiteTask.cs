using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteTask : MonoBehaviour {

    private float maxHeight;
    private bool getOrbs;
    private Vector3 finalPosition;
    private float finalX;
    private float finalY;
    private float finalZ;

	// Use this for initialization
	void Start () {
        maxHeight = 50.0f;
        getOrbs = true;
        finalX = 63.0f;
        finalY = 51.0f;
        finalZ = -2.0f;
        finalPosition = new Vector3(finalX, finalY, finalZ);
    }
	
    void FlyKite()
    {
        
        if (transform.position.y >= maxHeight && getOrbs == true)
        {
            transform.position = finalPosition;
            GetComponent<SpawnOrbs>().DropOrbs();
            getOrbs = false;
        }
        
    }

	// Update is called once per frame
	void Update () {
        FlyKite();
	}
}
