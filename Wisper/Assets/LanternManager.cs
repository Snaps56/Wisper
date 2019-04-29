using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternManager : MonoBehaviour
{

    public GameObject LanternLeader;

    public GameObject BeachLanters;

    public Vector3 Uplift = new Vector3(0, 1, 0);

    public bool end = false;

    private int count = 0;

    private List<Transform> LiftedLanters = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        end = false;
    }

    public void LanternLight()
    {
        foreach (Rigidbody rb in LanternLeader.GetComponentsInChildren<Rigidbody>())
        {
            rb.useGravity = false;
            rb.AddForce(Uplift);
        }
    }
    // Update is called once per frame
    void Update()
    {
        bool AllAbove = true;
        foreach (Transform T in BeachLanters.transform)
        {
            if (T.position.y > 10)
            {

            }
            else
            {
                AllAbove = false;
            }
        }
        if (AllAbove == true)
        {
            end = true;
        }
        if (end == true)
        {
            LanternLight();
            StartCoroutine(Wait());
        }
    }
}
