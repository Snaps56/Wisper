//--------------------------------------------------------
// Xbox 360 Gamepad 'Manager - Refresh' script
// Complete tutorial code, Part 4 by Lawrence M
//
// Disclaimer:
//--------------------------------------------------------
// * Code in this script is provided AS IS and is based
//   on tried and tested code from my past projects.
//
// * This script is NOT guaranteed to work for 3rd-Party
//   (non Microsoft official) controllers.
//
// * Script tested in Unity 5.0.0f4 'Personal', using
//   a Microsoft Xbox 360 controller (wired USB).
//--------------------------------------------------------

using UnityEngine;
using System.Collections;

public class RefreshGamepads : MonoBehaviour
{
    // Normal Unity update
    void Update()
    {
        // Refresh the GamepadManager script
        GamepadManager.Instance.Refresh();
    }
}
