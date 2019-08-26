using System;

namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// 
	/// </summary>
	public interface IWaitDialog : IMessageDialog
	{
		/// <summary>
		/// 
		/// </summary>
		Action WorkerReady { get; set; }

		/// <summary>
		/// 
		/// </summary>
		bool CloseWhenWorkerFinished { get; set; }
		/// <summary>
		/// 
		/// </summary>
		string ReadyMessage { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workerMethod"></param>
		void Show(Action workerMethod);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="uiWorker"></param>
		void InvokeUICall(Action uiWorker);
	}
}