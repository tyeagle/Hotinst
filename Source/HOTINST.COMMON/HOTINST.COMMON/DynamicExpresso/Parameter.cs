using System;
using System.Linq.Expressions;

namespace HOTINST.COMMON.DynamicExpresso
{
	/// <summary>
	/// An expression parameter. This class is thread safe.
	/// </summary>
	public class Parameter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public Parameter(string name, object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			Name = name;
			Type = value.GetType();
			Value = value;

			Expression = System.Linq.Expressions.Expression.Parameter(Type, name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public Parameter(string name, Type type, object value = null)
		{
			Name = name;
			Type = type;
			Value = value;

			Expression = System.Linq.Expressions.Expression.Parameter(type, name);
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public Type Type { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public object Value { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ParameterExpression Expression { get; private set; }
	}
}
