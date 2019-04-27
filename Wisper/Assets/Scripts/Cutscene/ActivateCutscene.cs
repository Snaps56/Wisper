using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ActivateCutscene : MonoBehaviour {
    [Header("Cutscene Objects")]
    public Camera cutsceneCamera;
    private Camera mainCamera;
    public GameObject rain;
    public GameObject thelight;
    private GameObject player;
    public GameObject windPB;
    public bool playOnStart;
    public PersistantStateData PSDchecker;


    // Use this for initialization
    void Start () {
        player = PlayerPersistance.player.transform.gameObject;
        mainCamera = PlayerPersistance.player.transform.Find("Main Camera").GetComponent<Camera>();
        PSDchecker = PersistantStateData.persistantStateData;

        // If PlaygroundIntroPan should play, does it here.
        if ((bool)PSDchecker.stateConditions["DoPlaygroundIntroPan"] && !(bool)PSDchecker.stateConditions["PlaygroundIntroPan1Started"])
        {
            //Debug.Log("Playing Intro");
            Cursor.visible = false;

            Debug.Log("Disable player movement from intro cutscene");
            player.GetComponent<PlayerMovement>().DisableMovement();
            mainCamera.gameObject.SetActive(false);
            cutsceneCamera.gameObject.SetActive(true);
            GameObject.Find("WindPowerBG").SetActive(false);
            if (!(bool)PSDchecker.stateConditions["PlaygroundIntroPan1Started"])
            {
                Hashtable tmpHash = new Hashtable();
                tmpHash.Add("DoPlaygroundIntroPan", false);
                tmpHash.Add("PlaygroundIntroPan1Started", true);
                PSDchecker.ChangeStateConditions(tmpHash);
                cutsceneCamera.GetComponent<Animation>().Play("PlaygroundIntroPan1");
            }
        }
    }


    // Update is called once per frame
    void Update () {

        //Plays cutscene while pressing "N" on keyboard
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    mainCamera.gameObject.SetActive(false);
        //    cutsceneCamera.gameObject.SetActive(true);
        //    windPB.SetActive(false);
        //    rain.SetActive(true);
        //    light.GetComponent<Light>().color = Color.black;
        //    cutsceneCamera.GetComponent<Animation>().Play();
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Debug.Log("Playing Intro");
        //    mainCamera.gameObject.SetActive(false);
        //    cutsceneCamera.gameObject.SetActive(true);
        //    windPB.SetActive(false);
        //    cutsceneCamera.GetComponent<Animation>().Play("Cutscene3");
        //}
        
        //skip cutscene
        if (/*Input.GetButton("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_X")*/ Input.GetKey(KeyCode.H) || Input.GetButtonDown("XBOX_Button_X"))
        {
            foreach(AnimationState anime in cutsceneCamera.GetComponent<Animation>())
            {
                if(cutsceneCamera.GetComponent<Animation>().IsPlaying(anime.name))
                {
                    //Debug.Log("Animation " + anime.name + " detected as playing by cutscene skipper");
                    foreach(AnimationEvent evento in anime.clip.events)
                    {
                        //Debug.Log("Checking AnimationEvent with function: " + evento.functionName);
                        if(!evento.isFiredByAnimator)
                        {
                            //Debug.Log(evento.functionName + " has not fired yet." + " Attempting to retrieve function from CutsceneEvents");
                            //Debug.Log("Type of CutsceneEvents is " + typeof(CutsceneEvents).Name);
                            MethodInfo eventoMethod;
                            ParameterInfo[] eventoParamInfo;
                            object[] passingParams;

                            try
                            {
                                eventoMethod = typeof(CutsceneEvents).GetMethod(evento.functionName);
                                if (eventoMethod == null)
                                {
                                    //Debug.Log("Could not locate MethodInfo for " + evento.functionName + " in " + typeof(CutsceneEvents).Name);
                                }
                                else
                                {
                                    //Debug.Log("Method info is " + eventoMethod.Name);
                                    eventoParamInfo = eventoMethod.GetParameters();
                                    passingParams = new object[eventoParamInfo.Length];
                                    int i = 0;
                                    foreach (ParameterInfo paramFo in eventoParamInfo)
                                    {
                                        //Debug.Log("Checking ParameterInfo of " + paramFo.Name);
                                        if (paramFo.ParameterType == typeof(string))
                                        {
                                            //Debug.Log(paramFo.Name + " is of type string");
                                            passingParams[i] = evento.stringParameter;
                                        }
                                        else if (paramFo.ParameterType == typeof(float))
                                        {
                                            //Debug.Log(paramFo.Name + " is of type float");
                                            passingParams[i] = evento.floatParameter;
                                        }
                                        else if (paramFo.ParameterType == typeof(int))
                                        {
                                            //Debug.Log(paramFo.Name + " is of type int");
                                            passingParams[i] = evento.intParameter;
                                        }
                                        else if (paramFo.ParameterType == typeof(object))
                                        {
                                            //Debug.Log(paramFo.Name + " is of type object");
                                            passingParams[i] = evento.objectReferenceParameter;
                                        }
                                        i++;
                                    }
                                    //Debug.Log("Attempting to invoke " + eventoMethod.Name);
                                    eventoMethod.Invoke(cutsceneCamera.GetComponent<CutsceneEvents>(), passingParams);
                                }
                            }
                            catch (AmbiguousMatchException)
                            {
                                //Debug.Log("Ambiguous Match on Method");
                            }
                        }
                    }
                }
            }
            
            /*
            Debug.Log("Skipped");
            foreach (AnimationState anim in cutsceneCamera.GetComponent<Animation>())
            {
                anim.time = anim.length;
            }
            */
            //foreach (AnimationClip clip in cutsceneCamera.GetComponent<Animation>())
            //{
            //    foreach (AnimationEvent e in clip.events)
            //    {
            //        Debug.Log("Events: " + e);
            //    }
            //}

            //AnimationClip PlaygroundIntroPan1 = cutsceneCamera.GetComponent<Animation>().GetClip("PlaygroundIntroPan1");

        }
        
        
    }
}
