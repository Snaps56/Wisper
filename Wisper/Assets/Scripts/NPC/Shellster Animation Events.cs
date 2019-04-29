using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellsterAnimationEvents: MonoBehaviour
{
    Animator anim;
    bool Walk;
    bool Idle;
    bool Fall;
    bool GetUp;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
}
