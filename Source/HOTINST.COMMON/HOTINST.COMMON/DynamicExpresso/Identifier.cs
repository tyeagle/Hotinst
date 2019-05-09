using System;
using System.Linq.Expressions;

namespace HOTINST.COMMON.DynamicExpresso
{
	/// <summary>
	/// 
	/// </summary>
	public class Identifier
	{
		/// <summary>
		/// 
		/// </summary>
		public Expression Expression { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="expression"></param>
		public Identifier(string name, Expression expression)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if (expression == null)
				throw new ArgumentNullException("expression");

			Expression = expression;
			Name = name;
		}
	}
}
