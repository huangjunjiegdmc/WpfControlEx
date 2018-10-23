using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlEx.Controls.Native
{
    public class Win32Api
    {
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bRevert"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uPosition"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="hMenu"></param>
        /// <param name="uIDEnableItem"></param>
        /// <param name="uEnable"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        /// <summary>
        /// TrackPopupMenuEx
        /// </summary>
        /// <param name="hmenu"></param>
        /// <param name="fuFlags"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hwnd"></param>
        /// <param name="lptpm"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);


        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsWindow(IntPtr hwnd);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MoveWindow(IntPtr hwnd, int xPoint, int yPoint, int nWidth, int nHeight, bool bRepaint);
    }
}
