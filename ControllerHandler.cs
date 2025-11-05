using SharpDX.XInput;
using System;
using System.Diagnostics;

public class ControllerHandler
{
    private Controller controller;
    private bool isConnected;

    private Stopwatch moveCooldown = new Stopwatch();
    private int moveCooldownMs = 180;

    public event Action<int> OnMove;
    public event Action OnSelect;

    public ControllerHandler()
    {
        controller = new Controller(UserIndex.One);
        isConnected = controller.IsConnected;
        moveCooldown.Start();
    }

    public void Update()
    {
        if (!isConnected) return;

        Gamepad gamepad = controller.GetState().Gamepad;

        if (moveCooldown.ElapsedMilliseconds > moveCooldownMs)
        {

            int moveDir = 0;

            if (gamepad.LeftThumbX > 12000) moveDir = 1;  // Right
            else if (gamepad.LeftThumbX < -12000) moveDir = -1; // Left
            else if (gamepad.LeftThumbY > 12000) moveDir = -3;  // Up
            else if (gamepad.LeftThumbY < -12000) moveDir = 3;   // Down
            else if ((gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0) moveDir = 1;
            else if ((gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0) moveDir = -1;
            else if ((gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0) moveDir = -3;
            else if ((gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0) moveDir = 3;

            if (moveDir != 0)
            {
                OnMove?.Invoke(moveDir);
                moveCooldown.Restart();
            }

            if ((gamepad.Buttons & GamepadButtonFlags.A) != 0)
            {
                OnSelect?.Invoke();
                moveCooldown.Restart();
            }
        }
    }
}
