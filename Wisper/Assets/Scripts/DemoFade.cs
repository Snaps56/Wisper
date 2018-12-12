using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoFade : MonoBehaviour {

    private bool AnimOver;

    public Animator animatorCurtain;
    public Animator animatorDemoFade;
    private PersistantStateData PSData;

	// Use this for initialization
	void Start () {
        PSData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        animatorDemoFade = GetComponent<Animator>();
	}
	
    public void EndGame()
    {
        AnimOver = true;
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        if (AnimOver)
        {
            animatorCurtain.SetTrigger("FadeOut");
        }

        if ((bool)PSData.stateConditions["DemoEnd"])
        {
            animatorDemoFade.SetTrigger("DemoFade");
        }
    }
}
