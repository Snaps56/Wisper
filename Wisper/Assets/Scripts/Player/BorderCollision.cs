using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCollision : MonoBehaviour {


    //Passthrough stuff
    private float passThrough;
	private float slowAmount = 0.9f;
    private int objectCount = 0;
    private float originalSpeed;
    private Vector3 velocityStamp;


    //Border Stuff
    private float shake;
    private Vector3 positionStamp;
    private float shakeAmount;
    public GameObject turnBackText;

    // Use this for initialization
    void Start () {
        turnBackText.SetActive(false);
        shakeAmount = 0.05f;
        shake = 0;
        passThrough = slowAmount;

    }

    // Update is called once per frame
    void Update () {
    }

    private void OnTriggerEnter(Collider other)
    {
        //Detects the border
        if (other.gameObject.CompareTag("Border"))
        {
            //Stores the position of the object when the player enters the border
            positionStamp = this.transform.position;
            //Turns on "Turn Back" UI element
            turnBackText.SetActive(true);
        }

        //Detects the border teleporter
        if (other.gameObject.CompareTag("BorderTele"))
        {
            //Turns the player velocity back to zero so they don't move
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            //Returns the player back to the location at which they entered the collision box
            this.transform.position = positionStamp;
        }

        //if (!other.gameObject.CompareTag("Terrain") && !other.gameObject.CompareTag("Pickup"))
        //{
        //    velocityStamp = this.transform.position;
        //}

    }

    //Trigger function activated while collision is being made
    void OnTriggerStay(Collider other)
    {
        shake = 1;
        //Activates when the player enters the border
        if (other.gameObject.CompareTag("Border"))
        {
            //Activate shake
            if (shake > 0)
            {
                this.transform.position = this.transform.position + Random.insideUnitSphere * shakeAmount;
            }
            //Reduce shake
            else
            {
                shake -= Time.deltaTime * 0.1f;
            }
        }
        //if (!other.gameObject.CompareTag("Terrain") && !other.gameObject.CompareTag("Pickup"))
        //{
        //    Debug.Log("Original Speed: " + GetComponent<Rigidbody>().velocity.magnitude);
        //    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity * passThrough;
        //    objectCount++;
        //    Debug.Log("Speed is reduced to :" + GetComponent<Rigidbody>().velocity);
        //}

    }

    private void OnTriggerExit(Collider other)
    {
        //if (!other.gameObject.CompareTag("Terrain") && !other.gameObject.CompareTag("Pickup"))
        //{
        //    objectCount--;
        //    if (objectCount == 0)
        //    {
        //        this.transform.position = positionStamp;
        //        Debug.Log("Speed is back to :" + GetComponent<Rigidbody>().velocity);
        //    }
        //}
        //Activates when the player is no longer touching the border
        if (other.gameObject.CompareTag("Border"))
        {
            //Turns off the turn back UI Text
            turnBackText.SetActive(false);
            //Turns off shake completly
            shake = 0;
        }
    }
}
