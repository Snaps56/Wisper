using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    public Transform player;
    public GameObject transitionText;
    public int nextSceneNumber;
    public float minDistance;

    private float currentDistance;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update() {
        currentDistance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(currentDistance);

        if (currentDistance < minDistance)
        {
            transitionText.SetActive(true);
            if (Input.GetButton("PC_Key_Interact") || Input.GetButton("XBOX_Button_A"))
            {
                SceneManager.LoadScene(nextSceneNumber);
            }
        }
        else
        {
            transitionText.SetActive(false);
        }
	}
}
