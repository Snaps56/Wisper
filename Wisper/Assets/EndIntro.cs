using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndIntro : MonoBehaviour {

    public VideoPlayer video;
	// Use this for initialization
	void Start () {
        video.loopPointReached += EndReached;
	}
	
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }
}
