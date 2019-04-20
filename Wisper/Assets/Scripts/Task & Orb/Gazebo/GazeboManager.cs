using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles progress of Gazebo task, signals to audio systems when to play parts of music.
public class GazeboManager : MonoBehaviour {
    public List<GameObject> gazeboMusicians;    // List of gazebo musicians
    private PersistantStateData PSD;
    private GameObject instrumentCollider;
    public GameObject[,] waypoints;
    public GameObject[,] unblockedWaypoints;
    private float gazeboWidth;
    private float gazeboLength;
    private GameObject waypointParent;
    private const int gridRes = 20;
    public bool pathfindingInProgress = false;

	// Use this for initialization
	void Start () {
        PSD = PersistantStateData.persistantStateData;
        instrumentCollider = this.transform.Find("Instrument Detection Zone").gameObject;
        gazeboWidth = Vector3.Dot(instrumentCollider.GetComponent<Collider>().bounds.extents, new Vector3(1,0,1));
        gazeboLength = Vector3.Dot(instrumentCollider.GetComponent<Collider>().bounds.extents, new Vector3(1, 0, 1));
        waypointParent = new GameObject("waypoint parent");
        waypoints = new GameObject[gridRes, gridRes];
        unblockedWaypoints = new GameObject[gridRes, gridRes];
        GenerateWaypoints();
	}
	
	// Update is called once per frame
	void Update () {
        /*
		for(int i = 0; i < gridRes; i++)
        {
            for(int j = 0; j < gridRes; j++)
            {
                if(unblockedWaypoints[i,j] == null)
                {
                    waypoints[i, j].GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else
                {
                    waypoints[i, j].GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }
        }
        */
	}

    void GenerateWaypoints()
    {
        Debug.Log("instrument detector x:y:z " + instrumentCollider.transform.position.x + ":" + instrumentCollider.transform.position.y + ":" + instrumentCollider.transform.position.z);
        
        waypointParent.transform.position = instrumentCollider.transform.position;
        
        Debug.Log("waypoint parent x:y:z " + waypointParent.transform.position.x + ":" + waypointParent.transform.position.y + ":" + waypointParent.transform.position.z);
        for (int i = 0; i < gridRes; i++)
        {
            for (int j = 0; j < gridRes; j++)
            {
                float xOffset = 0;
                float zOffset = 0;
                GameObject waypoint = new GameObject("Waypoint_" + i + "_" + j);
                waypoint.transform.parent = waypointParent.transform;

                if(i < gridRes / 2)        // Node is left of center
                {
                    xOffset = (gazeboWidth / gridRes) * (i - gridRes / 2); 
                }
                else if(i >= (gridRes + 1) / 2)   // Node is right of center
                {
                    xOffset = (gazeboWidth / gridRes) * (i - (gridRes) / 2);
                }
                else                        // Node is centered (occurs when odd gridResolution is used)
                {
                    Debug.Log("Centered node");
                    xOffset = 0;
                }

                if (j < gridRes / 2)        // Node is front of center
                {
                    zOffset = (gazeboWidth / gridRes) * (j - gridRes / 2);
                }
                else if (j  >= (gridRes + 1) / 2)   // Node is back of center
                {
                    zOffset = (gazeboWidth / gridRes) * (j - (gridRes) / 2);
                }
                else                        // Node is centered (occurs when odd gridResolution is used)
                {
                    Debug.Log("Centered node");
                    zOffset = 0;
                    Debug.Log("This should be working...");
                }

                if(gridRes % 2 == 0)
                {
                    xOffset += 0.5f * (gazeboWidth / gridRes);
                    zOffset += 0.5f * (gazeboWidth / gridRes);
                }

                

                waypoint.transform.position = instrumentCollider.transform.position + new Vector3(xOffset, 0, zOffset);

                
                waypoint.transform.localRotation = Quaternion.Euler(0, 0, 0);

                /*
                waypoint.AddComponent<MeshFilter>();
                waypoint.AddComponent<MeshRenderer>();
                GameObject tmpSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                waypoint.GetComponent<MeshFilter>().mesh = tmpSphere.GetComponent<MeshFilter>().mesh;
                Destroy(tmpSphere);
                */
                waypoint.AddComponent<NavNode>();
                NavNode wpNavNode = waypoint.GetComponent<NavNode>();
                wpNavNode.index1 = i;
                wpNavNode.index2 = j;
                wpNavNode.boxCastHalfExtents = new Vector3(gazeboWidth / (gridRes + 2), 0.5f, gazeboLength / (gridRes + 2));
                waypoints[i, j] = waypoint;
                unblockedWaypoints[i, j] = waypoint;
            }
        }
        waypointParent.transform.rotation = this.transform.rotation;
    }

    public void UpdateUnblockedPoints(GameObject node, bool blocked)
    {
        NavNode nvNode = node.GetComponent<NavNode>();
        if(nvNode.index1 > -1 && nvNode.index2 > -1)
        {
            if(blocked)
            {
                unblockedWaypoints[nvNode.index1, nvNode.index2] = null;
            }
            else
            {
                unblockedWaypoints[nvNode.index1, nvNode.index2] = node;
            }
        }
    }

    public void SetMusicianPath(Instrument instrument)
    {
        Debug.Log("Looking for musician to set path");
        foreach(GameObject musician in gazeboMusicians)
        {
            if(musician.GetComponent<Musician>().instrumentPlayed == instrument.instrumentType)
            {
                for(int i = 0; i < gridRes; i++)
                {
                    for(int j = 0; j < gridRes; j++)
                    {
                        waypoints[i, j].GetComponent<NavNode>().DetectBlocked();
                    }
                }
                Debug.Log("Musician found");
                musician.GetComponent<Musician>().MakeAPath(instrument.gameObject);
            }
        }
        
    }
}
