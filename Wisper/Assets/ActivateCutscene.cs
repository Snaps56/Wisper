using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCutscene : MonoBehaviour {
    [Header("Cutscene Objects")]
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public GameObject rain;
    public GameObject light;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (cutsceneCamera.gameObject.activeSelf == false)
            {
                mainCamera.gameObject.SetActive(false);
                cutsceneCamera.gameObject.SetActive(true);
                GameObject.Find("WindPowerBG").SetActive(false);
                rain.SetActive(true);
                //GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
                light.GetComponent<Light>().color = Color.black;
                cutsceneCamera.GetComponent<Animation>().Play();
                //if (!cutsceneCamera.GetComponent<Animation>().isPlaying)
                //{
                //    mainCamera.gameObject.SetActive(true);
                //    cutsceneCamera.gameObject.SetActive(false);
                //}
            }
        }
    }
}
