using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class LineRendererTestScript : MonoBehaviour
{

    //private Transform start;
    public GameObject end;
    private LineRenderer lineRenderer;

    private Vector3 startPos;
    private Vector3 endPos;
    // Use this for initialization
    void Start()
    {
        //LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        //start = GetComponent<Transform>();
       // end = GetComponent<Transform>();

        //startPos = start.position;
        //endPos = end.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        lineRenderer.SetPosition(0, transform.position );
        lineRenderer.SetPosition(1, end.transform.position);


       

    }

    
}