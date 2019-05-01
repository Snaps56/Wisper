using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternManager : MonoBehaviour
{

    public GameObject LanternLeader;

    public GameObject BeachLanterns;

    public GameObject Fade;

    public Vector3 Uplift = new Vector3(0, 1, 0);

    public bool end = false;

    private int count = 0;

    private List<Transform> LiftedLanters = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        //foreach (Rigidbody rb in BeachLanterns.GetComponentsInChildren<Rigidbody>())
        //{
        //    rb.mass = Random.Range(11, 13);
        //}
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        BeachLanterns.SetActive(false);
        end = false;
    }

    public void LanternLight()
    {
        LanternLeader.SetActive(true);
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
        foreach (Transform T in BeachLanterns.transform)
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
            Fade.SetActive(true);

        }
        if (end == true)
        {
            LanternLight();
            StartCoroutine(Wait());
        }
    }
}
