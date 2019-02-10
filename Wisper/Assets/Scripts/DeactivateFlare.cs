using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateFlare : MonoBehaviour {

    public LensFlare lensFlare;
    public GameObject player;
    private PersistantStateData PSDScript;
    public float deactivateDistance = 10;

	// Use this for initialization
	void Start () {
        PSDScript = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
	}
	
	// Update is called once per frame
	void Update () {
        lensFlare.brightness = Vector3.Distance(lensFlare.transform.position, player.transform.position);
        //Debug.Log("Distance: " + Vector3.Distance(lensFlare.transform.position, player.transform.position));
        //Debug.Log("Brightness: " + Vector3.Distance(lensFlare.transform.position, player.transform.position) / 50);
        if ((bool)PSDScript.stateConditions["TutorialFirstInteraction"])
        {
            //Turn off the flare
            this.gameObject.SetActive(false);
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= deactivateDistance)
        {
            lensFlare.brightness--;
            if (lensFlare.brightness <= 0) {
                this.gameObject.SetActive(false);
            }
        }
    }
}
