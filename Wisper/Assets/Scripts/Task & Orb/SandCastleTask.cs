using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCastleTask : MonoBehaviour {

    public Material transparentMaterial;
    public Material finalMaterial;
    public ParticleSystem sandCastleParticles;
    public ParticleSystem sandCastleFinishParticles;
    private float currentInterp = 0;
    private MeshRenderer sandMeshRenderer;
    private float materialInterpDuration;
    private float materialInterpRate = 0.01f;
    private float finishSandThreshold = 0.95f;

    private SpawnOrbs spawnOrbsScript;
    private bool finishedTask = false;

    private Color sandColor;

	// Use this for initialization
	void Start () {
        sandMeshRenderer = GetComponent<MeshRenderer>();
        sandColor = transparentMaterial.color;
        sandColor.a = 0.1f;
        sandMeshRenderer.material = transparentMaterial;

        finishedTask = (bool)PersistantStateData.persistantStateData.stateConditions["FinishedSandCastleTask"];

        if (finishedTask)
        {
            FinishSandCastleTask();
        }

        Debug.Log(materialInterpRate);
	}
	
	// Update is called once per frame
	void Update () {
        if (!finishedTask)
        {
            sandMeshRenderer.material.color = sandColor;
        }
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "SandParticle" && !finishedTask)
        {
            if (sandColor.a < finishSandThreshold)
            {
                sandColor.a += materialInterpRate;
                if (sandCastleParticles.isStopped)
                {
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
                    FinishSandCastleTask(other);
                }
            }
        }
    }
    void FinishSandCastleTask(Collider other)
    {
        sandCastleFinishParticles.Stop();
        sandMeshRenderer.material = finalMaterial;
        other.gameObject.GetComponent<ParticleSystem>().Stop();
        other.gameObject.GetComponent<Collider>().enabled = false;
        sandCastleParticles.Stop();

        spawnOrbsScript.DropOrbs();
        PersistantStateData.persistantStateData.stateConditions["FinishedSandCastleTask"] = true;
    }
    void FinishSandCastleTask()
    {
        sandCastleFinishParticles.Stop();
        sandMeshRenderer.material = finalMaterial;
        sandCastleParticles.Stop();
    }
}
