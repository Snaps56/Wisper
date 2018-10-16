using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMoving : MonoBehaviour {

    public GameObject leftSwing;
    public GameObject rightSwing;
    //public GameObject youngShellster;

    public float rotateX;
    public float maxHeight;
    private bool move;
	// Use this for initialization
	void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {


       
        leftSwing.transform.Rotate(Vector3.right, rotateX);
        leftSwing.transform.Translate(Vector3.down *maxHeight);       
        
           
        
    }

   
}
