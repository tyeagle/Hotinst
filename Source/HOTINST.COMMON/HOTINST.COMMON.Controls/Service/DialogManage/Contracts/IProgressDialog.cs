namespace HOTINST.COMMON.Controls.Service
{
	/// <summary>
	/// 
	/// </summary>
	public interface IProgressDialog : IWaitDialog
	{
		/// <summary>
		/// 
		/// </summary>
		int Progress { get; set; }
	}
}