namespace HOTINST.COMMON.ServiceProcess
{
	/// <summary>
	/// 表示服务状态
	/// </summary>
	public enum ServiceState
	{
		/// <summary>
		/// 正在运行
		/// </summary>
		Running,
		/// <summary>
		/// 已停止
		/// </summary>
		Stopped,
		/// <summary>
		/// 已暂停
		/// </summary>
		Paused,
		/// <summary>
		/// 已发生错误
		/// </summary>
		Error
	}
}