using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : MonoBehaviour {

    private float RandomNum;
    public NPCMovement NPCMovements;
    private bool roll;

    private void Start()
    {
        roll = false;
    }
    // Update is called once per frame
    void Update () {    
        Stop();
    }

    private void OnTriggerEnter(Collider other)
    {     
            RandomNum = Random.Range(0, 100);
            Debug.Log("random: " + RandomNum);
        
        Debug.Log(other.name);
    }

    public void Stop()
    {       
        for (int i = 0; i < 6; i++)
        {           
            if (Vector3.Distance(this.transform.position, NPCMovements.waypoints[i].transform.position) < NPCMovements.accuracyWP)
            {              
                if (RandomNum > 50)
                {
                    NPCMovements.move = false;
                    StartCoroutine(Search());
                }
            } 
        }
    }

    IEnumerator Search()
    {       
            yield return new WaitForSeconds(4);
            NPCMovements.move = true;           
    }
}
