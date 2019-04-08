using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{

    public int index1 = -1;
    public int index2 = -1;

    private int blockingObjectsSize = 0;
    public List<GameObject> blockingObjects = new List<GameObject>();
    public GazeboManager gazebo;

    // Start is called before the first frame update
    void Start()
    {
        gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
    }

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

    private void OnTriggerEnter(Collider other)
    {
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
    }

    private void OnTriggerExit(Collider other)
    {
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
        
    }

}
