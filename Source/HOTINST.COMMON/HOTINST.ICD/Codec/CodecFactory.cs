using System;
using System.Collections.Generic;
using HOTINST.ICD.Codec.Contract;
using HOTINST.ICD.Codec.Implement;
using HOTINST.ICD.ValueConvert;

namespace HOTINST.ICD.Codec
{
	/// <summary>
	/// ICD编解码器管理器。
	/// </summary>
    public sealed class CodecFactory : ICodecFactory
    {
        #region field

        private readonly string _version = "Ver1.0";

		private readonly Converter _converter = new Converter();
		
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public CodecFactory()
        {
	        
        }

	
		/// <summary>
		/// 获取总线的编解码器。
		/// </summary>
		/// <returns></returns>
        public IBusCodec CreateCodec(IList<ICDWords> wordsList)
        {
            IList<IFrameCodec> frames = new List<IFrameCodec>(wordsList.Count);
            int codecId = 0;
            foreach (var words in wordsList)
            {
                frames.Add(CreateBusFrame(codecId++, words));
            }
			IBusCodec codec = new BusCodec(CreateCodecMap(frames))
			{
				Version = _version
			};
			return codec;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="busConfigs"></param>
		/// <param name="frameCodecs"></param>
		/// <returns></returns>
        public static Dictionary<string, IFrameCodec> CreateCodecMap( IList<IFrameCodec> frameCodecs)
        {
            Dictionary<string, IFrameCodec> result = new Dictionary<string, IFrameCodec>();

            foreach (var frameCodec in frameCodecs)
            {
                result.Add(frameCodec.CodecName, frameCodec);
            }
            return result;
        }
        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codecId">ICD的Id</param>
        /// <param name="words">icd中的信号定义</param>
        /// <returns></returns>
        public FrameCodec CreateBusFrame(int codecId, ICDWords words)
        {
            IList<ISignalCodec> signalObjects = new List<ISignalCodec>(words.ICDFrame.Count);

	        ushort sigId = 0;
            foreach (var word in words.ICDFrame)
            {
				signalObjects.Add(CreateBusSignalObject(word, sigId++));
            }

            FrameCodec frame = new FrameCodec(words.ICDHead, signalObjects)
	        {
		        CodecID = codecId,
                CodecName = words.Name
	        };

	        return frame;
        }
        
		private BusSignalObject CreateBusSignalObject(ICDWord word, ushort sigId)
        {
            ConvertFunction convertPlug = null;
            ConvertBackFunction convBackPlug = null;
            
            if (word.IsCallFunction)
            {
	            convertPlug = _converter.GetConvert(word.FunctionName, word.Param1, word.Param2,
		            word.Param3, word.Param4);

	            convBackPlug = _converter.GetConvertBack(word.FunctionName, word.Param1, word.Param2,
		            word.Param3, word.Param4);

	            if(null == convertPlug || null == convBackPlug)
		            throw new Exception("设置失败：该信号配置了插件，但没有获取到插件信息。" + Environment.NewLine + "信号：" +
		                                word.Name);
            }

			return new BusSignalObject(word, convertPlug, convBackPlug) { ID = sigId };
        }
        #endregion
    }
}