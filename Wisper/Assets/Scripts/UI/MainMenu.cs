using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
//using XInputDotNetPure;

public class MainMenu : MonoBehaviour {

    public Animator animator;

    public GameObject EventSystem1;
    public GameObject EventSystem2;
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public GameObject LoadMenu;
    public GameObject Credits;
    public GameObject Image;
    public PersistantStateData PSD;
    private int LeveltoLoad = 0;
    private ControlDetector controlDetector;
    //TODO fix this. This script seems to be on an object in every scene, and is causing PSD to reset whenever a new scene is loaded.
    //Should only call PSD reset when a new game is started.
    private void Start()
    {
        controlDetector = GameObject.Find("ControllerDetector").GetComponent<ControlDetector>();

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

    public void DisplayLoadMenu ()
    {
        string[] saveFolders = Directory.GetDirectories(PSD.savePath);
        if(saveFolders.Length < 1)
        {
            // TODO display no load data message
        }
        else
        {
            if (saveFolders.Length == 1)
            {
                // TODO Turn on 1 LoadButton
                // Set initial values on LoadButton
            }
            else if (saveFolders.Length == 2)
            {
                // TODO Turn on 2 LoadButton
                // Set initial values on LoadButtons
            }
            else if (saveFolders.Length == 3)
            {
                // TODO Turn on 3 LoadButton
                // Set initial values on LoadButtons
            }
            else if (saveFolders.Length > 3)
            {
                // TODO Turn on 4 LoadButton
                // Set initial values on LoadButtons
            }
        }
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
        if (!controlDetector.isUsingController)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
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
