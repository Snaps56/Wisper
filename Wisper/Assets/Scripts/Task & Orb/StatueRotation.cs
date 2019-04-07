using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRotation : MonoBehaviour
{
    public Transform rotatePoint;
    public Transform statueShift;
    //float rotation = 0f;
    //Quaternion desiredRotation;
    float xOffset;
    float yOffset;
    float zOffset;

    // Use this for initialization
    void Start()
    {
        xOffset = 0f;
        yOffset = -90f;
        transform.rotation = Quaternion.Euler(xOffset, yOffset, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        //rotation = 0f;
        //xOffset = transform.localEulerAngles.x;
        //xOffset = 0f;
        //yOffset = transform.localEulerAngles.y - 90f;
        //yOffset = -90f;
        zOffset = transform.localEulerAngles.z;
        Debug.Log("zOffset: " + transform.localEulerAngles);
        if (zOffset > 30f && zOffset < 180f)
        {
            Debug.Log("Too far +!");
            zOffset = 29f;
        }
        else if (zOffset < 330f && zOffset > 180f)
        {
            Debug.Log("Too far -!");
            zOffset = -29f;
        }

        transform.rotation = Quaternion.Euler(xOffset, yOffset, zOffset);
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
