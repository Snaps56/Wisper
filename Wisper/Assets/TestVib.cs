using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class TestVib : MonoBehaviour {

    // Gamepad instance
    x360_Gamepad gamepad;

    public bool VibStop = false;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    // Use this for initialization
    void Start()
    {

    }
    void Update()
    {
        // Obtain the desired gamepad from GamepadManager
        gamepad = GamepadManager.Instance.GetGamepad(1);

        // Sample code to test button input and rumble
        if (Input.GetButtonDown("XBOX_Thumbstick_L_Click"))
        {
            TestRumble();
            GamePad.SetVibration(playerIndex, 1f, 1f);
            Debug.Log("A down");
        }
    }
    // Send some rumble events to the gamepad
    void TestRumble()
    {
        //                timer            power         fade
        gamepad.AddRumble(1.0f, new Vector2(0.9f, 0.9f), 0.5f);
        gamepad.AddRumble(2.5f, new Vector2(0.5f, 0.5f), 0.2f);
    }
}
