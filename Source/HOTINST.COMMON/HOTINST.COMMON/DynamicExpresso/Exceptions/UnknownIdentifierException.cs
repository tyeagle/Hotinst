using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class UnknownIdentifierException : ParseException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="position"></param>
		public UnknownIdentifierException(string identifier, int position)
			: base(string.Format("Unknown identifier '{0}'", identifier), position) 
		{
			Identifier = identifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected UnknownIdentifierException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			Identifier = info.GetString("Identifier");
		}

		/// <summary>
		/// 
		/// </summary>
		public string Identifier { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Identifier", Identifier);

			base.GetObjectData(info, context);
		}
	}
}
