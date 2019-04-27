using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinePersistance : MonoBehaviour
{
    public static GameObject shrine;

    private void Awake()
    {
        if (shrine == null)
        {
            shrine = this.gameObject;
        }
        else if (shrine != this)
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
