# WpfControlEx
A WPF control set that supports multi-language and multi-themes.

1、多语言支持，内置英文、中文简体、中文繁体，可通过LanguageHelper.PriorityLangSource控制使用内置语言资源还是外置语言资源；
2、外置语言资源文件必须放到Localizations目录下，且LanguageHelper.PriorityLangSource = LanguageSource.External，外置语言资源命名规则：Language_{0}.xaml，{0}是语言编码替代符，语言编码统一按如下方式获取：System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures)
