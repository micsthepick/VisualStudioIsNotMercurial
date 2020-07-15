using System;
using System.Runtime.InteropServices;

namespace VisualHg
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool SetWindowText(IntPtr hWnd, string lpString);
    }
}