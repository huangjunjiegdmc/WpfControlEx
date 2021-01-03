using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlEx.Controls.Native
{
    /// <summary>
    /// Win32 API封装
    /// </summary>
    public class WinUserApi
    {
        #region Window Messages
        public const uint WM_SYSCOMMAND = 0x0112;
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONDBLCLK = 0x0206;
        #endregion


        #region System Menu Command Values
        /// <summary>
        /// 移动
        /// </summary>
        public const uint SC_MOVE = 0xF010;
        /// <summary>
        /// 大小
        /// </summary>
        public const uint SC_SIZE = 0xF000;
        /// <summary>
        /// 最小化
        /// </summary>
        public const uint SC_MINIMIZE = 0xF020;
        /// <summary>
        /// 最大化
        /// </summary>
        public const uint SC_MAXIMIZE = 0xF030;
        /// <summary>
        /// 还原
        /// </summary>
        public const uint SC_RESTORE = 0xF120;
        /// <summary>
        /// 关闭
        /// </summary>
        public const uint SC_CLOSE = 0xF060;
        #endregion


        #region Menu flags for Add/Check/EnableMenuItem()
        /// <summary>
        /// 按命令方式
        /// </summary>
        public const uint MF_BYCOMMAND = 0x00;

        /// <summary>
        /// 
        /// </summary>
        public const uint MF_BYPOSITION = (uint)0x00000400L;
        /// <summary>
        /// 启用
        /// </summary>
        public const uint MF_ENABLED = 0x00;
        /// <summary>
        /// 灰掉
        /// </summary>
        public const uint MF_GRAYED = 0x01;
        /// <summary>
        /// 不可用
        /// </summary>
        public const uint MF_DISABLED = 0x02;
        #endregion


        #region Flags for TrackPopupMenu
        public const uint TPM_LEFTBUTTON = (uint)0x0000L;
        public const uint TPM_RETURNCMD = (uint)0x0100L;
        #endregion


        #region SetWindowsHook() codes
        /// <summary>
        /// 监控钩子所在模块的鼠标事件
        /// </summary>
        public const int WH_MOUSE = 7;
        /// <summary>
        /// 截获整个系统所有模块的鼠标事件
        /// </summary>
        public const int WH_MOUSE_LL = 14;
        #endregion


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

        [DllImport("user32.dll")]
        public static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        public static extern int GetMenuItemID(IntPtr hMenu, int nPos);

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
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsWindow(IntPtr hwnd);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool MoveWindow(IntPtr hwnd, int xPoint, int yPoint, int nWidth, int nHeight, bool bRepaint);


        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        /// <summary>
        /// 鼠标钩子回调函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 安装钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 卸载钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 调用下一个钩子
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        //public struct POINT
        //{
        //    public int X;
        //    public int Y;
        //    public POINT(int x, int y)
        //    {
        //        this.X = x;
        //        this.Y = y;
        //    }
        //}

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
    }
}
