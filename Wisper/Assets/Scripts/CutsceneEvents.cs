using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvents : MonoBehaviour {
    // Use this for initialization

    [Header("Cutscene Objects")]
    public Camera mainCamera;
    public Camera cutsceneCamera;
    public GameObject rain;
    public GameObject light;
    public GameObject windPowerUI;
    public GameObject player;


    void PSDVariableOn(string key)
    {
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().ChangeStateConditions(key, true);
    }

    void PSDVariableOff(string key)
    {
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().ChangeStateConditions(key, false);
    }

    void ProgressDialogue()
    {
        GameObject.Find("DialogueManager").GetComponent<DialogueManager>().CutsceneDislayNextSentence();
    }

    //Event called when the flower is supposed to animate
    void playFlower()
    {
        //Finds the flower and starts animation
        GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt1").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt2").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt3").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt4").GetComponent<Animator>().SetBool("Grow", true);
    }

    void playDeposit2 ()
    {
        Debug.Log("Hello there");
        GetComponent<Animation>().Play("Deposit2");
    }

    void playDeposit3()
    {
        GetComponent<Animation>().Play("Deposit3");
    }

    void playCutscene2 ()
    {
        GetComponent<Animation>().Play("Cutscene2");
    }

    void playCutscene3 ()
    {
        GetComponent<Animation>().Play("Cutscene3");
    }


    //Event called when the animation should end
    void endAnimation()
    {
        player.GetComponent<PlayerMovement>().ToggleMovement();
        //Activates main camera
        mainCamera.gameObject.SetActive(true);
        //Turns this game object off
        cutsceneCamera.gameObject.SetActive(false);
        windPowerUI.SetActive(true);
        //Turned rain off
        rain.SetActive(false);
        //Resets the rain tint back to normal
        Color tempColor = new Color(255, 147, 85, 255);
        light.GetComponent<Light>().color = Color.white;
    }



    void cutsceneSkipped()
    {
        PSDVariableOn("StartupShrineDialogue");
        endAnimation();
    }
}

[System.Serializable]
public class BooleanKeyValue
{
    public string key;
    public bool value;
}
