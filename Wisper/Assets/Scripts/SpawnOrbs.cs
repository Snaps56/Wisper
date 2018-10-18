using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbs : MonoBehaviour
{
    public GameObject orb;
    public float orbCount;
    private float launchSpeed = .1f;
    private Rigidbody rb;
    private bool buttonPressed = false;
    private float timePassed, timeBetweenSteps = 1f;


    // Use this for initialization
    void Start()
    {
        rb = orb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && buttonPressed == false)
        {
            Vector3 spawnPosition = Random.onUnitSphere * (1f ) + transform.position;
            //buttonPressed = true;
            //for (int i = 0; i < orbCount; i++)
            //{
                GameObject orbInstance = Instantiate(orb, spawnPosition, Quaternion.identity);
                orbInstance.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 5);
            //}
        }
        if (timePassed >= timeBetweenSteps)
        {
            timePassed = 0f;
            buttonPressed = false;
        }
    }

	public void DropOrbs (float orbs){
		for (int i = 0; i < orbs; i++) {
			Vector3 spawnPosition = Random.onUnitSphere * (1f ) + transform.position;
			GameObject orbInstance = Instantiate(orb, spawnPosition, Quaternion.identity);
			orbInstance.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 5);
		}
	}
}
