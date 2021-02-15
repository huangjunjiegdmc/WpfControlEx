using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfControlEx.Controls
{
    /// <summary>
    /// 视图模型命令类
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region 变量、属性

        public Func<Object, Boolean> CanExecuteCommand;
        public Action<Object> ExecuteCommand;
        /// <summary>
        /// 命令是否可执行，表示是否有该命令的权限
        /// </summary>
        public bool IsCommandEnable { get; set; }

        /// <summary>
        /// 命令ID，与后台维护的权限ID关联，用于权限控制，是否有权限由属性IsCmdEnable控制，需要控制权限时才赋值
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// 父窗口权限ID
        /// </summary>
        public string ParentCommandId { get; set; }

        /// <summary>
        /// 权限树级别
        /// </summary>
        public int CommandLevel { get; set; }

        /// <summary>
        /// 命令功能描述
        /// </summary>
        public string CommandDescription { get; set; }
        /// <summary>
        /// 命令路径
        /// </summary>
        public string CommandPath { get; set; }
        #endregion

        #region 构造器
        public RelayCommand()
        {
            IsCommandEnable = true;//默认设置为可执行
        }

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
            IsCommandEnable = true;//默认设置为可执行
        }

        public RelayCommand(Action<Task> execute) : this(execute, null)
        {
            IsCommandEnable = true;//默认设置为可执行
        }

        public RelayCommand(Action<object> execute, Func<Object, Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            IsCommandEnable = true;//默认设置为可执行

            ExecuteCommand = execute;
            CanExecuteCommand = canExecute;
        }

        public RelayCommand(Action<Task> execute, Func<Object, Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            IsCommandEnable = true;//默认设置为可执行

            CanExecuteCommand = canExecute;
        }
        #endregion

        #region ICommand 成员函数

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (CanExecuteCommand != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (CanExecuteCommand != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return CanExecuteCommand == null ? true : CanExecuteCommand(parameter);
        }

        public void Execute(Object parameter)
        {
            if (IsCommandEnable)
                ExecuteCommand?.Invoke(parameter);
        }

        #endregion
    }
}
