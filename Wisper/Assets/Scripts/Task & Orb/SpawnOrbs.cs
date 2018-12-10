using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbs : MonoBehaviour
{
    public GameObject orb;
    public Transform player;

    public float orbCount;
    public float initialForceMultiplier;
    public float moveSpeedMultiplier;
    public float slowMultiplier;
    public float orbSpawnOffsetY;

	private GameObject orbInstance;

    List<GameObject> orbsList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debuggging drop orbs
        if (Input.GetButtonDown("PC_Key_Orb"))
        {
            DropOrbs();
        }
        // make sure orbs list does not contain emptry indices
        orbsList.TrimExcess();
    }
    private void FixedUpdate()
    {
        // for every orb generated
        for (int i = 0; i < orbsList.Count; i++)
        {
            // make sure index in array is not null
            if (orbsList[i] != null)
            {
                // obtain vector of orb and player
                Rigidbody rb = orbsList[i].GetComponent<Rigidbody>();
                Vector3 deltaPosition = player.position - orbsList[i].GetComponent<Transform>().position;
                deltaPosition = deltaPosition.normalized;

                // obtain vector of player and object to scale speed
                float distanceToBase = (player.position - transform.position).magnitude;

                Vector3 finalForce = deltaPosition * moveSpeedMultiplier * distanceToBase;
                Vector3 minForce = deltaPosition * moveSpeedMultiplier * 10f;
                if (finalForce.magnitude < minForce.magnitude)
                {
                    finalForce = minForce;
                }
                rb.AddForce(finalForce);
                rb.AddForce(-rb.velocity * slowMultiplier);
            }
        }
    }
    public void DropOrbs (){
		for (int i = 0; i < orbCount; i++) {
            // Vector3 initialForce = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            Vector3 playerVector = player.transform.position - transform.position;

            Vector3 crossVector = Vector3.ProjectOnPlane(Random.insideUnitSphere, playerVector);
            Vector3 initialForce = crossVector.normalized;
            initialForce.y = Mathf.Abs(initialForce.y);
            initialForce = initialForce * initialForceMultiplier;

            Vector3 orbSpawnPosition = transform.position;
            orbSpawnPosition.y += orbSpawnOffsetY;
            GameObject orbInstance = Instantiate(orb, orbSpawnPosition, Quaternion.identity);
            orbsList.Add(orbInstance);

            orbInstance.GetComponent<Rigidbody>().AddForce(initialForce);
            // orbInstance.GetComponent<OrbSequence> ().setDestination (player, "player");
        }
	}
}
