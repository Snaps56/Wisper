using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbs : MonoBehaviour
{
    public GameObject orb;
    public float orbCount;
	public GameObject player;

    private float launchSpeed = .1f;
    private Rigidbody rb;
    private bool buttonPressed = false;
    private float timePassed, timeBetweenSteps = 1f;

	private Vector3 targetPosition;
	private float riseHeight;
	private float orbNum;
	private bool orbSpawned;
	private GameObject orbInstance;
	private float speed;
	private bool orbMovingToPlayer;

    // Use this for initialization
    void Start()
    {
        rb = orb.GetComponent<Rigidbody>();
		riseHeight = 2.5f;
		orbNum = 0;
		speed = 1f;
		orbSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.Q) && buttonPressed == false)
        {
            //Vector3 spawnPosition = Random.onUnitSphere * (1f ) + transform.position;
            //buttonPressed = true;
            //for (int i = 0; i < orbCount; i++)
            //{
                //GameObject orbInstance = Instantiate(orb, spawnPosition, Quaternion.identity);
                //orbInstance.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 5);
            //}

			orbInstance = Instantiate(orb, transform.position, Quaternion.identity);
			orbInstance.GetComponent<OrbSequence> ().setPlayer (player);
			//orbSpawned = true;

        }
        if (timePassed >= timeBetweenSteps)
        {
            timePassed = 0f;
            buttonPressed = false;
        }
		if (orbSpawned) {
			//targetPosition = transform.position + new Vector3 (0, riseHeight, 0);
			//orbInstance.transform.position = Vector3.MoveTowards (orbInstance.transform.position, targetPosition, 1 * Time.deltaTime);
			//orbNum++;
			//Debug.Log("Orb Rising");
			//if (orbInstance.transform.position == targetPosition) {
			//	orbSpawned = false;
			//	orbMovingToPlayer = true;
			//	Debug.Log ("Orb Stopped");
			//}
		}

		if (orbMovingToPlayer) {
			//targetPosition = player.transform.position;
			//orbInstance.transform.position = Vector3.MoveTowards (orbInstance.transform.position, targetPosition, 3 * Time.deltaTime);
		}
    }

	public void DropOrbs (){
		for (int i = 0; i < orbCount; i++) {
			Vector3 spawnPosition = Random.onUnitSphere * (1f ) + transform.position;
			GameObject orbInstance = Instantiate(orb, spawnPosition, Quaternion.identity);
			orbInstance.GetComponent<OrbSequence> ().setPlayer (player);
			orbInstance.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 5);
		}
	}

	void OrbRise () {
		GameObject orbInstance = Instantiate(orb, transform.position, Quaternion.identity);
		targetPosition = orbInstance.transform.position + new Vector3 (0, riseHeight, 0);
	}
}
