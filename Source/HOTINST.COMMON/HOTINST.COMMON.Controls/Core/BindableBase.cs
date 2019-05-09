/**
 * ==============================================================================
 *
 * ClassName: BindableBase
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/4/3 11:34:35
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.ComponentModel;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// 可绑定基类
	/// </summary>
	public class BindableBase : INotifyPropertyChanged
	{
		/// <summary>
		/// 属性变化事件
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// 属性变化
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}