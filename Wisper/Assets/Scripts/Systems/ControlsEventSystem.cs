using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlsEventSystem : MonoBehaviour {

    private GameObject pcButton;
    private GameObject gamepadButton;

    public GameObject pcControls;
    public GameObject gamepadControls;

    private GameObject currentActiveObject;
    private EventSystem controlEventSystem;

	// Use this for initialization
	void Start () {
        controlEventSystem = GetComponent<EventSystem>();
        currentActiveObject = controlEventSystem.firstSelectedGameObject;

        pcButton = GameObject.Find("PC Controls Button");
        gamepadButton = GameObject.Find("Gamepad Button");
        
    }
	
	// Update is called once per frame
	void Update () {
        currentActiveObject = controlEventSystem.currentSelectedGameObject;
        if (currentActiveObject == gamepadButton)
        {
            gamepadControls.SetActive(true);
            pcControls.SetActive(false);
        }
        else if (currentActiveObject == pcButton)
        {
            pcControls.SetActive(true);
            gamepadControls.SetActive(false);
        }
	}
}
