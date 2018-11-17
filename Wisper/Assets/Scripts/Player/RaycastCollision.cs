using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollision : MonoBehaviour {

    public float repelForce;

    private Rigidbody rb;
    private SphereCollider thisCollider;
    private float radius;

    private Vector3 playerForce;
    private Vector3 playerVelocity;
    private bool isSprinting;
	
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        thisCollider = GetComponent<SphereCollider>();
        radius = thisCollider.radius;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // 
        playerForce = GetComponent<PlayerMovement>().GetForce();
        playerVelocity = GetComponent<PlayerMovement>().GetVelocity();
        isSprinting = GetComponent<PlayerMovement>().GetIsSprinting();

        // add raycast detection to all 3 axis of movement
        RayCasterDiagonal(Vector3.right);
        RayCasterDiagonal(-Vector3.right);

        RayCasterDiagonal(Vector3.forward);
        RayCasterDiagonal(-Vector3.forward);

        RayCasterDiagonal(Vector3.up);
        RayCasterDiagonal(-Vector3.up);

        // add raycast detection for all flat diagonals
        RayCasterDiagonal((Vector3.right + Vector3.forward).normalized);
        RayCasterDiagonal((Vector3.right + -Vector3.forward).normalized);
        RayCasterDiagonal((-Vector3.right + Vector3.forward).normalized);
        RayCasterDiagonal((-Vector3.right + -Vector3.forward).normalized);

        RayCasterDiagonal((Vector3.up + Vector3.forward).normalized);
        RayCasterDiagonal((Vector3.up + -Vector3.forward).normalized);
        RayCasterDiagonal((-Vector3.up + Vector3.forward).normalized);
        RayCasterDiagonal((-Vector3.up + -Vector3.forward).normalized);

        // add raycast detection for all perfect diagonals
        RayCasterDiagonal((Vector3.up + Vector3.forward + Vector3.right).normalized);
        RayCasterDiagonal((Vector3.up + Vector3.forward + -Vector3.right).normalized);
        RayCasterDiagonal((Vector3.up + -Vector3.forward + Vector3.right).normalized);
        RayCasterDiagonal((Vector3.up + -Vector3.forward + -Vector3.right).normalized);

        RayCasterDiagonal((-Vector3.up + Vector3.forward + Vector3.right).normalized);
        RayCasterDiagonal((-Vector3.up + Vector3.forward + -Vector3.right).normalized);
        RayCasterDiagonal((-Vector3.up + -Vector3.forward + Vector3.right).normalized);
        RayCasterDiagonal((-Vector3.up + -Vector3.forward + -Vector3.right).normalized);
    }

    // creates a raycast that detects "collision" and adds a force in the opposite direction
    void RayCasterDiagonal(Vector3 direction)
    {
        RaycastHit hit;

        // find dot product for player speed and momentum in direction of vector
        float movementForce = Vector3.Dot(playerForce, direction);
        float movementVelocity = Vector3.Dot(playerVelocity, direction);

        // Use generated raycasts for collision detection
        if (Physics.Raycast(transform.position, direction, out hit, radius))
        {
            // "collide" only when collider is detecting an object that has these tags
            if (hit.collider.tag == "Terrain" || hit.collider.tag == "Water")
            {
                // calculate repel force via distance from ground
                Vector3 floatVector = transform.position - hit.point;
                Vector3 repelVector = (-direction - floatVector) * repelForce;

                // modify repel force based on player's velocity and momentum

                if (movementVelocity > 1)
                {
                    repelVector += repelVector * movementVelocity;
                }
                else
                {
                    repelVector += repelVector;
                }

                if (movementForce > 0)
                {
                    repelVector *= movementForce;
                }

                // apply repel force to player
                rb.AddForce(repelVector);
            }
        }
    }
}
