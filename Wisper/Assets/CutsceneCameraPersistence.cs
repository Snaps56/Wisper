using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCameraPersistence : MonoBehaviour
{

    public static GameObject cutsceneCamera;

    private void Awake()
    {
        if (cutsceneCamera == null)
        {
            cutsceneCamera = this.gameObject;
        }
        else if (cutsceneCamera != this)
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
