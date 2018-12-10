using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoFade : MonoBehaviour {

    private bool AnimOver = false;
    public Animator animatorCurtain;
    public Animator animatorDemoFade;

    private void Start()
    {
        animatorDemoFade = GetComponent<Animator>();
    }

    public void EndGame ()
    {
        AnimOver = true;
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update () {
		if ((AnimOver == true) && (Input.GetKeyDown(KeyCode.Space))) 
        {
            animatorCurtain.SetTrigger("FadeOut");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            animatorDemoFade.SetTrigger("DemoFade");
        }

    }
}
