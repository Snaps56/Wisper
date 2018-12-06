using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollison : MonoBehaviour {
    public GameObject NPC;
    public GameObject dialogueTrigger;
   
    // Use this for initialization
    void Start () {
       
    }


    // Update is called once per frame
    void Update () {
        Physics.IgnoreCollision(NPC.GetComponent<Collider>(), GetComponent<Collider>(), true);
        Physics.IgnoreCollision(dialogueTrigger.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }
}
