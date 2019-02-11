using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTask : MonoBehaviour {

    
    public GameObject finalPlace;
    private Vector3 returned;

    private bool getOrbs;
    private bool pickedUp;
    private bool letGo;
    private float height;
    // Use this for initialization
    void Start()
    {
        pickedUp = false;
        getOrbs = true;
        letGo = false;
        height = 0.2f;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == finalPlace.tag)
        {
            pickedUp = true;
        }
    }

    void PickUp()
    {
        returned = new Vector3(finalPlace.transform.position.x, finalPlace.transform.position.y + height, finalPlace.transform.position.z);
        transform.position = returned;
        
        if (transform.position == returned && getOrbs == true)
        {
            GetComponent<SpawnOrbs>().DropOrbs();
            getOrbs = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp == true)
        {
            PickUp();           
        }
              
    }
}
