using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashListCreator : MonoBehaviour {

	public List<GameObject> trashInArea = new List<GameObject>();
	//BoxCollider area = gameObject.GetComponent<BoxCollider> ();

	// Use this for initialization
	void Start () {
		//area.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//trashInArea.RemoveAll (GameObject => GameObject == null);
	}
	/*
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PickUp" && other.name.Contains("Trash"))
		{
			if (!trashInArea.Contains(other.gameObject))
			{
				trashInArea.Add(other.gameObject);
			}
		}
	}
	*/
}
