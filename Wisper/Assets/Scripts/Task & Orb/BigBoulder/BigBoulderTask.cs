using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoulderTask : MonoBehaviour {

    public GameObject parent1;
    public GameObject parent2;
    private float rotSpeed = 3.0f;
    private float speed = 1.5f;
    private Animator animator;
    private bool walk;
    private bool rotate;
    private float accuracy;

    //private Vector3 direction;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        walk = true;
        rotate = false;
        accuracy = 2.5f;     
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

        if (transform.position.y >= 300 && transform.position.y <= 301 && transform.position.z < -701.5 && transform.position.z > -770)
        {
            MoveToParent();
            //Debug.Log("NPC");
            this.gameObject.tag = "NPC";
        }
        else
        {
           // Debug.Log("Still needs Help!!!");
        }      
    }

    void MoveToParent()
    {
        //directions for the shellsters
        Vector3 direction = parent1.transform.position - transform.position;
        Vector3 direction2 = this.transform.position - parent1.transform.position;
        Vector3 direction3 = this.transform.position - parent2.transform.position;

        // parent shellsters look in the direction of their son
        //parent1.transform.rotation = Quaternion.Slerp(parent1.transform.rotation, Quaternion.LookRotation(direction2), rotSpeed * Time.deltaTime);
        //parent2.transform.rotation = Quaternion.Slerp(parent2.transform.rotation, Quaternion.LookRotation(direction3), rotSpeed * Time.deltaTime);


        if (walk == true)
        {

            parent1.transform.rotation = Quaternion.Slerp(parent1.transform.rotation, Quaternion.LookRotation(direction2), rotSpeed * Time.deltaTime);
            parent1.transform.Translate(0, 0, Time.deltaTime * speed);

            parent2.transform.rotation = Quaternion.Slerp(parent2.transform.rotation, Quaternion.LookRotation(direction3), rotSpeed * Time.deltaTime);
            parent2.transform.Translate(0, 0, Time.deltaTime * speed);
        }

        if (Vector3.Distance(parent1.transform.position, transform.position) <= accuracy || Vector3.Distance(parent2.transform.position, transform.position) <= accuracy)
        {
            walk = false;
            PersistantStateData.persistantStateData.ChangeStateConditions("BoulderBoyDown", true);
        }
        Debug.Log("walk " + walk);
    }
}
