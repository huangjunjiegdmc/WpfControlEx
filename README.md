# WpfControlEx
A WPF control set that supports multi-language and multi-themes.

1、多语言支持，内置英文、中文简体、中文繁体，可通过LanguageHelper.PriorityLangSource控制使用内置语言资源还是外置语言资源；
2、外置语言资源文件必须放到Localizations目录下，且LanguageHelper.PriorityLangSource = LanguageSource.External，外置语言资源命名规则：Language_{0}.xaml，{0}是语言编码替代符，语言编码统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
3、主题应用方法
3.1、应用全局主题
在Application的OnStartup事件中添加如下代码
this.ApplyTheme("LightBlue");
在其它代码中切换时的方法：Application.Current.ApplyTheme("LightBlue");
3.2、如果主题只应该到某窗口，则在窗口xaml代码中添加如下代码
xmlns:theme="clr-namespace:WpfControlEx.Themes;assembly=WpfControlEx"
theme:ThemeManager.ThemeName="LightBlue"