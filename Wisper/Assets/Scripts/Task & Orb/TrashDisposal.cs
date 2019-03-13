using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDisposal : MonoBehaviour {

	public List<List<GameObject>> trashLists = new List<List<GameObject>> ();
	public List<GameObject> trashAreas = new List<GameObject> ();
	private SpawnOrbs orbScript;
	public TrashListCreator[] trashListCreators;
	private List<GameObject> emptyList = new List<GameObject> ();

	void Awake () {
		trashAreas.AddRange(GameObject.FindGameObjectsWithTag("TrashArea"));
		trashListCreators = new TrashListCreator[trashAreas.Count];
		for (int i = 0; i < trashAreas.Count; i++) {
			Debug.Log (i);
			trashListCreators [i] = trashAreas [i].GetComponent<TrashListCreator> ();
			trashLists.Add (trashListCreators [i].trashInArea);
		}
	}

	// Use this for initialization
	void Start () {
		orbScript = GetComponent<SpawnOrbs>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PickUp" && other.name.Contains("Trash"))
		{
			for (int emptyTL = trashLists.Count - 1; emptyTL >= 0; emptyTL--) {
				if (trashLists [emptyTL].Count == 0) {
					trashLists.Remove (trashLists [emptyTL]);
				}
			}

			// Iterate through the trash lists
			for (int tl = 0; tl < trashLists.Count; tl++) {
				// Iterate through the elements of each trash list
				for (int tle = 0; tle < trashLists [tl].Count; tle++) {
					// If the name of the trash in the trigger matches the element in the trash list
					// Remove and destroy it
					if (other.name == trashLists [tl] [tle].name) {
						Debug.Log ("Yes Officer");
						trashLists [tl].Remove (trashLists [tl] [tle]);
						Destroy (other.gameObject);
					}
				}
				if (trashLists [tl].Count == 0) {
					Debug.Log ("Is Empty");
					orbScript.DropOrbs();
					emptyList = trashLists [tl];
					//trashLists.Remove (trashLists [j]);
				}
			}
			if (emptyList != null) {
				trashLists.Remove (emptyList);
				Debug.Log ("Empty List removed");
				emptyList = null;
			}
				
			//Destroy(other.gameObject);
			//orbScript.DropOrbs();
		}
	}
}
