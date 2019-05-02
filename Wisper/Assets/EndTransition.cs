using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndTransition : MonoBehaviour
{
    public GameObject LanternManager;
    public GameObject Fade;
    public GameObject player;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject PauseUI;
    public GameObject CreditsCanvas;
    public GameObject BlackScreen;

    public Animator animator1;
    //public Animator animator2;

    public void FadeToEnd()
    {
        animator1.SetTrigger("FadeIn");
    }

    public void OnFadeComplete()
    {
        player.SetActive(false);
        Camera1.SetActive(true);
        PauseUI.SetActive(false);
        FadeToEnd();
        StartCoroutine(CameraSwap01());
    }

    IEnumerator CameraSwap01()
    {
        yield return new WaitForSeconds(8);
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        StartCoroutine(CameraSwap02());
    }

    IEnumerator CameraSwap02()
    {
        yield return new WaitForSeconds(7);
        Camera2.SetActive(false);
        Camera3.SetActive(true);
        CreditsCanvas.SetActive(true);
    }

    public void FinalFade()
    {
        animator1.SetTrigger("FinalFade");
    }

    public void TheEnd()
    {
        BlackScreen.SetActive(true);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (LanternManager.GetComponent<LanternManager>().end == true)
        //{
        //    Fade.SetActive(true);
        //}
    }
}
