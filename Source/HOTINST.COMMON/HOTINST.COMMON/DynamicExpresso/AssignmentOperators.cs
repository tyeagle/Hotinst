﻿using System;

namespace HOTINST.COMMON.DynamicExpresso
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum AssignmentOperators
	{
		/// <summary>
		/// Disable all the assignment operators
		/// </summary>
		None = 0,
		/// <summary>
		/// Enable the assignment equal operator
		/// </summary>
		AssignmentEqual = 1,
		/// <summary>
		/// Enable all assignment operators
		/// </summary>
		All = AssignmentEqual
	}
}
