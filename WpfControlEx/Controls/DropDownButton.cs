using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfControlEx.Controls
{
    public class DropDownButton : ToggleButton
    {
        #region DropDownContextMenu

        /// <summary>
        /// DropDownContextMenu Dependency Property
        /// </summary>
        public static readonly DependencyProperty DropDownContextMenuProperty =
            DependencyProperty.Register("DropDownContextMenu", typeof(ContextMenu), typeof(DropDownButton),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DropDownContextMenu property.  This dependency property 
        /// indicates drop down menu to show up when user click on an anchorable menu pin.
        /// </summary>
        public ContextMenu DropDownContextMenu
        {
            get { return (ContextMenu)GetValue(DropDownContextMenuProperty); }
            set { SetValue(DropDownContextMenuProperty, value); }
        }

        #endregion



        #region 函数

        public DropDownButton()
        {
            this.Unloaded += new RoutedEventHandler(DropDownButton_Unloaded);
        }

        void DropDownButton_Unloaded(object sender, RoutedEventArgs e)
        {
            // When changing theme, Unloaded event is called, erasing the DropDownContextMenu.
            // Prevent this on theme changes.
            if (this.IsLoaded)
            {
                DropDownContextMenu = null;
            }
        }

        void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            //Debug.Assert(IsChecked.GetValueOrDefault());
            var ctxMenu = sender as ContextMenu;
            ctxMenu.Closed -= new RoutedEventHandler(OnContextMenuClosed);
            IsChecked = false;
        }



        #region override

        protected override void OnClick()
        {
            if (DropDownContextMenu != null)
            {
                //IsChecked = true;
                DropDownContextMenu.PlacementTarget = this;
                DropDownContextMenu.Placement = PlacementMode.Bottom;
                DropDownContextMenu.Closed += new RoutedEventHandler(OnContextMenuClosed);
                DropDownContextMenu.IsOpen = true;
            }

            base.OnClick();
        }

        #endregion

        #endregion
    }
}
