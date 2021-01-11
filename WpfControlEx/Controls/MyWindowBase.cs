using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlEx.Controls
{
    public class MyWindowBase : Window
    {
        #region 属性

        #region IsAppActive

        public static readonly DependencyProperty IsAppActiveProperty
            = DependencyProperty.Register("IsAppActive", typeof(bool), typeof(MyWindowBase),
                new PropertyMetadata(true));

        /// <summary>
        /// 进程是否系统当前激活进程
        /// </summary>
        public bool IsAppActive
        {
            get
            {
                return (bool)GetValue(IsAppActiveProperty);
            }
            set
            {
                SetValue(IsAppActiveProperty, value);
            }
        }

        #endregion

        #region MinimizeButtonTips

        /// <summary>
        /// 最小化
        /// </summary>
        public static readonly DependencyProperty MinimizeButtonTipsProperty
            = DependencyProperty.Register("MinimizeButtonTips", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Minimize"));

        /// <summary>
        /// 最小化
        /// </summary>
        public string MinimizeButtonTips
        {
            get
            {
                return (string)GetValue(MinimizeButtonTipsProperty);
            }
            set
            {
                SetValue(MinimizeButtonTipsProperty, value);
            }
        }

        #endregion

        #region MaximizeButtonTips

        /// <summary>
        /// 最大化
        /// </summary>
        public static readonly DependencyProperty MaximizeButtonTipsProperty
            = DependencyProperty.Register("MaximizeButtonTips", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Maximize"));

        /// <summary>
        /// 最大化
        /// </summary>
        public string MaximizeButtonTips
        {
            get
            {
                return (string)GetValue(MaximizeButtonTipsProperty);
            }
            set
            {
                SetValue(MaximizeButtonTipsProperty, value);
            }
        }

        #endregion

        #region RestoreButtonTips

        /// <summary>
        /// 向下还原
        /// </summary>
        public static readonly DependencyProperty RestoreButtonTipsProperty
            = DependencyProperty.Register("RestoreButtonTips", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Restore"));

        /// <summary>
        /// 向下还原
        /// </summary>
        public string RestoreButtonTips
        {
            get
            {
                return (string)GetValue(RestoreButtonTipsProperty);
            }
            set
            {
                SetValue(RestoreButtonTipsProperty, value);
            }
        }

        #endregion

        #region CloseButtonTips

        /// <summary>
        /// 关闭
        /// </summary>
        public static readonly DependencyProperty CloseButtonTipsProperty
            = DependencyProperty.Register("CloseButtonTips", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Close"));

        /// <summary>
        /// 关闭
        /// </summary>
        public string CloseButtonTips
        {
            get
            {
                return (string)GetValue(CloseButtonTipsProperty);
            }
            set
            {
                SetValue(CloseButtonTipsProperty, value);
            }
        }

        #endregion

        #region RestoreMenuContent

        /// <summary>
        /// 还原(R)
        /// </summary>
        public static readonly DependencyProperty RestoreMenuContentProperty
            = DependencyProperty.Register("RestoreMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Restore(&R)"));

        /// <summary>
        /// 还原(R)
        /// </summary>
        public string RestoreMenuContent
        {
            get
            {
                return (string)GetValue(RestoreMenuContentProperty);
            }
            set
            {
                SetValue(RestoreMenuContentProperty, value);
            }
        }

        #endregion

        #region MoveMenuContent

        /// <summary>
        /// 移动(M)
        /// </summary>
        public static readonly DependencyProperty MoveMenuContentProperty
            = DependencyProperty.Register("MoveMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Move(&M)"));

        /// <summary>
        /// 移动(M)
        /// </summary>
        public string MoveMenuContent
        {
            get
            {
                return (string)GetValue(MoveMenuContentProperty);
            }
            set
            {
                SetValue(MoveMenuContentProperty, value);
            }
        }

        #endregion

        #region SizeMenuContent

        /// <summary>
        /// 大小(S)
        /// </summary>
        public static readonly DependencyProperty SizeMenuContentProperty
            = DependencyProperty.Register("SizeMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Size(&S)"));

        /// <summary>
        /// 大小(S)
        /// </summary>
        public string SizeMenuContent
        {
            get
            {
                return (string)GetValue(SizeMenuContentProperty);
            }
            set
            {
                SetValue(SizeMenuContentProperty, value);
            }
        }

        #endregion

        #region MinimizeMenuContent

        /// <summary>
        /// 最小化(N)
        /// </summary>
        public static readonly DependencyProperty MinimizeMenuContentProperty
            = DependencyProperty.Register("MinimizeMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Minimize(&N)"));

        /// <summary>
        /// 最小化(N)
        /// </summary>
        public string MinimizeMenuContent
        {
            get
            {
                return (string)GetValue(MinimizeMenuContentProperty);
            }
            set
            {
                SetValue(MinimizeMenuContentProperty, value);
            }
        }

        #endregion

        #region MaximizeMenuContent

        /// <summary>
        /// 最大化(X)
        /// </summary>
        public static readonly DependencyProperty MaximizeMenuContentProperty
            = DependencyProperty.Register("MaximizeMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Maximize(&X)"));

        /// <summary>
        /// 最大化(X)
        /// </summary>
        public string MaximizeMenuContent
        {
            get
            {
                return (string)GetValue(MaximizeMenuContentProperty);
            }
            set
            {
                SetValue(MaximizeMenuContentProperty, value);
            }
        }

        #endregion

        #region CloseMenuContent

        /// <summary>
        /// 关闭(C)
        /// </summary>
        public static readonly DependencyProperty CloseMenuContentProperty
            = DependencyProperty.Register("CloseMenuContent", typeof(string), typeof(MyWindowBase),
                new PropertyMetadata("Close(&C)"));

        /// <summary>
        /// 关闭(C)
        /// </summary>
        public string CloseMenuContent
        {
            get
            {
                return (string)GetValue(CloseMenuContentProperty);
            }
            set
            {
                SetValue(CloseMenuContentProperty, value);
            }
        }

        #endregion

        #endregion



        #region 函数


        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            foreach (var w in Application.Current.Windows)
            {
                if (w is MyWindowBase)
                {
                    MyWindowBase win = w as MyWindowBase;
                    if (!win.IsAppActive)
                        win.IsAppActive = true;
                }
            }
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    bool hasActiveWindow = false;
                    foreach (var w in Application.Current.Windows)
                    {
                        if (w is MyWindowBase)
                        {
                            MyWindowBase win = w as MyWindowBase;
                            if (win.IsActive)
                            {
                                hasActiveWindow = true;
                                break;
                            }
                        }
                    }

                    if (hasActiveWindow)
                    {
                        foreach (var w in Application.Current.Windows)
                        {
                            if (w is MyWindowBase)
                            {
                                MyWindowBase win = w as MyWindowBase;
                                if (!win.IsAppActive)
                                    win.IsAppActive = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var w in Application.Current.Windows)
                        {
                            if (w is MyWindowBase)
                            {
                                MyWindowBase win = w as MyWindowBase;
                                if (win.IsAppActive)
                                    win.IsAppActive = false;
                            }
                        }
                    }
                }));
        }

        #endregion
    }
}
