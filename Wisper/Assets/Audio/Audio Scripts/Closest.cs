using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Closest : MonoBehaviour
{
    public GameObject sphere;
    private GameObject player;
    public Renderer boundsOfRiver;
    private AudioSource riverLoop;
    public float maxDistance = 100; 
    public float percentage; //holds reference to percentage of spatial blend
    // Start is called before the first frame update
    private float minDelay = 0.01f;
    private float maxDelay = 5.0f;
    void Start()
    {
        riverLoop = GetComponent<AudioSource>();
        player = GameObject.Find("Player");

        riverLoop.playOnAwake = false;

        if (riverLoop.clip != null )
        {
            float delayTime = Random.Range(minDelay, maxDelay);
            riverLoop.PlayDelayed(delayTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sphere.transform.position = boundsOfRiver.bounds.ClosestPoint(player.transform.position);
        CalculateSpatialBlend(Vector3.Distance(sphere.transform.position, player.transform.position));
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    sphere.GetComponent<AudioSource>.Play;

    //}

    private void CalculateSpatialBlend (float distance)
    {
        if (distance / maxDistance > 1) percentage = 1;
        else percentage = distance / maxDistance;

        sphere.GetComponent<AudioSource>().spatialBlend = percentage;
    }
}
