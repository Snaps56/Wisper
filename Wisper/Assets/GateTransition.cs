using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    public Animator fadeAnimator;
    public Transform player;
    public GameObject transitionText;
    public int nextSceneNumber;
    public float minDistance;
    AsyncOperation test;

    private float currentDistance;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update() {
        currentDistance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(currentDistance);

        if (currentDistance < minDistance)
        {
            if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_A"))
            {
                fadeAnimator.Play("FadeOut");
                StartCoroutine(LoadNewScene());
            }
        }
        else
        {
            transitionText.SetActive(false);
        }
    }
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneNumber);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
