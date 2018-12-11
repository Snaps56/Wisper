using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoFade : MonoBehaviour {

    private bool AnimOver = false;
    public Animator animatorCurtain;
    public Animator animatorDemoFade;
    private PersistantStateData PSData;

    private void Start()
    {
        PSData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
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
		if ((AnimOver == true) || (Input.GetKeyDown(KeyCode.Space))) 
        {
            animatorCurtain.SetTrigger("FadeOut");
        }

        if ((bool)PSData.stateConditions["DemoEnd"] || Input.GetKeyDown(KeyCode.K))
        {
            animatorDemoFade.SetTrigger("DemoFade");
        }

    }
}
