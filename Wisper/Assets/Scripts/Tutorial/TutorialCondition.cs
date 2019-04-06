using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCondition : MonoBehaviour {

    // this class is used primarily as a handler / support for other tutorial classes
    // works globally for all types of tutorials

    // bool that determines if tutorial is completed
    private bool condition = false;
    
    // returns if the tutorial condition has been met
    public bool GetCondition()
    {
        return condition;
    }

    // sets the tutorial complete bool
    public void SetCondition(bool setBool)
    {
        condition = setBool;
    }
}
