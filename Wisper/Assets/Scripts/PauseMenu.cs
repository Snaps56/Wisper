using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenu;
    public GameObject VideoMenu;
    public AudioMixer audioMixer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    // Resumes the game and resets the cursor lock
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        VideoMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Pauses the game and unlocks the cursor
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // Quits the game! But why would you want to do that?
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}

