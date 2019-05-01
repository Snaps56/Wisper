using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCastleTask : MonoBehaviour {
    
    public Material transparentMaterial;
    public Material finalMaterial;
    public ParticleSystem sandCastleParticles;
    public ParticleSystem sandCastleFinishParticles;
    public string psdVariable;

    public GameObject missingPiece;
    private MeshRenderer sandMeshRenderer;

    private float currentInterp = 0;
    private float materialInterpDuration;
    private float materialInterpRate = 0.005f;
    private float finishSandThreshold = 0.95f;
    private MeshCollider meshCollider;

    private SpawnOrbs spawnOrbsScript;
    private bool finishedTask = false;

    private Color sandColor;

	// Use this for initialization
	void Start () {
        sandColor = transparentMaterial.color;
        sandColor.a = 0.1f;
        spawnOrbsScript = GetComponent<SpawnOrbs>();

        sandMeshRenderer = missingPiece.GetComponent<MeshRenderer>();
        sandMeshRenderer.material = transparentMaterial;
        meshCollider = missingPiece.GetComponent<MeshCollider>();
        meshCollider.enabled = false;

        finishedTask = (bool)PersistantStateData.persistantStateData.stateConditions[psdVariable];

        if (finishedTask)
        {
            FinishSandCastleTask();
        }

        //Debug.Log(materialInterpRate);
	}
	
	// Update is called once per frame
	void Update () {
        if (!finishedTask)
        {
            sandMeshRenderer.material.color = sandColor;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("SandParticle") && !finishedTask)
        {
            sandCastleParticles.Stop();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("SandParticle") && !finishedTask)
        {
            if (sandColor.a < finishSandThreshold)
            {
                sandColor.a += materialInterpRate;
                if (sandCastleParticles.isStopped)
                {
                    Debug.Log("Play Sand Particles");
                    sandCastleParticles.Play();
                }
                if (sandColor.a < 1 && sandColor.a > finishSandThreshold)
                {
                    if (sandCastleFinishParticles.isStopped)
                    {
                        sandCastleFinishParticles.Play();
                    }
                }
            }
            else
            {
                if (sandCastleParticles.isPlaying)
                {
                    sandCastleParticles.Stop();
                    FinishSandCastleTask(other);
                }
            }
        }
    }
    void FinishSandCastleTask(Collider other)
    {
        finishedTask = true;
        if (sandCastleFinishParticles.isPlaying)
        {
            sandCastleFinishParticles.Stop();
        }
        sandMeshRenderer.material = finalMaterial;
        other.gameObject.GetComponent<ParticleSystem>().Stop();
        other.gameObject.GetComponent<Collider>().enabled = false;
        meshCollider.enabled = true;
        Debug.Log("Spawn Orbs from Sand CAstle");
        spawnOrbsScript.DropOrbs();
        PersistantStateData.persistantStateData.stateConditions[psdVariable] = true;
    }
    // overrided function called only from Start function
    void FinishSandCastleTask()
    {
        if (sandCastleFinishParticles.isPlaying)
        {
            sandCastleFinishParticles.Stop();
        }
        sandMeshRenderer.material = finalMaterial;
        finishedTask = true;
    }
}
