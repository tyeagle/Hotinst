/**
 * ==============================================================================
 *
 * ClassName: ThemeHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/11/17 10:28:30
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// 应用程序主题帮助类
	/// </summary>
	public static class ThemeHelper
	{
		#region 主题Uri

		/// <summary>
		/// 获取 Win7 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Aero => new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/aero.normalcolor.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 Win8 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_AeroLite => new Uri("/PresentationFramework.AeroLite, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/aerolite.normalcolor.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 WinXP(亮蓝) 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Royale => new Uri("/PresentationFramework.Royale, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/royale.normalcolor.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 WinXP(蓝色) 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Luna_Blue => new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.normalcolor.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 WinXP(银色) 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Luna_Silver => new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.metallic.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 WinXP(橄榄色) 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Luna_Olive => new Uri("/PresentationFramework.Luna, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/luna.homestead.xaml", UriKind.Relative);

		/// <summary>
		/// 获取 Win98 主题的Uri
		/// </summary>
		/// <returns></returns>
		public static Uri Theme_Classic => new Uri("/PresentationFramework.Classic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/classic.xaml", UriKind.Relative);

		#endregion

		#region 扩展方法

		/// <summary>
		/// 加载主题资源到资源字典
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ResourceDictionary Load(this Uri uri)
		{
			return Application.LoadComponent(uri) as ResourceDictionary;
		}

		#endregion
	}
}