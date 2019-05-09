using System;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class DynamicExpressoException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public DynamicExpressoException() { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public DynamicExpressoException(string message) : base(message) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public DynamicExpressoException(string message, Exception inner) : base(message, inner) { }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DynamicExpressoException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
