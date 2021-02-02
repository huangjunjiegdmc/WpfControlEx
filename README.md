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

## WindowEx

​		自定义普通窗口，包括标题栏、边框、系统菜单及常用事件如Win10的拖放窗口到屏幕顶部时自动最大化等，同时支持主题及配色、支持多语言等。目标并不是要定义多么强大灵活的窗口基类，像很多程序主界面都设计得比较个性化，这个基类不提供直接支持，但是可能通过修改控件模板大致目的。

### 普通窗口

​		创建WindowEx窗口实例时，默认就是跟Windows的普通窗口一样，默认的标题栏包括图标、标题、最小化、最大化、向下还原、关闭等，同时标题栏右键会弹出Windows的系统菜单。

### 模态窗口

​		创建WindowEx窗口实例时，如果用Window的ShowDialog()函数弹出模态窗口，窗口的默认状态也是跟Windows的模态窗口一样，标题栏包括图标、标题、关闭，标题栏弹出的右键菜单只有两个菜单：移动、关闭。

## MessageBoxWindow

​		默认的消息提示窗口比较简洁，一片白屏，标题栏和提示消息的唯一区别就是显示在顶部，字体较大。如下图，底部操作按钮右对齐排列。



