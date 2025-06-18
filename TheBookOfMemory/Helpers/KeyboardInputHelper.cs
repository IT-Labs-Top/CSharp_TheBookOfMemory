using System.Globalization;
using System.Runtime.InteropServices;

namespace TheBookOfMemory.Helpers
{
    public class KeyboardInputHelper
    {
        private const byte KEYUP = 0x2;
        private const byte KEYDOWN = 0x0;
        private const byte VK_SHIFT = 0x10;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        static extern int LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public static void SendKey(char key, bool isShiftPressed)
        {
            var vCode = (byte)VkKeyScan(key);
            if ("_+()!".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-Ru"));
                SendKey(vCode, true);
            }
            else if ("*".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(vCode, true);
            }
            else if("1234567890-".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-Ru"));
                SendKey(vCode, false);
            }
            else if ("\'".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(222, false);
            }
            else if ("\"".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(222, true);
            }
            else if (";".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(186, false);
            }
            else if (":".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(186, true);
            }
            else if ("@".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey((byte)VkKeyScan('2'), true);
            }
            else if ("#".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey((byte)VkKeyScan('3'), true);
            }
            else if ("%".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey((byte)VkKeyScan('5'), true);
            }
            else if ("&".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey((byte)VkKeyScan('7'), true);
            }
            else if (",".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(188, false);
            }
            else if ("/".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(191, false);
            }
            else if (".".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
                SendKey(191, false);
            }
            else if ("?".Contains(key))
            {
                InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                SendKey(191, true);
            }
            else
            {
                SendKey(vCode, isShiftPressed);
            }
           
        }

        public static void SendKey(byte vCode, bool isShiftPressed)
        {
            if (isShiftPressed)
                keybd_event(VK_SHIFT, 0, KEYDOWN, 0);

            keybd_event(vCode, 0, KEYDOWN, 0);
            keybd_event(vCode, 0, KEYUP, 0);

            if (isShiftPressed)
                keybd_event(VK_SHIFT, 0, KEYUP, 0);
        }

    }
}
