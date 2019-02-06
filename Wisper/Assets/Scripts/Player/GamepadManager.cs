using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Gamepad Manager class
public class GamepadManager : MonoBehaviour
{
    public int GamepadCount = 2; // Number of gamepads to manage

    private List<x360_Gamepad> gamepads;     // Holds gamepad instances
    private static GamepadManager singleton; // Singleton instance

    // Initialize on Awake
    void Awake()
    {
        // Found a duplicate instance of this class, destroy it
        if (singleton != null && singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Create singleton instance
            singleton = this;
            DontDestroyOnLoad(this.gameObject);

            // Lock gamepad count to supported range
            GamepadCount = Mathf.Clamp(GamepadCount, 1, 4);

            gamepads = new List<x360_Gamepad>();

            // Create specified number of gamepad instances
            for (int i = 0; i < GamepadCount; ++i)
                gamepads.Add(new x360_Gamepad(i + 1));
        }
    }

    // Return manager instance
    public static GamepadManager Instance
    {
        get
        {
            if (singleton == null)
            {
                Debug.LogError("[GamepadManager]: Instance does not exist!");
                return null;
            }

            return singleton;
        }
    }

    // Normal Unity update, update gamepad instances
    void Update()
    {
        for (int i = 0; i < gamepads.Count; ++i)
            gamepads[i].Update();
    }

    // Refresh gamepad instances for next update
    // (We call this method in the 'RefreshGamepads' script)
    public void Refresh()
    {
        for (int i = 0; i < gamepads.Count; ++i)
            gamepads[i].Refresh();
    }

    // Return specified gamepad
    // (Pass index of desired gamepad - eg. 1)
    public x360_Gamepad GetGamepad(int index)
    {
        for (int i = 0; i < gamepads.Count;)
        {
            // Indexes match, return this gamepad
            if (gamepads[i].Index == (index - 1))
                return gamepads[i];
            else
                ++i; // No match, continue iterating
        }

        Debug.LogError("[GamepadManager]: " + index
            + " is not a valid gamepad index!");

        return null;
    }

    // Return number of connected gamepads
    public int ConnectedTotal()
    {
        int total = 0;

        for (int i = 0; i < gamepads.Count; ++i)
        {
            if (gamepads[i].IsConnected)
                total++;
        }

        return total;
    }

    // Check across all connected gamepads for button press.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected && gamepads[i].GetButton(button))
                return true;
        }

        return false;
    }

    // Check across all connected gamepads for button press - CURRENT frame.
    // Return true if conditions are met by any gamepad
    public bool GetButtonDownAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected && gamepads[i].GetButtonDown(button))
                return true;
        }

        return false;
    }
}
