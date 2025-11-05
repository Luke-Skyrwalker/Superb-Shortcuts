using SharpDX.XInput;
using System;
using System.Diagnostics;

public class ControllerHandler
{
    private Controller controller;

    private Stopwatch inputCooldown = new Stopwatch();
    private int inputCooldownMin = 180;

    public event Action<int> OnMove;
    public event Action OnSelect;

    public ControllerHandler(Action<int> OnMove, Action OnSelect)
    {
        controller = new Controller(UserIndex.One);
        this.OnMove = OnMove;
        this.OnSelect = OnSelect;
        inputCooldown.Start();
    }

    public void Update(bool activeForm)
    {
        Gamepad gamepad = controller.GetState().Gamepad;

        if (inputCooldown.ElapsedMilliseconds > inputCooldownMin)
        {
            if (!activeForm) return;
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
                inputCooldown.Restart();
            }

            if ((gamepad.Buttons & GamepadButtonFlags.A) != 0)
            {
                OnSelect?.Invoke();
                inputCooldown.Restart();
            }
        }
    }
}
