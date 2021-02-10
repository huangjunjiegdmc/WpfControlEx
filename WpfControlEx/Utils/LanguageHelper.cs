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
        /// 当前语言，语言编码统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)，中文简体：zh-Hans；英文：en
        /// </summary>
        public static string CurrentLang { get; set; } = Enum.GetName(typeof(LanguageEnum), LanguageEnum.en).Replace("_", "-");

        /// <summary>
        /// 设置当前语言
        /// </summary>
        /// <param name="languageEnum">语言编码——统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)，中文简体：zh-Hans；英文：en</param>
        public static void SetLanguage(LanguageEnum languageEnum)
        {
            string language = Enum.GetName(typeof(LanguageEnum), languageEnum).Replace("_", "-");

            if (string.IsNullOrWhiteSpace(language)
                || !System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures).Any(p => p.Name == language))
            {
                language = System.Globalization.CultureInfo.CurrentCulture.ToString();
            }

            string languagePath = string.Empty;
            try
            {
                //先移除已添加的语言资源字典
                ResourceDictionary oldRd = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                    p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Localizations/Language_"));
                if (oldRd != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(oldRd);
                }

                languagePath = String.Format(@"/WpfControlEx;component/Localizations/Language_{0}.xaml", language);
                var newRd = new ResourceDictionary()
                {
                    Source = new Uri(languagePath, UriKind.Relative)
                };
                if (newRd == null)
                {
                    return;
                }
                Application.Current.Resources.MergedDictionaries.Add(newRd);

                #region 本地资源文件
                //{
                //    
                //    languagePath = AppDomain.CurrentDomain.BaseDirectory + String.Format(@"Localizations\Language_{0}.xaml", language);
                //    if (!File.Exists(languagePath))
                //    {
                //        return;
                //    }

                //    //先移除已添加的语言资源字典
                //    ResourceDictionary oldRd = app.Resources.MergedDictionaries.FirstOrDefault(
                //        p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Localizations/Language_") 
                //        || p.Source.OriginalString.Contains(@"\Localizations\Language_"));
                //    if (oldRd != null)
                //    {
                //        app.Resources.MergedDictionaries.Remove(oldRd);
                //    }

                //    ResourceDictionary newRd = new ResourceDictionary()
                //    {
                //        Source = new Uri(languagePath, UriKind.Absolute)
                //    };
                //    app.Resources.MergedDictionaries.Add(newRd);
                //}
                #endregion

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
        /// 根据指定的语言资源路径设置当前语言，通常用于设置控件库缺少的语言包
        /// </summary>
        /// <param name="path">语言资源路径</param>
        /// <param name="language">语言编码——统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)</param>
        public static void SetLanguage(string path, string language)
        {
            string languagePath = string.Empty;
            try
            {
                //先移除已添加的语言资源字典
                ResourceDictionary oldRd = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                    p => p.Source.OriginalString.Contains(@"WpfControlEx;component/Localizations/Language_"));
                if (oldRd != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(oldRd);
                }
                oldRd = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                   p => p.Source.OriginalString.Contains(path.Substring(0, path.IndexOf(language))));
                if (oldRd != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(oldRd);
                }

                var newRd = new ResourceDictionary()
                {
                    Source = new Uri(path, UriKind.RelativeOrAbsolute)
                };
                if (newRd == null)
                {
                    return;
                }
                Application.Current.Resources.MergedDictionaries.Add(newRd);


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

            if (CurrentLang == Enum.GetName(typeof(LanguageEnum), LanguageEnum.en).Replace("_", "-"))
            {
                return string.Concat(objActionText.ToString(), " ", objText.ToString());
            }
            else
            {
                return string.Concat(objActionText.ToString(), objText.ToString());
            }
        }
    }
}
