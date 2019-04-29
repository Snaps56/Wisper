using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Handles the musician's behaviour
public class Musician : MonoBehaviour {

    public InstrumentType instrumentPlayed;
    public List<GameObject> route;
    private GazeboManager gazebo;
    private bool hasInstrument;
    private Animator anim;
    private NPCMovement mover;
    private bool isDone;
    public Vector3 musicianPosition;
    public Quaternion musicianRotation;
    public GameObject myInstrumentModel;

    // AudioMixerSnapshots to fade in music (used in start for reloading scene after task is done, and in update for when task is complete) 
    public AudioMixerSnapshot notPlayingInstrument;
    public AudioMixerSnapshot playingInstrument;
    public float fadeTime = 3.0f;

    // Use this for initialization
    void Start () {
        musicianPosition = this.gameObject.transform.position;
        musicianRotation = this.gameObject.transform.rotation;
        gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
        hasInstrument = false;
        isDone = false;
        anim = GetComponent<Animator>();
        mover = GetComponent<NPCMovement>();
        myInstrumentModel.SetActive(false);

        //Debug.Log("Instrument is " + instrumentPlayed);
        if (instrumentPlayed.Equals(InstrumentType.Drum))
        {
           //Debug.Log("Detect musician plays drums");
            anim.SetBool("DrumPlayer", true);
            if ((bool)PersistantStateData.persistantStateData.stateConditions["DrumsGot"])
            {
                hasInstrument = true;
                isDone = true;
                myInstrumentModel.SetActive(true);
                // Fades in this musicians instrument
                playingInstrument.TransitionTo(fadeTime);
            }
        }
        else if (instrumentPlayed.Equals(InstrumentType.Saxophone))
        {
            //Debug.Log("Detect musician plays saxophone");
            anim.SetBool("SaxophonePlayer", true);
            if ((bool)PersistantStateData.persistantStateData.stateConditions["SaxGot"])
            {
                hasInstrument = true;
                isDone = true;
                myInstrumentModel.SetActive(true);
                // Fades in this musicians instrument
                playingInstrument.TransitionTo(fadeTime);
            }
        }
        else if (instrumentPlayed.Equals(InstrumentType.Tamborine))
        {
            //Debug.Log("Detect musician plays tamborine");
            anim.SetBool("TamborinePlayer", true);
            if ((bool)PersistantStateData.persistantStateData.stateConditions["TamboGot"])
            {
                hasInstrument = true;
                isDone = true;
                myInstrumentModel.SetActive(true);
                // Fades in this musicians instrument
                playingInstrument.TransitionTo(fadeTime);
            }
        }
        else if (instrumentPlayed.Equals(InstrumentType.Violin))
        {
            //Debug.Log("Detect musician plays violin");
            anim.SetBool("TamborinePlayer", true);
            if ((bool)PersistantStateData.persistantStateData.stateConditions["ViolinGot"])
            {
                hasInstrument = true;
                isDone = true;
                myInstrumentModel.SetActive(true);
                // Fades in this musicians instrument
                playingInstrument.TransitionTo(fadeTime);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(hasInstrument)
        {
            if(!mover.move && !isDone)
            {
                if(Quaternion.Angle(transform.rotation, musicianRotation) > 0.1)
                {
                    //Debug.Log("rotating musician to original facing");
                    transform.rotation = Quaternion.Slerp(transform.rotation, musicianRotation, 3 * Time.deltaTime);
                }
                else
                {
                    //Debug.Log("Setting animator HasInstrument");
                    anim.SetBool("HasInstrument", true);
                    isDone = true;
                    GetComponent<SpawnOrbs>().DropOrbs();
                    gazebo.CheckIfDone();
                    if(instrumentPlayed.Equals(InstrumentType.Drum))
                    {
                        PersistantStateData.persistantStateData.ChangeStateConditions("DrumsGot", true);
                    }
                    else if (instrumentPlayed.Equals(InstrumentType.Saxophone))
                    {
                        PersistantStateData.persistantStateData.ChangeStateConditions("SaxGot", true);
                    }
                    else if (instrumentPlayed.Equals(InstrumentType.Tamborine))
                    {
                        PersistantStateData.persistantStateData.ChangeStateConditions("TamboGot", true);
                    }
                    else if (instrumentPlayed.Equals(InstrumentType.Violin))
                    {
                        PersistantStateData.persistantStateData.ChangeStateConditions("ViolinGot", true);
                    }

                    // Fades in this musicians instrument
                    playingInstrument.TransitionTo(fadeTime);
                }
            }
        }
	}


    private void OnTriggerEnter(Collider other)
    {
       
        Instrument tmpInst = other.gameObject.GetComponent<Instrument>();
        if(tmpInst != null)
        {
            if (tmpInst.instrumentType == this.instrumentPlayed)
            {
                try
                {
                    //Debug.Log("Musician has reached instrument");
                    gazebo.gameObject.transform.Find("gazebo").Find("Instrument Detection Zone").GetComponent<InstrumentDetector>().DestroyInstrument(other.gameObject);
                    //Debug.Log("Instrument destroyed");
                    
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                finally
                {
                    hasInstrument = true;
                    myInstrumentModel.SetActive(true);
                    //Debug.Log("Creating instrument recall point");
                    GameObject tmpGO = new GameObject("Musician recall point");
                    tmpGO.AddComponent<SphereCollider>();
                    tmpGO.GetComponent<SphereCollider>().radius = 0.01f;

                    //Debug.Log("Moving instrument recall point");
                    tmpGO.transform.SetPositionAndRotation(musicianPosition, musicianRotation);
                    //Debug.Log("Setting musician to move to recall point");
                    gazebo.SetMusicianPath(this, tmpGO);
                    Destroy(tmpGO);
                }
            }
        }
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
    public void MakeAPath(GameObject target)
    {
        Debug.Log("Musician pathfinding called");
        GameObject[,] unblockedWaypoints = gazebo.unblockedWaypoints;
        GameObject[,] allWaypoints = gazebo.waypoints;
        List<LocalNodeCopy> checkNodes = new List<LocalNodeCopy>();
        List<LocalNodeCopy> closedNodes = new List<LocalNodeCopy>();

        UnblockSelf(unblockedWaypoints, allWaypoints);

        LocalNodeCopy startNode = FindClosestNode(this.gameObject, unblockedWaypoints, target.transform.position);
        LocalNodeCopy destination = FindClosestNode(target, unblockedWaypoints, this.gameObject.transform.position);

        Debug.Log("startNode set as (" + startNode.index1 + ", " + startNode.index2 + ")");
        Debug.Log("destination set as (" + destination.index1 + ", " + destination.index2 + ")");

        LocalNodeCopy checkpointNode = startNode;

        // A* algorithm
        bool done = false;
        int timeout = 1000;
        while(!done)
        {
            CheckAdjacents(checkpointNode, destination, unblockedWaypoints, checkNodes, closedNodes);
            timeout--;
            if(checkNodes.Contains(destination) || timeout < 0 || closedNodes.Count >= checkNodes.Count)
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

        if(checkNodes.Contains(destination))
        {
            // Follows linked list from end to start, adding to npcPath. Afterward, reverses order and assigns to class's route variable.
            List<GameObject> npcPath = new List<GameObject>();
            checkpointNode = destination;
            while (checkpointNode != startNode)
            {
                npcPath.Add(allWaypoints[checkpointNode.index1, checkpointNode.index2]);
                checkpointNode = checkpointNode.preceedingNode;
            }
            npcPath.Reverse();
            route = npcPath;
            this.gameObject.GetComponent<NPCMovement>().ReplaceWaypoints(route);
            anim.SetBool("Walking", true);
        }
        else
        {
            throw new System.Exception("Failed to find path with remaining attempts of " + timeout);
        }
        
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

    private LocalNodeCopy FindClosestNode(GameObject target, GameObject[,] nodeList, Vector3 directingPoint)
    {
        Debug.Log("Locating closest node to " + target.name);
        Vector3 targetPoint;
        if(directingPoint != null)
        {
            targetPoint = target.GetComponent<Collider>().ClosestPoint(directingPoint);
        }
        else
        {
            targetPoint = target.transform.position;
        }
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
                    else if (Vector3.Distance(nodeList[i, j].transform.position, targetPoint) < Vector3.Distance(closest.transform.position, targetPoint))
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
        Debug.Log("Returning node copy for " + target.name);
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
                int adjacentX = checkpoint.index1 + x;
                int adjacentZ = checkpoint.index2 + z;
                if (adjacentX < unblockedWaypoints.GetLength(0) && adjacentX >= 0 && adjacentZ < unblockedWaypoints.GetLength(1) && adjacentZ >= 0)
                {
                    if (unblockedWaypoints[checkpoint.index1 + x, checkpoint.index2 + z] != null)
                    {
                        // Create a local node copy
                        LocalNodeCopy disNode = new LocalNodeCopy(unblockedWaypoints[adjacentX, adjacentZ].GetComponent<NavNode>());
                        bool newNode = true;

                        // If it is not already in checkNodes, add it. Otherwise set disNode to be the version in checkNodes
                        foreach (LocalNodeCopy node in checkNodes)
                        {
                            if(disNode.index1 == target.index1 && disNode.index2 == target.index2)
                            {
                                disNode = target;
                            }
                            if (node.index1 == disNode.index1 && node.index2 == disNode.index2)
                            {
                                newNode = false;
                                disNode = node;
                            }
                        }
                        if (newNode)
                        {
                            checkNodes.Add(disNode);
                        }

                        // If a lower f value is found (default f values are max float value), then update the g, h, f values and preceedingNode
                        float tmpG = checkpoint.gCost + Vector3.Distance(checkpoint.position, disNode.position);
                        float tmpH = Vector3.Distance(disNode.position, target.position);
                        float tmpF = tmpG = tmpH;

                        if (tmpF < disNode.fCost)
                        {
                            disNode.gCost = tmpG;
                            disNode.hCost = tmpH;
                            disNode.fCost = tmpF;
                            disNode.preceedingNode = checkpoint;
                            // If this node was closed, reopen it
                            foreach (LocalNodeCopy node in closedNodes)
                            {
                                if (node.index1 == disNode.index1 && node.index2 == disNode.index2)
                                {
                                    closedNodes.Remove(node);
                                }
                            }
                        }

                        if (disNode == target)
                        {
                            return 0;
                        }
                    }
                }
            }
        }
        return 0;
    }
}

public enum InstrumentType { Banjo, Kazooie, Drum, Tamborine, Saxophone, Violin };

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