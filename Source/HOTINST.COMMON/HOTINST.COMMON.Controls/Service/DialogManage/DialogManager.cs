using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// 
	/// </summary>
	public class DialogManager : IDialogManager
	{
		/// <summary>
		/// DialogManager
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="dispatcher"></param>
		public DialogManager(
			ContentControl parent,
			Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			_dialogHost = new DialogLayeringHelper(parent);
		}

		private readonly Dispatcher _dispatcher;
		private readonly IDialogHost _dialogHost;

		#region Implementation of IDialogManager

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IMessageDialog CreateMessageDialog(string message, DialogButtons dialogMode, DialogIcon icon)
		{
			IMessageDialog dialog = null;
			InvokeInUIThread(() => dialog = new MessageDialog(_dialogHost, dialogMode, icon, message, _dispatcher));
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="caption"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IMessageDialog CreateMessageDialog(string message, string caption, DialogButtons dialogMode, DialogIcon icon)
		{
			IMessageDialog dialog = null;
			InvokeInUIThread(() => dialog = new MessageDialog(_dialogHost, dialogMode, icon, message, _dispatcher)
			{
				Caption = caption
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IProgressDialog CreateProgressDialog(DialogButtons dialogMode, DialogIcon icon)
		{
			IProgressDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = true;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IProgressDialog CreateProgressDialog(string message, DialogButtons dialogMode, DialogIcon icon)
		{
			IProgressDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = true;
				dialog.Message = message;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="readyMessage"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IProgressDialog CreateProgressDialog(string message, string readyMessage, DialogButtons dialogMode, DialogIcon icon)
		{
			IProgressDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateProgressDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = false;
				dialog.ReadyMessage = readyMessage;
				dialog.Message = message;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IWaitDialog CreateWaitDialog(DialogButtons dialogMode, DialogIcon icon)
		{
			IWaitDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = true;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IWaitDialog CreateWaitDialog(string message, DialogButtons dialogMode, DialogIcon icon)
		{
			IWaitDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = true;
				dialog.Message = message;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="readyMessage"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public IWaitDialog CreateWaitDialog(string message, string readyMessage, DialogButtons dialogMode, DialogIcon icon)
		{
			IWaitDialog dialog = null;
			InvokeInUIThread(() =>
			{
				dialog = WaitProgressDialog.CreateWaitDialog(_dialogHost, dialogMode, icon, _dispatcher);
				dialog.CloseWhenWorkerFinished = false;
				dialog.Message = message;
				dialog.ReadyMessage = readyMessage;
			});
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public ICustomContentDialog CreateCustomContentDialog(object content, DialogButtons dialogMode, DialogIcon icon)
		{
			ICustomContentDialog dialog = null;
			InvokeInUIThread(() => dialog = new CustomContentDialog(_dialogHost, dialogMode, content, _dispatcher));
			return dialog;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="caption"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		public ICustomContentDialog CreateCustomContentDialog(object content, string caption, DialogButtons dialogMode, DialogIcon icon)
		{
			ICustomContentDialog dialog = null;
			InvokeInUIThread(() => dialog = new CustomContentDialog(_dialogHost, dialogMode, content, _dispatcher)
			{
				Caption = caption
			});
			return dialog;
		}

		#endregion

		private void InvokeInUIThread(Action del)
		{
			_dispatcher.Invoke(del);
		}
	}
}