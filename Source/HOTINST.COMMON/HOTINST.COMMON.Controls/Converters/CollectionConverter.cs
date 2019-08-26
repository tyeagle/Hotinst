/**
 * ==============================================================================
 *
 * ClassName: CollectionConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/18 12:52:20
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 判断集合是否为空或不包含任何元素
	/// </summary>
	public class CollectionNullOrEmtpyConverter : IValueConverter
	{
		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IList list = value as IList;
			return list == null || list.Count == 0;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		#endregion
	}
}