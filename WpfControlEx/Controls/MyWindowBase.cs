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
