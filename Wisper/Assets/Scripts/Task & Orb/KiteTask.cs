using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteTask : MonoBehaviour {

    
    private bool getOrbs;
    private bool changed;

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
        maxHeight = 41.0f;
        getOrbs = true;
        changed = false;

        x1 = 9.31f;
        x2 = -16.1f;
        z1 = -83.5f;
        z2 = -104.3f;

        finalX = -2.31f;
        finalY = 43.0f;
        finalZ = -84.21f;
        
        
    }
	
    void FlyKite()
    {
        
        if (transform.position.y >= maxHeight && transform.position.x <= x1 && transform.position.x >= x2 &&
            transform.position.z <= z1 && transform.position.z >= z2)
        {
            if(changed == false)
            {
                finalPosition = new Vector3(this.transform.position.x, this.transform.position.y + 2.0f, this.transform.position.z);
                changed = true;
            }
            transform.position = finalPosition;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //transform.Rotate(-48.0f,0, 0, Space.Self);

            if (getOrbs == true)
            {
                
                GetComponent<SpawnOrbs>().DropOrbs();
                getOrbs = false;
            }               
        }
        
    }
    /*
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
*/
    // Update is called once per frame
    void Update () {
        FlyKite();
	}
}
