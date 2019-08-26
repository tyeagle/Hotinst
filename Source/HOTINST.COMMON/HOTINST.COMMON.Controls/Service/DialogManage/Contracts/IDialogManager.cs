namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// IDialogManager
	/// </summary>
	public interface IDialogManager
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IMessageDialog CreateMessageDialog(string message, DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="caption"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IMessageDialog CreateMessageDialog(string message, string caption, DialogButtons dialogMode, DialogIcon icon);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		ICustomContentDialog CreateCustomContentDialog(object content, DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="content"></param>
		/// <param name="caption"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		ICustomContentDialog CreateCustomContentDialog(object content, string caption, DialogButtons dialogMode, DialogIcon icon);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IProgressDialog CreateProgressDialog(DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IProgressDialog CreateProgressDialog(string message, DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="readyMessage"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IProgressDialog CreateProgressDialog(string message, string readyMessage, DialogButtons dialogMode, DialogIcon icon);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IWaitDialog CreateWaitDialog(DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IWaitDialog CreateWaitDialog(string message, DialogButtons dialogMode, DialogIcon icon);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="readyMessage"></param>
		/// <param name="dialogMode"></param>
		/// <param name="icon"></param>
		/// <returns></returns>
		IWaitDialog CreateWaitDialog(string message, string readyMessage, DialogButtons dialogMode, DialogIcon icon);
	}
}