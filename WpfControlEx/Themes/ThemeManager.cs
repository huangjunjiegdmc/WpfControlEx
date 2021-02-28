using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            if (GetThemes().Any(p => p.ThemeKey == themeName))
            {
                Application.Current.ApplyTheme(themeName);
            }
        }

        #endregion


        /// <summary>
        /// 获取支持的主题列表
        /// </summary>
        /// <returns></returns>
        public static List<ThemeModel> GetThemes()
        {
            List<ThemeModel> themes = new List<ThemeModel>();
            themes.Add(new ThemeModel()
            {
                ThemeColorPreview = new SolidColorBrush(Color.FromArgb(255, 24, 144, 255)),
                ThemeKey = "LightBlue",
                ThemeName = Utils.LanguageHelper.L("Blue")
            });
            themes.Add(new ThemeModel()
            {
                ThemeColorPreview = new SolidColorBrush(Color.FromArgb(255, 66, 160, 79)),
                ThemeKey = "LightGreen",
                ThemeName = Utils.LanguageHelper.L("Green")
            });
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

    public class ThemeInstance
    {
        /// <summary>
        /// 新建静态属性变更通知
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        private static List<ThemeModel> _ThemeList = ThemeManager.GetThemes();

        public static List<ThemeModel> ThemeList
        {
            get
            {
                return _ThemeList;
            }
            set
            {
                _ThemeList = value;
                //调用通知
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(ThemeList)));
            }
        }
    }
}
