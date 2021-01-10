using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlEx.Themes
{
    /// <summary>
    /// 主题管理
    /// </summary>
    public static class ThemeManager
    {
        #region Theme

        /// <summary>
        /// Theme Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ThemeNameProperty =
            DependencyProperty.RegisterAttached("ThemeName", typeof(string), typeof(ThemeManager),
                new FrameworkPropertyMetadata((string)string.Empty,
                    new PropertyChangedCallback(OnThemeNameChanged)));

        /// <summary>
        /// Gets the Theme property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static string GetThemeName(DependencyObject d)
        {
            return (string)d.GetValue(ThemeNameProperty);
        }

        /// <summary>
        /// Sets the Theme property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetThemeName(DependencyObject d, string value)
        {
            d.SetValue(ThemeNameProperty, value);
        }

        /// <summary>
        /// Handles changes to the Theme property.
        /// </summary>
        private static void OnThemeNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string themeName = e.NewValue as string;
            if (themeName == string.Empty)
                return;

            ContentControl control = d as ContentControl;
            if (control != null)
            {
                control.ApplyTheme(themeName);
            }
        }

        #endregion


        /// <summary>
        /// 获取支持的主题列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetThemes()
        {
            string[] themes = new string[]
            {
                "LightBlue","LightGreen"
            };
            return themes;
        }

        /// <summary>
        /// 应用控件主题
        /// </summary>
        /// <param name="app">应用程序</param>
        /// <param name="theme">主题名称</param>
        public static void ApplyTheme(this ContentControl contentControl, string themeName)
        {
            try
            {
                //先移除已添加的主题资源字典
                ResourceDictionary oldRd = contentControl.Resources.MergedDictionaries.FirstOrDefault(
                    p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Themes/Theme"));
                if (oldRd != null)
                {
                    contentControl.Resources.MergedDictionaries.Remove(oldRd);
                }

                string themeURL = String.Format(@"/WpfControlEx;component/Themes/Theme{0}.xaml", themeName);
                var newRd = new ResourceDictionary()
                {
                    Source = new Uri(themeURL, UriKind.Relative)
                };
                if (newRd == null)
                {
                    return;
                }
                contentControl.Resources.MergedDictionaries.Add(newRd);
            }
            catch { }
        }

        /// <summary>
        /// 应用全局主题
        /// </summary>
        /// <param name="app"></param>
        /// <param name="themeName"></param>
        public static void ApplyTheme(this Application app, string themeName)
        {
            try
            {
                //先移除已添加的主题资源字典
                ResourceDictionary oldRd = app.Resources.MergedDictionaries.FirstOrDefault(
                    p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Themes/Theme"));
                if (oldRd != null)
                {
                    app.Resources.MergedDictionaries.Remove(oldRd);
                }

                string themeURL = String.Format(@"/WpfControlEx;component/Themes/Theme{0}.xaml", themeName);
                var newRd = new ResourceDictionary()
                {
                    Source = new Uri(themeURL, UriKind.Relative)
                };
                if (newRd == null)
                {
                    return;
                }
                app.Resources.MergedDictionaries.Add(newRd);
            }
            catch { }
        }
    }
}
