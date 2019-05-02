using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
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

    // AudioMixerSnapshots to fade in music (used in start for reloading scene after task is done, and in update for when task is complete) 
    public AudioMixerSnapshot NightScene_Off;
    public AudioMixerSnapshot NightScene_L1On;
    public AudioMixerSnapshot NightScene_L2On;
    public float fadeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Plays L1 by default
        NightScene_L1On.TransitionTo(fadeTime);
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
            // Starts L2 here



        }
        if (end == true)
        {
            LanternLight();
            StartCoroutine(Wait());
        }
    }
}
