using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlEx.Controls.Helper;
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
    [TemplatePart(Name = PART_ResizeGrip, Type = typeof(UIElement))]
    [TemplatePart(Name = BottomRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopRightResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = TopLeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = BottomLeftResizerName, Type = typeof(Thumb))]
    [TemplatePart(Name = PART_MaskGrid, Type = typeof(UIElement))]
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
        private const string PART_ResizeGrip = "PART_ResizeGrip";
        private const string BottomRightResizerName = "PART_BottomRightResizer";
        private const string TopRightResizerName = "PART_TopRightResizer";
        private const string TopLeftResizerName = "PART_TopLeftResizer";
        private const string BottomLeftResizerName = "PART_BottomLeftResizer";
        private const string PART_MaskGrid = "PART_MaskGrid";
        #endregion



        #region 变量
        /// <summary>
        /// 需要在最终使用该窗口的窗口里面设置Icon的源
        /// </summary>
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
        private ResizeGrip resizeGrip;
        private Thumb bottomRightResizer;
        private Thumb topRightResizer;
        private Thumb topLeftResizer;
        private Thumb bottomLeftResizer;

        private Grid maskGrid;

        /// <summary>
        /// 全局鼠标钩子
        /// </summary>
        private MouseHook mouseHook;

        /// <summary>
        /// 父窗口句柄
        /// </summary>
        private IntPtr m_parentWindowHandle;

        /// <summary>
        /// 鼠标点击的窗口句柄
        /// </summary>
        private IntPtr m_clickWindowHandle;

        /// <summary>
        /// 窗口闪烁动画
        /// </summary>
        private Storyboard m_flashWindowAnimation;
        #endregion



        #region 属性
        /// <summary>
        /// 标题栏高度
        /// </summary>
        public double TitleBarHeight { get; private set; }

        /// <summary>
        /// 是否模式窗口
        /// </summary>
        private static readonly DependencyProperty IsModelWindowProperty
            = DependencyProperty.Register("IsModelWindow", typeof(bool), typeof(WindowEx),
                new PropertyMetadata(true));
        
        /// <summary>
        /// 是否模式窗口
        /// </summary>
        public bool IsModelWindow
        {
            get
            {
                return (bool)GetValue(IsModelWindowProperty);
            }
            private set
            {
                SetValue(IsModelWindowProperty, value);
            }
        }

        /// <summary>
        /// 是否显示标题栏
        /// </summary>
        public static readonly DependencyProperty TitleBarVisibleProperty
            = DependencyProperty.Register("TitleBarVisible", typeof(bool), typeof(WindowEx),
                new PropertyMetadata(true, OnTitleBarVisiblePropertyChangedCallback));

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

        /// <summary>
        /// 是否显示蒙板
        /// </summary>
        public static readonly DependencyProperty ShowMaskGirdProperty
            = DependencyProperty.Register("ShowMaskGird", typeof(bool), typeof(WindowEx),
                new PropertyMetadata(false, OnShowMaskGirdPropertyChangedCallback));

        /// <summary>
        /// 是否显示蒙板
        /// </summary>
        public bool ShowMaskGird
        {
            get
            {
                return (bool)GetValue(ShowMaskGirdProperty);
            }
            set
            {
                SetValue(ShowMaskGirdProperty, value);
            }
        }

        /// <summary>
        /// 是否显示蒙板属性修改响应函数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnShowMaskGirdPropertyChangedCallback(DependencyObject d,
           DependencyPropertyChangedEventArgs e)
        {
            WindowEx windowEx = d as WindowEx;

            if (e.NewValue != e.OldValue)
            {
                if ((bool)e.NewValue)
                {
                    windowEx.maskGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    windowEx.maskGrid.Visibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        static WindowEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowEx), new FrameworkPropertyMetadata(typeof(WindowEx)));
        }

        
        private void MouseHook_MouseClickEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.Owner == null)
            {
                return;
            }

            //通过判断点击窗口的句柄是否是父窗口的句柄来判断是否点击的是父窗口
            m_parentWindowHandle = new WindowInteropHelper(this.Owner).Handle;
            m_clickWindowHandle = WinUserApi.WindowFromPoint(e.X, e.Y);
            //如果点击的是父窗口，则闪烁模态窗口
            if (m_clickWindowHandle.ToString().Equals(m_parentWindowHandle.ToString()))
            {
                m_flashWindowAnimation.Begin();
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

            resizeGrip = GetTemplateChild<ResizeGrip>(PART_ResizeGrip);

            bottomRightResizer = GetTemplateChild<Thumb>(BottomRightResizerName);
            bottomRightResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomRight);

            topRightResizer = GetTemplateChild<Thumb>(TopRightResizerName);
            topRightResizer.DragDelta += new DragDeltaEventHandler(ResizeTopRight);

            topLeftResizer = GetTemplateChild<Thumb>(TopLeftResizerName);
            topLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeTopLeft);

            bottomLeftResizer = GetTemplateChild<Thumb>(BottomLeftResizerName);
            bottomLeftResizer.DragDelta += new DragDeltaEventHandler(ResizeBottomLeft);

            maskGrid = GetTemplateChild<Grid>(PART_MaskGrid);

            SetModelWindowProperty();
        }

        /// <summary>
        /// 隐藏非模式窗口属性
        /// </summary>
        private void SetModelWindowProperty()
        {
            IsModelWindow = WindowHelper.IsModal(this);
            if (IsModelWindow)
            {
                btnMaximize.Visibility = Visibility.Collapsed;
                btnRestore.Visibility = Visibility.Collapsed;
                btnMinimize.Visibility = Visibility.Collapsed;

                topResizer.Visibility = Visibility.Collapsed;
                leftResizer.Visibility = Visibility.Collapsed;
                rightResizer.Visibility = Visibility.Collapsed;
                bottomResizer.Visibility = Visibility.Collapsed;
                resizeGrip.Visibility = Visibility.Collapsed;
                bottomRightResizer.Visibility = Visibility.Collapsed;
                topRightResizer.Visibility = Visibility.Collapsed;
                topLeftResizer.Visibility = Visibility.Collapsed;
                bottomLeftResizer.Visibility = Visibility.Collapsed;

                GreateFlashWindowAnimate();

                //全局鼠标勾子，闪烁模式窗口自定义标题栏和边框
                mouseHook = new MouseHook();
                mouseHook.SetHook();
                mouseHook.MouseClickEvent += MouseHook_MouseClickEvent;
                mouseHook.MouseMoveEvent += MouseHook_MouseMoveEvent;
            }
        }

        private void MouseHook_MouseMoveEvent(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //Title = e.X.ToString() + " - " + e.Y.ToString();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (IsModelWindow)
            {
                mouseHook.UnHook();
            }
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
                //System.Windows.SystemCommands.ShowSystemMenu(this, PointToScreen(new Point(0, TitleBarHeight)));//不能控制菜单，不用
                //弹出系统菜单
                ShowSystemMenuPhysicalCoordinates(this, PointToScreen(new Point(0, TitleBarHeight)));
            }
        }

        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                //模式窗口不响应最大化、还原
                if (IsModelWindow)
                {
                    return;
                }

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


        #region 窗口大小调整
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

        /// <summary>
        /// 显示系统菜单
        /// </summary>
        /// <param name="window"></param>
        /// <param name="physicalScreenLocation"></param>
        private void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
        {
            if (window == null) return;

            var hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero || !WinUserApi.IsWindow(hwnd))
                return;

            var hmenu = WinUserApi.GetSystemMenu(hwnd, false);

            //禁用启用系统菜单项
            if (window.WindowState == WindowState.Maximized)
            {
                WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_RESTORE,
                    WinUserApi.MF_BYCOMMAND | WinUserApi.MF_ENABLED);

                WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_SIZE,
                   WinUserApi.MF_BYCOMMAND | WinUserApi.MF_GRAYED | WinUserApi.MF_DISABLED);
                WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_MOVE,
                    WinUserApi.MF_BYCOMMAND | WinUserApi.MF_GRAYED | WinUserApi.MF_DISABLED);
                WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_MAXIMIZE,
                    WinUserApi.MF_BYCOMMAND | WinUserApi.MF_GRAYED | WinUserApi.MF_DISABLED);
            }
            else
            {
                if (IsModelWindow)
                {
                    //模式窗口只保留移动和关闭菜单
                    WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_MOVE,
                        WinUserApi.MF_BYCOMMAND | WinUserApi.MF_ENABLED);

                    WinUserApi.DeleteMenu(hmenu, WinUserApi.SC_RESTORE,
                        WinUserApi.MF_BYCOMMAND);
                    WinUserApi.DeleteMenu(hmenu, WinUserApi.SC_SIZE,
                        WinUserApi.MF_BYCOMMAND);
                    WinUserApi.DeleteMenu(hmenu, WinUserApi.SC_MINIMIZE,
                        WinUserApi.MF_BYCOMMAND);
                    WinUserApi.DeleteMenu(hmenu, WinUserApi.SC_MAXIMIZE,
                        WinUserApi.MF_BYCOMMAND);
                }
                else
                {
                    WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_MOVE,
                                        WinUserApi.MF_BYCOMMAND | WinUserApi.MF_ENABLED);
                    WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_MAXIMIZE,
                        WinUserApi.MF_BYCOMMAND | WinUserApi.MF_ENABLED);

                    WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_RESTORE,
                        WinUserApi.MF_BYCOMMAND | WinUserApi.MF_GRAYED | WinUserApi.MF_DISABLED);
                    WinUserApi.EnableMenuItem(hmenu, WinUserApi.SC_SIZE,
                        WinUserApi.MF_BYCOMMAND | WinUserApi.MF_GRAYED | WinUserApi.MF_DISABLED);
                }                
            }            

            var cmd = WinUserApi.TrackPopupMenuEx(hmenu, WinUserApi.TPM_LEFTBUTTON | WinUserApi.TPM_RETURNCMD,
                (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, hwnd, IntPtr.Zero);
            if (0 != cmd)
                WinUserApi.PostMessage(hwnd, WinUserApi.WM_SYSCOMMAND, new IntPtr(cmd), IntPtr.Zero);
        }

        /// <summary>
        /// 创建窗口闪烁动画
        /// </summary>
        private void GreateFlashWindowAnimate()
        {
            m_flashWindowAnimation = new Storyboard();

            for (int i = 0; i < 14; i += 2)
            {
                DoubleAnimation doubleAnimation1 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromMilliseconds(i * 100),
                    To = 0.75,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100))
                };
                Storyboard.SetTarget(doubleAnimation1, this);
                Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath(WindowEx.OpacityProperty));
                m_flashWindowAnimation.Children.Add(doubleAnimation1);

                DoubleAnimation doubleAnimation2 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromMilliseconds((i + 1) * 100),
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100))
                };
                Storyboard.SetTarget(doubleAnimation2, this);
                Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(WindowEx.OpacityProperty));
                m_flashWindowAnimation.Children.Add(doubleAnimation2);
            }
        }
    }
}
