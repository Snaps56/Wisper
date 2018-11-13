using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFlower : MonoBehaviour {
    public Animator flowerGrow;
    public GameObject flower;
    public bool grown;


    // Use this for initialization
    void Start()
    {
        grown = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Flower
        if ((Input.GetKey(KeyCode.G)))
        {
            if (!grown)
            {
                //Debug.Log("MapNotActive");
                //miniMap.SetActive(false);
                flowerGrow.SetBool("Grow", true);
                grown = true;
            }
            else
            {
                //Debug.Log("Map");
                //miniMap.SetActive(true);
                flowerGrow.SetBool("Grow", false);
                grown = false;
            }
        }

    }
}
