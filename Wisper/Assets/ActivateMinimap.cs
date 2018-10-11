using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMinimap : MonoBehaviour {

    public Animator slide;
    private float timePassed, timeBetweenSteps = .1f;
    public GameObject miniMap;
    public bool slid;


    // Use this for initialization
    void Start () {
        slid = false;
	}
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;
        //MINIMAP
        if ((Input.GetKey(KeyCode.E) || Input.GetButton("BButton")))
        {

            if (timePassed >= timeBetweenSteps)
            {
                if (!slid)
                {
                    Debug.Log("MapNotActive");
                    //miniMap.SetActive(false);
                    slide.SetBool("SlideOut", true);
                    slid = true;
                }
                else
                {
                    Debug.Log("Map");
                    //miniMap.SetActive(true);
                    slide.SetBool("SlideOut", false);
                    slid = false;
                }
                timePassed = 0f;
            }

        }
    }
}
