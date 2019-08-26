/**
 * ==============================================================================
 *
 * Filename: CCodec.cs
 * Description: ICD服务对象，从ICD文件服务端获取并解析ICD文件，并封装编解码库方法
 *				供外部调用。
 * Version: 1.0
 * Created: 2016/6/2 10:05:02
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Collections.Generic;
using System.Linq;
using HOTINST.ICD.Codec.Contract;

#pragma warning disable 1591

namespace HOTINST.ICD.Codec.Implement
{
	/// <summary>
	/// 表示ICD编解码器对象
	/// </summary>
	public class BusCodec : IBusCodec
	{
		#region props & fields

		private readonly Dictionary<string, IFrameCodec> _frameCodecs;
        
		#endregion props & fields
        public  string Version { get; set; } 
	    
		#region 构造

        public BusCodec(Dictionary<string, IFrameCodec> frames)
	    {
	        _frameCodecs = frames;
	    }

		#endregion

		public IList<IFrameCodec> GetAllFrames()
        {
            return _frameCodecs.Values.ToList();
        }

        public IFrameCodec GetFrameCodec(string frameName)
        {
            return _frameCodecs[frameName];
        }

        public ISignalCodec GetSignalCodec(string frameName, string signalName)
        {
            return _frameCodecs[frameName].GetSignalCodec(signalName);
        }

        public IFrameCodec this[string id] => _frameCodecs[id];

    }
}