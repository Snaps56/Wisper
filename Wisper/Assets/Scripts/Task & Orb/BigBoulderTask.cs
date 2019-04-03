using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoulderTask : MonoBehaviour {
    public GameObject parent;
    public GameObject player;
    private float rotSpeed = 3.0f;
    private float speed = 0.5f;
    private Animator animator;
    private Vector3 direction;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
       direction = player.transform.position - this.transform.position;
        if (transform.position.y >= 300 && transform.position.y <= 301)
        {
            Debug.Log("NPC");
            this.gameObject.tag = "NPC";

        }
	}

    void MoveToParent()
    {
        direction = parent.transform.position - transform.position;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        this.transform.Translate(0, 0, Time.deltaTime * speed);
    }


}
