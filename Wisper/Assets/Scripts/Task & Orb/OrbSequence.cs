using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSequence : MonoBehaviour {
	private GameObject player;

	private Vector3 startPosition;
	private Vector3 newPosition;

	private float riseHeight;
	private float riseSpeed;
	private bool isRising;

	private float moveToPlayerSpeed;
	private bool isMovingToPlayer;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		startPosition = transform.position;

		riseHeight = 2.5f;
		isRising = true;
		isMovingToPlayer = false;

		riseSpeed = 1f;
		moveToPlayerSpeed = 3f;
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the orb is in the rising part of the sequence
		if (isRising) {
			//Debug.Log("Orb Rising");
			newPosition = startPosition + new Vector3 (0, riseHeight, 0);
			transform.position = Vector3.MoveTowards (transform.position, newPosition, riseSpeed * Time.deltaTime);
			// Once the orb finished rising, move to the next part of the sequence: Moving to the player
			if (transform.position == newPosition) {
				isRising = false;
				isMovingToPlayer = true;
				//Debug.Log ("Orb Stopped");
			}
		}
		// Check if the orb is in the moving part of the sequence
		if (isMovingToPlayer) {
			newPosition = player.transform.position;
			transform.position = Vector3.MoveTowards (transform.position, newPosition, moveToPlayerSpeed * Time.deltaTime);
			//Debug.Log ("Orb Moving To Player");
		}
	}

	// Sets the player for the orb to move to
	public void setPlayer(GameObject player) {
		this.player = player;
	}
}
