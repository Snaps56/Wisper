using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{

    private PersistantStateData persistantStateData;
    public GameObject orbCollectionCollider;

    // Start is called before the first frame update
    void Start()
    {
        persistantStateData = PersistantStateData.persistantStateData;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug tool to increase orb count
        if (Input.GetKeyDown(KeyCode.P))
        {
            orbCollectionCollider.GetComponent<OrbCount>().IncreaseOrbCount();
        }
        //Debug tool to decrease orb count
        else if (Input.GetKeyDown(KeyCode.O))
        {
            orbCollectionCollider.GetComponent<OrbCount>().DecreaseOrbCount();
        }
    }
}
