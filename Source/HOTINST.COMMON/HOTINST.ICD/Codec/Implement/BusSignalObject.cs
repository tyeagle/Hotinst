/**
 * ==============================================================================
 *
 * Filename: SignalObject.cs
 * Description: 
 *
 * Version: 1.0
 * Created: 2016/8/31 14:32:35
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Diagnostics;
using HOTINST.ICD.Codec.Contract;
using HOTINST.ICD.ValueConvert;

namespace HOTINST.ICD.Codec.Implement
{
    /// <summary>
    /// 表示一个信号对象。
    /// </summary>
    public class BusSignalObject : ISignalCodec
    {
        #region props

        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BusSignalObject));

        /// <summary>
        /// 信号ID
        /// </summary>
        public int ID { get; set; }

        public string Name { get; }

        /// <summary>
        /// 信号基础内容
        /// </summary>
        public ICDWord Word { get; set; }

        /// <summary>
        /// 内部类型
        /// </summary>
        internal IInnerType InnerObject { get; set; }

        /// <summary>
        /// 外部类型
        /// </summary>
        internal IOutterType OutterObject { get; set; }

        /// <summary>
        /// 帧ID
        /// </summary>
        public uint IcdID { get; set; }

        #endregion props

        #region Construct

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="icd">ICDWord对象</param>
        /// <param name="conv">值转换器</param>
        /// <param name="convback">反转换器</param>
        public BusSignalObject(ICDWord icd, ConvertFunction conv, ConvertBackFunction convback)
        {
            Word = icd;
            Name = icd.Name;
            InnerObject = InnerFactory.CreateInnerType(Word);
            OutterObject = OutterFactory.CreateOutter(InnerObject, (DataType) Word.Type, conv, convback);

            Debug.Assert(InnerObject != null && OutterObject != null);
            if (InnerObject == null || OutterObject == null)
            {
                var errMessage = $"信号[{icd.Name}]编解码器创建失败.";

                logger.Error(errMessage);
                throw new ArgumentException(errMessage);
            }
        }

        #endregion

        #region get & set

        public int Id
        {
            get { return ID; }
        }

        /// <summary>
        /// 获取信号值
        /// </summary>
        /// <returns>信号值</returns>
        public object GetValue(byte[] buffer, UInt32 index = 0)
        {
            object Value = OutterObject.GetValue(buffer, index);
#if DEBUG
            ulong rawValue = OutterObject.GetRawValue(buffer, index);

            logger.Debug(
                $"[{Value}, {rawValue.ToString("X8")}] = GetValue({Name}, {Word.Offset}, {Word.StartBit}, {Word.BitWidth}, {Word.Endian})");
#endif
            return Value;
        }

        /// <summary>
        /// 获取帧内存中的信号原始值
        /// </summary>
        /// <returns>信号值</returns>
        public uint GetRawValue(byte[] buffer, UInt32 index = 0)
        {
            object Value = OutterObject.GetRawValue(buffer, index);
            return Convert.ToUInt32(Value);
        }

        /// <summary>
        /// 设置信号值
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="value">要设置的值</param>
        /// <param name="index"></param>
        public void SetValue(byte[] buffer, object value, UInt32 index = 0)
        {
            OutterObject.SetValue(buffer, index, value);
#if DEBUG
            ulong rawValue = OutterObject.GetRawValue(buffer, index);
            logger.Debug(
                $"[{value}, {rawValue.ToString("X8")}]SetValue(Name:{Name}, {Word.Offset}, {Word.StartBit}, {Word.BitWidth}, {Word.Endian})");
#endif
        }

        #endregion get & set
    }
}