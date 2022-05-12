using AutoIt;
using KO.Core.Constants;
using System.Drawing;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace KO.Core.Helpers.Utility
{
    public class InputHelper : WinApi
    {
        public static void ClickMouse(int x, int y, string button = "LEFT")
        {
            AutoItX.MouseClick(button, x, y);
        }

        public static void SendKey(string key)
        {
            AutoItX.Send(key);
        }

        public static void ClickMouse(int x, int y, bool left)
        {
            Cursor.Position = new Point(x, y);
            if (left)
            {
                mouse_event(0x0002, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                mouse_event(0x0004, Cursor.Position.X, Cursor.Position.Y, 0, 0);
            }
            else
            {
                mouse_event(0x0008, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                mouse_event(0x0010, Cursor.Position.X, Cursor.Position.Y, 0, 0);
            }
        }

        public static void ClickMouseLeftSimulator(int x, int y)
        {
            var simulator = new InputSimulator();
            x = 65535 * x / Screen.PrimaryScreen.Bounds.Width;
            y = 65535 * y / Screen.PrimaryScreen.Bounds.Height;
            simulator.Mouse.MoveMouseTo(x, y);
            simulator.Mouse.LeftButtonClick();
        }

        public static void SendKey(VirtualKeyCode keyCode)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyDown(keyCode).KeyUp(keyCode);
        }

        public static void SendText(string text)
        {
            var simulator = new InputSimulator();
            simulator.Keyboard.TextEntry(text);
        }
    }
}
