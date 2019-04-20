using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{

    public int index1 = -1;
    public int index2 = -1;
    public Vector3 boxCastHalfExtents;
    private RaycastHit[] hitInfo;



    private int blockingObjectsSize = 0;
    public List<GameObject> blockingObjects = new List<GameObject>();
    public GazeboManager gazebo;

    private void Awake()
    {
        this.gameObject.layer = 15;
    }
    // Start is called before the first frame update
    void Start()
    {
        gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
    }

    public void DetectBlocked()
    {
        //Debug.Log("Checking for blocked on node " + index1 + ":" + index2);
        bool blocked = false;
        
        // Only counts points as unblocked if they are above the gazebo itself
        if(DetectGazeboBelow())
        {
            hitInfo = Physics.BoxCastAll(this.gameObject.transform.position + new Vector3(0, .5f, 0), boxCastHalfExtents, new Vector3(0, 1, 0), this.gameObject.transform.parent.rotation, 3f);
            foreach (RaycastHit hitFo in hitInfo)
            {
                // Ignore any instrument detection zone of gazebo
                if (hitFo.transform.gameObject.name != "Instrument Detection Zone")
                {
                    // Ignore player
                    if (hitFo.transform.root.tag != "Player")
                    {
                        // Ignore DialogueTriggers
                        if (hitFo.collider.name != "DialogueTrigger")
                        {
                            // For NPCs, ignore their sphere collider (only utilize the capsule collider)
                            if (hitFo.transform.CompareTag("NPC"))
                            {
                                if (!(hitFo.collider is SphereCollider))
                                {
                                    Debug.Log("Found collision with " + hitFo.transform.name);
                                    blocked = true;
                                }
                            }
                            else
                            {
                                Debug.Log("Found collision with " + hitFo.transform.name);
                                blocked = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Setting node " + index1 + ":" + index2 + " to blocked as there is no gazebo below it");
            blocked = true;
        }
        
        gazebo.UpdateUnblockedPoints(this.gameObject, blocked);
    }

    public bool DetectGazeboBelow()
    {
        Debug.Log("Checking for gazebo below");
        bool gazeboBelow = false;
        hitInfo = Physics.RaycastAll(this.gameObject.transform.position, new Vector3(0, -1, 0), 2f);
        foreach(RaycastHit hitFo in hitInfo)
        {
            try
            {
                if (hitFo.transform.parent.gameObject.name == "Gazebo Colliders")
                {
                    Debug.Log("Gazebo collider found");
                    gazeboBelow = true;
                }
                else
                {
                    Debug.Log("Gazebo collider not found");
                }
            }
            catch(System.NullReferenceException e)
            {
                Debug.Log("Caught null reference exception in NavNode. Null reference when attempting to find name of " + 
                    "parent for gameobject with collider. Will assume there are no gazebo colliders below. Error Message: " + e.Message);
            }
            
        }
        return gazeboBelow;
    }
}
