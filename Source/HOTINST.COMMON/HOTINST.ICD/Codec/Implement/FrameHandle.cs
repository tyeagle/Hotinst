namespace HOTINST.ICD.Codec.Implement
{
	/// <summary>
	/// 帧的唯一句柄
	/// </summary>
    public  class FrameHandle
    {
		#region 构造

		/// <summary>
		/// 根据总线，设备，帧的ID构造一个帧句柄对象。
		/// </summary>
		/// <param name="busId">总线ID</param>
		/// <param name="devId">设备ID</param>
		/// <param name="frmId">帧ID</param>
		public FrameHandle(ushort busId, ushort devId, ushort frmId)
		{
			UniqueHandle = ((ulong)((long)busId << 48) & 0xFFFF000000000000) | ((ulong)((long)devId << 32) & 0xFFFF00000000) | ((ulong)((long)frmId << 16) & 0xFFFF0000);
		}

        public FrameHandle(int seed)
        {
            UniqueHandle = (ulong)seed;
        }
		/// <summary>
		/// 根据帧句柄值构造一个帧句柄对象。
		/// </summary>
		/// <param name="handle"></param>
		public FrameHandle(ulong handle)
	    {
	        UniqueHandle = handle;
	    }

		#endregion

		#region 属性

	    public ulong UniqueHandle { get; }

	    /// <summary>
		/// 64-49位存储总线ID
		/// </summary>
		public ushort BusId => (ushort)(UniqueHandle >> 48);
		/// <summary>
	    /// 48-33位存储总线ID
	    /// </summary>
	    public ushort DevId => (ushort)((UniqueHandle & 0xFFFF00000000) >> 32);
		/// <summary>
		/// 32-17位存储总线ID
		/// </summary>
		public ushort FrmId => (ushort)((UniqueHandle & 0xFFFF0000) >> 16);

		#endregion

		#region 方法

		/// <summary>
		/// 创建一个对象
		/// </summary>
		/// <param name="busId">总线ID</param>
		/// <param name="devId">设备ID</param>
		/// <param name="frmId">帧ID</param>
		/// <returns></returns>
		public static FrameHandle CreateHandle(ushort busId, ushort devId, ushort frmId)
		{
			return new FrameHandle(busId, devId, frmId);
		}

	    public static FrameHandle CreateHandle(ulong handle)
	    {
		    return new FrameHandle(handle);
	    }

       public static ushort BusIdFromeHandle(ulong UniqueHandle)
        {
            return (ushort)(UniqueHandle >> 48);
        }

        public static ushort DeviceIdFromeHandle(ulong UniqueHandle)
        {
            return (ushort)((UniqueHandle & 0xFFFF00000000) >> 32);
        }

        public static ushort FrameIdFromeHandle(ulong UniqueHandle)
        {
            return (ushort)((UniqueHandle & 0xFFFF0000) >> 16);
        }

        
        #endregion

        #region 运算符重载

        /// <summary>
        /// 小于操作符
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(FrameHandle lhs, FrameHandle rhs)
        {
            return lhs.UniqueHandle < rhs.UniqueHandle;
        }
        /// <summary>
        /// 大于操作符
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(FrameHandle lhs, FrameHandle rhs)
        {
            return lhs.UniqueHandle > rhs.UniqueHandle;
        }
        /// <summary>
        /// ==操作符
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(FrameHandle lhs, FrameHandle rhs)
        {
            return lhs?.UniqueHandle == rhs?.UniqueHandle;
        }
        /// <summary>
        /// 不等操作符
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(FrameHandle lhs, FrameHandle rhs)
        {
            return lhs?.UniqueHandle != rhs?.UniqueHandle;
        }
        /// <summary>
        /// 等于判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            FrameHandle rec = (FrameHandle)obj;

            return UniqueHandle == rec.UniqueHandle;
        }
        /// <summary>
        /// 哈希函数
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return UniqueHandle.GetHashCode();
        }

        #endregion
    }

	/// <summary>
	/// 信号的唯一句柄
	/// </summary>
    public class SignalHandle
    {
	    #region 属性

		public ulong UniqueHandle { get; }

		public FrameHandle FrameHandle { get; }

		public ushort BusId { get; }
		public ushort DevId { get; }
		public ushort FrmId { get; }
		public ushort SignalId { get; }

	    #endregion

		#region .ctor

		public SignalHandle(ulong frameHandle, ushort signalId)
		{
			FrameHandle = FrameHandle.CreateHandle(frameHandle);
			BusId = FrameHandle.BusId;
			DevId = FrameHandle.DevId;
			FrmId = FrameHandle.FrmId;
			SignalId = signalId;

			UniqueHandle = frameHandle | signalId;
		}

        #endregion

        #region 运算符重载

        public static bool operator <(SignalHandle lhs, SignalHandle rhs)
        {
            return lhs.UniqueHandle < rhs.UniqueHandle;
        }

        public static bool operator >(SignalHandle lhs, SignalHandle rhs)
        {
            return lhs.UniqueHandle > rhs.UniqueHandle;
        }

        public static bool operator ==(SignalHandle lhs, SignalHandle rhs)
        {
	        return Equals(lhs, rhs);
        }

        public static bool operator !=(SignalHandle lhs, SignalHandle rhs)
        {
            if (lhs == null && rhs == null)
                return false;
            if (lhs != null && rhs != null)
                return lhs.UniqueHandle != rhs.UniqueHandle;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            return UniqueHandle == ((SignalHandle)obj).UniqueHandle;
        }

        public override int GetHashCode()
        {
            return UniqueHandle.GetHashCode();
        }

		#endregion

		#region 方法

		public static SignalHandle CreateHandle(ushort busId, ushort devId, ushort frameId, ushort signalId)
        {
	        ulong frameHandle = FrameHandle.CreateHandle(busId, devId, frameId).UniqueHandle;

            return CreateHandle(frameHandle, signalId);
        }

        public static SignalHandle CreateHandle(ulong frameHandle, ushort signalId)
        {
	        return new SignalHandle(frameHandle, signalId);
		}

        public static SignalHandle CreateHandle(FrameHandle frameHandle, ushort signalId)
        {
	        return new SignalHandle(frameHandle.UniqueHandle, signalId);
        }

		#endregion
	}
}