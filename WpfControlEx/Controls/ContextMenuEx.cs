using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfControlEx.Controls
{
    public class ContextMenuEx : ContextMenu
    {
        /// <summary>
        /// 设置MenuItemEx的自定义属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="item"></param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            MenuItemEx menuItemEx = element as MenuItemEx;
            Themes.ThemeModel themeModel = item as Themes.ThemeModel;

            base.PrepareContainerForItemOverride(element, item);
            menuItemEx.ThemeColorPreview = themeModel.ThemeColorPreview;
        }

        /// <summary>
        /// 使用自定义的MenuItemEx
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            //return base.GetContainerForItemOverride()
            return new MenuItemEx();
        }
    }
}
