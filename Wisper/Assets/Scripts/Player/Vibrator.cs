using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

class xRumble
{
    public float timer;    // Rumble timer
    public float fadeTime; // Fade-out (in seconds)
    public Vector2 power;  // Rumble 'power'

    // Decrease timer
    public void Update()
    {
        this.timer -= Time.deltaTime;
    }
}

public class Vibrator : MonoBehaviour {

    private List<xRumble> rumbleEvents; // Stores rumble events
    public bool EnableRumble; // Toggle rumble on/off
    // Vibrate Settings
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    // Update and apply rumble event(s)
    void HandleRumble()
    {
        // Ignore if there are no events
        if (rumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(1, 1);

            for (int i = 0; i < rumbleEvents.Count; ++i)
            {
                // Update current event
                rumbleEvents[i].Update();

                if (rumbleEvents[i].timer > 0)
                {
                    // Calculate current power
                    float timeLeft = Mathf.Clamp(rumbleEvents[i].timer / rumbleEvents[i].fadeTime, 0f, 1f);
                    currentPower = new Vector2(Mathf.Max(rumbleEvents[i].power.x * timeLeft, currentPower.x),
                                               Mathf.Max(rumbleEvents[i].power.y * timeLeft, currentPower.y));

                    GamePad.SetVibration(playerIndex, currentPower.x, currentPower.y);
                }
                else
                {
                    // Cancel out any phantom vibration
                    GamePad.SetVibration(playerIndex, 0.0f, 0.0f);

                    // Remove expired event
                    rumbleEvents.Remove(rumbleEvents[i]);
                }
            }
        }
    }

    // Add a rumble event to the gamepad
    // (vibration is set during 'HandleRumble' function)
    public void AddRumble(float timer, Vector2 power, float fadeTime = 0f)
    {
        xRumble rumble = new xRumble();

        rumble.timer = timer;
        rumble.power = power;
        rumble.fadeTime = fadeTime;

        // Add rumble event to container
        rumbleEvents.Add(rumble);
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i< 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

}
