using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfControlEx.Utils;

namespace WpfControlEx.Themes
{
    /// <summary>
    /// 主题模型类
    /// </summary>
    public class ThemeModel
    {
        /// <summary>
        /// 主题颜色预览
        /// </summary>
        public SolidColorBrush ThemeColorPreview { get; set; }

        /// <summary>
        /// 主题编码
        /// </summary>
        public string ThemeKey { get; set; }

        /// <summary>
        /// 主题名称，建议与多语言资源名称定义一致
        /// </summary>
        public string ThemeName { get; set; }
    }
}
