using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Service
{
	internal class CustomContentDialog : DialogBase, ICustomContentDialog
	{
		public CustomContentDialog(
			IDialogHost dialogHost, 
			DialogButtons dialogMode,
			object content,
			Dispatcher dispatcher)
			: base(dialogHost, dialogMode, DialogIcon.None, dispatcher)
		{
			SetContent(content);
		}
	}
}