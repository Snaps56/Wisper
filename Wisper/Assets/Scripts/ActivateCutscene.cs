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

    void Awake()
    {
        Debug.Log("Playing Intro");
        mainCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        GameObject.Find("WindPowerBG").SetActive(false);
        cutsceneCamera.GetComponent<Animation>().Play("Cutscene1");
    }

    // Update is called once per frame
    void Update () {
        //Plays cutscene while pressing "N" on keyboard
        if (Input.GetKeyDown(KeyCode.N))
        {
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            GameObject.Find("WindPowerBG").SetActive(false);
            rain.SetActive(true);
            light.GetComponent<Light>().color = Color.black;
            cutsceneCamera.GetComponent<Animation>().Play();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Playing Intro");
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            GameObject.Find("WindPowerBG").SetActive(false);
            cutsceneCamera.GetComponent<Animation>().Play("Cutscene3");
        }
    }
}
