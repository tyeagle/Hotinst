/**
 * ==============================================================================
 *
 * ClassName: BitmapImageHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/19 16:38:54
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// BitmapImageHelper
	/// </summary>
	public static class BitmapImageHelper
	{
		/// <summary>
		/// 使用绝对路径创建Iamge
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ImageSource GetImgAbsolute(string uri)
		{
			return new BitmapImage(new Uri(uri, UriKind.Absolute));
		}

		/// <summary>
		/// 使用相对路径创建Iamge
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ImageSource GetImgRelative(string uri)
		{
			return new BitmapImage(new Uri(uri, UriKind.Relative));
		}

		/// <summary>
		/// 使用相对或绝对路径创建Iamge
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ImageSource GetImgRelativeOrAbsolute(string uri)
		{
			return new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
		}
	}
}