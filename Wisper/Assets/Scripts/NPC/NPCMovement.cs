using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> waypoints;
    public GameObject waypointParent;
    int currentWP = 0;
    public float rotSpeed = 3.0f;
    public float speed = 0.5f;
    public float accuracyWP = 1.0f;

    public bool move = true;
    public bool loop = true;    // Loop movement around waypoints or stop at final waypoint (clears waypoints at end if false)

    private void Awake()
    {
        if (waypointParent != null)
        {
            foreach (Transform child in waypointParent.transform)
            {
                waypoints.Add(child.gameObject);
                //Debug.Log("Added waypoint " + child.name);
            }
            OrderWaypoints();
            /*foreach(GameObject wp in waypoints)
            {
                Debug.Log("Order waypoint is " + wp.name);
            }*/
        }
    }

    // Use this for initialization
    void Start()
    {
        player = PlayerPersistance.player.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - this.transform.position;

        //Movement loop
        if (waypoints.Count > 0)
        {
            try
            {
                lock (waypoints)
                {
                    //If move, transform npc toward waypoint (rot and pos with interpolation)
                    if (move == true)
                    {
                        direction = waypoints[currentWP].transform.position - transform.position;
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                        this.transform.Translate(0, 0, Time.deltaTime * speed);
                    }

                    if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
                    {
                        currentWP++;
                    }
                    //Debug.Log("current wp is " + currentWP);
                    if (currentWP >= waypoints.Count)
                    {
                        Debug.Log("Setting current wp to 0");
                        // If at end of waypoints, set next waypoint to 0
                        currentWP = 0;
                        // If not loop, clear waypoint
                        
                        if(!loop)
                        {
                            //Debug.Log("Clearing current wp");
                            waypoints.Clear();

                            // Attempt to turn off the Walking variable on this objects animator
                            try
                            {
                                Debug.Log("Setting animator Walking bool false");
                                GetComponent<Animator>().SetBool("Walking", false);
                            }
                            catch(System.Exception e)
                            {
                                Debug.LogError(e);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                //Debug.Log("Exception caught in NPC movement: " + e.Message);
            }
        }
        else
        {
            move = false;
        }
    }

    public void ReplaceWaypoints(List<GameObject> newPoints)
    {
        //Debug.Log("Replacing npc waypoints through script");
        StartCoroutine(SwapWaypoints(newPoints));
    }

    // Replace the waypoint list with a new list
    IEnumerator SwapWaypoints(List<GameObject> newPoints)
    {
        if (System.Threading.Monitor.TryEnter(waypoints, 5000))
        {
            waypoints = newPoints;
            currentWP = 0;
            move = true;
        }
        else
        {
            throw new System.TimeoutException("Timeout in NPCMovement SwapWaypoints. Timeout after 5000 milli.");
        }
        yield return 0;
    }


    private void OrderWaypoints()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            WaypointIndex waypointIndex = waypoints[i].GetComponent<WaypointIndex>();
            if (waypointIndex != null)
            {
                if(waypointIndex.wpIndex != i)
                {
                    try
                    {
                        GameObject tmpWp = waypoints[waypointIndex.wpIndex];
                        waypoints[waypointIndex.wpIndex] = waypoints[i];
                        waypoints[i] = tmpWp;
                    }
                    catch(System.Exception e)
                    {
                        Debug.LogError("Exception thrown while ordering waypoints.\n"
                            + "Likely a waypoint is incorrectly index and caused an out of bounds exception.\n"
                            + "Here is the exception thrown: " + e);
                    }
                    
                }
            }
        }
    }

    public int getCurrentWP()
    {
        return currentWP;
    }
}