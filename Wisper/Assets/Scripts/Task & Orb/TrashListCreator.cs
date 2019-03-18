using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashListCreator : MonoBehaviour {

	public List<GameObject> trashInArea = new List<GameObject>();
	public string pSDVariable;
	BoxCollider area;

	// Use this for initialization
	void Start () {
		//Debug.Log ("Start");
		//area = gameObject.GetComponent<BoxCollider> ();
		//area.enabled = false;

		if ((bool)PersistantStateData.persistantStateData.stateConditions[pSDVariable]) {
			Debug.Log ("Destroy all objects");
			for (int i = 0; i < trashInArea.Count; i++) {
				Destroy (trashInArea [i]);
			}
			trashInArea.Clear ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//trashInArea.RemoveAll (GameObject => GameObject == null);
	}
	/*
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Trigger");
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
