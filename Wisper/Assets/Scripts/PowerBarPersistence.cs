using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBarPersistence : MonoBehaviour
{
    public static GameObject powerbar;

    private void Awake()
    {
        if (powerbar == null)
        {
            powerbar = this.gameObject;
        }
        else if (powerbar != this)
        {
            //Debug.Log("PSD static self reference not null. Destroying this.");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
