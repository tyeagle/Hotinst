using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ParseException : DynamicExpressoException
	{
		const string PARSE_EXCEPTION_FORMAT = "{0} (at index {1}).";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="position"></param>
		public ParseException(string message, int position)
			: base(string.Format(PARSE_EXCEPTION_FORMAT, message, position)) 
		{
			Position = position;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ParseException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			Position = info.GetInt32("Position");
		}

		/// <summary>
		/// 
		/// </summary>
		public int Position { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Position", Position);

			base.GetObjectData(info, context);
		}
	}
}
