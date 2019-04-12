using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode()]
public class LineRenderer : MonoBehaviour
{

    private Transform start;
    public Transform end;
    //public LineRenderer lineRenderer;

    private Vector3 startPos;
    private Vector3 endPos;
    // Use this for initialization
    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        start = GetComponent<Transform>();
        end = GetComponent<Transform>();

        startPos = start.position;
        endPos = end.position;
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.SetPosition(0, startPos);
        //lineRenderer.SetPosition(1, endPos);
        //lineRenderer.SetPosition(1, end.position);

       

    }

    
}