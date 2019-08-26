/**
 * ==============================================================================
 *
 * ClassName: ObservableExtension
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/6/13 12:43:41
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 扩展 <see cref="ObservableCollection{T}"/> , 增加排序方法
	/// </summary>
	public static class ObservableExtension
	{
		/// <summary>
		/// 排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source"></param>
		/// <param name="keySelector"></param>
		public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
		{
			List<TSource> sortedList = source.OrderBy(keySelector).ToList();

			for(int i = 0; i < sortedList.Count; i++)
			{
				int oldId = source.IndexOf(sortedList[i]);
				if(oldId != i)
				{
					source.Move(source.IndexOf(sortedList[i]), i);
				}
			}
		}
	}
}