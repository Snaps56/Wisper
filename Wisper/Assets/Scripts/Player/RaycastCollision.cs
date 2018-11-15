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
        playerForce = GetComponent<PlayerMovement>().GetForce();
        playerVelocity = GetComponent<PlayerMovement>().GetVelocity();
        isSprinting = GetComponent<PlayerMovement>().GetIsSprinting();

        RayCaster(Vector3.right, 'x');
        RayCaster(-Vector3.right, 'x');
        RayCaster(Vector3.up, 'y');
        RayCaster(-Vector3.up, 'y');
        RayCaster(Vector3.forward, 'z');
        RayCaster(-Vector3.forward, 'z');
    }
    void RayCaster(Vector3 direction, char axis)
    {
        RaycastHit hit;
        float movementForce = 0.0f;
        float movementVelocity = 0.0f;

        if (axis == 'x')
        {
            movementForce = Mathf.Abs(playerForce.x);
            movementVelocity = Mathf.Abs(playerVelocity.x);
        }
        else if (axis == 'y')
        {
            movementForce = Mathf.Abs(playerForce.y);
            movementVelocity = Mathf.Abs(playerVelocity.y);
        }
        else
        {
            movementForce = Mathf.Abs(playerForce.z);
            movementVelocity = Mathf.Abs(playerVelocity.z);
        }

        if (Physics.Raycast(transform.position, direction, out hit, radius))
        {
            if (hit.collider.tag == "Terrain" || hit.collider.tag == "Water")
            {
                Vector3 floatVector = transform.position - hit.point;

                Vector3 repelVector = (-direction - floatVector) * repelForce;
                
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
                
                rb.AddForce(repelVector);
            }
        }
    }
}
