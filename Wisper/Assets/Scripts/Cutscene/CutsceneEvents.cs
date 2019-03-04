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

    private void Start()
    {
        if ((bool)PersistantStateData.persistantStateData.stateConditions["DoPlaygroundIntroPan"])
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }


    }

    public void PSDVariableOn(string key)
    {
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().ChangeStateConditions(key, true);
    }

    public void PSDVariableOff(string key)
    {
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().ChangeStateConditions(key, false);
    }

    public void ProgressDialogue()
    {
        GameObject.Find("DialogueManager").GetComponent<DialogueManager>().CutsceneDislayNextSentence();
    }

    //Event called when the flower is supposed to animate
    public void playFlower()
    {
        //Finds the flower and starts animation
        GameObject.Find("flower_wilt").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt1").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt2").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt3").GetComponent<Animator>().SetBool("Grow", true);
        GameObject.Find("flower_wilt4").GetComponent<Animator>().SetBool("Grow", true);
    }

	public void enableGate() {
		cityGate.SetActive(true);
	}

    public void PlayDeposit2 ()
    {
        Debug.Log("Hello there");
        GetComponent<Animation>().Play("Deposit2");
    }

    public void PlayDeposit3()
    {
        GetComponent<Animation>().Play("Deposit3");
    }

    public void PlayCutscene2 ()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name  + " called from " + this.GetType().Name);
        GetComponent<Animation>().Play("PlaygroundIntroPan2");
    }

    public void PlayCutscene3 ()
    {
        GetComponent<Animation>().Play("PlaygroundIntroPan3");
    }

    public bool GetCutsceneIsPlaying()
    {
        return cutsceneIsPlaying;
    }
    public void EnableCutsceneIsPlaying()
    {
        cutsceneIsPlaying = true;
    }
    public void DisableCutsceneIsPlaying()
    {
        cutsceneIsPlaying = false;
    }
    //Event called when the animation should end
    public void EndAnimation()
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

    /*
    public void cutsceneSkipped()
    {
        Debug.Log("cutsceneSkipped was called in CutsceneEvents. Effect: Turns on a PSD variable and calls endAnimation");
        PSDVariableOn("StartupShrineDialogue");
        EndAnimation();
    }
    */

}

[System.Serializable]
public class BooleanKeyValue
{
    public string key;
    public bool value;
}
