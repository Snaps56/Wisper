using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class MainMenu : MonoBehaviour {

    public Animator animator;

    public GameObject EventSystem1;
    public GameObject EventSystem2;
    public GameObject StartMenu;
    public GameObject OptionsMenu;
    public GameObject Credits;
    public GameObject Image;
    public PersistantStateData PSD;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private int LeveltoLoad = 0;

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
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    void Update ()
    {
        if (Input.GetButtonDown("XBOX_Button_B"))
        {
            EventSystem1.SetActive(true);
            EventSystem2.SetActive(false);
            StartMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            Credits.SetActive(false);
            Image.SetActive(true);
        }

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

}
