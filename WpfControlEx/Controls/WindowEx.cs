using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlEx.Controls.Native;

namespace WpfControlEx.Controls
{
    /// <summary>
    /// 自定义普通窗口
    /// </summary>
    [TemplatePart(Name = PART_Icon, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleMinimizeButton, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleMaximizeButton, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleRestoreButton, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_TitleCloseButton, Type = typeof(UIElement))]
    [TemplatePart(Name = TopResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = LeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = RightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopLeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomLeftResizerName, Type = typeof(Thumb))]
    public class WindowEx : Window
    {
        #region 常量
        private const string PART_Icon = "PART_Icon";
        private const string PART_TitleBar = "PART_TitleBar";
        private const string PART_TitleMinimizeButton = "PART_TitleMinimizeButton";
        private const string PART_TitleMaximizeButton = "PART_TitleMaximizeButton";
        private const string PART_TitleRestoreButton = "PART_TitleRestoreButton";
        private const string PART_TitleCloseButton = "PART_TitleCloseButton";

        private const string TopResizerName = "PART_TopResizer";
        private const string LeftResizerName = "PART_LeftResizer";
        private const string RightResizerName = "PART_RightResizer";
        private const string BottomResizerName = "PART_BottomResizer";
        private const string BottomRightResizerName = "PART_BottomRightResizer";
        private const string TopRightResizerName = "PART_TopRightResizer";
        private const string TopLeftResizerName = "PART_TopLeftResizer";
        private const string BottomLeftResizerName = "PART_BottomLeftResizer";
        #endregion



        #region 变量
        private Image icon;
        private Grid titleBar;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnRestore;
        private Button btnClose;

        private Thumb topResizer;
        private Thumb leftResizer;
        private Thumb rightResizer;
        private Thumb bottomResizer;
        private Thumb bottomRightResizer;
        private Thumb topRightResizer;
        private Thumb topLeftResizer;
        private Thumb bottomLeftResizer;
        #endregion



        #region 属性
        /// <summary>
        /// 标题栏高度
        /// </summary>
        public double TitleBarHeight { get; set; }

        /// <summary>
        /// 是否显示标题栏
        /// </summary>
        public static readonly DependencyProperty TitleBarVisibleProperty
            = DependencyProperty.Register("TitleBarVisible", typeof(bool), typeof(WindowEx),
                new PropertyMetadata(true, OnTitleBarVisiblePropertyChangedCallback));

        /// <summary>
        /// 是否显示标题栏
        /// </summary>
        public bool TitleBarVisible
        {
            get
            {
                return (bool)GetValue(TitleBarVisibleProperty);
            }
            set
            {
                SetValue(TitleBarVisibleProperty, value);
            }
        }

        #endregion

        static WindowEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowEx), new FrameworkPropertyMetadata(typeof(WindowEx)));
        }

        private static void OnTitleBarVisiblePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WindowEx windowEx = d as WindowEx;

            if (e.NewValue != e.OldValue)
            {
                if (!(bool)e.NewValue)
                {
                    windowEx.titleBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    windowEx.titleBar.Visibility = Visibility.Visible;
                }
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //设置无标题窗口工作区，不遮挡任务栏
            Rect rect = SystemParameters.WorkArea;
            MaxWidth = rect.Width;
            MaxHeight = rect.Height;

            icon = GetTemplateChild(PART_Icon) as Image;
            icon.MouseDown += Icon_MouseDown;

            titleBar = GetTemplateChild(PART_TitleBar) as Grid;
            TitleBarHeight = titleBar.Height;
            titleBar.MouseDown += TitleBar_MouseDown;
            titleBar.MouseRightButtonUp += TitleBar_MouseRightButtonUp;
            titleBar.MouseLeftButtonDown += TitleBar_MouseLeftButtonDown;

            btnMinimize = GetTemplateChild(PART_TitleMinimizeButton) as Button;
            btnMinimize.Click += BtnMinimize_Click;

            btnMaximize = GetTemplateChild(PART_TitleMaximizeButton) as Button;
            btnMaximize.Click += BtnMaximize_Click;

            btnRestore = GetTemplateChild(PART_TitleRestoreButton) as Button;
            btnRestore.Click += BtnRestore_Click;

            btnClose = GetTemplateChild(PART_TitleCloseButton) as Button;
            btnClose.Click += BtnClose_Click;

            topResizer = GetTemplateChild<Thumb>(TopResizerName);
            topResizer.DragDelta += new DragDeltaEventHandler(ResizeTop);

            leftResizer = GetTemplateChild<Thumb>(LeftResizerName);
            leftResizer.DragDelta += new DragDeltaEventHandler(ResizeLeft);

            rightResizer = GetTemplateChild<Thumb>(RightResizerName);
            rightResizer.DragDelta += new DragDeltaEventHandler(ResizeRight);

            bottomResizer = GetTemplateChild<Thumb>(BottomResizerName);
            bottomResizer.DragDelta += new DragDeltaEventHandler(ResizeBottom);

            bottomRightResizer = GetTemplateChild<Thumb>(BottomRightResizerName);
            bottomRightResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomRight);

            topRightResizer = GetTemplateChild<Thumb>(TopRightResizerName);
            topRightResizer.DragDelta += new DragDeltaEventHandler(ResizeTopRight);

            topLeftResizer = GetTemplateChild<Thumb>(TopLeftResizerName);
            topLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeTopLeft);

            bottomLeftResizer = GetTemplateChild<Thumb>(BottomLeftResizerName);
            bottomLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomLeft);
        }

        /// <summary>
        /// 窗口图标单击双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Icon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //关闭窗口
                Close();
            }
            else
            {
                //弹出系统菜单
                ShowSystemMenuPhysicalCoordinates(this, PointToScreen(new Point(0, TitleBarHeight)));
            }
        }

        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
        }

        private void TitleBar_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point point = PointToScreen(e.GetPosition(sender as Image));
            ShowSystemMenuPhysicalCoordinates(this, point);
        }

        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch { }
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private T GetTemplateChild<T>(string childName) where T : FrameworkElement, new()
        {
            return (GetTemplateChild(childName) as T) ?? new T();
        }


        #region Resize
        private void ResizeBottomLeft(object sender, DragDeltaEventArgs e)
        {
            ResizeLeft(sender, e);
            ResizeBottom(sender, e);
        }

        private void ResizeTopLeft(object sender, DragDeltaEventArgs e)
        {
            ResizeTop(sender, e);
            ResizeLeft(sender, e);
        }

        private void ResizeTopRight(object sender, DragDeltaEventArgs e)
        {
            ResizeRight(sender, e);
            ResizeTop(sender, e);
        }

        private void ResizeBottomRight(object sender, DragDeltaEventArgs e)
        {
            ResizeBottom(sender, e);
            ResizeRight(sender, e);
        }

        private void ResizeBottom(object sender, DragDeltaEventArgs e)
        {
            if (ActualHeight <= MinHeight && e.VerticalChange < 0)
            {
                return;
            }

            if (double.IsNaN(Height))
            {
                Height = ActualHeight;
            }

            Height += e.VerticalChange;
        }

        private void ResizeRight(object sender, DragDeltaEventArgs e)
        {
            if (ActualWidth <= MinWidth && e.HorizontalChange < 0)
            {
                return;
            }

            if (double.IsNaN(Width))
            {
                Width = ActualWidth;
            }

            Width += e.HorizontalChange;
        }

        private void ResizeLeft(object sender, DragDeltaEventArgs e)
        {
            if (ActualWidth <= MinWidth && e.HorizontalChange > 0)
            {
                return;
            }

            if (double.IsNaN(Width))
            {
                Width = ActualWidth;
            }

            Width -= e.HorizontalChange;
            Left += e.HorizontalChange;
        }

        private void ResizeTop(object sender, DragDeltaEventArgs e)
        {
            if (ActualHeight <= MinHeight && e.VerticalChange > 0)
            {
                return;
            }

            if (double.IsNaN(Height))
            {
                Height = ActualHeight;
            }

            Height -= e.VerticalChange;
            Top += e.VerticalChange;
        }
        #endregion

#pragma warning disable 618
        private static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
        {
            if (window == null) return;

            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero || !Win32Api.IsWindow(hwnd))
                return;

            var hmenu = Win32Api.GetSystemMenu(hwnd, false);

            //禁用启用系统菜单项
            if (window.WindowState == WindowState.Maximized)
            {
                Win32Api.EnableMenuItem(hmenu, Constants.SC_RESTORE,
                    Constants.MF_BYCOMMAND | Constants.MF_ENABLED);

                Win32Api.EnableMenuItem(hmenu, Constants.SC_SIZE,
                   Constants.MF_BYCOMMAND | Constants.MF_GRAYED | Constants.MF_DISABLED);
                Win32Api.EnableMenuItem(hmenu, Constants.SC_MOVE,
                    Constants.MF_BYCOMMAND | Constants.MF_GRAYED | Constants.MF_DISABLED);
                Win32Api.EnableMenuItem(hmenu, Constants.SC_MAXIMIZE,
                    Constants.MF_BYCOMMAND | Constants.MF_GRAYED | Constants.MF_DISABLED);
            }
            else
            {
                Win32Api.EnableMenuItem(hmenu, Constants.SC_MOVE,
                    Constants.MF_BYCOMMAND | Constants.MF_ENABLED);
                Win32Api.EnableMenuItem(hmenu, Constants.SC_MAXIMIZE,
                    Constants.MF_BYCOMMAND | Constants.MF_ENABLED);

                Win32Api.EnableMenuItem(hmenu, Constants.SC_RESTORE,
                    Constants.MF_BYCOMMAND | Constants.MF_GRAYED | Constants.MF_DISABLED);
                Win32Api.EnableMenuItem(hmenu, Constants.SC_SIZE,
                    Constants.MF_BYCOMMAND | Constants.MF_GRAYED | Constants.MF_DISABLED);
            }            

            var cmd = Win32Api.TrackPopupMenuEx(hmenu, Constants.TPM_LEFTBUTTON | Constants.TPM_RETURNCMD,
                (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, hwnd, IntPtr.Zero);
            if (0 != cmd)
                Win32Api.PostMessage(hwnd, WM.SYSCOMMAND, new IntPtr(cmd), IntPtr.Zero);
        }
#pragma warning restore 618
    }
}
