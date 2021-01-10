using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace WpfControlEx.Utils
{
    /// <summary>
    /// 多语言帮助类
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// 优化使用内置语言资源还是外置语言资源，外置语言资源命名规则：Language_{0}.xaml，{0}是语言编码替代符，语言编码统一按如下方式获取：
        /// System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
        /// </summary>
        public static LanguageSource PriorityLangSource { get; set; } = LanguageSource.Internal;

        /// <summary>
        /// 当前语言，语言编码统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
        /// </summary>
        public static string CurrentLang { get; set; } = Constants.LANG_EN;

        /// <summary>
        /// 设置当前语言，根据属性PriorityLangSource的配置，支持内置及外置资源文件
        /// </summary>
        /// <param name="app">扩展Application方法</param>
        /// <param name="language">语言编码——按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)，中文简体：zh-Hans；英文：en</param>
        public static void SetLanguage(this Application app, string language = "")
        {
            if (string.IsNullOrWhiteSpace(language)
                || !System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures).Any(p => p.Name == language))
            {
                language = System.Globalization.CultureInfo.CurrentCulture.ToString();
            }

            if (language == CurrentLang)
            {
                return;
            }

            string languagePath = string.Empty;
            try
            {
                if (PriorityLangSource == LanguageSource.Internal)
                {
                    //先移除已添加的语言资源字典
                    ResourceDictionary oldRd = app.Resources.MergedDictionaries.FirstOrDefault(
                        p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Localizations/Language_")
                        || p.Source.OriginalString.Contains(@"\Localizations\Language_"));
                    if (oldRd != null)
                    {
                        app.Resources.MergedDictionaries.Remove(oldRd);
                    }

                    languagePath = String.Format(@"pack://application:,,,/WpfControlEx;component/Localizations/Language_{0}.xaml", language);
                    var newRd = new ResourceDictionary()
                    {
                        Source = new Uri(languagePath, UriKind.Absolute)
                    };
                    if (newRd == null)
                    {
                        return;
                    }
                    app.Resources.MergedDictionaries.Add(newRd);
                }
                else
                {
                    languagePath = AppDomain.CurrentDomain.BaseDirectory + String.Format(@"Localizations\Language_{0}.xaml", language);
                    if (!File.Exists(languagePath))
                    {
                        return;
                    }

                    //先移除已添加的语言资源字典
                    ResourceDictionary oldRd = app.Resources.MergedDictionaries.FirstOrDefault(
                        p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Localizations/Language_") 
                        || p.Source.OriginalString.Contains(@"\Localizations\Language_"));
                    if (oldRd != null)
                    {
                        app.Resources.MergedDictionaries.Remove(oldRd);
                    }

                    ResourceDictionary newRd = new ResourceDictionary()
                    {
                        Source = new Uri(languagePath, UriKind.Absolute)
                    };
                    app.Resources.MergedDictionaries.Add(newRd);
                }

                CurrentLang = language;

                //设置全局语言
                var culture = new System.Globalization.CultureInfo(language);

                #region .net4.5及以前版本用如下方法
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                #endregion

                #region .net4.6以上版本用如下方法
                System.Globalization.CultureInfo.CurrentCulture = culture;
                System.Globalization.CultureInfo.CurrentUICulture = culture;
                #endregion
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// messagebox之类的提示通过该方法获取语言资源
        /// </summary>
        /// <param name="key">语言资源Key</param>
        /// <returns></returns>
        public static string L(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return key;
            var objLanguage = Application.Current.TryFindResource(key);
            if (objLanguage == null) return key;
            return objLanguage.ToString();
        }

        /// <summary>
        /// 获取当前语言的资源。如果是英文，动词和名词之间有空格，中文不需要
        /// </summary>
        /// <param name="actionKey">动作语言资源Key</param>
        /// <param name="key">语言资源Key</param>
        /// <returns></returns>
        public static string L(string actionKey, string key)
        {
            if (string.IsNullOrWhiteSpace(actionKey)
                || string.IsNullOrWhiteSpace(key))
            {
                return string.Concat(actionKey, key);
            }
            var objActionText = Application.Current.TryFindResource(actionKey);
            var objText = Application.Current.TryFindResource(key);
            if (objActionText == null || objText == null)
            {
                return string.Concat(actionKey, key);
            }

            if (CurrentLang == Constants.LANG_EN)
            {
                return string.Concat(objActionText.ToString(), " ", objText.ToString());
            }
            else
            {
                return string.Concat(objActionText.ToString(), objText.ToString());
            }
        }

        /// <summary>
        /// 获取语言资源字典，该方法不好的是ResourceDictionary.Source是null，没办法根据关键信息移除特定的ResourceDictionary再添加
        /// </summary>
        /// <param name="language">语言编码——按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)，中文简体：zh-Hans；英文：en</param>
        /// <returns></returns>
        public static ResourceDictionary GetLanguageResourceDictionary(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();//or LoadFrom("WpfControlEx.dll");
                string packUri = String.Format(@"/WpfControlEx;component/Localizations/Language_{0}.xaml", language);
                return Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
            }
            return null;
        }
    }
}
