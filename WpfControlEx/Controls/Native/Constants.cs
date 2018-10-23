using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfControlEx.Controls.Native
{
    public static class Constants
    {
        public const uint TPM_LEFTBUTTON = 0;
        public const uint TPM_RETURNCMD = 256;

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

        /// <summary>
        /// 按命令方式
        /// </summary>
        public const uint MF_BYCOMMAND = 0x00;

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
    }
}
