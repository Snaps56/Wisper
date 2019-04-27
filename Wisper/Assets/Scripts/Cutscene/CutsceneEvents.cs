using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvents : MonoBehaviour {
    // Use this for initialization

    [Header("Cutscene Objects")]
    private Camera mainCamera;
    public Camera cutsceneCamera;
    public GameObject rain;
    private GameObject directionalLight;
    private GameObject windPowerUI;
    private GameObject player;
	public GameObject cityGate;
    public AudioSource rainSound;

    private bool cutsceneIsPlaying = false;

    private void Start()
    {
        rainSound = ShrinePersistance.shrine.transform.GetComponent<AudioSource>();
        mainCamera = PlayerPersistance.player.transform.Find("Main Camera").GetComponent<Camera>();
        windPowerUI = PowerBarPersistence.powerbar.transform.gameObject;
        //windPowerUI = PowerBarPersistence.powerbar.transform;
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
        DialogueManager.dialogueManager.CutsceneDislayNextSentence();
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
        //Debug.Log("Hello there");
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

        //directionalLight.GetComponent<Light>().color = Color.white;
    }
}

[System.Serializable]
public class BooleanKeyValue
{
    public string key;
    public bool value;
}
