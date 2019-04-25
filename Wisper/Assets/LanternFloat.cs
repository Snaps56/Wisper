using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class LanternFloat : MonoBehaviour
{

    //GameObjects
    public GameObject lantern;
    public GameObject player;
    public GameObject ability;

    //Looking at Variables
    private bool isPulling;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;
    public float deactivateDistance = 5;

    // Vibration Variables
    bool playerIndexSet = false;
    private bool TaskisDone = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    //The camera's forward vector
    private Vector3 cameraForward;

    //The vector between the task and camera
    private Vector3 toTask;

    // PTSD
    private PersistantStateData persistantStateData;

    //Rigidbody
    protected Rigidbody Rigidbody;

    public Vector3 Uplift = new Vector3(0,-1,0);

    // Start is called before the first frame update
    void Start()
    {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= deactivateDistance)
        {
            if (TaskisDone == false && ability.GetComponent<ObjectLift>().GetIsLiftingObjects() == true)
            {
                Rigidbody.useGravity = false;
                Rigidbody.AddForce(Uplift);
            }
        }
    }
}
