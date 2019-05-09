using System;
using System.Linq.Expressions;
using System.Reflection;
using HOTINST.COMMON.DynamicExpresso.Exceptions;

namespace HOTINST.COMMON.DynamicExpresso.Visitors
{
	/// <summary>
	/// 
	/// </summary>
	public class DisableReflectionVisitor : ExpressionVisitor
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Object != null
				&& (typeof(Type).IsAssignableFrom(node.Object.Type)
				|| typeof(MemberInfo).IsAssignableFrom(node.Object.Type)))
			{
				throw new ReflectionNotAllowedException();
			}

			return base.VisitMethodCall(node);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		protected override Expression VisitMember(MemberExpression node)
		{
			if ((typeof(Type).IsAssignableFrom(node.Member.DeclaringType)
				|| typeof(MemberInfo).IsAssignableFrom(node.Member.DeclaringType))
				&& node.Member.Name != "Name")
			{
				throw new ReflectionNotAllowedException();
			}

			return base.VisitMember(node);
		}
	}
}
