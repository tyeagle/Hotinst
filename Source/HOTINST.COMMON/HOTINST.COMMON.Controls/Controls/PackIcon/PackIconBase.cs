/**
 * ==============================================================================
 *
 * ClassName: PackIconBase
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/8/27 14:25:17
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
	/// <summary>
	/// PackIconBase
	/// </summary>
	public abstract class PackIconBase : Control
	{
		internal abstract void UpdateData();
	}

	/// <summary>
	/// Base class for creating an icon control for icon packs.
	/// </summary>
	/// <typeparam name="TKind"></typeparam>
	public abstract class PackIconBase<TKind> : PackIconBase
	{
		private static Lazy<IDictionary<TKind, string>> _dataIndex;

		/// <param name="dataIndexFactory">
		/// Inheritors should provide a factory for setting up the path data index (per icon kind).
		/// The factory will only be utilised once, across all closed instances (first instantiation wins).
		/// </param>
		protected PackIconBase(Func<IDictionary<TKind, string>> dataIndexFactory)
		{
			if(dataIndexFactory == null) throw new ArgumentNullException(nameof(dataIndexFactory));

			if(_dataIndex == null)
				_dataIndex = new Lazy<IDictionary<TKind, string>>(dataIndexFactory);
		}

		/// <summary>
		/// KindProperty
		/// </summary>
		public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
			nameof(Kind), typeof(TKind), typeof(PackIconBase<TKind>), new PropertyMetadata(default(TKind), KindPropertyChangedCallback));

		private static void KindPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((PackIconBase)dependencyObject).UpdateData();
		}

		/// <summary>
		/// Gets or sets the icon to display.
		/// </summary>
		public TKind Kind
		{
			get => (TKind)GetValue(KindProperty);
			set => SetValue(KindProperty, value);
		}
		
		private static readonly DependencyPropertyKey DataPropertyKey
			= DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(PackIconBase<TKind>), new PropertyMetadata(""));

		/// ReSharper disable once StaticMemberInGenericType
		public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

		/// <summary>
		/// Gets the icon path data for the current <see cref="Kind"/>.
		/// </summary>
		[TypeConverter(typeof(GeometryConverter))]
		public string Data
		{
			get => (string)GetValue(DataProperty);
			private set => SetValue(DataPropertyKey, value);
		}
		
		/// <summary>
		/// 
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			UpdateData();
		}

		/// <summary>
		/// 
		/// </summary>
		internal override void UpdateData()
		{
			string data = null;
			_dataIndex.Value?.TryGetValue(Kind, out data);
			Data = data;
		}
	}
}