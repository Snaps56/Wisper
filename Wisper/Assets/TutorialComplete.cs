using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComplete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)PersistantStateData.persistantStateData.stateConditions["DemoEnd"])
        {
            this.gameObject.SetActive(false);
        }
    }
}
