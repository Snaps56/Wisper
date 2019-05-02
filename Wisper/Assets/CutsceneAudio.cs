using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAudio : MonoBehaviour
{

    public AudioSource[] riverTrackers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)PersistantStateData.persistantStateData.stateConditions["IntroCutscenePlaying"])
        {
            for(int i = 0; i < riverTrackers.Length; i++)
            {
                riverTrackers[i].volume = 0.1f;
            }
        }
        else
        {
            for (int i = 0; i < riverTrackers.Length; i++)
            {
                riverTrackers[i].volume = 0.6f;
            }
        }
    }
}
