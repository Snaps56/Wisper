using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Option{
    public int sentenceIndex;
    public List<Choice> choices;
}

[System.Serializable]
public class Choice{
    public string reply;
    public List<TargetCondition> changeConditions;
}