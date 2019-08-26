namespace HOTINST.COMMON.Wcf
{
	/// <summary>
	/// WCF通讯绑定方式
	/// </summary>
	public enum BindingType
	{
		/// <summary>
		/// TCP
		/// </summary>
		NetTcpBinding,
		/// <summary>
		/// 命名管道
		/// </summary>
		NetNamedPipeBinding
	}
}