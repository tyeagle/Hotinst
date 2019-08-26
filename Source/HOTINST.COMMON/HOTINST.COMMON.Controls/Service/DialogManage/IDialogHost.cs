using System.Windows;

namespace HOTINST.COMMON.Controls.Service
{
	internal interface IDialogHost
	{
		void ShowDialog(DialogBaseControl dialog);
		void HideDialog(DialogBaseControl dialog);
		FrameworkElement GetCurrentContent();
	}
}