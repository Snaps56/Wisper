using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteTask : MonoBehaviour {

    
    private bool getOrbs;

    private Vector3 finalPosition;
    private Vector3 kitePos;

    private float finalX;
    private float finalY;
    private float finalZ;

    private float x1;
    private float x2;
    private float z1;
    private float z2;
    private float maxHeight;

    // Use this for initialization
    void Start () {
        maxHeight = 50.0f;
        getOrbs = true;
        finalX = -2.31f;
        finalY = 51.0f;
        finalZ = -84.21f;
        finalPosition = new Vector3(finalX, finalY, finalZ);
    }
	
    void FlyKite()
    {
        
        if (transform.position.y >= finalPosition.y && transform.position.x >= finalPosition.x && transform.position.z >= finalPosition.z)
        {
            transform.position = finalPosition;
            GetComponent<SpawnOrbs>().DropOrbs();
            getOrbs = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Returned")
        {
            Debug.Log("Touched");
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
