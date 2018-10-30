using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{

    public GameObject leftSwing;
    public GameObject rightSwing;
    public GameObject topBeam;
    //public GameObject youngShellster;

    public float rotateX;
    public float speed;
    private bool moveFoward;
    public float maxY;
    public float maxZ;
    public float minZ;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
        rightSwing.transform.RotateAround(topBeam.transform.position, Vector3.forward, 20 * Time.deltaTime * speed);
        
        if(leftSwing.transform.position.y <= maxY && leftSwing.transform.position.z <= maxZ)
        {
            leftSwing.transform.RotateAround(topBeam.transform.position, Vector3.forward, 20 * Time.deltaTime * speed);
        }
        
        if (leftSwing.transform.position.y >= maxY && leftSwing.transform.position.z >= maxZ)
        {
            leftSwing.transform.RotateAround(topBeam.transform.position, Vector3.back, 20 * Time.deltaTime * speed);
        }
        
        /*
        if (leftSwing.transform.position.y == maxHeight)
        {
            moveFoward = false;
        }

        if (moveFoward == false)
        {
            leftSwing.transform.RotateAround(topBeam.transform.position, Vector3.forward, 20 * Time.deltaTime * speed);
        }
        */

    }


}



