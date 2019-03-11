using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineDialogueTimeListener : MonoBehaviour {

    private PersistantStateData psd;
    private DialogueManager dm;
    private float sceneInitializeTime;
    private float followTheLightStartTime = 0;
    private float followTheLightCompleteTime = 0;

    private float followTheLightPart2Time = 2.5f;
    private float areYouFollowingTheLightRepeaterTime = 90;

    private bool disableAreYouFollowingTheLightRepeaterTime = false;
    private bool disableFindTheShrine = false;

    private float firstConversationPrimerActivationTime = 0;
    private float failedToCleanShrineDialogueDelay = 3f;
    private bool disableFirstConversation = false;
    // Use this for initialization
    void Start () {
		psd = PersistantStateData.persistantStateData;
        dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        sceneInitializeTime = psd.globalTime;

        if((bool)psd.stateConditions["ShrineFirstConversationOver"] || (bool)psd.stateConditions["WaitingForCleanAttempt"] || (bool)psd.stateConditions["ShrineFirstConversation2"])
        {
            disableAreYouFollowingTheLightRepeaterTime = true;
        }

        if((bool)psd.stateConditions["ShrineIsClean"])
        {
            disableFirstConversation = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!disableFindTheShrine)
        {
            // Activate StartupShrinePart2 2.5 seconds after part 1
            if ((followTheLightCompleteTime != 0) && (psd.globalTime - followTheLightCompleteTime) > followTheLightPart2Time && (psd.globalTime - followTheLightCompleteTime) < (followTheLightPart2Time + 2))
            {
                if(!(bool)psd.stateConditions["StartupShrinePart2"])
                {
                    psd.ChangeStateConditions("StartupShrinePart2", true);
                }
            }
            else if ((psd.globalTime - followTheLightCompleteTime) > 30 && (psd.globalTime - followTheLightCompleteTime) % areYouFollowingTheLightRepeaterTime < 0.1 && !disableAreYouFollowingTheLightRepeaterTime)
            {
                // 30 seconds after "follow the light" dialogue and repeating every 90 seconds while not disabled, play the repeating "are you following the light" dialogue
                disableAreYouFollowingTheLightRepeaterTime = true;
                
                Hashtable tmpHash = new Hashtable();
                tmpHash.Add("ShrineFirstConversation", false);
                tmpHash.Add("StartupShrineRepeatDirections", true);
                psd.ChangeStateConditions(tmpHash);
                disableAreYouFollowingTheLightRepeaterTime = false;
            }

            // Disables the repeating dialogue when "First Shrine Dialogue" is active
            if (dm.GetActiveDialogue() != null)
            {
                if (dm.GetActiveDialogue().dialogueName == "First Shrine Dialogue")
                {
                    disableAreYouFollowingTheLightRepeaterTime = true;
                    disableFindTheShrine = true;
                }
            }

            // Initializes start and complete time of initial "follow the light" dialogue
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
        else if(!disableFirstConversation)
        {
            //If not disabled and waiting for clean attempt, update the activation time, 
            //else if primed for part 2 of first convo and 3 seconds passed after attempting to clean shrine, display dialogue
            if((bool)psd.stateConditions["WaitingForCleanAttempt"])
            {
                firstConversationPrimerActivationTime = psd.globalTime;
            }
            else if((bool)psd.stateConditions["ShrineFirstConversation2Primer"])
            {
                if(psd.globalTime - firstConversationPrimerActivationTime >= failedToCleanShrineDialogueDelay)
                {
                    disableFirstConversation = true;
                    Hashtable tmpHash = new Hashtable();
                    tmpHash.Add("ShrineFirstConversation2Primer", false);
                    tmpHash.Add("ShrineFirstConversation2", true);
                    psd.ChangeStateConditions(tmpHash);
                }
            }
        }
	}
}
