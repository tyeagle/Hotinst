using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HOTINST.ICD.Codec.Contract;

namespace HOTINST.ICD.Codec.Implement
{
	/// <summary>
	/// 
	/// </summary>
	public class FrameCodec : IFrameCodec
	{
		#region 字段

	    private log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FrameCodec));
		private readonly IList<ISignalCodec> _signalObjects;
	    private int _codecId;

	    #endregion

		#region 属性

		public int CodecID { get; set; }

	    public string CodecName { get; set; }

        public ICDHead Header { get; set; }

		#endregion

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="header"></param>
		/// <param name="signsls"></param>
		public FrameCodec(ICDHead header, IList<ISignalCodec> signsls)
		{
			Debug.Assert(null != header && null != signsls);
			Header = header;
			_signalObjects = signsls;
		}

	    int IFrameCodec.CodecID
	    {
	        get { return _codecId; }
	        set { _codecId = value; }
	    }
        
	    public IList<ISignalCodec> GetAllSignals()
		{
			return _signalObjects;
		}

	    public ISignalCodec GetSignalCodec(int id)
	    {
	        return _signalObjects.FirstOrDefault(n => n.ID == id);
	    }

	    public ISignalCodec GetSignalCodec(string name)
	    {
	        return _signalObjects.FirstOrDefault(n => n.Name == name);
	    }
        public object GetValue(int signalid, byte[] buffer, UInt32 index = 0)
        {
            return _signalObjects[signalid].GetValue(buffer, index);
        }

	    public object GetValue(string signalName, byte[] buffer, uint index = 0)
	    {
	        var signalCodec = _signalObjects.FirstOrDefault(n => n.Name == signalName);
	        if (signalCodec != null)
	        {
	            return signalCodec.GetValue(buffer, index);
	        }
	        else
	        {
	            string exceptionMessage = $"帧[{this.CodecName}]没有信号[{signalName}]";

                logger.Warn(exceptionMessage);
                throw new ArgumentException(exceptionMessage);
	        }
	    }

	    public void SetValue(int signalid, byte[] buffer, object value, UInt32 index = 0)
        {
            _signalObjects[signalid].SetValue(buffer, value, index);
        }

	    public void SetValue(string signalName, byte[] buffer, object value, uint index = 0)
	    {
            var signalCodec = _signalObjects.FirstOrDefault(n => n.Name == signalName);
            if (signalCodec != null)
            {
                signalCodec.SetValue(buffer, value, (UInt32)index);
            }
            else
            {
                string exceptionMessage = $"帧[{this.CodecName}]没有信号[{signalName}]";

                logger.Warn(exceptionMessage);
                throw new ArgumentException(exceptionMessage);
            }
        }
	}
}