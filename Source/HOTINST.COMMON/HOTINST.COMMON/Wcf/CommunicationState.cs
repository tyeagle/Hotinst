namespace HOTINST.COMMON.Wcf
{
    /// <summary>
    /// 通讯状态
    /// </summary>
    public enum CommunicationState
    {
        /// <summary>
        /// 无状态（通常因为从未执行过连接操作导致）
        /// </summary>
        None = -1,
        /// <summary>
        /// 指示通信对象已实例化且可配置，但尚未打开或无法使用。
        /// </summary>
        Created = 0,
        /// <summary>
        /// 指示通信对象正从 System.ServiceModel.CommunicationState.Created 状态转换到 System.ServiceModel.CommunicationState.Opened状态。
        /// </summary>
        Opening = 1,
        /// <summary>
        /// 指示通信对象目前已打开，且随时可供使用。
        /// </summary>
        Opened = 2,
        /// <summary>
        /// 指示通信对象正转换到 System.ServiceModel.CommunicationState.Closed 状态。
        /// </summary>
        Closing = 3,
        /// <summary>
        /// 指示通信对象已关闭，且不再可用。
        /// </summary>
        Closed = 4,
        /// <summary>
        /// 指示通信对象发生错误，无法恢复且不再可用。
        /// </summary>
        Faulted = 5,
    }
}
