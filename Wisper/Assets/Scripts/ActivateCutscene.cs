using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCutscene : MonoBehaviour {
    [Header("Cutscene Objects")]
    public Camera cutsceneCamera;
    public Camera mainCamera;
    public GameObject rain;
    public GameObject light;
    public GameObject player;
    public GameObject windPB;
    public GameObject psData;
    public bool playOnStart;


    // Use this for initialization
    void Start () {
        if  (playOnStart)
        {
            //Debug.Log("Playing Intro");
            Cursor.visible = false;
            player.GetComponent<PlayerMovement>().ToggleMovement();
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            GameObject.Find("WindPowerBG").SetActive(false);
            if (!(bool)psData.GetComponent<PersistantStateData>().stateConditions["Cutscene1Started"])
            {
                psData.GetComponent<PersistantStateData>().stateConditions["Cutscene1Started"] = true;
                psData.GetComponent<PersistantStateData>().updateCount++;
                cutsceneCamera.GetComponent<Animation>().Play("Cutscene1");
            }
        }
    }


    // Update is called once per frame
    void Update () {

        //Plays cutscene while pressing "N" on keyboard
        if (Input.GetKeyDown(KeyCode.N))
        {
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            windPB.SetActive(false);
            rain.SetActive(true);
            light.GetComponent<Light>().color = Color.black;
            cutsceneCamera.GetComponent<Animation>().Play();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Playing Intro");
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            windPB.SetActive(false);
            cutsceneCamera.GetComponent<Animation>().Play("Cutscene3");
        }
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("XBOX_Button_X"))
        {
            foreach (AnimationState anim in cutsceneCamera.GetComponent<Animation>())
            {
                anim.time = anim.length;
            }
        }

    }
}
