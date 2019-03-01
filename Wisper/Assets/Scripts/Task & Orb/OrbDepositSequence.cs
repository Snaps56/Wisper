using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDepositSequence : MonoBehaviour {

	public GameObject[] pathNodes;
	public float forceModifier;
	public float initialForceMultiplier = 400; // force applied on orb on generation

    private int currentNode;
	private float travelSpeed;

	private Vector3 pointA;
	private Vector3 pointB;

	private Rigidbody rb;

	private bool canMove;

	private Shrine shrineScript;


	// Use this for initialization
	void Start () {
		pointA = transform.position;
		canMove = true;
		currentNode = 0;
		travelSpeed = 2f;

		rb = GetComponent<Rigidbody> ();

		shrineScript = GameObject.FindGameObjectWithTag ("Shrine").GetComponent<Shrine> ();

		for (int i = 0; i < pathNodes.Length; i++) {
			string nodeName = "Deposit Path Node " + i;
			pathNodes [i] = GameObject.Find (nodeName);
		}

		Vector3 playerVector = pointA - pathNodes[0].transform.position;
		Vector3 crossVector = Vector3.ProjectOnPlane(Random.insideUnitSphere, playerVector);
		Vector3 initialForce = crossVector.normalized;

		// make sure when generating orbs, that they only spawn upwards when applying initial force
		initialForce.y = Mathf.Abs(initialForce.y);
		initialForce = initialForce * initialForceMultiplier * forceModifier;

		// spawn orbs at position of parent object, along with y offset
		//Vector3 orbSpawnPosition = transform.position;
		//orbSpawnPosition.y += orbSpawnOffsetY;

		rb.AddForce(initialForce);
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (canMove) {
			// New position is now the destination
			destination = pathNodes[currentNode].transform.position;
			// Move to destination

			transform.position = Vector3.MoveTowards (transform.position, destination, travelSpeed * Time.deltaTime);
			if (transform.position == destination) {
				currentNode++;
				if (currentNode == pathNodes.Length) {
					//Debug.Log ("End of the line");
					canMove = false;
					shrineScript.destroyOrbDeposit (this.gameObject);
				}
			}
		} 
		*/
	}

	void FixedUpdate () {
		if (canMove) {
			// New position is now the destination
			pointB = pathNodes[currentNode].transform.position;
			// Move to destination

			Vector3 deltaPosition = pointB - transform.position;
			deltaPosition = deltaPosition.normalized;

			//float distanceToBase = (transform.position - pointB).magnitude;
			Vector3 finalForce = deltaPosition * 10f * forceModifier;

			/*
			Vector3 minForce = deltaPosition * 10f;
			if (finalForce.magnitude < minForce.magnitude)
			{
				finalForce = minForce;
			}
			*/

			//finalForce *= forceModifier;

			rb.AddForce(finalForce);
			rb.AddForce(-rb.velocity * forceModifier * 2);
			//transform.position = Vector3.MoveTowards (transform.position, destination, travelSpeed * Time.deltaTime);

			/*
			if (transform.position == pointB) {
				Debug.Log ("Reached Point");
				pointA = pointB;
				currentNode++;
				if (currentNode == pathNodes.Length) {
					//Debug.Log ("End of the line");
					canMove = false;
					shrineScript.destroyOrbDeposit (this.gameObject);
				}
			}
			*/
		} 
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("DepositPath")) {
			Debug.Log ("Collided with chime");
			pointA = pointB;
			currentNode++;
			rb.AddForce(-rb.velocity * forceModifier);
			if (currentNode == pathNodes.Length) {
				//Debug.Log ("End of the line");
				canMove = false;
				shrineScript.destroyOrbDeposit (this.gameObject);
			}
		}
	}
}
