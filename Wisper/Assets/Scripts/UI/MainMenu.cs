using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

}
