/**
 * ==============================================================================
 *
 * ClassName: DelegateCommand
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/1/23 10:55:42
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows.Input;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// 实现<see cref="ICommand"/>接口
	/// </summary>
	public class DelegateCommand : ICommand
	{
		#region fields

		#endregion

		#region porps

		/// <summary>
		/// 此命令的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 确定此命令是否可以执行的方法
		/// </summary>
		public Predicate<object> CanExecuteDelegate { get; set; }

		/// <summary>
		/// 此命令的执行方法
		/// </summary>
		public Action<object> ExecuteDelegate { get; set; }

		/// <summary>
		/// 回调
		/// </summary>
		public Action Callback { get; set; }

		#endregion

		#region .ctor

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		public DelegateCommand()
		{

		}

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		/// <param name="name">此命令的名称</param>
		public DelegateCommand(string name)
		{
			Name = name;
		}

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		/// <param name="execute">此命令的执行方法</param>
		public DelegateCommand(Action<object> execute)
		{
			ExecuteDelegate = execute;
		}

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		/// <param name="execute">此命令的执行方法</param>
		/// <param name="canExecute">确定此命令是否可以执行的方法</param>
		public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
			:this(execute)
		{
			CanExecuteDelegate = canExecute;
		}

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		/// <param name="name">此命令的名称</param>
		/// <param name="execute">此命令的执行方法</param>
		public DelegateCommand(string name, Action<object> execute)
			: this(name)
		{
			ExecuteDelegate = execute;
		}

		/// <summary>
		/// 初始化类<see cref="DelegateCommand"/>的新实例
		/// </summary>
		/// <param name="name">此命令的名称</param>
		/// <param name="execute">此命令的执行方法</param>
		/// <param name="canExecute">确定此命令是否可以执行的方法</param>
		public DelegateCommand(string name, Action<object> execute, Predicate<object> canExecute)
			: this(name, execute)
		{
			CanExecuteDelegate = canExecute;
		}

		#endregion

		#region Implementation of ICommand

		/// <summary>定义在调用此命令时调用的方法。</summary>
		/// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
		public void Execute(object parameter)
		{
			ExecuteDelegate?.Invoke(parameter);

			Callback?.Invoke();
		}

		/// <summary>定义用于确定此命令是否可以在其当前状态下执行的方法。</summary>
		/// <returns>如果可以执行此命令，则为 true；否则为 false。</returns>
		/// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null。</param>
		public bool CanExecute(object parameter)
		{
			return CanExecuteDelegate == null || CanExecuteDelegate(parameter);
		}

		/// <summary>
		/// 命令是否可执行的状态变化事件
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		#endregion
	}
}