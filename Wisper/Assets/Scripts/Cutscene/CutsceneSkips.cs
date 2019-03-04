using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CutsceneSkips: MonoBehaviour{

    private GameObject cutsceneManager;


    private void Start()
    {
        cutsceneManager = GameObject.Find("CutsceneManager");
    }
    //Skips a group of cutscenes containing the given name
    public void SkipCutscene(string cutsceneGroup)
    {
        MethodInfo skipMeth = typeof(CutsceneSkips).GetMethod(cutsceneGroup + "Skip");
        skipMeth.Invoke(null, null);
    }

    public void PlaygroundIntroPanSkip()
    {
        Hashtable PSDUpdates = new Hashtable();
        PSDUpdates.Add("StartupShrineDialogue", true);
        PersistantStateData.persistantStateData.ChangeStateConditions(PSDUpdates);  // updates the PSD to the post-cutscene state
        this.GetComponent<Animation>().Stop();  // Ends the animation
        cutsceneManager.GetComponent<ActivateCutscene>().mainCamera.gameObject.SetActive(true);
        cutsceneManager.GetComponent<ActivateCutscene>().cutsceneCamera.gameObject.SetActive(false);
    }
}
