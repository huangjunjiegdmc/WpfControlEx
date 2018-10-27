using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlEx.Controls.Helper
{
    public static class WindowHelper
    {
        /// <summary>
        /// 判断窗口是否模式窗口
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static bool IsModal(this Window window)
        {
            var filedInfo = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic);

            return filedInfo != null && (bool)filedInfo.GetValue(window);
        }
}
}
