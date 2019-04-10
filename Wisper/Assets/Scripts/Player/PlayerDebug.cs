using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{

    private PersistantStateData psd;
    public GameObject orbCollectionCollider;
    private bool debugMode = false;
    // Start is called before the first frame update
    void Start()
    {
        psd = PersistantStateData.persistantStateData;
    }

    // Update is called once per frame
    void Update()
    {
        debugMode = psd.enableDebugMode;
        //Debug tool to increase orb count
        if (Input.GetKeyDown(KeyCode.P) && debugMode)
        {
            orbCollectionCollider.GetComponent<OrbCount>().IncreaseOrbCount();
        }
        //Debug tool to decrease orb count
        else if (Input.GetKeyDown(KeyCode.O) && debugMode)
        {
            orbCollectionCollider.GetComponent<OrbCount>().DecreaseOrbCount();
        }
    }
}
