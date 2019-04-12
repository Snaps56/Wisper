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

   
    /*
    public void CheckBlocked()
    {
        if (blockingObjectsSize != blockingObjects.Count)
        {
            if (blockingObjectsSize == 0)
            {
                blockingObjectsSize = blockingObjects.Count;
                gazebo.UpdateUnblockedPoints(this.gameObject, true);
            }
            else if (blockingObjects.Count == 0)
            {
                blockingObjectsSize = blockingObjects.Count;
                gazebo.UpdateUnblockedPoints(this.gameObject, false);
            }
            else
            {
                blockingObjectsSize = blockingObjects.Count;
            }
        }
    }
    */
    public void DetectBlocked()
    {
        bool blocked = false;
        hitInfo = Physics.BoxCastAll(this.gameObject.transform.position, boxCastHalfExtents, new Vector3(0, 1, 0), this.gameObject.transform.parent.rotation, 3f);
        foreach(RaycastHit hitFo in hitInfo)
        {
            // Ignore any instrument detection zone of gazebo
            if(hitFo.transform.gameObject.name != "Instrument Detection Zone")
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
                                //Debug.Log("Found collision with " + hitFo.transform.name);
                                blocked = true;
                            }
                        }
                        else
                        {
                            //Debug.Log("Found collision with " + hitFo.transform.name);
                            blocked = true;
                        }
                    }
                }
            }
        }
        gazebo.UpdateUnblockedPoints(this.gameObject, blocked);
    }

    /*
// Update is called once per frame
void Update()
{
   if(blockingObjectsSize != blockingObjects.Count)
   {
       if(blockingObjectsSize == 0)
       {
           blockingObjectsSize = blockingObjects.Count;
           gazebo.UpdateUnblockedPoints(this.gameObject, true);
       }
       else if(blockingObjects.Count == 0)
       {
           blockingObjectsSize = blockingObjects.Count;
           gazebo.UpdateUnblockedPoints(this.gameObject, false);
       }
       else
       {
           blockingObjectsSize = blockingObjects.Count;
       }
   }
}
*/
    /*
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.transform.root.tag != "Player")
        {
            if (other.name != "Instrument Detection Zone")
            {
                if(other.name != "DialogueTrigger")
                {
                    if (other.tag == "NPC" && !(other is SphereCollider))
                    {
                        Debug.Log("node trigger with " + other.name);
                        if (gazebo == null)
                        {
                            gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
                        }
                        blockingObjects.Add(other.gameObject);
                    }
                }
            }
        }
        /
        if (other.name != "DialogueTrigger")
        {
            if (other.tag == "NPC" && !(other is SphereCollider))
            {
                Debug.Log("node trigger with " + other.name);
                if (gazebo == null)
                {
                    gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
                }
                blockingObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if (other.transform.root.tag != "Player")
        {
            if (other.name != "Instrument Detection Zone")
            {
                if (other.name != "DialogueTrigger")
                {
                    if(other.tag == "NPC" && !(other is SphereCollider))
                    {
                        if (gazebo == null)
                        {
                            gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
                        }
                        Debug.Log("node trigger exit with " + other.name);
                        blockingObjects.Remove(other.gameObject);
                    }
                }
            }
        }
        /
        if (other.name != "DialogueTrigger")
        {
            if (other.tag == "NPC" && !(other is SphereCollider))
            {
                if (gazebo == null)
                {
                    gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
                }
                Debug.Log("node trigger exit with " + other.name);
                blockingObjects.Remove(other.gameObject);
            }
        }
    }
*/
}
