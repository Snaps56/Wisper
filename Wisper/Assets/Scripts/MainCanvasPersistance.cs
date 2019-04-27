using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasPersistance : MonoBehaviour
{
    public static GameObject mainCanvas;

    private void Awake()
    {
        if (mainCanvas == null)
        {
            mainCanvas = this.gameObject;
        }
        else if (mainCanvas != this)
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
