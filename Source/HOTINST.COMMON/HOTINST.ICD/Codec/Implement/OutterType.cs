using System;
using HOTINST.ICD.ValueConvert;

namespace HOTINST.ICD.Codec.Implement
{
	/// <summary>
	/// 定义外部类型接口
	/// </summary>
	internal interface IOutterType
	{
		/// <summary>
		/// 获取值
		/// </summary>
		/// <param name="buffer">帧内存</param>
		/// <param name="offset">偏移</param>
		/// <returns></returns>
        object GetValue(byte[] buffer, uint offset);

		/// <summary>
		/// 设置值
		/// </summary>
		/// <param name="buffer">帧内存</param>
		/// <param name="offset">偏移</param>
		/// <param name="outValue">要设置的值</param>
        void SetValue(byte[] buffer, uint offset, object outValue);

        UInt32 GetRawValue(byte[] buffer, uint offset);

    }

	internal class OutterTypeBase
	{
		public IInnerType InnerType { get; set; }
		public DataType OutterType { get; set; }
		public ConvertFunction Converter { get; set; }
		public ConvertBackFunction BackConverter { get; set; }

		protected OutterTypeBase(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
		{
			InnerType = innerObj;
			OutterType = outType;
			Converter = converter;
			BackConverter = convback;
		}
        public UInt32 GetRawValue(byte[]buffer, uint offset)
        {
            return this.InnerType.GetRawValue(buffer, offset);
        }
	}

	internal class OutterTypeBool : OutterTypeBase, IOutterType
	{
		public OutterTypeBool(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			bool ret = 0 != innerValue;
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
        {
            uint innerValue = 0u;
            switch (outValue.ToString().ToLower())
            {
                case "true":
                case "1":
                    innerValue = 1u;
                    break;
                case "false":
                case "0":
                    innerValue = 0u;
                    break;
                default:
                    break;
            }
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

    internal class OutterTypeInt8:OutterTypeBase, IOutterType
    {
        public OutterTypeInt8(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback) 
            : base(innerObj, outType, converter, convback)
        {
        }

        public object GetValue(byte[] buffer, uint offset)
        {
            uint innerValue = InnerType.GetValue(buffer, offset);
            sbyte ret = (sbyte)innerValue;

            if (Converter != null)
            {
                ret = (sbyte)Converter(innerValue);
            }
            return ret;
        }

        public void SetValue(byte[] buffer, uint offset, object outValue)
        {
            uint innerValue = Convert.ToUInt32(outValue);
            InnerType.SetValue(buffer, offset, innerValue);
        }
    }

	internal class OutterTypeUint8 : OutterTypeBase, IOutterType
	{
		public OutterTypeUint8(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			byte ret = (byte)innerValue;

			if (Converter != null)
			{
				ret = (byte)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt32(outValue);
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

	internal class OutterTypeInt16 : OutterTypeBase, IOutterType
	{
		public OutterTypeInt16(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			short ret = (short)innerValue;
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
        {
	        uint innerValue = Convert.ToUInt16(Convert.ToInt16(outValue));
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

	internal class OutterTypeUint16 : OutterTypeBase, IOutterType
	{
		public OutterTypeUint16(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			ushort ret = (ushort)innerValue;
			if (Converter != null)
			{
				ret = (ushort)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt16(outValue);
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

	internal class OutterTypeInt32 : OutterTypeBase, IOutterType
	{
		public OutterTypeInt32(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			int ret = (int)innerValue;
			if (Converter != null)
			{
				ret = (int)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt32(Convert.ToInt32(outValue));
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

	internal class OutterTypeUint32 : OutterTypeBase, IOutterType
	{
		public OutterTypeUint32(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			uint ret = innerValue;
			if (Converter != null)
			{
				ret = (uint)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt32(outValue);
			InnerType.SetValue(buffer, offset, innerValue);
		}

	}

	internal class OutterTypeInt64 : OutterTypeBase, IOutterType
	{
		public OutterTypeInt64(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);

			long ret = innerValue;

			if (Converter != null)
			{
				ret = (long)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt32(Convert.ToInt64(outValue));
			InnerType.SetValue(buffer, offset, innerValue);
		}

	}

	internal class OutterTypeUint64 : OutterTypeBase, IOutterType
	{
		public OutterTypeUint64(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			ulong ret = innerValue;
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
		{
			uint innerValue = Convert.ToUInt32(Convert.ToUInt64(outValue));
			InnerType.SetValue(buffer, offset, innerValue);
		}
	}

	internal class OutterTypeDouble : OutterTypeBase, IOutterType
	{
		public OutterTypeDouble(IInnerType innerObj, DataType outType, ConvertFunction converter, ConvertBackFunction convback)
			: base(innerObj, outType, converter, convback)
		{

		}

        public object GetValue(byte[] buffer, uint offset)
		{
			uint innerValue = InnerType.GetValue(buffer, offset);
			double ret = innerValue;

			if(Converter != null)
			{
				ret = (double)Converter(innerValue);
			}
			return ret;
		}

        public void SetValue(byte[] buffer, uint offset, object outValue)
        {
            if (BackConverter != null)
            {
                UInt32 uintValue = BackConverter(HOTINST.COMMON.Data.Converter.ToDouble(outValue));

                InnerType.SetValue(buffer, offset, uintValue);
            }
            else
            {
                double dbValue = Convert.ToDouble(outValue);

                uint innerValue = Convert.ToUInt32(dbValue);

                InnerType.SetValue(buffer, offset, innerValue);
            }
		}
	}

	internal class OutterFactory
	{
	    private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(OutterFactory));
		public static IOutterType CreateOutter(IInnerType innerObj, DataType outterType, ConvertFunction converter, ConvertBackFunction convback)
		{
			IOutterType ret = null;
			switch(outterType)
            {
				case DataType.Boolean:
					ret = new OutterTypeBool(innerObj, outterType, converter, convback);
					break;
                case DataType.Int8:
                    ret = new OutterTypeInt8(innerObj, outterType, converter, convback);
                    break;
				case DataType.UInt8:
					ret = new OutterTypeUint8(innerObj, outterType, converter, convback);
					break;
				case DataType.UInt16:
					ret = new OutterTypeUint16(innerObj, outterType, converter, convback);
					break;
				case DataType.UInt32:
					ret = new OutterTypeUint32(innerObj, outterType, converter, convback);
					break;
				case DataType.UInt64:
					ret = new OutterTypeUint64(innerObj, outterType, converter, convback);
					break;
				case DataType.Int16:
					ret = new OutterTypeInt16(innerObj, outterType, converter, convback);
					break;
				case DataType.Int32:
					ret = new OutterTypeInt32(innerObj, outterType, converter, convback);
					break;
				case DataType.Int64:
					ret = new OutterTypeInt64(innerObj, outterType, converter, convback);
                    break;
                case DataType.InValid://....
				case DataType.Float:
				case DataType.Double:
					ret = new OutterTypeDouble(innerObj, outterType, converter, convback);
					break;
				case DataType.Frame:
					break;
                default:
                    var msg = $"不支持的信号外部类型[{outterType}]";
                    logger.Error(msg);
                    break;
			}

			return ret;
		}
	}
}