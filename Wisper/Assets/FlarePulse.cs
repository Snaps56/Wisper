using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlarePulse : MonoBehaviour
{
    public LensFlare lensFlare;
    public GameObject player;
    public float dotProductAngle = 0.9f;
    //The camera's forward vector
    private Vector3 cameraForward;
    //The vector between the shrine and camera
    private Vector3 toFlare;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Update the forward vector
        cameraForward = player.transform.forward;

        //Find direction between the shrine and camera
        toFlare = lensFlare.transform.position - player.transform.position;

        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toFlare.normalized) > dotProductAngle)
        {
            //Make the flare pulse

        }
    }
}
