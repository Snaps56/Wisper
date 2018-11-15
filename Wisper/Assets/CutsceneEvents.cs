using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvents : MonoBehaviour {
    // Use this for initialization

    [Header("Cutscene Objects")]
    public Camera mainCamera;
    public GameObject rain;
    public GameObject light;

    void Start () {
		
	}

    void playFlower()
    {
        GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
    }

    void endAnimation()
    {
        mainCamera.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        rain.SetActive(false);
        light.GetComponent<Light>().color = Color.white;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
