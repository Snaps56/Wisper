using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLift : MonoBehaviour {

    public Collider radiusCollider;

    private bool isLiftingObjects = false;
    List<GameObject> liftedObjects = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLiftingObjects = true;
        }
        if (isLiftingObjects && Input.GetMouseButtonUp(1))
        {
            isLiftingObjects = false;
        }

        if (isLiftingObjects)
        {
            /*
            for (int i = 0; i < liftedObjects.Count; i++)
            {
                liftedObjects[i].transform.parent = transform;
            }
            */
        }
        else
        {
            Debug.Log("Drop Objects");
            liftedObjects.Clear();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp")
        {
            if (isLiftingObjects)
            {
                addToLiftedObjects(other);
            }
        }
    }
    void addToLiftedObjects(Collider other)
    {
        Debug.Log("Size of List = " + liftedObjects.Count);
        /*
        if (liftedObjects.Count > 0)
        {
            for (int i = 0; i < liftedObjects.Count; i++)
            {
                if (other.transform.gameObject != liftedObjects[i])
                {
                    liftedObjects.Add(other.transform.gameObject);
                }
            }
        }
        else
        {
            liftedObjects.Add(other.transform.gameObject);
        }
        */
        liftedObjects.Add(other.transform.gameObject);
    }
}
