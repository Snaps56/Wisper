using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedrawLine : MonoBehaviour {

	public LineRenderer[] lines;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		foreach (LineRenderer line in lines) {
			line.SetPosition (1, transform.localPosition);
		}
	}
}