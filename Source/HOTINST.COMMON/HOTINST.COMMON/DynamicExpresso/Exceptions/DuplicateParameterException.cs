using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class DuplicateParameterException : DynamicExpressoException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="identifier"></param>
		public DuplicateParameterException(string identifier)
			: base(string.Format("The parameter '{0}' was defined more than once", identifier)) 
		{
			Identifier = identifier;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DuplicateParameterException(
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
