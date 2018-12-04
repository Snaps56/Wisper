using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Animator animator;

    private int LeveltoLoad = 0;
    //private float horizontalSpeed = 2.0f;
    //private float verticalSpeed = 2.0f;

    public void PlayGame()
    {
        FadeToLevel(1);
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
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void Back ()
    {
        if (GameObject.V == false)
        {

        }
    }

    //void Update()
    //{
        // Get the mouse delta. This is not in the range -1...1
        //float h = horizontalSpeed * Input.GetAxis("XBOX_Thumbstick_L_X");
        //float v = verticalSpeed * Input.GetAxis("XBOX_Thumbstick_L_Y");
        //
  //      transform.Translate(v, h, 0);
//    }

}
