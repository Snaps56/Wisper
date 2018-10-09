using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixed : MonoBehaviour {

    public Transform character;
    public bool cameraMovesWithCharacter = false;
    private Vector3 deltaPosition;
    private Vector3 newPosition;

    // Use this for initialization
    void Start ()
    {
        transform.LookAt(character);
        deltaPosition = character.position - transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (cameraMovesWithCharacter)
        {
            newPosition = character.position - deltaPosition;
            transform.position = newPosition;
        }
        else
        {
            transform.LookAt(character);
        }
	}
}
