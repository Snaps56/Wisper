using UnityEngine;
using XInputDotNetPure;
using System.Collections.Generic;

// Stores states of a single gamepad button
public struct xButton
{
    public ButtonState prev_state;
    public ButtonState state;
}

// Stores state of a single gamepad trigger
public struct TriggerState
{
    public float prev_value;
    public float current_value;
}

// Rumble (vibration) event
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

// Xbox 360 Gamepad class
public class x360_Gamepad
{
    private GamePadState prev_state; // Previous gamepad state
    private GamePadState state;      // Current gamepad state

    private int gamepadIndex;           // Numeric index (1,2,3 or 4)
    private PlayerIndex playerIndex;    // XInput 'Player' index
    private List<xRumble> rumbleEvents; // Stores rumble events

    // Button input map
    private Dictionary<string, xButton> inputMap;

    // States for all buttons/inputs supported
    private xButton A, B, X, Y; // Action (face) buttons
    private xButton DPad_Up, DPad_Down, DPad_Left, DPad_Right;

    private xButton Guide;       // Xbox logo button
    private xButton Back, Start;
    private xButton L3, R3;      // Thumbstick buttons
    private xButton LB, RB;      // 'Bumper' (shoulder) buttons
    private TriggerState LT, RT; // Trigger states

    public bool EnableRumble; // Toggle rumble on/off

    // Constructor
    public x360_Gamepad(int index)
    {
        // Set gamepad index
        gamepadIndex = index - 1;
        playerIndex = (PlayerIndex)gamepadIndex;

        EnableRumble = true;

        // Create rumble container and input map
        rumbleEvents = new List<xRumble>();
        inputMap = new Dictionary<string, xButton>();
    }

    // Update input map
    void UpdateInputMap()
    {
        inputMap["A"] = A;
        inputMap["B"] = B;
        inputMap["X"] = X;
        inputMap["Y"] = Y;

        inputMap["DPad_Up"] = DPad_Up;
        inputMap["DPad_Down"] = DPad_Down;
        inputMap["DPad_Left"] = DPad_Left;
        inputMap["DPad_Right"] = DPad_Right;

        inputMap["Guide"] = Guide;
        inputMap["Back"] = Back;
        inputMap["Start"] = Start;

        inputMap["L3"] = L3;
        inputMap["R3"] = R3;
        inputMap["LB"] = LB;
        inputMap["RB"] = RB;
    }

    #region Update

    // Update gamepad state
    public void Update()
    {
        // Get current state
        state = GamePad.GetState(playerIndex);

        // Check gamepad is connected
        if (state.IsConnected)
        {
            A.state = state.Buttons.A;
            B.state = state.Buttons.B;
            X.state = state.Buttons.X;
            Y.state = state.Buttons.Y;

            DPad_Up.state = state.DPad.Up;
            DPad_Down.state = state.DPad.Down;
            DPad_Left.state = state.DPad.Left;
            DPad_Right.state = state.DPad.Right;

            Guide.state = state.Buttons.Guide;
            Back.state = state.Buttons.Back;
            Start.state = state.Buttons.Start;
            L3.state = state.Buttons.LeftStick;
            R3.state = state.Buttons.RightStick;
            LB.state = state.Buttons.LeftShoulder;
            RB.state = state.Buttons.RightShoulder;

            LT.current_value = state.Triggers.Left;
            RT.current_value = state.Triggers.Right;

            UpdateInputMap(); // Update inputMap dictionary
            HandleRumble();   // Update rumble events
        }
    }

    #endregion

    #region Refresh

    // Refresh previous gamepad state
    public void Refresh()
    {
        // This 'saves' the current state for next update
        prev_state = state;

        // Check gamepad is connected
        if (state.IsConnected)
        {
            A.prev_state = prev_state.Buttons.A;
            B.prev_state = prev_state.Buttons.B;
            X.prev_state = prev_state.Buttons.X;
            Y.prev_state = prev_state.Buttons.Y;

            DPad_Up.prev_state = prev_state.DPad.Up;
            DPad_Down.prev_state = prev_state.DPad.Down;
            DPad_Left.prev_state = prev_state.DPad.Left;
            DPad_Right.prev_state = prev_state.DPad.Right;

            Guide.prev_state = prev_state.Buttons.Guide;
            Back.prev_state = prev_state.Buttons.Back;
            Start.prev_state = prev_state.Buttons.Start;
            L3.prev_state = prev_state.Buttons.LeftStick;
            R3.prev_state = prev_state.Buttons.RightStick;
            LB.prev_state = prev_state.Buttons.LeftShoulder;
            RB.prev_state = prev_state.Buttons.RightShoulder;

            // Read previous trigger values into trigger states
            LT.prev_value = prev_state.Triggers.Left;
            RT.prev_value = prev_state.Triggers.Right;

            UpdateInputMap(); // Update button map
        }
    }

    #endregion

    // Return numeric gamepad index
    public int Index { get { return gamepadIndex; } }

    // Check if the gamepad is connected
    public bool IsConnected { get { return state.IsConnected; } }

    // Update and apply rumble event(s)
    void HandleRumble()
    {
        // Ignore if there are no events
        if (rumbleEvents.Count > 0)
        {
            Vector2 currentPower = new Vector2(0, 0);

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

    // Return axes of left thumbstick
    public GamePadThumbSticks.StickValue GetStick_L()
    {
        return state.ThumbSticks.Left;
    }

    // Return axes of right thumbstick
    public GamePadThumbSticks.StickValue GetStick_R()
    {
        return state.ThumbSticks.Right;
    }

    // Return axis of left trigger
    public float GetTrigger_L { get { return state.Triggers.Left; } }

    // Return axis of right trigger
    public float GetTrigger_R { get { return state.Triggers.Right; } }

    // Check if left trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_L()
    {
        return (LT.prev_value == 0f && LT.current_value >= 0.1f) ? true : false;
    }

    // Check if right trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_R()
    {
        return (RT.prev_value == 0f && RT.current_value >= 0.1f) ? true : false;
    }

    // Return button state
    public bool GetButton(string button)
    {
        return inputMap[button].state == ButtonState.Pressed ? true : false;
    }

    // Return button state - on CURRENT frame
    public bool GetButtonDown(string button)
    {
        return (inputMap[button].prev_state == ButtonState.Released &&
                inputMap[button].state == ButtonState.Pressed) ? true : false;
    }
}