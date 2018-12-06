using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineDialogueTimeListener : MonoBehaviour {

    private PersistantStateData psd;
    private DialogueManager dm;
    private float sceneInitializeTime;
    private float followTheLightStartTime = 0;
    private float followTheLightCompleteTime = 0;

    private float followTheLightPart2Time = 3;
    private float areYouFollowingTheLightRepeaterTime = 60;

    private bool disableAreYouFollowingTheLightRepeaterTime = false;
    private bool disableFindTheShrine = false;
    // Use this for initialization
    void Start () {
		psd = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        sceneInitializeTime = psd.globalTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (!disableFindTheShrine)
        {
            if ((followTheLightCompleteTime != 0) && (psd.globalTime - followTheLightCompleteTime) > followTheLightPart2Time && (psd.globalTime - followTheLightCompleteTime) < (followTheLightPart2Time + 2))
            {
                if(!(bool)psd.stateConditions["StartupShrinePart2"])
                {
                    psd.ChangeStateConditions("StartupShrinePart2", true);
                }
            }
            else if ((psd.globalTime - followTheLightCompleteTime) > 30 && (psd.globalTime - followTheLightCompleteTime) % areYouFollowingTheLightRepeaterTime < 0.1 && !disableAreYouFollowingTheLightRepeaterTime)
            {
                disableAreYouFollowingTheLightRepeaterTime = true;
                
                Hashtable tmpHash = new Hashtable();
                tmpHash.Add("ShrineFirstConversation", false);
                tmpHash.Add("StartupShrineRepeatDirections", true);
                psd.ChangeStateConditions(tmpHash);
                disableAreYouFollowingTheLightRepeaterTime = false;
            }

            if (dm.GetActiveDialogue() != null)
            {
                if (dm.GetActiveDialogue().dialogueName == "First Shrine Dialogue")
                {
                    disableAreYouFollowingTheLightRepeaterTime = true;
                    disableFindTheShrine = true;
                }
            }

            if(followTheLightCompleteTime == 0)
            {
                if (followTheLightStartTime == 0)
                {
                    if ((bool)psd.stateConditions["StartupShrineDialogue"] == true)
                    {
                        followTheLightStartTime = psd.globalTime;
                    }
                }
                else if ((bool)psd.stateConditions["StartupShrineDialogue"] == false)
                {
                    Debug.Log("followTheLightCompleteTime set");
                    followTheLightCompleteTime = psd.globalTime;
                }
            }
        }
        

	}
}
