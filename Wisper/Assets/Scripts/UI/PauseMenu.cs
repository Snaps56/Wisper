using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class PauseMenu : MonoBehaviour
{
    public Animator animator;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenu;
    public GameObject VideoMenu;
    public GameObject MainEventSystem;
    public GameObject GraphicsEventSystem;
    public GameObject AudioEventSystem;

    public AudioMixer audioMixer;

    private float CurrentVolume;

    //Vibrate Settings
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

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

