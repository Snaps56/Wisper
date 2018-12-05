using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCondition : MonoBehaviour {

    // this class can be referenced easily as a way to return whether if a tutorial has been completed
    private bool condition;
    
    public bool GetCondition()
    {
        return condition;
    }
    public void SetCondition(bool setBool)
    {
        condition = setBool;
    }
}
