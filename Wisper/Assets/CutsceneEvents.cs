﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvents : MonoBehaviour {
    // Use this for initialization

    [Header("Cutscene Objects")]
    public Camera mainCamera;
    public GameObject rain;
    public GameObject light;

    //Event called when the flower is supposed to animate
    void playFlower()
    {
        //Finds the flower and starts animation
        GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
    }

    //Event called when the animation should end
    void endAnimation()
    {
        //Activates main camera
        mainCamera.gameObject.SetActive(true);
        //Turns this game object off
        this.gameObject.SetActive(false);
        //Turned rain off
        rain.SetActive(false);
        //Resets the rain tint back to normal
        light.GetComponent<Light>().color = Color.white;
    }
}
