using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSequence : MonoBehaviour {

	// The object the orb will move to
	private GameObject destination;
	// ID that will affect the travel path
	private string destID;

	// Where the orb will spawn
	private Vector3 startPosition;
	private Vector3 newPosition;

	// How much the orb will rise vertically
	private float riseHeight;
	// How fast the orb will rise
	private float riseSpeed;
	// Condition for if orb is rising vertically
	private bool isRising;

	// How fast the orb moves to the destination
	private float moveToDestSpeed;
	// Condition if the orb is moving to destination
	private bool isMovingToDest;

	// WIP
	public GameObject[] curveNodes;

	// Use this for initialization
	void Start () {
		// Initialize start position based on transform
		startPosition = transform.position;

		// Initialize rise height, is rising, is moving to destination, rise speed, and move to destination speed
		riseHeight = 2.5f;
		isRising = true;
		isMovingToDest = false;

		riseSpeed = 1f;
		moveToDestSpeed = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		// Sequence is as follows:
		// When orb spawns, it will rise vertically
		// After rising, the orb will move towards the destination

		// Check if the orb is in the rising part of the sequence
		if (isRising) {
			//Debug.Log("Orb Rising");
			// Position the orb will rise to is directly above the spawn point
			newPosition = startPosition + new Vector3 (0, riseHeight, 0);
			// Move towards the new position
			transform.position = Vector3.MoveTowards (transform.position, newPosition, riseSpeed * Time.deltaTime);
			// Once the orb finished rising, move to the next part of the sequence: Moving to the player
			if (transform.position == newPosition) {
				isRising = false;
				isMovingToDest = true;
				//Debug.Log ("Orb Stopped");
			}
		}
		// Check if the orb is in the moving part of the sequence
		if (isMovingToDest) {
			// New position is now the destination
			newPosition = destination.transform.position;
			// Move to destination
			transform.position = Vector3.MoveTowards (transform.position, newPosition, moveToDestSpeed * Time.deltaTime);
			//Debug.Log ("Orb Moving To Player");
		}
		// When orb reaches player, it will be collected
	}

	// Sets the destination for the orb to move to
	// Takes a GameObject the orb will move to and an ID of the destination
	public void setDestination(GameObject destination, string destID) {
		//Debug.Log ("destID: " + destID);
		this.destID = destID;
		this.destination = destination;
	}

	// Set a new start position
	public void setStart(Vector3 start){
		this.startPosition = start;
	}
}
