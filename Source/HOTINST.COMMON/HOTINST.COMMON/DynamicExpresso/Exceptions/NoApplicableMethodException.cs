using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class NoApplicableMethodException : ParseException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="methodTypeName"></param>
		/// <param name="position"></param>
		public NoApplicableMethodException(string methodName, string methodTypeName, int position)
			: base(string.Format("No applicable method '{0}' exists in type '{1}'", methodName, methodTypeName), position) 
		{
			MethodTypeName = methodTypeName;
			MethodName = methodName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected NoApplicableMethodException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			MethodTypeName = info.GetString("MethodTypeName");
			MethodName = info.GetString("MethodName");
		}

		/// <summary>
		/// 
		/// </summary>
		public string MethodTypeName { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public string MethodName { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("MethodName", MethodName);
			info.AddValue("MethodTypeName", MethodTypeName);

			base.GetObjectData(info, context);
		}
	}
}
