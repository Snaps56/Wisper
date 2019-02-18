using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvents : MonoBehaviour {
    // Use this for initialization

    [Header("Cutscene Objects")]
    public Camera mainCamera;
    public Camera cutsceneCamera;
    public GameObject rain;
    public GameObject directionalLight1;
    public GameObject directionalLight2;
    public GameObject windPowerUI;
    public GameObject player;
	public GameObject cityGate;
    public AudioSource rainSound;

    private bool cutsceneIsPlaying = false;

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

	void enableGate() {
		cityGate.SetActive(true);
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

    bool GetCutsceneIsPlaying()
    {
        return cutsceneIsPlaying;
    }
    void EnableCutsceneIsPlaying()
    {
        cutsceneIsPlaying = true;
    }
    void DisableCutsceneIsPlaying()
    {
        cutsceneIsPlaying = false;
    }
    //Event called when the animation should end
    void endAnimation()
    {
        if (cutsceneIsPlaying)
        {
            player.GetComponent<PlayerMovement>().EnableMovement();
        }
        //Activates main camera
        mainCamera.gameObject.SetActive(true);
        //Turns this game object off
        cutsceneCamera.gameObject.SetActive(false);
        windPowerUI.SetActive(true);
        //Turned rain off
        rain.SetActive(false);
        //Turn off rain sound
        rainSound.Stop();
        //Resets the rain tint back to normal
        Color directionalLight1Color = new Color(0, 253, 248, 255);
        Color directionalLight2Color = new Color(255, 222, 170, 255);

        directionalLight1.GetComponent<Light>().color = Color.white;
        directionalLight2.GetComponent<Light>().color = Color.white;
    }


    public void cutsceneSkipped()
    {
        Debug.Log("cutsceneSkipped was called in CutsceneEvents. Effect: Turns on a PSD variable and calls endAnimation");
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
