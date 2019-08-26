/**
 * ==============================================================================
 *
 * ClassName: DialogService
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/10/25 16:16:38
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using HOTINST.COMMON.Controls.Controls;
using HOTINST.COMMON.Controls.Helper;

namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// 
	/// </summary>
	public static class DialogService
	{
		private static readonly Dictionary<Type, Type> _dic;

		static DialogService()
		{
			_dic = new Dictionary<Type, Type>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="view"></param>
		public static void Register(Type viewModel, Type view)
		{
			_dic[viewModel] = view;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="viewModel"></param>
		/// <param name="showDialog"></param>
		public static void EditData(object viewModel, bool showDialog = true)
		{
			TrySetData(viewModel, null, false, true, showDialog);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="setAction"></param>
		/// <param name="showDialog"></param>
		/// <returns></returns>
		public static bool TryGetData<T>(Action<T> setAction, bool showDialog = true)
		{
			var viewModel = Activator.CreateInstance<T>();
			return TrySetData(viewModel, setAction, false, false, showDialog);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewModel"></param>
		/// <param name="setAction"></param>
		/// <param name="clone"></param>
		/// <param name="onlyClose"></param>
		/// <param name="showDialog"></param>
		/// <returns></returns>
		public static bool TrySetData<T>(T viewModel, Action<T> setAction, bool clone, bool onlyClose, bool showDialog = true)
		{
			if(!_dic.ContainsKey(viewModel.GetType()))
				throw new ArgumentException("ViewModel类型没有被注册");

			return TrySetData(_dic[viewModel.GetType()], viewModel, setAction, clone, onlyClose, showDialog);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewModel"></param>
		/// <param name="setAction"></param>
		/// <returns></returns>
		public static bool TrySetData<T>(T viewModel, Action<T> setAction)
		{
			if(!_dic.ContainsKey(viewModel.GetType()))
				throw new ArgumentException("ViewModel类型没有被注册");

			return TrySetData(_dic[viewModel.GetType()], viewModel, setAction, false, false, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dlgType"></param>
		/// <param name="viewModel"></param>
		/// <param name="setAction"></param>
		/// <param name="cloneData"></param>
		/// <param name="onlyClose"></param>
		/// <param name="showDialog"></param>
		/// <returns></returns>
		private static bool TrySetData<T>(Type dlgType, T viewModel, Action<T> setAction, bool cloneData, bool onlyClose, bool showDialog)
		{
			if(!typeof(System.Windows.UIElement).IsAssignableFrom(dlgType))
				throw new ArgumentException("dlgType必须是UIElement");
			if(cloneData && !(viewModel is ICloneable))
				throw new ArgumentException("viewModel必须继承ICloneable接口");

			T clone = cloneData ? (T)((ICloneable)viewModel).Clone() : viewModel;
			bool updated = false;
			var ui = (System.Windows.UIElement)Activator.CreateInstance(dlgType);
			var dlgChrome = new DialogChrome(ui, clone, onlyClose);
			dlgChrome.DataUpdated += (ss, ee) =>
			{
				updated = true;
				setAction?.Invoke(clone);
			};

			if(showDialog)
			{
				DialogHelper.ShowDialog(dlgChrome);
			}
			else
			{
				dlgChrome.ShowInTaskbar = true;
				DialogHelper.Show(dlgChrome);
			}

			return updated;
		}
	}
}