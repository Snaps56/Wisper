using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the musician's behaviour
public class Musician : MonoBehaviour {

    public InstrumentType instrumentPlayed;
    public List<GameObject> route;
    private GazeboManager gazebo;

	// Use this for initialization
	void Start () {
        gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
     * A* 
     * Remove "blocked" nodes by checking for objects above them (done in NavNode.cs)
     * Unblock nodes occupied by this musician
     * Determine closest node to shellster. This is the starting node
     * Determine closest remaining node to instrument. This is the target node
     * Perform A* algorithm to find path
     *  
     */
    public void MakeAPath(Vector3 target)
    {
        GameObject[,] unblockedWaypoints = gazebo.unblockedWaypoints;
        GameObject[,] allWaypoints = gazebo.waypoints;
        List<LocalNodeCopy> checkNodes = new List<LocalNodeCopy>();
        List<LocalNodeCopy> closedNodes = new List<LocalNodeCopy>();

        UnblockSelf(unblockedWaypoints, allWaypoints);

        LocalNodeCopy startNode = FindClosestNode(this.gameObject.transform.position, unblockedWaypoints);
        LocalNodeCopy destination = FindClosestNode(target, unblockedWaypoints);

        LocalNodeCopy checkpointNode = startNode;

        // A* algorithm
        bool done = false;
        while(!done)
        {
            CheckAdjacents(checkpointNode, destination, unblockedWaypoints, checkNodes, closedNodes);
            if(checkNodes.Contains(destination))
            {
                done = true;
            }
            else
            {
                // Close this node and determine next favorable node to check. (lowest f cost, not closed)
                closedNodes.Add(checkpointNode);
                foreach(LocalNodeCopy node in checkNodes)
                {
                    if(!closedNodes.Contains(node))
                    {
                        if(closedNodes.Contains(checkpointNode))
                        {
                            checkpointNode = node;
                        }
                        else if(node.fCost < checkpointNode.fCost)
                        {
                            checkpointNode = node;
                        }
                    }
                }
            }
        }

        // Follows linked list from end to start, adding to npcPath. Afterward, reverses order and assigns to class's route variable.
        List<GameObject> npcPath = new List<GameObject>();
        checkpointNode = destination;
        while(checkpointNode != startNode)
        {
            npcPath.Add(allWaypoints[checkpointNode.index1, checkpointNode.index2]);
            checkpointNode = checkpointNode.preceedingNode;
        }
        npcPath.Reverse();
        route = npcPath;
    }

    /*
     * For all waypoints, checks to see if the musician is the only object blocking it.
     * If so add those waypoints into the list of unblocked waypoints, as the musician does not block itself.
     */
    private void UnblockSelf(GameObject[,] unblockedWaypoints, GameObject[,] allWaypoints)
    {
        for(int i = 0; i < unblockedWaypoints.GetLength(0); i++)
        {
            for(int j = 0; j < unblockedWaypoints.GetLength(1); j++)
            {
                if(unblockedWaypoints[i,j] == null)
                {
                    if(allWaypoints[i,j].GetComponent<NavNode>().blockingObjects.Contains(this.gameObject) && allWaypoints[i, j].GetComponent<NavNode>().blockingObjects.Count == 1)
                    {
                        unblockedWaypoints[i, j] = allWaypoints[i, j];
                    }
                }
            }
        }
    }

    private LocalNodeCopy FindClosestNode(Vector3 target, GameObject[,] nodeList)
    {
        GameObject closest = null;
        for(int i = 0; i < nodeList.GetLength(0); i++)
        {
            for(int j = 0; j < nodeList.GetLength(1); j++)
            {
                if(nodeList[i,j] != null)
                {
                    if(closest == null)
                    {
                        closest = nodeList[i, j];
                    }
                    else if (Vector3.Distance(nodeList[i, j].transform.position, target) < Vector3.Distance(closest.transform.position, target))
                    {
                        closest = nodeList[i, j];
                    }
                }
            }
        }
        LocalNodeCopy closestCopy = null;
        if(closest != null)
        {
            closestCopy = new LocalNodeCopy(closest.GetComponent<NavNode>());
        }
        return closestCopy;
    }

    int CheckAdjacents(LocalNodeCopy checkpoint, LocalNodeCopy target, GameObject[,] unblockedWaypoints, List<LocalNodeCopy> checkNodes, List<LocalNodeCopy> closedNodes)
    {
        // For all nodes adjacent...
        for(int x = -1; x <= 1; x++)
        {
            for(int z = -1; z <= 1; z++)
            {
                // If the node exists and is not blocked...
                if(unblockedWaypoints[checkpoint.index1 + x, checkpoint.index2 + z] != null)
                {
                    // Create a local node copy
                    LocalNodeCopy disNode = new LocalNodeCopy(unblockedWaypoints[checkpoint.index1 + x, checkpoint.index2 + z].GetComponent<NavNode>());
                    bool newNode = true;

                    // If it is not already in checkNodes, add it. Otherwise set disNode to be the version in checkNodes
                    foreach (LocalNodeCopy node in checkNodes)
                    {
                        if(node.index1 == disNode.index1 && node.index2 == disNode.index2)
                        {
                            newNode = false;
                            disNode = node;
                        }
                    }
                    if(newNode)
                    {
                        checkNodes.Add(disNode);
                    }
                    
                    // If a lower f value is found (default f values are max float value), then update the g, h, f values and preceedingNode
                    float tmpG = checkpoint.gCost + Vector3.Distance(checkpoint.position, disNode.position);
                    float tmpH = Vector3.Distance(disNode.position, target.position);
                    float tmpF = tmpG = tmpH;

                    if(tmpF < disNode.fCost)
                    {
                        disNode.gCost = tmpG;
                        disNode.hCost = tmpH;
                        disNode.fCost = tmpF;
                        disNode.preceedingNode = checkpoint;
                        // If this node was closed, reopen it
                        foreach (LocalNodeCopy node in closedNodes)
                        {
                            if(node.index1 == disNode.index1 && node.index2 == disNode.index2)
                            {
                                closedNodes.Remove(node);
                            }
                        }
                    }

                    if(disNode == target)
                    {
                        return 0;
                    }
                }
            }
        }
        return 0;
    }
}

public enum InstrumentType { Banjo, Kazooie };

public class LocalNodeCopy
{
    public Vector3 position;
    public int index1 = -1;
    public int index2 = -1;

    public float gCost = float.MaxValue;
    public float hCost = 0;
    public float fCost = float.MaxValue;

    public LocalNodeCopy preceedingNode = null;

    public LocalNodeCopy(NavNode src)
    {
        this.position = src.gameObject.transform.position;
        this.index1 = src.index1;
        this.index2 = src.index2;
    }
    
}