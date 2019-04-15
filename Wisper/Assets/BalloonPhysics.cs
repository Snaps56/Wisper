using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPhysics : MonoBehaviour
{
    public bool isAnchored = false;
    public float antiGravityMod = 0.1f;
    private GameObject balloonBase;
    private GameObject balloonAnchor;
    private LineRenderer lineRenderer;
    private ParticleSystem unanchoredParticles;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        balloonBase = transform.Find("balloonBase").gameObject;
        balloonAnchor = transform.Find("balloonAnchor").gameObject;
        lineRenderer = GetComponent<LineRenderer>();
        unanchoredParticles = balloonBase.GetComponent<ParticleSystem>();

        SetHasAnchor(isAnchored);
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(-Physics.gravity * antiGravityMod);
        if (isAnchored)
        {
            RedrawLine();
        }
    }
    void RedrawLine()
    {
        Debug.Log(balloonBase.transform.position);
        Debug.Log(balloonAnchor.transform.position);
        lineRenderer.SetPosition(0, balloonBase.transform.position);
        lineRenderer.SetPosition(1, balloonAnchor.transform.position);
    }
    void SetHasAnchor(bool changeAnchor)
    {
        isAnchored = changeAnchor;
        lineRenderer.enabled = changeAnchor;
        balloonAnchor.SetActive(changeAnchor);
        if (isAnchored)
        {
            unanchoredParticles.Stop();
            balloonAnchor.GetComponent<Rigidbody>().mass = 9999;
        }
        else
        {
            unanchoredParticles.Play();
            balloonAnchor.GetComponent<Rigidbody>().mass = .0001f;
        }
        balloonAnchor.SetActive(changeAnchor);
    }
}
