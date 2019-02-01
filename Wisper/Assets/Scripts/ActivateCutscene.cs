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
    public bool playOnStart;
    public PersistantStateData PSDchecker;


    // Use this for initialization
    void Start () {

        PSDchecker = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();

        if (playOnStart)
        {
            //Debug.Log("Playing Intro");
            Cursor.visible = false;
            player.GetComponent<PlayerMovement>().ToggleMovement();
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            GameObject.Find("WindPowerBG").SetActive(false);
            if ((bool)PSDchecker.GetComponent<PersistantStateData>().stateConditions["Cutscene1Started"] != true)
            {

                PSDchecker.GetComponent<PersistantStateData>().stateConditions["Cutscene1Started"] = true;
                Debug.Log("Cutscene1Started: " + PSDchecker.GetComponent<PersistantStateData>().stateConditions["Cutscene1Started"]);
                PSDchecker.GetComponent<PersistantStateData>().updateCount++;
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


        //skip cutscene
        if (/*Input.GetButton("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X")*/ Input.GetKey(KeyCode.H))
        {
            Debug.Log("Skipped");
            foreach (AnimationState anim in cutsceneCamera.GetComponent<Animation>())
            {
                anim.time = anim.length;
            }

            //foreach (AnimationClip clip in cutsceneCamera.GetComponent<Animation>())
            //{
            //    foreach (AnimationEvent e in clip.events)
            //    {
            //        Debug.Log("Events: " + e);
            //    }
            //}
        }

    }
}
