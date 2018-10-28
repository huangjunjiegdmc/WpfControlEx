using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WpfControlEx.Controls.Native
{
    public class MouseHook
    {
        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;
              

        /// <summary>
        /// 
        /// </summary>
        private int hHook;

        /// <summary>
        /// 鼠标钩子回调函数指针
        /// </summary>
        public WinUserApi.HookProc hProc;
        
        /// <summary>
        /// 安装鼠标钩子
        /// </summary>
        /// <returns></returns>
        public int SetHook()
        {
            hProc = new WinUserApi.HookProc(MouseHookProc);
            hHook = WinUserApi.SetWindowsHookEx(WinUserApi.WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }

        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void UnHook()
        {
            WinUserApi.UnhookWindowsHookEx(hHook);
        }

        /// <summary>
        /// 鼠标钩子回调函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode < 0)
                {
                    return WinUserApi.CallNextHookEx(hHook, nCode, wParam, lParam);
                }
                else
                {
                    WinUserApi.POINT point = new WinUserApi.POINT();
                    WinUserApi.GetCursorPos(out point);

                    
                    {
                        switch ((uint)wParam)
                        {
                            case WinUserApi.WM_LBUTTONDOWN:
                                MouseEventArgs LMouseClickEventArgs = new MouseEventArgs(MouseButtons.Left, 1, point.x, point.y, 0);
                                MouseClickEvent?.Invoke(this, LMouseClickEventArgs);
                                break;
                            case WinUserApi.WM_RBUTTONDOWN:
                                MouseEventArgs RMouseClickEventArgs = new MouseEventArgs(MouseButtons.Right, 1, point.x, point.y, 0);
                                MouseClickEvent?.Invoke(this, RMouseClickEventArgs);
                                break;
                            case WinUserApi.WM_RBUTTONDBLCLK:
                                MouseEventArgs RBDBLMouseClickEventArgs = new MouseEventArgs(MouseButtons.Right, 2, point.x, point.y, 0);
                                MouseClickEvent?.Invoke(this, RBDBLMouseClickEventArgs);
                                break;
                            case WinUserApi.WM_MOUSEMOVE:
                                MouseEventArgs MouseMoveEventArgs = new MouseEventArgs(MouseButtons.None, 0, point.x, point.y, 0);
                                MouseMoveEvent?.Invoke(this, MouseMoveEventArgs);
                                break;
                        }
                    }
                    return WinUserApi.CallNextHookEx(hHook, nCode, wParam, lParam);
                }
            }
            catch { }

            return 0;
        }
    }
}
