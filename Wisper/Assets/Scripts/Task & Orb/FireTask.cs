using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FireTask : MonoBehaviour {


    // "Looking at" Variables
    public GameObject firepit;
    public GameObject player;
    public GameObject ability;
    private bool isThrowing;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;
    public float deactivateDistance = 5;

    //particle variables
    public ParticleSystem Particle_Fire;
    public ParticleSystem Particle_Ember_Flame;
    private ParticleSystem.MainModule mainModule;
    private float ParticleSize = 0;
    private float ParticleSpeed = 0;
    private int count;

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

    // Use this for initialization
    void Start()
    {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        mainModule = Particle_Ember_Flame.main;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    void FixedUpdate()
    {
        //Update the forward vector
        cameraForward = PlayerPersistance.player.transform.Find("Main Camera").transform.forward;

        //Find direction between the shrine and camera
        toTask = firepit.transform.position - PlayerPersistance.player.transform.Find("Main Camera").transform.position;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (Vector3.Dot(cameraForward.normalized, toTask.normalized) > (dotProductAngle) && distance <= deactivateDistance)
        {
            if (TaskisDone == false && PlayerPersistance.player.transform.Find("Abilities Collider").GetComponent<ObjectThrow>().GetIsThrowingObjects() == true)
            {
                count++;
                ParticleSize += 0.006f;
                ParticleSpeed += 0.01f;
                mainModule.startSize = ParticleSize;
                mainModule.startSpeed = ParticleSpeed;
                
                Debug.Log(ParticleSize);

                if (!Particle_Ember_Flame.isPlaying)
                {
                    Particle_Ember_Flame.Play();
                    
                }
            }

            if (TaskisDone == false && PlayerPersistance.player.transform.Find("Abilities Collider").GetComponent<ObjectThrow>().GetIsThrowingObjects() == false && count != 0)
            {
                count--;
                ParticleSize -= 0.006f;
                ParticleSpeed -= 0.01f;
                mainModule.startSize = ParticleSize;
                mainModule.startSpeed = ParticleSpeed;

                if (Particle_Ember_Flame.isPlaying)
                {
                    Particle_Ember_Flame.Stop();
                }
            }
        }

        //  if ((bool)persistantStateData.stateConditions["ShrineFirstConversationOver"])
        // {
        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toTask.normalized) > (dotProductAngle) && distance <= deactivateDistance)
        {
            if (TaskisDone == false && count >= 200)
            {
                // Task is complete
                TaskisDone = true;
                Particle_Fire.Play();
                Particle_Ember_Flame.Stop();
                GetComponent<SpawnOrbs>().DropOrbs();
                persistantStateData.stateConditions["FireTaskDone"] = true;
                persistantStateData.updateCount++;
                GamePad.SetVibration(playerIndex, 0f, 1f);
                StartCoroutine(Wait());
            }

        }
        //}
    }

    // Update is called once per frame
    void Update () {
        ////Update the forward vector
        //cameraForward = mainCamera.transform.forward;

        ////Find direction between the shrine and camera
        //toTask = firepit.transform.position - mainCamera.transform.position;

        //float distance = Vector3.Distance(player.transform.position, transform.position);

      ////  if ((bool)persistantStateData.stateConditions["ShrineFirstConversationOver"])
      // // {
      //      //Player is looking at shrine
      //      if (Vector3.Dot(cameraForward.normalized, toTask.normalized) > (dotProductAngle) && distance <= deactivateDistance)
      //      {
      //          if (TaskisDone == false && ability.GetComponent<ObjectThrow>().GetIsThrowingObjects() == true)
      //          {
      //              // Task is complete
      //              TaskisDone = true;
      //              fire.Play();
      //              GetComponent<SpawnOrbs>().DropOrbs();
      //              persistantStateData.stateConditions["FireTaskDone"] = true;
      //              persistantStateData.updateCount++;
      //              GamePad.SetVibration(playerIndex, 0f, 1f);
      //              StartCoroutine(Wait());
      //          }

      //      }
      //  //}
    }
}
