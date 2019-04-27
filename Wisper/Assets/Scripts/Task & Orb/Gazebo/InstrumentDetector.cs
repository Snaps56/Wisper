using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentDetector : MonoBehaviour
{
    private List<Instrument> instrumentList;
    private GazeboManager gazebo;

    private void Awake()
    {
        instrumentList = new List<Instrument>();
    }

    private void Start()
    {
        gazebo = GameObject.Find("Gazebo").GetComponent<GazeboManager>();
        GameObject i1 = new GameObject();
        i1.AddComponent<Instrument>();
        Debug.Log("instrumnet list initialized with count " + instrumentList.Count);
        instrumentList.Add(i1.GetComponent<Instrument>());
        Debug.Log("instrumnet list count after adding i1 " + instrumentList.Count);
        instrumentList.Remove(i1.GetComponent<Instrument>());
        Debug.Log("instrumnet list count after removing i1 " + instrumentList.Count);
        instrumentList.TrimExcess();
        Debug.Log("instrumnet list count after TrimExcess " + instrumentList.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        Instrument tmpInst = other.gameObject.GetComponent<Instrument>();
        if(tmpInst != null)
        {
            if(!instrumentList.Contains(tmpInst))
            {
                Debug.Log("Adding instrument " + tmpInst.name + " to instrument list");
                instrumentList.Add(tmpInst);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Instrument tmpInst = other.gameObject.GetComponent<Instrument>();
        if(tmpInst != null)
        {
            Debug.Log("Removing instrument " + tmpInst.name + " after leaving gazebo area");
            instrumentList.Remove(tmpInst);
        }
    }

    public void DestroyInstrument(GameObject instrument)
    {
        Debug.Log("Destroying instrument");
        Instrument tmpInst = instrument.GetComponent<Instrument>();
        if (tmpInst != null)
        {
            if (instrumentList.Remove(tmpInst))
            {
                Debug.Log("Removing instrument " + tmpInst.name + " after retrieved by musician");
                instrumentList.TrimExcess();
            }
            else
            {
                Debug.Log("Could not removed instrument from list in InstrumentDetector");
            }
            Destroy(instrument);
        }
        
    }

    /*
     * If velocity is below a "resting" threshold
     * If the lastPos is null or it has moved beyond an "update" threshold
     * Update lastPos and recalculate path for that musician
     */
    private void Update()
    {
        
        if(instrumentList.Count > 0)
        {
            Debug.Log("InstrumentList count is " + instrumentList.Count);
            foreach (Instrument inst in instrumentList)
            {
                if(inst != null)
                {
                    if (Vector3.Magnitude(inst.gameObject.GetComponent<Rigidbody>().velocity) < 0.01)
                    {
                        if (inst.lastPos == null)
                        {
                            Debug.Log("Instrument at rest in zone");
                            inst.lastPos = inst.gameObject.transform.position;
                            gazebo.SetMusicianPath(inst);
                        }
                        else if (Vector3.Distance(inst.lastPos, inst.gameObject.transform.position) > 0.5)
                        {
                            Debug.Log("Instrument moved and at rest in zone again");
                            inst.lastPos = inst.gameObject.transform.position;
                            gazebo.SetMusicianPath(inst);
                        }
                    }
                }
                else
                {
                    Debug.Log("Attempted to access non-existant instrument in InstrumentDetector update. List count is " + instrumentList.Count);
                }
            }
        }
        
    }

}
