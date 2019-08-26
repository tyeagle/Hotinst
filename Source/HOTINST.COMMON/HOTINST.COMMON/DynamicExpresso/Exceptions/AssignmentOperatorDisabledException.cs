using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace HOTINST.COMMON.DynamicExpresso.Exceptions
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class AssignmentOperatorDisabledException : ParseException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="operatorString"></param>
		/// <param name="position"></param>
		public AssignmentOperatorDisabledException(string operatorString, int position)
			: base(string.Format("Assignment operator '{0}' not allowed", operatorString), position) 
		{
			OperatorString = operatorString;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AssignmentOperatorDisabledException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			OperatorString = info.GetString("OperatorString");
		}

		/// <summary>
		/// 
		/// </summary>
		public string OperatorString { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("OperatorString", OperatorString);

			base.GetObjectData(info, context);
		}
	}
}
