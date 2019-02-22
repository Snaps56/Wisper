using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ShrineCleanHandler : MonoBehaviour {

    private bool withinCleanRange = false;
    private ObjectLift objectLiftScript;
    private bool playerIsSwirling = false;
    private ParticleSystem cleaningParticles;
    private bool playParticles = false;

    // Parts of the shrine that change material
    private GameObject shrineMeshMain;
    private GameObject shrineMeshInner;
    // Materials representing the clean shrine
    public Material cleanMaterialMain;
    public Material cleanMaterialInner;
    // Materials representing the dirty shrine
    public Material dirtyMaterialMain;
    public Material dirtyMaterialInner;

    private float cleanProgress = 0;
    private float cleanTick = 0.006f;
    private PersistantStateData persistantStateData;

    private OrbCount orbCountScript;
    private float currentOrbCount;
    private float minOrbRequired = 10;
    private bool doMaterialLerp = false;
    private float minCleanRequired = 1f;

    private bool firstTime = false;

    GamePadState state;
    GamePadState prevState;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;

    // Use this for initialization
    void Start () {
        shrineMeshInner = GameObject.Find("shrineInner");
        shrineMeshMain = GameObject.Find("shrineMain");
        shrineMeshMain.GetComponent<MeshRenderer>().material = dirtyMaterialMain;
        shrineMeshInner.GetComponent<MeshRenderer>().material = dirtyMaterialInner;

        objectLiftScript = GameObject.Find("Abilities Collider").GetComponent<ObjectLift>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();

        orbCountScript = GameObject.Find("Orb Collection Collider").GetComponent<OrbCount>();
        cleaningParticles = GameObject.Find("Cleaning Particles").GetComponent<ParticleSystem>();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }
    private void FixedUpdate()
    {
        if (doMaterialLerp)
        {
            cleanProgress += cleanTick;

            if (cleanProgress > minCleanRequired)
            {
                cleanProgress = 1f;
                persistantStateData.ChangeStateConditions("ShrineIsClean", true);
                persistantStateData.ChangeStateConditions("ShrineFirstTurnIn", true);
                GetComponent<SpawnOrbs>().DropOrbs();
                GamePad.SetVibration(playerIndex, 0f, 1f);
                StartCoroutine(Wait());
                doMaterialLerp = false;
            }
            shrineMeshInner.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialInner, cleanMaterialInner, cleanProgress);
            shrineMeshMain.GetComponent<MeshRenderer>().material.Lerp(dirtyMaterialMain, cleanMaterialMain, cleanProgress);
        }
    }
    // Update is called once per frame
    void Update () {
        if (!(bool)persistantStateData.stateConditions["ShrineIsClean"])
        {
            if (playerIsSwirling && withinCleanRange)
            {
                if (!cleaningParticles.isPlaying)
                {
                    cleaningParticles.Play();
                }
            }
            else
            {
                cleaningParticles.Stop();
            }
            //Debug.Log("within clean range: " + withinCleanRange + ", player is swirling: " + playerIsSwirling + ", cleaning particles is playing: " + cleaningParticles.isPlaying);
            playerIsSwirling = objectLiftScript.GetIsLiftingObjects();
            currentOrbCount = orbCountScript.GetOrbCount();
            if (withinCleanRange)
            {
                if (playerIsSwirling)
                {
                    if (currentOrbCount >= minOrbRequired)
                    {
                        doMaterialLerp = true;
                    }
                    else
                    {
                        doMaterialLerp = false;
                        if (!firstTime && (bool)persistantStateData.stateConditions["WaitingForCleanAttempt"])
                        {
                            firstTime = true;
                            Hashtable tmpHash = new Hashtable();
                            tmpHash.Add("ShrineFirstConversation2Primer", true);
                            tmpHash.Add("WaitingForCleanAttempt", false);
                            persistantStateData.ChangeStateConditions(tmpHash);
                        }
                    }
                }
            }
        }
        else
        {
            cleaningParticles.Stop();
        }
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            withinCleanRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            withinCleanRange = false;
        }
    }
}
