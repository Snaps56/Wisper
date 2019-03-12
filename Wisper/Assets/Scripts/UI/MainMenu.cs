using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
        PSD = PersistantStateData.persistantStateData;
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

    // Initializes file names and numbers in load buttons, as well as making the objects active
    public void DisplayLoadMenu ()
    {
        Debug.Log("Save path is " + PSD.savePath);
        string[] saveFolders = Directory.GetDirectories(PSD.savePath);
        if(saveFolders.Length < 1)
        {
            // TODO display "no load data" message and make sure correct menus are enabled
        }
        else
        {
            GameObject loadMenu = GameObject.Find("Canvas").transform.Find("SaveLoadMenu").gameObject;
            GameObject lb1 = loadMenu.transform.Find("SaveLoadButton1").gameObject;
            GameObject lb2 = loadMenu.transform.Find("SaveLoadButton2").gameObject;
            GameObject lb3 = loadMenu.transform.Find("SaveLoadButton3").gameObject;
            GameObject lb4 = loadMenu.transform.Find("SaveLoadButton4").gameObject;
            Debug.Log("Located load buttons: " + lb1.name + "\n" + lb2.name + "\n" + lb3.name + "\n" + lb4.name);
            if (saveFolders.Length >= 1)
            {
                ChangeLoadButton(lb1, saveFolders[0]);
                lb1.SetActive(true);
            }
            if (saveFolders.Length >= 2)
            {
                ChangeLoadButton(lb2, saveFolders[1]);
                lb2.SetActive(true);
            }
            if (saveFolders.Length >= 3)
            {
                ChangeLoadButton(lb3, saveFolders[2]);
                lb3.SetActive(true);
            }
            if (saveFolders.Length > 3)
            {
                ChangeLoadButton(lb4, saveFolders[3]);
                lb4.SetActive(true);
            }
            loadMenu.transform.Find("Panel").gameObject.SetActive(true);
        }
    }

    public void ChangeLoadButton(GameObject lb, string saveFolder)
    {
        Debug.Log("Finding save from: " + saveFolder);
        string lbNum = LoadMenu.GetComponent<SaveLoadMenu>().ParseFinalPathPortion(saveFolder);
        string lbName = LoadMenu.GetComponent<SaveLoadMenu>().ParseFinalPathPortion(Directory.GetFiles(saveFolder)[0]);
        Debug.Log("Searching for name and num for load button: " + lb.name);
        lb.transform.Find("SaveNumber").gameObject.GetComponent<Text>().text = lbNum;
        lb.transform.Find("SaveName").gameObject.GetComponent<Text>().text = lbName;
    }

    public void Quitgame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    void FixedUpdate()
    {
        //SetVibration should be sent in a slower rate.
        //Set vibration according to triggers
        //GamePad.SetVibration(playerIndex, 0f, System.Math.Max(state.Triggers.Right, state.Triggers.Left));
    }

    void Update ()
    {
        if (!controlDetector.isUsingController)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Debug.Log(EventSystem.current.currentSelectedGameObject);

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
        /*
        if (Input.GetButtonDown("XBOX_Button_B") || Input.GetButtonDown("Cancel"))
        {
            EventSystem1.SetActive(true);
            EventSystem2.SetActive(false);
            StartMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            Credits.SetActive(false);
            Image.SetActive(true);
        }
        */
        if(PSD == null)
        {
            Debug.Log("no PSD set");
            PSD = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        }
    }

}
