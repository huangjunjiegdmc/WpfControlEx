# WpfControlEx
A WPF control set that supports multi-language and multi-themes.

## 本地化

1、多语言支持，内置英文、中文简体、中文繁体，可通过以下方法设置控件库外（引用控件库的程序）、控件库未支持的语言资源；

```c#
LanguageHelper.SetLanguage(this Application app, string path, string language);
```

2、设置控件库外（引用控件库的程序）语言资源时，语言资源文件命名必须包括语言编码，如：Language_{0}.xaml，{0}是语言编码替代符，语言编码统一按如下方式获取：

```c#
System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures);
```

## 主题

1、应用全局主题
	在Application的OnStartup事件中添加如下代码：

```c#
this.ApplyTheme("LightBlue");
```

​	或者在App.xaml中添加默认主题

```c#
<Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/WpfControlEx;component/Themes/ThemeLightBlue.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
```

​	在其它代码中切换时的方法：

```c#
Application.Current.ApplyTheme("LightBlue");
```

2、如果主题只想应该到某窗口，则在窗口xaml代码中添加如下代码：

```c#
xmlns:theme="clr-namespace:WpfControlEx.Themes;assembly=WpfControlEx"
theme:ThemeManager.ThemeName="LightBlue"
```

