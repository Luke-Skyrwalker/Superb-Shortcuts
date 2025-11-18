using SharpDX.XInput;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class ControllerHandler
{
    [StructLayout(LayoutKind.Sequential)]
    private struct XINPUT_GAMEPAD
    {
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct XINPUT_STATE
    {
        public uint dwPacketNumber;
        public XINPUT_GAMEPAD Gamepad;
    }

    [DllImport("XInput1_4.dll", EntryPoint = "#100")]
    private static extern int XInputGetStateEx(int dwUserIndex, ref XINPUT_STATE pState);

    private Controller controller;
    private XINPUT_STATE state;

    private Stopwatch inputCooldown = new Stopwatch();
    private int inputCooldownMin = 180;
    private Stopwatch doubleClickTime = new Stopwatch();
    private int doubleClickTimeMax = 600;
    private int homeButtonCounter = 0;

    public event Action<int> OnMove;
    public event Action OnSelect;
    public event Action<int> OnHomeButton;

    public ControllerHandler(Action<int> OnMove, Action OnSelect, Action<int> OnHomeButton)
    {
        controller = new Controller(UserIndex.One);
        state = new XINPUT_STATE();
        this.OnMove = OnMove;
        this.OnSelect = OnSelect;
        this.OnHomeButton = OnHomeButton;
        inputCooldown.Start();
    }

    public void Update(bool activeForm)
    {
        if (inputCooldown.ElapsedMilliseconds < inputCooldownMin || !controller.IsConnected) return;
        CheckHomeButton();
        if (!activeForm) return;

        Gamepad gamepad = controller.GetState().Gamepad;
        CheckJoystickDPad(gamepad);
        if ((gamepad.Buttons & GamepadButtonFlags.A) != 0)
        {
            OnSelect?.Invoke();
            inputCooldown.Restart();
        }  
    }

    // ToDo: better name?
    private void CheckHomeButton()
    {
        if (XInputGetStateEx(0, ref state) == 0 && state.Gamepad.wButtons == 1024) // HomeButton pressed
        {
            doubleClickTime.Start();
            homeButtonCounter += 1;
            inputCooldown.Restart();
        }
        if (doubleClickTime.IsRunning && doubleClickTime.ElapsedMilliseconds > doubleClickTimeMax)
        {
            doubleClickTime.Stop();
            doubleClickTime.Reset();
            OnHomeButton?.Invoke(homeButtonCounter);
            homeButtonCounter = 0;
        }
    }

    // ToDo: better name?
    private void CheckJoystickDPad(Gamepad gamepad)
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
            inputCooldown.Restart();
        }
    }
}
