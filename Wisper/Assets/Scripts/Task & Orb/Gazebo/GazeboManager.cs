using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles progress of Gazebo task, signals to audio systems when to play parts of music.
public class GazeboManager : MonoBehaviour {
    public List<GameObject> gazeboMusicians;    // List of gazebo musicians
    private PersistantStateData PSD;
    private GameObject instrumentCollider;
    public List<List<GameObject>> waypoints;
    private float gazeboWidth;
    private float gazeboLength;
    private const int gridRes = 10;

	// Use this for initialization
	void Start () {
        PSD = PersistantStateData.persistantStateData;
        instrumentCollider = this.transform.Find("Instrument Detection Zone").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateWaypoints()
    {
        for(int i = 0; i < gridRes; i++)
        {
            List<GameObject> row = new List<GameObject>();
            for (int j = 0; j < gridRes; j++)
            {
                float xOffset = 0;
                float zOffset = 0;
                GameObject waypoint = new GameObject("Waypoint_" + i + "_" + j);

                if(i < gridRes / 2)        // Node is left of center
                {
                    xOffset = (gazeboWidth / gridRes) * (i - gridRes / 2); 
                }
                else if(i+1 >= gridRes/2)   // Node is right of center
                {
                    xOffset = (gazeboWidth / gridRes) * (i + (gridRes+1) / 2);
                }
                else                        // Node is exactly center (occurs when odd gridResolution is used)
                {
                    xOffset = 0;
                }

                if (j < gridRes / 2)        // Node is front of center
                {
                    zOffset = (gazeboWidth / gridRes) * (j - gridRes / 2);
                }
                else if (j + 1 >= gridRes / 2)   // Node is back of center
                {
                    zOffset = (gazeboWidth / gridRes) * (j + (gridRes + 1) / 2);
                }
                else                        // Node is exactly center (occurs when odd gridResolution is used)
                {
                    zOffset = 0;
                }


                waypoint.transform.position.Set(instrumentCollider.transform.position.x + xOffset, instrumentCollider.transform.position.y, instrumentCollider.transform.position.z + zOffset);
                row.Add(waypoint);
            }
            waypoints.Add(row);
        }
    }
}
