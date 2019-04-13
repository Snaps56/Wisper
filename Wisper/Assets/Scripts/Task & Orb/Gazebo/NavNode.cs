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
        bool blocked = false;
        hitInfo = Physics.BoxCastAll(this.gameObject.transform.position + new Vector3(0, .5f, 0), boxCastHalfExtents, new Vector3(0, 1, 0), this.gameObject.transform.parent.rotation, 3f);
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
        gazebo.UpdateUnblockedPoints(this.gameObject, blocked);
    }
}
