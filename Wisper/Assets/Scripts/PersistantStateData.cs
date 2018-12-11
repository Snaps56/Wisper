﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized location to check for various conditions that determine what should or should not be updated in the game.
// When the instance of this class is accessed, you may use the public hashtable to search conditions.
// Syntax for a search would be stateConditions["Key"], which will return the corresponding value
public class PersistantStateData : MonoBehaviour
{
    public static PersistantStateData persistantStateData;
    public int updateCount;         // A count of how many times the hashtable has been updated after game launched. Should be incremented when modifying hashtable. Need not be preserved after closing application.
    public Hashtable stateConditions;   // Hashtable containing key/value pairs of state conditions (probably limited to string/bool pairs).
    public float globalTime;       // The total elapsed time in seconds
    public bool pauseGlobalTimer = false;

    // When scene with this loads, initialize the static variable to object with this script if there is none. Object is persistant through scenes.
    // Otherwise if persistantStateData is already loaded into the game/scene, don't overwrite it and delete this object. This enforces singleton status.
    void Awake()
    {
        if (persistantStateData == null)
        {
            DontDestroyOnLoad(gameObject);
            persistantStateData = this;

            stateConditions = new Hashtable();
            globalTime = 0f;
            PopulateStateConditions();
            updateCount = 1;
        }
        else if (persistantStateData != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if(!pauseGlobalTimer)
        {
            globalTime += Time.deltaTime;
        }
    }

    // fills the persistantStateConditions with the various conditions. We can consider passing in arguments for initialization when considering save/load functionality.
    public void PopulateStateConditions()
    {
        stateConditions.Add("StartupFadeFinished", false);
        stateConditions.Add("StartupShrineDialogueFinished", false);

        stateConditions.Add("StartupShrineDialogue", false);    // Plays dialogue after opening cutscene
        stateConditions.Add("StartupShrinePart2", false);       // Plays dialogue 3 seconds after previous opening dialogue
        stateConditions.Add("StartupShrineRepeatDirections", false);   // Plays every 1 minute after the other dialogue is finished, before player talks to shrine

        // Variables control dialogue of shrine the first time the player talks to it
        stateConditions.Add("ShrineFirstConversation", false);  // Allows player to initiate conversation
        stateConditions.Add("WaitingForCleanAttempt", false);   // Indicates that the shrine will respond once the player attempts to clean it.
        stateConditions.Add("ShrineFirstConversation2Primer", false);   // A primer that tells the shrine time listener to ready activation of conversation
        stateConditions.Add("ShrineFirstConversation2", false); // After player fails to clean shrine, this enables the conversation to continue
        stateConditions.Add("ShrineFirstConversationYes", false);   // If player agrees to help, this plays a dialogue
        stateConditions.Add("ShrineFirstConversationNo", false);    // If player refuses to help, this plays a dialogue
        stateConditions.Add("ShrineFirstConversationOver", false);  // Signifies the first conversation has been completed.

        // Variables control shrine dialogue after player has completed the Garden intro tasks
        stateConditions.Add("ShrineFirstTurnIn", false);  // Marks the first turn in for the tutorial area
        stateConditions.Add("ShrineFirstTurnInNo", false);  // Player says no to the first turn in
        stateConditions.Add("FirstTurnInCutsceneDialogue", false);  // Used to start Dialogue during trun in cutscene
        stateConditions.Add("GoForth", false);  // Used to enable dialogue telling player to leave the garden

        stateConditions.Add("TutorialFirstDialogueFinished", false); // Check if opening dialogue done, then trigger look tutorial
        stateConditions.Add("TutorialLookFinished", false); // check if look tutorial done, then trigger move tutorial
        stateConditions.Add("TutorialMovementFinished", false); // check if move tutorial done, then remove move ui tutorial
        stateConditions.Add("TutorialWithinShrineRange", false); // check if player within ui marker range of shrine, then trigger ui marker tutorial
        stateConditions.Add("TutorialFirstInteraction", false); // check if player talking with shrine, then trigger dialogue tutorial
        stateConditions.Add("TutorialTalkedWithShrine", false); // ????
        stateConditions.Add("TutorialInteractFinished", false); // ????
        stateConditions.Add("TutorialDialogueSkipped", false); // check if player done with first converation, then trigger attempt clean tutorial
        stateConditions.Add("TutorialAttemptedClean", false); // check if player attempted to clean shrine, then end attempt clean tutorial
        stateConditions.Add("TutorialWithinShellsterRange", false); // check if player within shellster range, then end generate orb tutorial
        stateConditions.Add("TutorialHasEnoughOrbs", false); // check if player within shellster range, then end generate orb tutorial

        stateConditions.Add("ShamusHasHat", false);
        stateConditions.Add("ShrineIsClean", false);

        stateConditions.Add("SwingTaskDone", false);

        stateConditions.Add("Cutscene1Started", false);

        stateConditions.Add("OrbDepositInProgress", false);
    }

    public void ResetPersistantStateData()
    {
        stateConditions.Clear();
        PopulateStateConditions();

    }

    public void ChangeStateConditions(string key, bool value)
    {
        if((bool)stateConditions[key] != value)
        {
            stateConditions[key] = value;
            updateCount++;
        }
    }

    public void ChangeStateConditions(string key, int value)
    {
        if ((int)stateConditions[key] != value)
        {
            stateConditions[key] = value;
            updateCount++;
        }
    }

    public void ChangeStateConditions(string key, float value)
    {
        if ((float)stateConditions[key] != value)
        {
            stateConditions[key] = value;
            updateCount++;
        }
    }

    public void ChangeStateConditions(Hashtable kvPairs)
    {
        bool modified = false;
        foreach(DictionaryEntry de in kvPairs)
        {
            if(stateConditions[de.Key] != de.Value)
            {
                stateConditions[de.Key] = de.Value;
                modified = true;
            }
        }

        if (modified)
        {
            updateCount++;
        }
    }
}

