using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using XInputDotNetPure;

public class MainMenu : MonoBehaviour {

    public Animator animator;

    public GameObject EventSystem1;
    public GameObject EventSystem2;
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public GameObject Credits;
    public GameObject Image;
    public PersistantStateData PSD;

    private int LeveltoLoad = 0;

    //TODO fix this. This script seems to be on an object in every scene, and is causing PSD to reset whenever a new scene is loaded.
    //Should only call PSD reset when a new game is started.
    private void Start()
    {
        PSD = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        PSD.ResetPersistantStateData();
    }

    public void PlayGame()
    {
        FadeToLevel(2);
    }

    public void FadeToLevel (int levelIndex)
    {
        LeveltoLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void FadeToNextLevel ()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete ()
    {
        SceneManager.LoadScene(2);
    }

    public void Quitgame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    void FixedUpdate()
    {
        //SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        //GamePad.SetVibration(playerIndex, 0f, System.Math.Max(state.Triggers.Right, state.Triggers.Left));
    }

    void Update ()
    {
        if (Input.GetButtonDown("XBOX_Button_B") || Input.GetButtonDown("Cancel"))
        {
            EventSystem1.SetActive(true);
            EventSystem2.SetActive(false);
            StartMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            Credits.SetActive(false);
            Image.SetActive(true);
        }
    }

}
