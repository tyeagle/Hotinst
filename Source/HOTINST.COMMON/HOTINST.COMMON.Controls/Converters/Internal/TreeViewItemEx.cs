/**
 * ==============================================================================
 *
 * ClassName: TreeViewItemEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/7 15:58:59
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Converters.Internal
{
	/// <summary>
	/// <see cref="System.Windows.Controls.TreeViewItem"/> 的扩展方法。
	/// </summary>
	public static class TreeViewItemEx
	{
		/// <summary>
		/// 返回指定 <see cref="System.Windows.Controls.TreeViewItem"/> 的深度。
		/// </summary>
		/// <param name="item">要获取深度的 <see cref="System.Windows.Controls.TreeViewItem"/> 对象。</param>
		/// <returns><see cref="System.Windows.Controls.TreeViewItem"/> 所在的深度。</returns>
		public static int GetDepth(this TreeViewItem item)
		{
			int depth = 0;
			while((item = item.GetAncestor<TreeViewItem>()) != null)
			{
				depth++;
			}
			return depth;
		}
	}

	/// <summary>
	/// WPF 可视化树的扩展方法。
	/// </summary>
	internal static class VisualTreeEx
	{
		/// <summary>
		/// 返回指定对象的特定类型的祖先。
		/// </summary>
		/// <typeparam name="T">要获取的祖先的类型。</typeparam>
		/// <param name="source">获取的祖先，如果不存在则为 <c>null</c>。</param>
		/// <returns>获取的祖先对象。</returns>
		public static T GetAncestor<T>(this DependencyObject source) where T : DependencyObject
		{
			do
			{
				source = VisualTreeHelper.GetParent(source);
			}
			while(source != null && !(source is T));

			return source as T;
		}
	}
}