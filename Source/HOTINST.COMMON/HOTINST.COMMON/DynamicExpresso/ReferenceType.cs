﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HOTINST.COMMON.DynamicExpresso.Reflection;

namespace HOTINST.COMMON.DynamicExpresso
{
	/// <summary>
	/// 
	/// </summary>
	public class ReferenceType
	{
		/// <summary>
		/// 
		/// </summary>
		public Type Type { get; private set; }

		/// <summary>
		/// Public name that must be used in the expression.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public IList<MethodInfo> ExtensionMethods { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		public ReferenceType(string name, Type type)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if (type == null)
				throw new ArgumentNullException("type");

			Type = type;
			Name = name;
			ExtensionMethods = ReflectionExtensions.GetExtensionMethods(type).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public ReferenceType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			Type = type;
			Name = type.Name;
			ExtensionMethods = ReflectionExtensions.GetExtensionMethods(type).ToList();
		}
	}
}
