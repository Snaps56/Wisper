using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    public InstrumentType instrumentType;
    public Vector3 lastPos;

    private void Start()
    {
        if(instrumentType.Equals(InstrumentType.Drum))
        {
            if((bool)PersistantStateData.persistantStateData.stateConditions["DrumsGot"])
            {
                Destroy(this);
            }
        }
        else if(instrumentType.Equals(InstrumentType.Saxophone))
        {
            if ((bool)PersistantStateData.persistantStateData.stateConditions["SaxGot"])
            {
                Destroy(this);
            }
        }
        else if (instrumentType.Equals(InstrumentType.Tamborine))
        {
            if ((bool)PersistantStateData.persistantStateData.stateConditions["TamboGot"])
            {
                Destroy(this);
            }
        }
    }
}
