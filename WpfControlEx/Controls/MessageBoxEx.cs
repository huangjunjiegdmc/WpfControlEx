using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlEx.Controls
{
    public sealed class MessageBoxEx
    {
        //
        // 摘要:
        //     前面显示一个消息框在指定的时段。 该消息框显示消息、 标题栏标题、 按钮和图标;接受默认消息框的结果并返回一个结果。
        //
        // 参数:
        //   owner:
        //     一个 System.Windows.Window ，它表示该消息框的所有者窗口。
        //
        //   messageBoxText:
        //     一个 System.String ，它指定要显示的文本。
        //
        //   caption:
        //     一个 System.String ，它指定要显示的标题栏标题。
        //
        //   button:
        //     一个 System.Windows.MessageBoxButton 值，该值指定哪个按钮或要显示的按钮。
        //
        //   icon:
        //     一个 System.Windows.MessageBoxImage 值，该值指定要显示的图标。
        //
        //   defaultResult:
        //     一个 System.Windows.MessageBoxResult 值，该值指定消息框的默认结果。
        //
        // 返回结果:
        //     一个 System.Windows.MessageBoxResult 值，该值指定在用户单击哪个消息框按钮。
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxResult defaultResult)
        {
            MessageBoxWindow messageBoxEx = new MessageBoxWindow(owner, messageBoxText, caption, button, defaultResult);
            messageBoxEx.ShowDialog();
            return messageBoxEx.MessageBoxResult;
        }

        //
        // 摘要:
        //     前面显示一个消息框在指定的时段。 该消息框显示消息、 标题栏标题和按钮;它也会返回一个结果。
        //
        // 参数:
        //   owner:
        //     一个 System.Windows.Window ，它表示该消息框的所有者窗口。
        //
        //   messageBoxText:
        //     一个 System.String ，它指定要显示的文本。
        //
        //   caption:
        //     一个 System.String ，它指定要显示的标题栏标题。
        //
        //   button:
        //     一个 System.Windows.MessageBoxButton 值，该值指定哪个按钮或要显示的按钮。
        //
        // 返回结果:
        //     一个 System.Windows.MessageBoxResult 值，该值指定在用户单击哪个消息框按钮。
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
        {
            MessageBoxWindow messageBoxEx = new MessageBoxWindow(owner, messageBoxText, caption, button);
            messageBoxEx.ShowDialog();
            return messageBoxEx.MessageBoxResult;
        }
        //
        // 摘要:
        //     前面显示一个消息框在指定的时段。 该消息框显示消息和标题栏标题;它将返回结果。
        //
        // 参数:
        //   owner:
        //     一个 System.Windows.Window ，它表示该消息框的所有者窗口。
        //
        //   messageBoxText:
        //     一个 System.String ，它指定要显示的文本。
        //
        //   caption:
        //     一个 System.String ，它指定要显示的标题栏标题。
        //
        // 返回结果:
        //     一个 System.Windows.MessageBoxResult 值，该值指定在用户单击哪个消息框按钮。
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            MessageBoxWindow messageBoxEx = new MessageBoxWindow(owner, messageBoxText, caption);
            messageBoxEx.ShowDialog();
            return messageBoxEx.MessageBoxResult;
        }
        //
        // 摘要:
        //     前面显示一个消息框在指定的时段。 消息框中显示一条消息，并返回一个结果。
        //
        // 参数:
        //   owner:
        //     一个 System.Windows.Window ，它表示该消息框的所有者窗口。
        //
        //   messageBoxText:
        //     一个 System.String ，它指定要显示的文本。
        //
        // 返回结果:
        //     一个 System.Windows.MessageBoxResult 值，该值指定在用户单击哪个消息框按钮。
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            MessageBoxWindow messageBoxEx = new MessageBoxWindow(owner, messageBoxText);
            messageBoxEx.ShowDialog();
            return messageBoxEx.MessageBoxResult;
        }
    }
}
