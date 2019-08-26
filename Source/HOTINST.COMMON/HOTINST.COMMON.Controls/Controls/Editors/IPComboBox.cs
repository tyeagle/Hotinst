/**
 * ==============================================================================
 * Classname   : IPComboBox
 * Description : 
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/3/14 14:47:08
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Controls;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// IP编辑下拉框
	/// </summary>
	public class IPComboBox : ComboBox
	{
		#region .ctor

		static IPComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(IPComboBox),
				new FrameworkPropertyMetadata(typeof(IPComboBox)));
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public IPComboBox()
		{
			Text = "___.___.___.___";
		}

		#endregion
	}
}