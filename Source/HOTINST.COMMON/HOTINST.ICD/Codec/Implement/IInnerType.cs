using HOTINST.COMMON.Bitwise;

namespace HOTINST.ICD.Codec.Implement
{
	/// <summary>
	/// 定义内部类型枚举
	/// </summary>
	internal enum InnerType
	{
		/// <summary>
		/// 8位
		/// </summary>
		InnerBit8,
		/// <summary>
		/// 16位
		/// </summary>
		InnerBit16,
		/// <summary>
		/// 32位
		/// </summary>
		InnerBit32,
		/// <summary>
		/// 64位
		/// </summary>
		InnerBit64,
        /// <summary>
        /// Byte[]
        /// </summary>
        InnerArray
	}

	/// <summary>
	/// 定义内部类型接口
	/// </summary>
	internal interface IInnerType
	{
		#region Property

		/// <summary>
		/// ICDWord对象
		/// </summary>
		ICDWord ICD { get; set; }
		/// <summary>
		/// 字节偏移
		/// </summary>
		int ByteOffset { get; set; }
		/// <summary>
		/// 位偏移
		/// </summary>
		int BitOffset { get; set; }
		/// <summary>
		/// 位宽
		/// </summary>
		int BitWidth { get; set; }
		/// <summary>
		/// 字节序
		/// </summary>
		Endian EndianType { get; set; }

		#endregion

		/// <summary>
		/// 获取值
		/// </summary>
		/// <param name="buffer">帧内存</param>
		/// <param name="offset">字节偏移</param>
		/// <returns></returns>
        uint GetValue(byte[] buffer, uint offset);

		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="buffer">帧内存</param>
		/// <param name="offset">字节偏移</param>
		/// <param name="val">要设置的值</param>
        void SetValue(byte[] buffer, uint offset, uint val);

	    uint GetRawValue(byte[] buffer, uint offset);

    }

	abstract class InnerTypeBase
	{
		#region Property

		public ICDWord ICD { get; set; }
		public int ByteOffset { get; set; }
		public int BitOffset { get; set; }
		public int BitWidth { get; set; }
		public Endian EndianType { get; set; } = Endian.BigEndian;       

		#endregion

		protected InnerTypeBase(ICDWord icd)
		{
			ICD = icd;
            ByteOffset = (int)icd.Offset;
            BitOffset = (int)icd.StartBit;
            BitWidth = (int)icd.BitWidth;
            EndianType = (Endian)icd.Endian;
		}        
	}

	internal class InnerElement8 : InnerTypeBase, IInnerType
	{
        #region operation

        public uint GetRawValue(byte[] buffer, uint offset)
        {
            byte value;
            ByteArrayHelper.GetValue(buffer, offset + (uint)ByteOffset, 0, 8, out value);
            return value;
        }
        public uint GetValue(byte[] buffer, uint offset)
		{
			byte btValue = 0;
			ByteArrayHelper.GetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, out btValue);

			return btValue;
		}

		public void SetValue(byte[] buffer, uint offset, uint val)
		{
			byte btValue = (byte)val;
			ByteArrayHelper.SetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, btValue);
		}

		#endregion

		public InnerElement8(ICDWord icd)
			: base(icd)
		{

		}
	}

	internal class InnerElement16 : InnerTypeBase, IInnerType
	{
        #region operation
        public uint GetRawValue(byte[] buffer, uint offset)
        {
            ushort value;
            ByteArrayHelper.GetValue(buffer, offset + (uint)ByteOffset, 0, 16, out value, EndianType);
            return value;
        }
        public uint GetValue(byte[] buffer, uint offset)
		{
			ushort usValue = 0;
			ByteArrayHelper.GetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, out usValue, EndianType);

			return usValue;
		}

        public void SetValue(byte[] buffer, uint offset, uint val)
		{
			ushort btValue = (ushort)val;
			ByteArrayHelper.SetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, btValue, EndianType);
		}

		#endregion

		public InnerElement16(ICDWord icd)
			: base(icd)
		{

		}
	}

	internal class InnerElement32 : InnerTypeBase, IInnerType
	{
        #region operation
        public uint GetRawValue(byte[] buffer, uint offset)
        {
            uint value;
            ByteArrayHelper.GetValue(buffer, offset + (uint)ByteOffset, 0, 32, out value, EndianType);
            return value;
        }
        public uint GetValue(byte[] buffer, uint offset)
		{
			uint uiValue = 0;
			ByteArrayHelper.GetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, out uiValue, EndianType);

			return uiValue;
		}

        public void SetValue(byte[] buffer, uint offset, uint val)
		{
			uint btValue = val;
			ByteArrayHelper.SetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, btValue, EndianType);
		}

		#endregion

		public InnerElement32(ICDWord icd)
			: base(icd)
		{           
		}
	}

	internal class InnerElement64 : InnerTypeBase, IInnerType
	{
        #region operation
        public uint GetRawValue(byte[] buffer, uint offset)
        {
            ulong value;
            ByteArrayHelper.GetValue(buffer, offset + (uint)ByteOffset, 0, 64, out value, EndianType);
            return (uint)value;
        }
        public uint GetValue(byte[] buffer, uint offset)
		{
			ulong ulValue = 0;
			ByteArrayHelper.GetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, out ulValue, EndianType);

			return (uint)ulValue;
		}

        public void SetValue(byte[] buffer, uint offset, uint val)
		{
			ulong btValue = val;
			ByteArrayHelper.SetValue(buffer, offset + ICD.Offset, ICD.StartBit, ICD.BitWidth, btValue, EndianType);
		}

		#endregion

		public InnerElement64(ICDWord icd)
			: base(icd)
		{

		}
	}
	
	internal class InnerFactory
	{
		public static IInnerType CreateInnerType(ICDWord icd)
		{
            InnerType innerType = DataTypeToInnerType(icd.InnerType);
            IInnerType innerObj = null;

            switch (innerType)
			{
				case InnerType.InnerBit8: innerObj = new InnerElement8(icd);
					break;
				case InnerType.InnerBit16: innerObj = new InnerElement16(icd);
					break;
				case InnerType.InnerBit32: innerObj = new InnerElement32(icd);
					break;
				case InnerType.InnerBit64: innerObj = new InnerElement64(icd);
					break;
			}

            innerObj.EndianType = (Endian)icd.Endian;
			return innerObj;
		}

        private static InnerType DataTypeToInnerType(DataType dataType)
        {
            InnerType innerType = InnerType.InnerBit32;
            switch(dataType)
            {
                case DataType.Boolean:
                case DataType.UInt8:
                    innerType = InnerType.InnerBit8;
                    break;
                case DataType.Int16:
                case DataType.UInt16:
                    innerType = InnerType.InnerBit16; break;

                case DataType.Float:
                case DataType.Int32:
                case DataType.UInt32:
                    innerType = InnerType.InnerBit32;break;

                case DataType.Int64:
                case DataType.UInt64:
                case DataType.Double:
                    innerType = InnerType.InnerBit64;break;

            }

            return innerType;
        }
    }
}