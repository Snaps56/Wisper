using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPhysics : MonoBehaviour
{
    public bool isAnchored = false;
    public float antiGravityMod = 0.1f;

    public GameObject overrideAnchorObject;
    public bool overrideAnchor = false;

    private MeshRenderer meshRenderer;

    private GameObject balloonBase;
    private GameObject balloonAnchor;
    private LineRenderer lineRenderer;
    private ParticleSystem unanchoredParticles;
    private SpringJoint springJoint;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Color color = new Color(Random.value * 1.5f, Random.value * 1.5f, Random.value * 1.5f);
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
        rb = GetComponent<Rigidbody>();

        balloonBase = transform.Find("balloonBase").gameObject;

        if (overrideAnchor)
        {
            balloonAnchor = overrideAnchorObject;
        }
        else
        {
            balloonAnchor = transform.Find("balloonAnchor").gameObject;
        }
        lineRenderer = GetComponent<LineRenderer>();
        unanchoredParticles = balloonBase.GetComponent<ParticleSystem>();
        springJoint = GetComponent<SpringJoint>();

        SetHasAnchor(isAnchored);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnchored)
        {
            RedrawLine();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAnchored = !isAnchored;
            SetHasAnchor(isAnchored);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(-Physics.gravity * antiGravityMod);
    }
    void RedrawLine()
    {
        //Debug.Log(balloonBase.transform.position);
        //Debug.Log(balloonAnchor.transform.position);
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
            springJoint.connectedBody = balloonAnchor.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("Disabled Joint");
            unanchoredParticles.Play();
            balloonAnchor.GetComponent<Rigidbody>().mass = .0001f;
            springJoint.connectedBody = null;
        }
        balloonAnchor.SetActive(changeAnchor);
    }
}
