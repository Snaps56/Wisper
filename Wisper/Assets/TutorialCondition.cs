using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCondition : MonoBehaviour {

    private bool condition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public bool GetCondition()
    {
        return condition;
    }
    public void SetCondition(bool setBool)
    {
        condition = setBool;
    }
}
