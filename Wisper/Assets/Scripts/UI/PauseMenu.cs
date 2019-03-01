using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class PauseMenu : MonoBehaviour
{
    public Animator animator;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenu;
    public GameObject VideoMenu;
    public GameObject SaveLoadMenu;
    public GameObject MainEventSystem;
    public GameObject GraphicsEventSystem;
    public GameObject AudioEventSystem;
    public AudioMixer audioMixer;

    private float CurrentVolume;
    private PersistantStateData PSD;

    //Vibrate Settings
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private void Start()
    {
        PSD = PersistantStateData.persistantStateData;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("XBOX_Button_Start"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        //// Find a PlayerIndex, for a single player game
        //// Will find the first controller that is connected and use it
        //if (!playerIndexSet || !prevState.IsConnected)
        //{
        //    for (int i = 0; i < 4; ++i)
        //    {
        //        PlayerIndex testPlayerIndex = (PlayerIndex)i;
        //        GamePadState testState = GamePad.GetState(testPlayerIndex);
        //        if (testState.IsConnected)
        //        {
        //            //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
        //            playerIndex = testPlayerIndex;
        //            playerIndexSet = true;
        //        }
        //    }
        //}

        //prevState = state;
        //state = GamePad.GetState(playerIndex);
    }
    // Resumes the game and resets the cursor lock
    public void Resume()
    {
        MainEventSystem.SetActive(false);
        GraphicsEventSystem.SetActive(false);
        AudioEventSystem.SetActive(false);
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        VideoMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        if (CurrentVolume == 0)
        {
            SetVolume(CurrentVolume);
        }
        else if (CurrentVolume <= -15)
        {
            SetVolume(CurrentVolume + 15);
        }
        FindObjectOfType<AudioManager>().Play("buttonSelect");
    }
    // Pauses the game and unlocks the cursor
    void Pause()
    {
       // GamePad.SetVibration(playerIndex, 0f, 0f);
        pauseMenuUI.SetActive(true);
        MainEventSystem.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        SetVolume(CurrentVolume - 15);
    }

    public void DisplayPauseMenu()
    {
        // TODO: Implement
    }

    public void DisplaySaveMenu()
    {
        Debug.Log("Save path is " + PSD.savePath);
        string[] saveFolders = Directory.GetDirectories(PSD.savePath);
        GameObject loadMenu = GameObject.Find("SaveLoadMenu");
        GameObject slb1 = loadMenu.transform.Find("SaveLoadButton1").gameObject;
        GameObject slb2 = loadMenu.transform.Find("SaveLoadButton2").gameObject;
        GameObject slb3 = loadMenu.transform.Find("SaveLoadButton3").gameObject;
        GameObject slb4 = loadMenu.transform.Find("SaveLoadButton4").gameObject;

        if (saveFolders.Length >= 1)
        {
            ChangeSaveLoadButton(slb1, saveFolders[0]);
        }
        else
        {
            ChangeSaveLoadButton(slb1);
        }
        slb1.SetActive(true);
        if (saveFolders.Length >= 2)
        {
            ChangeSaveLoadButton(slb2, saveFolders[1]);
            
        }
        else
        {
            ChangeSaveLoadButton(slb2);
        }
        slb2.SetActive(true);
        if (saveFolders.Length >= 3)
        {
            ChangeSaveLoadButton(slb3, saveFolders[2]);
            
        }
        else
        {
            ChangeSaveLoadButton(slb3);
        }
        slb3.SetActive(true);
        if (saveFolders.Length > 3)
        {
            ChangeSaveLoadButton(slb4, saveFolders[3]);
            
        }
        else
        {
            ChangeSaveLoadButton(slb4);
        }
        slb4.SetActive(true);
        loadMenu.transform.Find("Panel").gameObject.SetActive(true);
    }

    public void DisplayLoadMenu()
    {
        string[] saveFolders = Directory.GetDirectories(PSD.savePath);
        if (saveFolders.Length < 1)
        {
            // TODO display "no load data" message and make sure correct menus are enabled
        }
        else
        {
            GameObject saveLoadMenu = GameObject.Find("SaveLoadMenu");
            GameObject slb1 = saveLoadMenu.transform.Find("SaveLoadButton1").gameObject;
            GameObject slb2 = saveLoadMenu.transform.Find("SaveLoadButton2").gameObject;
            GameObject slb3 = saveLoadMenu.transform.Find("SaveLoadButton3").gameObject;
            GameObject slb4 = saveLoadMenu.transform.Find("SaveLoadButton4").gameObject;
            
            if (saveFolders.Length >= 1)
            {
                ChangeSaveLoadButton(slb1, saveFolders[0]);
                slb1.SetActive(true);
            }
            if (saveFolders.Length >= 2)
            {
                ChangeSaveLoadButton(slb2, saveFolders[1]);
                slb2.SetActive(true);
            }
            if (saveFolders.Length >= 3)
            {
                ChangeSaveLoadButton(slb3, saveFolders[2]);
                slb3.SetActive(true);
            }
            if (saveFolders.Length > 3)
            {
                ChangeSaveLoadButton(slb4, saveFolders[3]);
                slb4.SetActive(true);
            }
            saveLoadMenu.transform.Find("Panel").gameObject.SetActive(true);
        }
    }

    public void ChangeSaveLoadButton(GameObject slb = null, string saveFolder = "")
    {
        if(!saveFolder.Equals(""))
        {
            Debug.Log("Finding save from: " + saveFolder);
            string slbNum = SaveLoadMenu.GetComponent<SaveLoadMenu>().ParseFinalPathPortion(saveFolder);
            string slbName = SaveLoadMenu.GetComponent<SaveLoadMenu>().ParseFinalPathPortion(Directory.GetFiles(saveFolder)[0]);
            Debug.Log("Searching for name and num for load button: " + slb.name);
            slb.transform.Find("SaveNumber").gameObject.GetComponent<Text>().text = slbNum;
            slb.transform.Find("SaveName").gameObject.GetComponent<Text>().text = slbName;
        }
        else
        {
            string slbNum = slb.name.Substring(slb.name.Length - 1);
            string slbName = "Empty";
            slb.transform.Find("SaveNumber").gameObject.GetComponent<Text>().text = slbNum;
            slb.transform.Find("SaveName").gameObject.GetComponent<Text>().text = slbName;
        }
        
    }

    // Quits the game! But why would you want to do that?
    public void QuitGame()
    {
        Resume();
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(1);
    }
    // Function to set the master volume
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        CurrentVolume = volume;
    }

}

