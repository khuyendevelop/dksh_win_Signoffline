using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace eInvoice.Signer
{
    public class AutoPin
    {
        private static string PINTitle = ConfigurationManager.AppSettings["PinTitle"].ToString();
        private static string ButtonTitle = ConfigurationManager.AppSettings["ButtonTitle"].ToString();
        private static IntPtr WindowHandle = IntPtr.Zero;
        private static IntPtr EditHandle = IntPtr.Zero;
        private static IntPtr ButtonHandle = IntPtr.Zero;


        private delegate int EnumWindowsProc(IntPtr hwnd, int lParam);

        [DllImport("user32.Dll")]
        private static extern int EnumWindows(EnumWindowsProc x, int y);
        [DllImport("user32")]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowsProc callback, int lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount); 

        /// <summary>
        /// The FindWindowEx API
        /// </summary>
        /// <param name="parentHandle">a handle to the parent window </param>
        /// <param name="childAfter">a handle to the child window to start search after</param>
        /// <param name="className">the class name for the window to search for</param>
        /// <param name="windowTitle">the name of the window to search for</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);


        public const int GWL_ID = -12;
        public const int WM_GETTEXT = 0x000D;

        [DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int index);
        [DllImport("User32.dll")]
        public static extern IntPtr SendDlgItemMessage(IntPtr hWnd, int IDDlgItem, int uMsg, int nMaxCount, StringBuilder lpString);
        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("User32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        [DllImport("User32.dll")]
        public static extern bool SetFocus(IntPtr hWnd);

        private static List<IntPtr> _results = new List<IntPtr>();

        /// <summary>
        /// The SendMessage API
        /// </summary>
        /// <param name="hWnd">handle to the required window</param>
        /// <param name="msg">the system/Custom message to send</param>
        /// <param name="wParam">first message parameter</param>
        /// <param name="lParam">second message parameter</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private static void SendKey(IntPtr hWnd, int msg, string key)
        {
            for(int i=0;i<key.Length;i++)
                SendMessage(hWnd, msg, 0, key);
        }

        public static void GetWindowList()
        {
            try
            {
                WindowHandle = IntPtr.Zero;
                EditHandle = IntPtr.Zero;
                ButtonHandle = IntPtr.Zero;
                Process procesInfo = Process.GetCurrentProcess();
                foreach (ProcessThread threadInfo in procesInfo.Threads)
                {
                    IntPtr[] windows = GetWindowHandlesForThread(threadInfo.Id);
                    if (windows != null && windows.Length > 0)
                        foreach (IntPtr hWnd in windows)
                        {
                            if (GetText(hWnd).Contains(PINTitle))
                                WindowHandle = hWnd;

                            if ("Edit".Equals(GetClass(hWnd)) && WindowHandle.Equals(GetParent(hWnd)))
                                EditHandle = hWnd;
                            //SendKey(hWnd, "12345678");

                            if ("Button".Equals(GetClass(hWnd))
                                && WindowHandle.Equals(GetParent(hWnd))
                                && GetText(hWnd).Contains(ButtonTitle))
                                ButtonHandle = hWnd;
                        }
                    //if (GetText(hWnd).Contains(PINTitle))
                    //{
                    //    PINWindow = hWnd;
                    //    break;
                    //}

                    //Console.WriteLine("\twindow {0:x} text:{1} class:{2}",
                    //    hWnd.ToInt32(), GetText(hWnd), GetClass(hWnd));
                }

                if (WindowHandle != IntPtr.Zero && EditHandle != IntPtr.Zero && ButtonHandle != IntPtr.Zero)
                {
                    //SendKey(EditHandle, 0x000C, "12345678");
                    SetFocus(EditHandle);
                    System.Windows.Forms.SendKeys.Send("1");
                    System.Windows.Forms.SendKeys.Send("2");
                    System.Windows.Forms.SendKeys.Send("3");
                    System.Windows.Forms.SendKeys.Send("4");
                    System.Windows.Forms.SendKeys.Send("5");
                    System.Windows.Forms.SendKeys.Send("6");
                    System.Windows.Forms.SendKeys.Send("7");
                    System.Windows.Forms.SendKeys.Send("8");
                    EnableWindow(ButtonHandle, true);
                    SendKey(ButtonHandle, 0x00F5, "0");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("AutoPin: " + ex.Message);
            }
        }
        private static IntPtr[] GetWindowHandlesForThread(int threadHandle)
        {
            _results.Clear();
            EnumWindows(WindowEnum, threadHandle);
            return _results.ToArray();
        }
        private static int WindowEnum(IntPtr hWnd, int lParam)
        {
            int processID = 0;
            int threadID = GetWindowThreadProcessId(hWnd, out processID);
            if (threadID == lParam)
            {
                _results.Add(hWnd);
                EnumChildWindows(hWnd, WindowEnum, threadID);
            }
            return 1;
        }
        private static string GetText(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }
        private static StringBuilder GetEditText(IntPtr hWnd)
        {
            Int32 dwID = GetWindowLong(hWnd, GWL_ID);
            IntPtr hWndParent = GetParent(hWnd);
            StringBuilder title = new StringBuilder(128);
            SendDlgItemMessage(hWndParent, dwID, WM_GETTEXT, 128, title);
            return title;
        }
        private static string GetClass(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder();
            GetClassName(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}
