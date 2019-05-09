using System;
using System.Windows;

namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDialog
	{
		/// <summary>
		/// 
		/// </summary>
		DialogButtons Mode { get; }
		/// <summary>
		/// 
		/// </summary>
		DialogResultState Result { get; }
		/// <summary>
		/// 
		/// </summary>
		DialogCloseBehavior CloseBehavior { get; set; }

		/// <summary>
		/// 
		/// </summary>
		Action Ok { get; set; }
		/// <summary>
		/// 
		/// </summary>
		Action Cancel { get; set; }
		/// <summary>
		/// 
		/// </summary>
		Action Yes { get; set; }
		/// <summary>
		/// 
		/// </summary>
		Action No { get; set; }

		/// <summary>
		/// 
		/// </summary>
		bool CanOk { get; set; }
		/// <summary>
		/// 
		/// </summary>
		bool CanCancel { get; set; }
		/// <summary>
		/// 
		/// </summary>
		bool CanYes { get; set; }
		/// <summary>
		/// 
		/// </summary>
		bool CanNo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string OkText { get; set; }
		/// <summary>
		/// 
		/// </summary>
		string CancelText { get; set; }
		/// <summary>
		/// 
		/// </summary>
		string YesText { get; set; }
		/// <summary>
		/// 
		/// </summary>
		string NoText { get; set; }

		/// <summary>
		/// 
		/// </summary>
		string Caption { get; set; }
		/// <summary>
		/// 
		/// </summary>
		DialogIcon Icon { get; set; }

		/// <summary>
		/// 
		/// </summary>
		VerticalAlignment VerticalDialogAlignment { set; }
		/// <summary>
		/// 
		/// </summary>
		HorizontalAlignment HorizontalDialogAlignment { set; }

		/// <summary>
		/// 
		/// </summary>
		void Show();
		/// <summary>
		/// 
		/// </summary>
		void Close();

		/// <summary>
		/// 
		/// </summary>
		event EventHandler Closed;
	}
}