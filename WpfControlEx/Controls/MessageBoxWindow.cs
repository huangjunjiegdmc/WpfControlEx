using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using WpfControlEx.Controls.Native;

namespace WpfControlEx.Controls
{
    [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
    public class MessageBoxWindow : MyWindowBase
    {
        /// <summary>
        /// 模板路径
        /// </summary>
        private const string TemplateUri = "pack://application:,,,/WpfControlEx;component/Themes/Generic.xaml";
        private const string PART_TitleBar = "PART_TitleBar";

        #region 变量
        /// <summary>
        /// 全局鼠标勾子
        /// </summary>
        private MouseHook m_mouseHook;

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

        /// <summary>
        /// 标题栏
        /// </summary>
        private Grid titleBar;

        /// <summary>
        /// 确定
        /// </summary>
        private Button btn_PART_ButtonOK;

        /// <summary>
        /// 是
        /// </summary>
        private Button btn_PART_ButtonYes;

        /// <summary>
        /// 否
        /// </summary>
        private Button btn_PART_ButtonNo;

        /// <summary>
        /// 取消
        /// </summary>
        private Button btn_PART_ButtonCancel;

        /// <summary>
        /// 按钮栏
        /// </summary>
        private StackPanel sp_PART_ButtonBarStackPanel;

        /// <summary>
        /// 
        /// </summary>
        ResourceDictionary genericResourceDictionary;
        #endregion



        #region 属性
        public string Message { get; set; }

        public MessageBoxResult MessageBoxResult { get; set; }

        public MessageBoxButton MessageBoxButton { get; set; }

        #endregion



        #region 函数
        public MessageBoxWindow(Window owner, string messageBoxText)
        {
            Owner = owner;
            Message = messageBoxText;
            AutoSetWindowWidth();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //窗口样式
            genericResourceDictionary = new ResourceDictionary { Source = new Uri(TemplateUri) };
            Style = genericResourceDictionary["MessageBoxExStyle"] as Style;
        }

        public MessageBoxWindow(Window owner, string messageBoxText, string caption)
        {
            Owner = owner;
            Message = messageBoxText;
            Title = caption;
            AutoSetWindowWidth();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //窗口样式
            genericResourceDictionary = new ResourceDictionary { Source = new Uri(TemplateUri) };
            Style = genericResourceDictionary["MessageBoxExStyle"] as Style;
        }

        public MessageBoxWindow(Window owner, string messageBoxText, string caption,
            MessageBoxButton button)
        {
            Owner = owner;
            Message = messageBoxText;
            Title = caption;
            MessageBoxButton = button;
            AutoSetWindowWidth();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //窗口样式
            genericResourceDictionary = new ResourceDictionary { Source = new Uri(TemplateUri) };
            Style = genericResourceDictionary["MessageBoxExStyle"] as Style;
        }

        public MessageBoxWindow(Window owner, string messageBoxText, string caption,
            MessageBoxButton button, MessageBoxResult defaultResult)
        {
            Owner = owner;
            Message = messageBoxText;
            Title = caption;
            MessageBoxButton = button;
            MessageBoxResult = defaultResult;
            AutoSetWindowWidth();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //窗口样式
            genericResourceDictionary = new ResourceDictionary { Source = new Uri(TemplateUri) };
            Style = genericResourceDictionary["MessageBoxExStyle"] as Style;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MessageBoxWindow()
        {
            m_mouseHook.UnHook();//卸载全局鼠标勾子
        }

        /// <summary>
        /// 自动计算窗口宽度
        /// </summary>
        private void AutoSetWindowWidth()
        {
            int messageByteCount;

            Height = 140;//默认，激活窗口时自动调整

            //小窗口每行可以20个中文左右，40字节，3行以上转中窗口
            //中窗口每行可以40个中文左右，80字节，3行以上转大窗口
            //大窗口每行可以60个中文左右，120字节
            //80个中文左右，160字节
            //100个中文左右，200字节
            messageByteCount = Encoding.Default.GetByteCount(Message);
            if (messageByteCount <= 40 * 3)
            {
                Width = 300;
            }
            else if (messageByteCount > 40 * 3 && messageByteCount <= 80 * 3)
            {
                Width = 400;
            }
            else if (messageByteCount > 80 * 3 && messageByteCount <= 120 * 3)
            {
                Width = 500;
            }
            else if (messageByteCount > 120 * 3 && messageByteCount <= 160 * 3)
            {
                Width = 600;
            }
            else if (messageByteCount > 160 * 3 && messageByteCount <= 200 * 3)
            {
                Width = 700;
            }
            else
            {
                Width = 400;
            }
        }

        /// <summary>
        /// 闪烁模态窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                   new Action(() => { this.Activate(); }));
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;            

            //全局鼠标勾子，闪烁模式窗口自定义标题栏和边框
            m_mouseHook = new MouseHook();
            m_mouseHook.SetHook();
            m_mouseHook.MouseClickEvent += MouseHook_MouseClickEvent;

            GreateFlashWindowAnimate();

            titleBar = GetTemplateChild(PART_TitleBar) as Grid;

            //生成按钮栏
            sp_PART_ButtonBarStackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 10, 0)
            };
            
            btn_PART_ButtonOK = new Button()
            {
                Content = "确定",
                Style = genericResourceDictionary["ButtonBlue"] as Style,
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 0, 5, 0),
                IsDefault = true
            };
            btn_PART_ButtonOK.Click += Btn_PART_ButtonOK_Click;
            sp_PART_ButtonBarStackPanel.Children.Add(btn_PART_ButtonOK);

            btn_PART_ButtonYes = new Button()
            {
                Content = "是",
                Style = genericResourceDictionary["ButtonBlue"] as Style,
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 0, 5, 0)
            };
            btn_PART_ButtonYes.Click += Btn_PART_ButtonYes_Click;
            sp_PART_ButtonBarStackPanel.Children.Add(btn_PART_ButtonYes);

            btn_PART_ButtonNo = new Button()
            {
                Content = "否",
                Style = genericResourceDictionary["ButtonBlue"] as Style,
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 0, 5, 0)
            };
            btn_PART_ButtonNo.Click += Btn_PART_ButtonNo_Click;
            sp_PART_ButtonBarStackPanel.Children.Add(btn_PART_ButtonNo);

            btn_PART_ButtonCancel = new Button()
            {
                Content = "取消",
                Style = genericResourceDictionary["ButtonWhite"] as Style,
                Width = 100,
                Height = 30,
                Margin = new Thickness(0, 0, 10, 0)
            };
            btn_PART_ButtonCancel.Click += Btn_PART_ButtonCancel_Click;
            sp_PART_ButtonBarStackPanel.Children.Add(btn_PART_ButtonCancel);


            if (MessageBoxButton == MessageBoxButton.OK)
            {
                //只有一个按钮，设置按钮居中
                sp_PART_ButtonBarStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                sp_PART_ButtonBarStackPanel.Margin = new Thickness(0);
                btn_PART_ButtonOK.Margin = new Thickness(0);
            }

            if (MessageBoxButton == MessageBoxButton.OK)
            {
                btn_PART_ButtonOK.IsCancel = true;

                btn_PART_ButtonYes.Visibility = Visibility.Collapsed;
                btn_PART_ButtonNo.Visibility = Visibility.Collapsed;
                btn_PART_ButtonCancel.Visibility = Visibility.Collapsed;
            }
            else if (MessageBoxButton == MessageBoxButton.OKCancel)
            {
                btn_PART_ButtonCancel.IsCancel = true;

                btn_PART_ButtonYes.Visibility = Visibility.Collapsed;
                btn_PART_ButtonNo.Visibility = Visibility.Collapsed;
            }
            else if (MessageBoxButton == MessageBoxButton.YesNo)
            {
                btn_PART_ButtonNo.IsCancel = true;

                btn_PART_ButtonOK.Visibility = Visibility.Collapsed;
                btn_PART_ButtonCancel.Visibility = Visibility.Collapsed;
            }
            else if (MessageBoxButton == MessageBoxButton.YesNoCancel)
            {
                btn_PART_ButtonCancel.IsCancel = true;

                btn_PART_ButtonOK.Visibility = Visibility.Collapsed;
            }

            //生成消息
            Grid mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            TextBlock textBlockMsg = new TextBlock()
            {
                Text = Message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(20, 5, 20, 5)
            };
            mainGrid.Children.Add(textBlockMsg);//添加消息到窗口
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });
            Grid gridButtonBar = new Grid();
            Grid.SetRow(gridButtonBar, 1);
            gridButtonBar.Children.Add(sp_PART_ButtonBarStackPanel);//添加按钮栏
            mainGrid.Children.Add(gridButtonBar);
            this.Content = mainGrid;

            if (Owner is WindowEx)
            {
                WindowEx windowEx = Owner as WindowEx;
                windowEx.ShowMaskGird = true;
            }
            else
            {
                Grid maskGrid = Owner.FindName("PART_MaskGrid") as Grid;
                if (maskGrid != null)
                {
                    maskGrid.Visibility = Visibility.Visible;
                }
            }
        }
        
        private void Btn_PART_ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            DialogResult = true;
        }

        private void Btn_PART_ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.No;
            DialogResult = true;
        }

        private void Btn_PART_ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            DialogResult = true;
        }

        private void Btn_PART_ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.OK;
            DialogResult = true;
        }



        /// <summary>
        /// 拖动对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PartMainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// 去掉蒙板
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            //蒙板
            if (Owner is WindowEx)
            {
                WindowEx windowEx = Owner as WindowEx;
                windowEx.ShowMaskGird = false;
            }
            else
            {
                Grid maskGrid = Owner.FindName("PART_MaskGrid") as Grid;
                if (maskGrid != null)
                {
                    maskGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 设置窗口高度及默认焦点按钮
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (this.Content == null)
            {
                return;
            }

            double titleHeight = titleBar.Height;

            Grid mainGrid = this.Content as Grid;
            Height = titleHeight + mainGrid.ActualHeight;

            if (MessageBoxResult == MessageBoxResult.None || MessageBoxResult == MessageBoxResult.OK)
            {
                btn_PART_ButtonOK.Focus();
            }
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                btn_PART_ButtonYes.Focus();
            }
            if (MessageBoxResult == MessageBoxResult.No)
            {
                btn_PART_ButtonNo.Focus();
            }
            if (MessageBoxResult == MessageBoxResult.Cancel)
            {
                btn_PART_ButtonCancel.Focus();
            }
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
        #endregion
    }
}
