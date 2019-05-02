using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeCity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!(bool)PersistantStateData.persistantStateData.stateConditions["PlayerEnteredCityFirstTime"])
        {
            PersistantStateData.persistantStateData.ChangeStateConditions("PlayerEnteredCityFirstTime", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
