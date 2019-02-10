using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDepositSequence : MonoBehaviour {

	public GameObject[] pathNodes;

    private int currentNode;
	private float travelSpeed;

	private Vector3 start;
	private Vector3 destination;

	private bool canMove;

	private Shrine shrineScript;


	// Use this for initialization
	void Start () {
		start = transform.position;
		canMove = true;
		currentNode = 0;
		travelSpeed = 2f;
		shrineScript = GameObject.FindGameObjectWithTag ("Shrine").GetComponent<Shrine> ();

		for (int i = 0; i < pathNodes.Length; i++) {
			string nodeName = "Deposit Path Node " + i;
			pathNodes [i] = GameObject.Find (nodeName);
		}
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
