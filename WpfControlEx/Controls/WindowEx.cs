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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlEx.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlEx"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfControlEx;assembly=WpfControlEx"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [TemplatePart(Name = HeaderContainerName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = MinimizeButtonName, Type = typeof(Button))]
    [TemplatePart(Name = RestoreButtonName, Type = typeof(ToggleButton))]
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
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
        #region Template Part Name        
        private const string HeaderContainerName = "PART_HeaderContainer";
        private const string MinimizeButtonName = "PART_MinimizeButton";
        private const string RestoreButtonName = "PART_RestoreButton";
        private const string CloseButtonName = "PART_CloseButton";
        private const string TopResizerName = "PART_TopResizer";
        private const string LeftResizerName = "PART_LeftResizer";
        private const string RightResizerName = "PART_RightResizer";
        private const string BottomResizerName = "PART_BottomResizer";
        private const string BottomRightResizerName = "PART_BottomRightResizer";
        private const string TopRightResizerName = "PART_TopRightResizer";
        private const string TopLeftResizerName = "PART_TopLeftResizer";
        private const string BottomLeftResizerName = "PART_BottomLeftResizer";
        #endregion

        #region Dependency Properties
        
        public static readonly DependencyProperty ShowDefaultHeaderProperty =
            DependencyProperty.Register("ShowDefaultHeader", typeof(bool), typeof(WindowEx), new FrameworkPropertyMetadata(true));
        
        public static readonly DependencyProperty ShowResizeGripProperty =
            DependencyProperty.Register("ShowResizeGrip", typeof(bool), typeof(WindowEx), new FrameworkPropertyMetadata(false));
        
        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register("CanResize", typeof(bool), typeof(WindowEx), new FrameworkPropertyMetadata(true));
        
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(WindowEx), new FrameworkPropertyMetadata(null, OnHeaderChanged));
        
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(WindowEx), new FrameworkPropertyMetadata(null));
        
        public static readonly DependencyProperty HeaderTempateSelectorProperty =
            DependencyProperty.Register("HeaderTempateSelector", typeof(DataTemplateSelector), typeof(WindowEx), new FrameworkPropertyMetadata(null));
        
        public static readonly DependencyProperty IsFullScreenMaximizeProperty =
            DependencyProperty.Register("IsFullScreenMaximize", typeof(bool), typeof(WindowEx), new FrameworkPropertyMetadata(false));
        
        private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WindowEx win = sender as WindowEx;
            win.RemoveLogicalChild(e.OldValue);
            win.AddLogicalChild(e.NewValue);
        }

        public bool ShowDefaultHeader
        {
            get { return (bool)GetValue(ShowDefaultHeaderProperty); }
            set { SetValue(ShowDefaultHeaderProperty, value); }
        }

        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set { SetValue(CanResizeProperty, value); }
        }

        public bool ShowResizeGrip
        {
            get { return (bool)GetValue(ShowResizeGripProperty); }
            set { SetValue(ShowResizeGripProperty, value); }
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplateSelector HeaderTempateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTempateSelectorProperty); }
            set { SetValue(HeaderTempateSelectorProperty, value); }
        }

        public bool IsFullScreenMaximize
        {
            get { return (bool)GetValue(IsFullScreenMaximizeProperty); }
            set { SetValue(IsFullScreenMaximizeProperty, value); }
        }
        #endregion

        static WindowEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowEx), new FrameworkPropertyMetadata(typeof(WindowEx)));
        }

        #region Private Fields
        private FrameworkElement headerContainer;

        private Button minimizeButton;

        private ToggleButton restoreButton;

        private Button closeButton;

        private Thumb topResizer;

        private Thumb leftResizer;

        private Thumb rightResizer;

        private Thumb bottomResizer;

        private Thumb bottomRightResizer;

        private Thumb topRightResizer;

        private Thumb topLeftResizer;

        private Thumb bottomLeftResizer;

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            headerContainer = GetTemplateChild<FrameworkElement>(HeaderContainerName);
            headerContainer.MouseLeftButtonDown += HeaderContainerMouseLeftButtonDown;

            closeButton = GetTemplateChild<Button>(CloseButtonName);
            closeButton.Click += delegate { Close(); };

            restoreButton = GetTemplateChild<ToggleButton>(RestoreButtonName);
            restoreButton.Checked += delegate { ChangeWindowState(WindowState.Maximized); };
            restoreButton.Unchecked += delegate { ChangeWindowState(WindowState.Normal); };

            StateChanged += new EventHandler(HeaderedWindowStateChanged);

            minimizeButton = GetTemplateChild<Button>(MinimizeButtonName);

            minimizeButton.Click += delegate { ChangeWindowState(WindowState.Minimized); };


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

        private T GetTemplateChild<T>(string childName) where T : FrameworkElement, new()
        {
            return (GetTemplateChild(childName) as T) ?? new T();
        }

        private void HeaderContainerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                DragMove();
            }
            else
            {
                ChangeWindowState(WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
            }
        }

        private void ChangeWindowState(WindowState state)
        {
            if (state == WindowState.Maximized)
            {
                if (!IsFullScreenMaximize && IsLocationOnPrimaryScreen())
                {
                    MaxHeight = SystemParameters.WorkArea.Height;
                    MaxWidth = SystemParameters.WorkArea.Width;
                }
                else
                {
                    MaxHeight = double.PositiveInfinity;
                    MaxWidth = double.PositiveInfinity;
                }
            }

            WindowState = state;
        }


        private void HeaderedWindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                restoreButton.IsChecked = null;
            }
            else
            {
                restoreButton.IsChecked = WindowState == WindowState.Maximized;
            }

        }

        private bool IsLocationOnPrimaryScreen()
        {
            return Left < SystemParameters.PrimaryScreenWidth && Top < SystemParameters.PrimaryScreenHeight;
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
    }
}
