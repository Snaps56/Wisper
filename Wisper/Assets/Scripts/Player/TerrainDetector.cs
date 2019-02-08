using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour {

	public ParticleSystem waterParticles;
	public ParticleSystem sandParticles;
	public ParticleSystem grassParticles;
	private ParticleSystem terrainParticles;

	private bool nearWater;
	private bool nearSand;
	private bool nearGrass;
	private bool nearTerrain;

	// Use this for initialization
	void Start () {
		nearWater = false;
		nearSand = false;
		nearGrass = false;
		nearTerrain = false;
		terrainParticles = grassParticles;
		if(grassParticles.isPlaying) {
			grassParticles.Stop ();
		}
		if(waterParticles.isPlaying) {
			waterParticles.Stop ();
		}
		if(sandParticles.isPlaying) {
			sandParticles.Stop ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (nearWater) {
			if (!waterParticles.isPlaying) {
				waterParticles.Play ();
			}
		} else {
			if (waterParticles.isPlaying) {
				waterParticles.Stop ();
			}
		}
		if (nearSand) {
			if (!sandParticles.isPlaying) {
				sandParticles.Play ();
			}
		} else {
			if (sandParticles.isPlaying) {
				sandParticles.Stop ();
			}
		}
		if (nearGrass) {
			if (!grassParticles.isPlaying) {
				grassParticles.Play ();
			}
		} else {
			if (grassParticles.isPlaying) {
				grassParticles.Stop ();
			}
		}
*/
		if (nearTerrain) {
			if (!terrainParticles.isPlaying) {
				terrainParticles.Play ();
			}
		} else {
			if (terrainParticles.isPlaying) {
				terrainParticles.Stop ();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Water")) {
			nearWater = true;
			nearTerrain = true;
			terrainParticles = waterParticles;
			Debug.Log("Close to water");
		}
		if(other.gameObject.CompareTag("TerrainSand")) {
			nearSand = true;
			nearTerrain = true;
			terrainParticles = sandParticles;
			Debug.Log("Close to sand");
		}
		if(other.gameObject.CompareTag("Terrain")) {
			nearGrass = true;
			nearTerrain = true;
			terrainParticles = grassParticles;
			Debug.Log("Close to grass");
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.CompareTag("Water")) {
			Debug.Log("Far from water");
			nearWater = false;
			nearTerrain = false;
			//terrainParticles = null;
		}
		if(other.gameObject.CompareTag("TerrainSand")) {
			Debug.Log("Far from sand");
			nearSand = false;
			nearTerrain = false;
			//terrainParticles = null;
		}
		if(other.gameObject.CompareTag("Terrain")) {
			Debug.Log("Far from grass");
			nearGrass = false;
			nearTerrain = false;
			//terrainParticles = null;
		}
	}
}
