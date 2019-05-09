using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Service
{
	internal class MessageDialog : DialogBase, IMessageDialog
	{
		public MessageDialog(
			IDialogHost dialogHost, 
			DialogButtons dialogMode,
			DialogIcon icon,
			string message,
			Dispatcher dispatcher)
			: base(dialogHost, dialogMode, icon, dispatcher)
		{
			InvokeUICall(() =>
				{
					_messageTextBlock = new TextBlock
					{
						Text = message,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						TextWrapping = TextWrapping.Wrap,
						Focusable = false
					};
					SetContent(_messageTextBlock);
				});
		}

		private TextBlock _messageTextBlock;

		#region Implementation of IMessageDialog

		public string Message
		{
			get
			{
				var text = string.Empty;
				InvokeUICall(
					() => text = _messageTextBlock.Text);
				return text;
			}
			set
			{
				InvokeUICall(
					() => _messageTextBlock.Text = value);
			}
		}

		#endregion
	}
}