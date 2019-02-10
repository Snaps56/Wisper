using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolderDeposit : MonoBehaviour {

    public GameObject dangerDepositText;
    private Transform player;
    private OrbCount orbDeposit;
    private float depositDistance = 5;
    private float currentDistance = 999;
    private bool fadeText = false;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").transform;
        orbDeposit = GameObject.Find("Orb Collection Collider").GetComponent<OrbCount>();
    }
	
	// Update is called once per frame
	void Update () {
        currentDistance = Vector3.Distance(transform.position, player.position);
		if (currentDistance < depositDistance)
        {
            if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_A"))
            {
                if (orbDeposit.GetOrbCount() >= 50)
                {
                    dangerDepositText.SetActive(true);
                    fadeText = true;
                }
                orbDeposit.SetOrbCount(0);
            }
        }
	}
    private void FixedUpdate()
    {
       if (fadeText)
       {
            if (dangerDepositText.GetComponent<CanvasGroup>().alpha > 0)
            {
                dangerDepositText.GetComponent<CanvasGroup>().alpha -= 0.005f;
            }
       }
    }
}