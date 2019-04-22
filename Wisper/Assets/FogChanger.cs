using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogChanger : MonoBehaviour
{
    public float deepFogDensity = 0.04f;
    public float lightFogDensity = 0.0004f;
    public float changeValue = 0.001f;
    public float currentFogDensity;
    public float interVal;
    public bool nearFog;
    
    
    // Start is called before the first frame update
    void Start()
    {
        nearFog = false;
        currentFogDensity = RenderSettings.fogDensity;
        interVal = currentFogDensity;
        //RenderSettings.fogDensity = 0.04f;
    }

    // Update is called once per frame
    void Update()
    {
        currentFogDensity = RenderSettings.fogDensity;
        if (nearFog && interVal < deepFogDensity)
        {
            RenderSettings.fogDensity = interVal;
            interVal += changeValue;
            //RenderSettings.fogDensity = Mathf.Lerp(deepFogDensity, lightFogDensity, -Time.deltaTime);
        }
        else if (!nearFog && interVal > lightFogDensity)
        {
            RenderSettings.fogDensity = interVal;
            interVal -= changeValue;
            //RenderSettings.fogDensity = Mathf.Lerp(lightFogDensity, deepFogDensity, -Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In");
        //if (other.tag == "Player")
        //{
            Debug.Log("Player");
        nearFog = true;
        //RenderSettings.fogDensity = Mathf.Lerp(lightFogDensity, deepFogDensity, Time.time);
        //RenderSettings.fogDensity = deepFogDensity;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        //if (other.tag == "Player")
        //{
        Debug.Log("Player");
        nearFog = false;
        //RenderSettings.fogDensity = lightFogDensity;
        //}
    }
}