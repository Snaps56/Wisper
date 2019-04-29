using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachShellsterControl : MonoBehaviour
{
    public SwimMode swimMode;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Swimmode: " + swimMode);
        anim = GetComponent<Animator>();
        if(swimMode.Equals(SwimMode.Swim))
        {
            Debug.Log("Swimming");
            anim.SetBool("Swim", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum SwimMode {Swim, Surf, Splash, Tread}

