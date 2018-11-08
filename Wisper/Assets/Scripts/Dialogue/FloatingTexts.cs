﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTexts : MonoBehaviour
{

    public List<Dialogue> floatingTexts;
    private bool inRange = false;
    public int range = 8;
    

    private void Start()
    {
        inRange = false;
    }

    public bool GetInRange()
    {
        return inRange;
    }

    public void SetInRange(bool state)
    {
        inRange = state;
    }
}