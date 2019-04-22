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
    }

    private void OnTriggerEnter(Collider other)
    {
        Instrument tmpInst = other.gameObject.GetComponent<Instrument>();
        if(tmpInst != null)
        {
            instrumentList.Add(tmpInst);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Instrument tmpInst = other.gameObject.GetComponent<Instrument>();
        if(tmpInst != null)
        {
            instrumentList.Remove(tmpInst);
        }
    }

    public void DestroyInstrument(GameObject instrument)
    {
        Debug.Log("Destroying instrument");
        Instrument tmpInst = instrument.GetComponent<Instrument>();
        if (tmpInst != null)
        {
            instrumentList.Remove(tmpInst);
        }
        Destroy(instrument);
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
            foreach (Instrument inst in instrumentList)
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
        }
        
    }

}
