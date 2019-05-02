using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoulderTask : MonoBehaviour {

    public GameObject parent1;
    public GameObject parent2;
    private float rotSpeed = 1.5f;
    private float speed = 1.5f;
    private Animator animator;
    private bool walk;
    private bool grounded;
    private bool dropOrbs;
    private float accuracy;

    //private Vector3 direction;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        walk = true;
        grounded = false;
        dropOrbs = true;
        accuracy = 3.0f;     
    }
	
    public bool GetGrounded()
    {
        return grounded;
    }
    public bool GetWalk()
    {
        return walk;
    }

	// Update is called once per frame
	void Update () {
        parent1.transform.LookAt(transform.position);
        parent2.transform.LookAt(transform.position);
        parent1.transform.rotation = Quaternion.Euler(new Vector3(0, parent1.transform.rotation.y * 180, 0));
        parent2.transform.rotation = Quaternion.Euler(new Vector3(0, parent2.transform.rotation.y * 180, 0));

        
        if (transform.position.y >= 270f && transform.position.y <= 274.5f 
            && transform.position.z > -680f && transform.position.z < -623f 
            && transform.position.x > -300f && transform.position.x < -134f)
        {
            grounded = true;
            MoveToSon();
            Debug.Log("NPC");
            this.gameObject.tag = "NPC";
        }
       

    }

    void MoveToSon()
    {
        //directions for the shellsters
        Vector3 direction = parent1.transform.position - transform.position;
        Vector3 direction2 = this.transform.position - parent1.transform.position;
        Vector3 direction3 = this.transform.position - parent2.transform.position;

        if (walk == true)
        {

            parent1.transform.rotation = Quaternion.Slerp(parent1.transform.rotation, 
                Quaternion.LookRotation(direction2), rotSpeed * Time.deltaTime);
            parent1.transform.Translate(0, 0, Time.deltaTime * speed);

            parent2.transform.rotation = Quaternion.Slerp(parent2.transform.rotation, 
                Quaternion.LookRotation(direction3), rotSpeed * Time.deltaTime);
            parent2.transform.Translate(0, 0, Time.deltaTime * speed);
        }

        if (Vector3.Distance(parent1.transform.position, transform.position) <= accuracy || 
            Vector3.Distance(parent2.transform.position, transform.position) <= accuracy)
        {
            walk = false;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            PersistantStateData.persistantStateData.ChangeStateConditions("BoulderBoyDown", true);

            if(dropOrbs == true)
            {
                GetComponent<SpawnOrbs>().DropOrbs();
                dropOrbs = false;
            }
            
        }
       // Debug.Log("walk " + walk);
    }
}
