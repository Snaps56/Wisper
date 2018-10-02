using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    public GameObject player;
    public GameObject hat;
    public GameObject[] waypoints;
    int currentWP = 0;
    float rotSpeed = 3.0f;
    float speed = 0.5f;
    float accuracyWP = 1.0f;
    float detect = 5.0f;


    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.transform.position - this.transform.position;
        if (waypoints.Length > 0)
        {

            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
            {

                currentWP++;
                if (currentWP >= waypoints.Length)
                {

                    currentWP = 0;

                }
                //else if (hat.GetComponent<ItemReturnEvent>().hatPos == )

            }

        }

        direction = waypoints[currentWP].transform.position - transform.position;
        StartCoroutine(Waitpoint());
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);


    }

    private IEnumerator Waitpoint()
    {
        yield return new WaitForSeconds(5f);
        this.transform.Translate(0, 0, Time.deltaTime * speed);


    }

}

