using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreWayPoint : MonoBehaviour
{

    public GameObject[] wayPoint;

    // Use this for initialization
    void Start()
    {
        Physics.IgnoreCollision(wayPoint[0].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[1].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[2].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[3].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[4].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[5].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[6].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        IgnoreCollision();
    }

    void IgnoreCollision()
    {
        Physics.IgnoreCollision(wayPoint[0].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[1].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[2].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[3].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[4].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[5].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
        Physics.IgnoreCollision(wayPoint[6].GetComponent<Collider>(), this.GetComponent<SphereCollider>());
    }
}
