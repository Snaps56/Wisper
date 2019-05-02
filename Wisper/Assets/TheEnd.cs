using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TheEnd : MonoBehaviour
{

    public Animator animator1;

    public void FinalFade()
    {
        animator1.SetTrigger("FinalFade");
    }

    public void TheEndTransition()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H) || Input.GetButtonDown("XBOX_Button_X"))
        {
            animator1.SetTrigger("FinalFade");
        }
    }
}
