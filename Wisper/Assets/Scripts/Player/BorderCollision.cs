using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCollision : MonoBehaviour {
    private int treeCount = 0;
    private float shake;
    private float speed;
    public GameObject turnBackText;
    private float treeSpeed;
	private float treeSlow = 0.7f;
    private Vector3 positionStamp;
    private PlayerMovement playermovementScript;
    private float originalSpeed;
    [Header("Collision Handeling")]
    private float shakeAmount;

    // Use this for initialization
    void Start () {
        turnBackText.SetActive(false);
        shakeAmount = 0.05f;
        shake = 0;
        speed = GetComponent<Rigidbody>().velocity.magnitude;
        treeSpeed = treeSlow * speed;

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
        if (other.gameObject.CompareTag("Tree"))
        {
            speed = treeSpeed;
            treeCount++;
            Debug.Log("Speed is reduced to :" + speed);
        }
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

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree"))
        {
            treeCount--;
            if (treeCount == 0)
            {
                speed = originalSpeed;
                Debug.Log("Speed is back to :" + speed);
            }
        }
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
