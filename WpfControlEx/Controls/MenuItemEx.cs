using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfControlEx.Controls
{
    public class MenuItemEx : MenuItem
    {
        #region ThemeColorPreview

        public static readonly DependencyProperty ThemeColorPreviewProperty =
            DependencyProperty.Register("ThemeColorPreview", typeof(SolidColorBrush), typeof(MenuItemEx),
                new FrameworkPropertyMetadata(Brushes.White));

        /// <summary>
        /// 主题颜色预览
        /// </summary>
        public SolidColorBrush ThemeColorPreview
        {
            get { return (SolidColorBrush)GetValue(ThemeColorPreviewProperty); }
            set { SetValue(ThemeColorPreviewProperty, value); }
        }

        #endregion

        /// <summary>
        /// 主题修改命令
        /// </summary>
        public RelayCommand ChangeThemeCommand
        {
            get
            {
                return new RelayCommand((obj) => 
                {
                    string themeName = obj == null? "": obj.ToString();
                    Themes.ThemeManager.ApplyTheme(Application.Current, themeName);
                });
            }
        }
    }
}
