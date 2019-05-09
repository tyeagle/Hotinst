﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using HOTINST.COMMON.DynamicExpresso.Exceptions;
using Microsoft.CSharp.RuntimeBinder;

// Code based on the Dynamic.cs file of the DynamicQuery sample by Microsoft
// http://msdn.microsoft.com/en-us/vstudio/bb894665.aspx
// http://weblogs.asp.net/scottgu/archive/2008/01/07/dynamic-linq-part-1-using-the-linq-dynamic-query-library.aspx
//
// Copyright (C) Microsoft Corporation.  All rights reserved.

namespace HOTINST.COMMON.DynamicExpresso.Parsing
{
	internal class Parser
	{
		public static Expression Parse(ParserArguments arguments)
		{
			return new Parser(arguments).Parse();
		}

		const NumberStyles ParseLiteralNumberStyle = NumberStyles.AllowLeadingSign;
		const NumberStyles ParseLiteralUnsignedNumberStyle = NumberStyles.AllowLeadingSign;
		const NumberStyles ParseLiteralDecimalNumberStyle = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
		static CultureInfo ParseCulture = CultureInfo.InvariantCulture;

		ParserArguments _arguments;

		// Working context implementation
		//ParameterExpression it;

		int _parsePosition;
		string _expressionText;
		int _expressionTextLength;
		char _parseChar;
		Token _token;

		BindingFlags _bindingCase;
		MemberFilter _memberFilterCase;

		Parser(ParserArguments arguments)
		{
			_arguments = arguments;

			_bindingCase = arguments.Settings.CaseInsensitive ? BindingFlags.IgnoreCase : BindingFlags.Default;
			_memberFilterCase = arguments.Settings.CaseInsensitive ? Type.FilterNameIgnoreCase : Type.FilterName;

			_expressionText = arguments.ExpressionText ?? string.Empty;
			_expressionTextLength = _expressionText.Length;
			SetTextPos(0);
			NextToken();
		}

		Expression Parse()
		{
			Expression expr = ParseExpressionSegment(_arguments.ExpressionReturnType);

			ValidateToken(TokenId.End, ErrorMessages.SyntaxError);
			return expr;
		}

		Expression ParseExpressionSegment(Type returnType)
		{
			int errorPos = _token.pos;
			var expression = ParseExpressionSegment();

			if (returnType != typeof(void))
			{
				return GenerateConversion(expression, returnType, errorPos);
			}

			return expression;
		}

		Expression ParseExpressionSegment()
		{
			// The following methods respect the operator precedence as defined in
			// MSDN C# "Operator precedence and associativity"
			// http://msdn.microsoft.com/en-us/library/aa691323(v=vs.71).aspx

			return ParseAssignement();
		}

		// = operator
		Expression ParseAssignement()
		{
			var left = ParseConditional();
			if (_token.id == TokenId.Equal)
			{
				if (!_arguments.Settings.AssignmentOperators.HasFlag(AssignmentOperators.AssignmentEqual))
					throw new AssignmentOperatorDisabledException("=", _token.pos);

				var op = _token;
				NextToken();
				var right = ParseAssignement();
				CheckAndPromoteOperands(typeof(ParseSignatures.IEqualitySignatures), op.text, ref left, ref right, op.pos);
				left = Expression.Assign(left, right);
			}
			return left;
		}

		// ?: operator
		Expression ParseConditional()
		{
			var errorPos = _token.pos;
			var expr = ParseLogicalOr();
			if (_token.id == TokenId.Question)
			{
				NextToken();
				var expr1 = ParseExpressionSegment();
				ValidateToken(TokenId.Colon, ErrorMessages.ColonExpected);
				NextToken();
				var expr2 = ParseExpressionSegment();
				expr = GenerateConditional(expr, expr1, expr2, errorPos);
			}
			return expr;
		}

		// || operator
		Expression ParseLogicalOr()
		{
			var left = ParseLogicalAnd();
			while (_token.id == TokenId.DoubleBar)
			{
				var op = _token;
				NextToken();
				var right = ParseLogicalAnd();
				CheckAndPromoteOperands(typeof(ParseSignatures.ILogicalSignatures), op.text, ref left, ref right, op.pos);
				left = Expression.OrElse(left, right);
			}
			return left;
		}

		// && operator
		Expression ParseLogicalAnd()
		{
			var left = ParseComparison();
			while (_token.id == TokenId.DoubleAmphersand)
			{
				var op = _token;
				NextToken();
				var right = ParseComparison();
				CheckAndPromoteOperands(typeof(ParseSignatures.ILogicalSignatures), op.text, ref left, ref right, op.pos);
				left = Expression.AndAlso(left, right);
			}
			return left;
		}

		// ==, !=, >, >=, <, <= operators
		Expression ParseComparison()
		{
			var left = ParseTypeTesting();
			while (_token.id == TokenId.DoubleEqual || _token.id == TokenId.ExclamationEqual ||
					_token.id == TokenId.GreaterThan || _token.id == TokenId.GreaterThanEqual ||
					_token.id == TokenId.LessThan || _token.id == TokenId.LessThanEqual)
			{
				var op = _token;
				NextToken();
				var right = ParseAdditive();
				var isEquality = op.id == TokenId.DoubleEqual || op.id == TokenId.ExclamationEqual;

				//if (isEquality && !left.Type.IsValueType && !right.Type.IsValueType)
				//{
				//	if (left.Type != right.Type)
				//	{
				//		if (left.Type.IsAssignableFrom(right.Type))
				//		{
				//			right = Expression.Convert(right, left.Type);
				//		}
				//		else if (right.Type.IsAssignableFrom(left.Type))
				//		{
				//			left = Expression.Convert(left, right.Type);
				//		}
				//		else
				//		{
				//			throw CreateParseException(op.pos, ErrorMessages.IncompatibleOperands,
				//				op.text, GetTypeName(left.Type), GetTypeName(right.Type));
				//		}
				//	}
				//}
				//else if (IsEnumType(left.Type) || IsEnumType(right.Type))
				//{
				//	if (left.Type != right.Type)
				//	{
				//		Expression e;
				//		if ((e = PromoteExpression(right, left.Type, true)) != null)
				//		{
				//			right = e;
				//		}
				//		else if ((e = PromoteExpression(left, right.Type, true)) != null)
				//		{
				//			left = e;
				//		}
				//		else
				//		{
				//			throw CreateParseException(op.pos, ErrorMessages.IncompatibleOperands,
				//				op.text, GetTypeName(left.Type), GetTypeName(right.Type));
				//		}
				//	}
				//}
				//else
				//{
				//	CheckAndPromoteOperands(isEquality ? typeof(ParseSignatures.IEqualitySignatures) : typeof(ParseSignatures.IRelationalSignatures),
				//			op.text, ref left, ref right, op.pos);
				//}

				CheckAndPromoteOperands(isEquality ? typeof(ParseSignatures.IEqualitySignatures) : typeof(ParseSignatures.IRelationalSignatures),
					op.text, ref left, ref right, op.pos);

				switch (op.id)
				{
					case TokenId.DoubleEqual:
						left = GenerateEqual(left, right);
						break;
					case TokenId.ExclamationEqual:
						left = GenerateNotEqual(left, right);
						break;
					case TokenId.GreaterThan:
						left = GenerateGreaterThan(left, right);
						break;
					case TokenId.GreaterThanEqual:
						left = GenerateGreaterThanEqual(left, right);
						break;
					case TokenId.LessThan:
						left = GenerateLessThan(left, right);
						break;
					case TokenId.LessThanEqual:
						left = GenerateLessThanEqual(left, right);
						break;
				}
			}
			return left;
		}

		// is, as operators
		Expression ParseTypeTesting()
		{
			var left = ParseAdditive();
			while (_token.text == ParserConstants.KEYWORD_IS
					|| _token.text == ParserConstants.KEYWORD_AS)
			{
				var typeOperator = _token.text;

				var op = _token;
				NextToken();

				Type knownType;
				if (!_arguments.TryGetKnownType(_token.text, out knownType))
					throw CreateParseException(op.pos, ErrorMessages.TypeIdentifierExpected);

				if (typeOperator == ParserConstants.KEYWORD_IS)
					left = Expression.TypeIs(left, knownType);
				else if (typeOperator == ParserConstants.KEYWORD_AS)
					left = Expression.TypeAs(left, knownType);
				else
					throw CreateParseException(_token.pos, ErrorMessages.SyntaxError);

				NextToken();
			}

			return left;
		}

		// +, -, & operators
		Expression ParseAdditive()
		{
			var left = ParseMultiplicative();
			while (_token.id == TokenId.Plus || _token.id == TokenId.Minus)
			{
				var op = _token;
				NextToken();
				var right = ParseMultiplicative();
				switch (op.id)
				{
					case TokenId.Plus:
						if (left.Type == typeof(string) || right.Type == typeof(string))
						{
							left = GenerateStringConcat(left, right);
						}
						else
						{
							CheckAndPromoteOperands(typeof(ParseSignatures.IAddSignatures), op.text, ref left, ref right, op.pos);
							left = GenerateAdd(left, right);
						}
						break;
					case TokenId.Minus:
						CheckAndPromoteOperands(typeof(ParseSignatures.ISubtractSignatures), op.text, ref left, ref right, op.pos);
						left = GenerateSubtract(left, right);
						break;
				}
			}
			return left;
		}

		// *, /, % operators
		Expression ParseMultiplicative()
		{
			var left = ParseUnary();
			while (_token.id == TokenId.Asterisk || _token.id == TokenId.Slash ||
					_token.id == TokenId.Percent)
			{
				var op = _token;
				NextToken();
				var right = ParseUnary();

				CheckAndPromoteOperands(typeof(ParseSignatures.IArithmeticSignatures), op.text, ref left, ref right, op.pos);

				switch (op.id)
				{
					case TokenId.Asterisk:
						left = Expression.Multiply(left, right);
						break;
					case TokenId.Slash:
						left = Expression.Divide(left, right);
						break;
					case TokenId.Percent:
						left = Expression.Modulo(left, right);
						break;
				}
			}
			return left;
		}

		// +,-, ! unary operators
		Expression ParseUnary()
		{
			if (_token.id == TokenId.Minus || _token.id == TokenId.Exclamation || _token.id == TokenId.Plus)
			{
				var op = _token;
				NextToken();
				if (_token.id == TokenId.IntegerLiteral ||
						_token.id == TokenId.RealLiteral)
				{
					if (op.id == TokenId.Minus)
					{
						_token.text = "-" + _token.text;
						_token.pos = op.pos;
						return ParsePrimary();
					}

					if (op.id == TokenId.Plus)
					{
						_token.text = "+" + _token.text;
						_token.pos = op.pos;
						return ParsePrimary();
					}
				}
				var expr = ParseUnary();
				if (op.id == TokenId.Minus)
				{
					CheckAndPromoteOperand(typeof(ParseSignatures.INegationSignatures), op.text, ref expr, op.pos);
					expr = Expression.Negate(expr);
				}
				else if (op.id == TokenId.Plus)
				{

				}
				else if (op.id == TokenId.Exclamation)
				{
					CheckAndPromoteOperand(typeof(ParseSignatures.INotSignatures), op.text, ref expr, op.pos);
					expr = Expression.Not(expr);
				}
				return expr;
			}
			return ParsePrimary();
		}

		Expression ParsePrimary()
		{
			var tokenPos = _token.pos;
			var expr = ParsePrimaryStart();
			while (true)
			{
				if (_token.id == TokenId.Dot)
				{
					NextToken();
					expr = ParseMemberAccess(null, expr);
				}
				else if (_token.id == TokenId.OpenBracket)
				{
					expr = ParseElementAccess(expr);
				}
				else if (_token.id == TokenId.OpenParen)
				{
					var lambda = expr as LambdaExpression;
					if (lambda != null)
						return ParseLambdaInvocation(lambda, tokenPos);
					else if (typeof(Delegate).IsAssignableFrom(expr.Type))
						expr = ParseDelegateInvocation(expr, tokenPos);
					else
						throw CreateParseException(tokenPos, ErrorMessages.InvalidMethodCall, GetTypeName(expr.Type));
				}
				else
				{
					break;
				}
			}
			return expr;
		}

		Expression ParsePrimaryStart()
		{
			switch (_token.id)
			{
				case TokenId.Identifier:
					return ParseIdentifier();
				case TokenId.CharLiteral:
					return ParseCharLiteral();
				case TokenId.StringLiteral:
					return ParseStringLiteral();
				case TokenId.IntegerLiteral:
					return ParseIntegerLiteral();
				case TokenId.RealLiteral:
					return ParseRealLiteral();
				case TokenId.OpenParen:
					return ParseParenExpression();
				case TokenId.End:
					return Expression.Empty();
				default:
					throw CreateParseException(_token.pos, ErrorMessages.ExpressionExpected);
			}
		}

		Expression ParseCharLiteral()
		{
			ValidateToken(TokenId.CharLiteral);
			var s = _token.text.Substring(1, _token.text.Length - 2);

			s = EvalEscapeStringLiteral(s);

			if (s.Length != 1)
				throw CreateParseException(_token.pos, ErrorMessages.InvalidCharacterLiteral);

			NextToken();
			return CreateLiteral(s[0], s);
		}

		Expression ParseStringLiteral()
		{
			ValidateToken(TokenId.StringLiteral);
			var s = _token.text.Substring(1, _token.text.Length - 2);

			s = EvalEscapeStringLiteral(s);

			//int start = 0;
			//while (true)
			//{
			//    int i = s.IndexOf(quote, start);
			//    if (i < 0)
			//        break;
			//    s = s.Remove(i, 1);
			//    start = i + 1;
			//}

			NextToken();
			return CreateLiteral(s, s);
		}

		string EvalEscapeStringLiteral(string source)
		{
			var builder = new StringBuilder();

			for (int i = 0; i < source.Length; i++)
			{
				var c = source[i];
				if (c == '\\')
				{
					if ((i + 1) == source.Length)
						throw CreateParseException(_token.pos, ErrorMessages.InvalidEscapeSequence);

					builder.Append(EvalEscapeChar(source[++i]));
				}
				else
					builder.Append(c);
			}

			return builder.ToString();
		}

		char EvalEscapeChar(char source)
		{
			switch (source)
			{
				case '\'':
					return '\'';
				case '"':
					return '"';
				case '\\':
					return '\\';
				case '0':
					return '\0';
				case 'a':
					return '\a';
				case 'b':
					return '\b';
				case 'f':
					return '\f';
				case 'n':
					return '\n';
				case 'r':
					return '\r';
				case 't':
					return '\t';
				case 'v':
					return '\v';
				default:
					throw CreateParseException(_token.pos, ErrorMessages.InvalidEscapeSequence);
			}
		}

		Expression ParseIntegerLiteral()
		{
			ValidateToken(TokenId.IntegerLiteral);
			string text = _token.text;
			if (text[0] != '-')
			{
				ulong value;
				if (!UInt64.TryParse(text, ParseLiteralUnsignedNumberStyle, ParseCulture, out value))
					throw CreateParseException(_token.pos, ErrorMessages.InvalidIntegerLiteral, text);

				NextToken();

				if (value <= (ulong)Int32.MaxValue)
					return CreateLiteral((int)value, text);
				if (value <= (ulong)UInt32.MaxValue)
					return CreateLiteral((uint)value, text);
				if (value <= (ulong)Int64.MaxValue)
					return CreateLiteral((long)value, text);

				return CreateLiteral(value, text);
			}
			else
			{
				long value;
				if (!Int64.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out value))
					throw CreateParseException(_token.pos, ErrorMessages.InvalidIntegerLiteral, text);

				NextToken();

				if (value >= Int32.MinValue && value <= Int32.MaxValue)
					return CreateLiteral((int)value, text);

				return CreateLiteral(value, text);
			}
		}

		Expression ParseRealLiteral()
		{
			ValidateToken(TokenId.RealLiteral);
			string text = _token.text;
			object value = null;
			char last = text[text.Length - 1];
			if (last == 'F' || last == 'f')
			{
				float f;
				if (float.TryParse(text.Substring(0, text.Length - 1), ParseLiteralDecimalNumberStyle, ParseCulture, out f))
					value = f;
			}
			else if (last == 'M' || last == 'm')
			{
				decimal dc;
				if (decimal.TryParse(text.Substring(0, text.Length - 1), ParseLiteralDecimalNumberStyle, ParseCulture, out dc))
					value = dc;
			}
			else
			{
				double d;
				if (double.TryParse(text, ParseLiteralDecimalNumberStyle, ParseCulture, out d))
					value = d;
			}

			if (value == null)
				throw CreateParseException(_token.pos, ErrorMessages.InvalidRealLiteral, text);

			NextToken();

			return CreateLiteral(value, text);
		}

		Expression CreateLiteral(object value, string text)
		{
			ConstantExpression expr = Expression.Constant(value);

			return expr;
		}

		Expression ParseParenExpression()
		{
			ValidateToken(TokenId.OpenParen, ErrorMessages.OpenParenExpected);
			NextToken();
			Expression innerParenthesesExpression = ParseExpressionSegment();
			ValidateToken(TokenId.CloseParen, ErrorMessages.CloseParenOrOperatorExpected);

			var constExp = innerParenthesesExpression as ConstantExpression;
			if (constExp != null && constExp.Value is Type)
			{
				NextToken();
				var nextExpression = ParseExpressionSegment();

				// cast: (constExp)nextExpression
				return Expression.Convert(nextExpression, (Type)constExp.Value);
			}

			NextToken();
			return innerParenthesesExpression;
		}

		Expression ParseIdentifier()
		{
			ValidateToken(TokenId.Identifier);

			// Working context implementation
			//if (token.text == ParserConstants.keywordIt)
			//    return ParseIt();

			if (_token.text == ParserConstants.KEYWORD_NEW)
				return ParseNew();
			if (_token.text == ParserConstants.KEYWORD_TYPEOF)
				return ParseTypeof();

			Type knownType;
			if (_arguments.TryGetKnownType(_token.text, out knownType))
			{
				return ParseTypeKeyword(knownType);
			}

			Expression keywordExpression;
			if (_arguments.TryGetIdentifier(_token.text, out keywordExpression))
			{
				NextToken();
				return keywordExpression;
			}

			ParameterExpression parameterExpression;
			if (_arguments.TryGetParameters(_token.text, out parameterExpression))
			{
				NextToken();
				return parameterExpression;
			}

			// Working context implementation
			//if (it != null)
			//    return ParseMemberAccess(null, it);

			throw new UnknownIdentifierException(_token.text, _token.pos);
		}

		// Working context implementation
		//Expression ParseIt()
		//{
		//    if (it == null)
		//        throw ParseError(ErrorMessages.NoItInScope);
		//    NextToken();
		//    return it;
		//}

		//Expression ParseIif()
		//{
		//    int errorPos = token.pos;
		//    NextToken();
		//    Expression[] args = ParseArgumentList();
		//    if (args.Length != 3)
		//        throw ParseError(errorPos, ErrorMessages.IifRequiresThreeArgs);
		//    return GenerateConditional(args[0], args[1], args[2], errorPos);
		//}

		Expression ParseTypeof()
		{
			var errorPos = _token.pos;
			NextToken();
			var args = ParseArgumentList();
			if (args.Length != 1)
				throw CreateParseException(errorPos, ErrorMessages.TypeofRequiresOneArg);

			var constExp = args[0] as ConstantExpression;
			if (constExp == null || !(constExp.Value is Type))
				throw CreateParseException(errorPos, ErrorMessages.TypeofRequiresAType);

			return constExp;
		}

		Expression GenerateConditional(Expression test, Expression expr1, Expression expr2, int errorPos)
		{
			if (test.Type != typeof(bool))
				throw CreateParseException(errorPos, ErrorMessages.FirstExprMustBeBool);
			if (expr1.Type != expr2.Type)
			{
				var expr1As2 = expr2 != ParserConstants.NULL_LITERAL_EXPRESSION ? PromoteExpression(expr1, expr2.Type, true) : null;
				var expr2As1 = expr1 != ParserConstants.NULL_LITERAL_EXPRESSION ? PromoteExpression(expr2, expr1.Type, true) : null;
				if (expr1As2 != null && expr2As1 == null)
				{
					expr1 = expr1As2;
				}
				else if (expr2As1 != null && expr1As2 == null)
				{
					expr2 = expr2As1;
				}
				else
				{
					var type1 = expr1 != ParserConstants.NULL_LITERAL_EXPRESSION ? expr1.Type.Name : "null";
					var type2 = expr2 != ParserConstants.NULL_LITERAL_EXPRESSION ? expr2.Type.Name : "null";
					if (expr1As2 != null && expr2As1 != null)
						throw CreateParseException(errorPos, ErrorMessages.BothTypesConvertToOther, type1, type2);

					throw CreateParseException(errorPos, ErrorMessages.NeitherTypeConvertsToOther, type1, type2);
				}
			}
			return Expression.Condition(test, expr1, expr2);
		}

		Expression ParseNew()
		{
			NextToken();
			ValidateToken(TokenId.Identifier, ErrorMessages.IdentifierExpected);

			Type newType;
			if (!_arguments.TryGetKnownType(_token.text, out newType))
				throw new UnknownIdentifierException(_token.text, _token.pos);

			NextToken();
			var args = ParseArgumentList();

			var constructor = newType.GetConstructor(args.Select(p => p.Type).ToArray());
			if (constructor == null)
				throw CreateParseException(_token.pos, ErrorMessages.NoApplicableConstructor, newType);

			return Expression.MemberInit(Expression.New(constructor, args));
		}

		Expression ParseLambdaInvocation(LambdaExpression lambda, int errorPos)
		{
			var args = ParseArgumentList();

			if (!PrepareDelegateInvoke(lambda.Type, ref args))
				throw CreateParseException(errorPos, ErrorMessages.ArgsIncompatibleWithLambda);

			return Expression.Invoke(lambda, args);
		}

		Expression ParseDelegateInvocation(Expression delegateExp, int errorPos)
		{
			var args = ParseArgumentList();

			if (!PrepareDelegateInvoke(delegateExp.Type, ref args))
				throw CreateParseException(errorPos, ErrorMessages.ArgsIncompatibleWithDelegate);

			return Expression.Invoke(delegateExp, args);
		}

		bool PrepareDelegateInvoke(Type type, ref Expression[] args)
		{
			var applicableMethods = FindMethods(type, "Invoke", false, args);
			if (applicableMethods.Length != 1)
				return false;

			args = applicableMethods[0].PromotedParameters;

			return true;
		}

		Expression ParseTypeKeyword(Type type)
		{
			var errorPos = _token.pos;
			NextToken();
			if (_token.id == TokenId.Question)
			{
				if (!type.IsValueType || IsNullableType(type))
					throw CreateParseException(errorPos, ErrorMessages.TypeHasNoNullableForm, GetTypeName(type));
				type = typeof(Nullable<>).MakeGenericType(type);
				NextToken();
			}

			//if (token.id == TokenId.OpenParen)
			//{
			//    return ParseTypeConstructor(type, errorPos);
			//}

			if (_token.id == TokenId.CloseParen)
			{
				return Expression.Constant(type);
			}

			ValidateToken(TokenId.Dot, ErrorMessages.DotOrOpenParenExpected);
			NextToken();
			return ParseMemberAccess(type, null);
		}

		//private Expression ParseTypeConstructor(Type type, int errorPos)
		//{
		//    Expression[] args = ParseArgumentList();
		//    MethodBase method;
		//    switch (FindBestMethod(type.GetConstructors(), args, out method))
		//    {
		//        case 0:
		//            if (args.Length == 1)
		//                return GenerateConversion(args[0], type, errorPos);
		//            throw ParseError(errorPos, ErrorMessages.NoMatchingConstructor, GetTypeName(type));
		//        case 1:
		//            return Expression.New((ConstructorInfo)method, args);
		//        default:
		//            throw ParseError(errorPos, ErrorMessages.AmbiguousConstructorInvocation, GetTypeName(type));
		//    }
		//}

		Expression GenerateConversion(Expression expr, Type type, int errorPos)
		{
			var exprType = expr.Type;
			if (exprType == type)
			{
				return expr;
			}

			//if (exprType.IsValueType && type.IsValueType)
			//{
			//	if ((IsNullableType(exprType) || IsNullableType(type)) &&
			//			GetNonNullableType(exprType) == GetNonNullableType(type))
			//		return Expression.Convert(expr, type);
			//	if ((IsNumericType(exprType) || IsEnumType(exprType)) &&
			//			(IsNumericType(type)) || IsEnumType(type))
			//		return Expression.ConvertChecked(expr, type);
			//}

			//if (exprType.IsAssignableFrom(type) || type.IsAssignableFrom(exprType) ||
			//				exprType.IsInterface || type.IsInterface)
			//{
			//	return Expression.Convert(expr, type);
			//}

			try
			{
				return Expression.ConvertChecked(expr, type);
			}
			catch (InvalidOperationException)
			{
				throw CreateParseException(errorPos, ErrorMessages.CannotConvertValue,
						GetTypeName(exprType), GetTypeName(type));
			}
		}

		Expression ParseMemberAccess(Type type, Expression instance)
		{
			if (instance != null) type = instance.Type;
			var errorPos = _token.pos;
			var id = GetIdentifier();
			NextToken();
			if (_token.id == TokenId.OpenParen)
				return ParseMethodInvocation(type, instance, errorPos, id);

			return GeneratePropertyOrFieldExpression(type, instance, errorPos, id);
		}

		Expression GeneratePropertyOrFieldExpression(Type type, Expression instance, int errorPos, string propertyOrFieldName)
		{
			var member = FindPropertyOrField(type, propertyOrFieldName, instance == null);
			if (member != null)
			{
				return member is PropertyInfo ?
						Expression.Property(instance, (PropertyInfo)member) :
						Expression.Field(instance, (FieldInfo)member);
			}

			if (IsDynamicType(type) || IsDynamicExpression(instance))
				return ParseDynamicProperty(type, instance, propertyOrFieldName);

			throw CreateParseException(errorPos, ErrorMessages.UnknownPropertyOrField, propertyOrFieldName, GetTypeName(type));
		}

		Expression ParseMethodInvocation(Type type, Expression instance, int errorPos, string methodName)
		{
			var args = ParseArgumentList();

			var methodInvocationExpression = ParseNormalMethodInvocation(type, instance, errorPos, methodName, args);
			if (methodInvocationExpression == null && instance != null)
			{
				methodInvocationExpression = ParseExtensionMethodInvocation(type, instance, errorPos, methodName, args);
			}

			if (methodInvocationExpression != null)
				return methodInvocationExpression;

			if (IsDynamicType(type) || IsDynamicExpression(instance))
				return ParseDynamicMethodInvocation(type, instance, methodName, args);

			throw new NoApplicableMethodException(methodName, GetTypeName(type), errorPos);
		}

		Expression ParseExtensionMethodInvocation(Type type, Expression instance, int errorPos, string id, Expression[] args)
		{
			var extensionMethodsArguments = new Expression[args.Length + 1];
			extensionMethodsArguments[0] = instance;
			args.CopyTo(extensionMethodsArguments, 1);

			var extensionMethods = FindExtensionMethods(type, id, extensionMethodsArguments);
			if (extensionMethods.Length > 1)
				throw CreateParseException(errorPos, ErrorMessages.AmbiguousMethodInvocation, id, GetTypeName(type));

			if (extensionMethods.Length == 1)
			{
				var method = extensionMethods[0];

				extensionMethodsArguments = method.PromotedParameters;

				return Expression.Call((MethodInfo)method.MethodBase, extensionMethodsArguments);
			}

			return null;
		}

		Expression ParseNormalMethodInvocation(Type type, Expression instance, int errorPos, string id, Expression[] args)
		{
			var applicableMethods = FindMethods(type, id, instance == null, args);
			if (applicableMethods.Length > 1)
				throw CreateParseException(errorPos, ErrorMessages.AmbiguousMethodInvocation, id, GetTypeName(type));

			if (applicableMethods.Length == 1)
			{
				var method = applicableMethods[0];

				return Expression.Call(instance, (MethodInfo)method.MethodBase, method.PromotedParameters);
			}

			return null;
		}

		//static Type FindGenericType(Type generic, Type type)
		//{
		//	while (type != null && type != typeof(object))
		//	{
		//		if (type.IsGenericType && type.GetGenericTypeDefinition() == generic) return type;
		//		if (generic.IsInterface)
		//		{
		//			foreach (Type intfType in type.GetInterfaces())
		//			{
		//				Type found = FindGenericType(generic, intfType);
		//				if (found != null) return found;
		//			}
		//		}
		//		type = type.BaseType;
		//	}
		//	return null;
		//}

		//Expression ParseAggregate(Expression instance, Type elementType, string methodName, int errorPos)
		//{
		//    ParameterExpression outerIt = it;
		//    ParameterExpression innerIt = Expression.Parameter(elementType, "");
		//    it = innerIt;
		//    Expression[] args = ParseArgumentList();
		//    it = outerIt;
		//    MethodBase signature;
		//    if (FindMethod(typeof(IEnumerableSignatures), methodName, false, args, out signature) != 1)
		//        throw ParseError(errorPos, ErrorMessages.NoApplicableAggregate, methodName);
		//    Type[] typeArgs;
		//    if (signature.Name == "Min" || signature.Name == "Max")
		//    {
		//        typeArgs = new Type[] { elementType, args[0].Type };
		//    }
		//    else
		//    {
		//        typeArgs = new Type[] { elementType };
		//    }
		//    if (args.Length == 0)
		//    {
		//        args = new Expression[] { instance };
		//    }
		//    else
		//    {
		//        args = new Expression[] { instance, Expression.Lambda(args[0], innerIt) };
		//    }
		//    return Expression.Call(typeof(Enumerable), signature.Name, typeArgs, args);
		//}

		private static Expression ParseDynamicProperty(Type type, Expression instance, string propertyOrFieldName)
		{
			var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(
				CSharpBinderFlags.None,
				propertyOrFieldName,
				type,
				new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }
				);

			return Expression.Dynamic(binder, typeof(object), instance);
		}

		private static Expression ParseDynamicMethodInvocation(Type type, Expression instance, string methodName, Expression[] args)
		{
			var argsDynamic = args.ToList();
			argsDynamic.Insert(0, instance);
			var binderM = Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(
				CSharpBinderFlags.None,
				methodName,
				null,
				type,
				argsDynamic.Select(x => CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null))
				);

			return Expression.Dynamic(binderM, typeof(object), argsDynamic);
		}

		Expression[] ParseArgumentList()
		{
			ValidateToken(TokenId.OpenParen, ErrorMessages.OpenParenExpected);
			NextToken();
			var args = _token.id != TokenId.CloseParen ? ParseArguments() : new Expression[0];
			ValidateToken(TokenId.CloseParen, ErrorMessages.CloseParenOrCommaExpected);
			NextToken();
			return args;
		}

		Expression[] ParseArguments()
		{
			var argList = new List<Expression>();
			while (true)
			{
				argList.Add(ParseExpressionSegment());
				if (_token.id != TokenId.Comma) break;
				NextToken();
			}
			return argList.ToArray();
		}

		Expression ParseElementAccess(Expression expr)
		{
			var errorPos = _token.pos;
			ValidateToken(TokenId.OpenBracket, ErrorMessages.OpenParenExpected);
			NextToken();
			var args = ParseArguments();
			ValidateToken(TokenId.CloseBracket, ErrorMessages.CloseBracketOrCommaExpected);
			NextToken();
			if (expr.Type.IsArray)
			{
				if (expr.Type.GetArrayRank() != 1 || args.Length != 1)
					throw CreateParseException(errorPos, ErrorMessages.CannotIndexMultiDimArray);
				var index = PromoteExpression(args[0], typeof(int), true);
				if (index == null)
					throw CreateParseException(errorPos, ErrorMessages.InvalidIndex);
				return Expression.ArrayIndex(expr, index);
			}
			else
			{
				var applicableMethods = FindIndexer(expr.Type, args);
				if (applicableMethods.Length == 0)
				{
					throw CreateParseException(errorPos, ErrorMessages.NoApplicableIndexer,
							GetTypeName(expr.Type));
				}

				if (applicableMethods.Length > 1)
				{
					throw CreateParseException(errorPos, ErrorMessages.AmbiguousIndexerInvocation,
							GetTypeName(expr.Type));
				}

				var method = applicableMethods[0];

				return Expression.Call(expr, (MethodInfo)method.MethodBase, method.PromotedParameters);
			}
		}

		static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		static bool IsDynamicType(Type type)
		{
			return typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type);
		}

		static bool IsDynamicExpression(Expression instance)
		{
			return instance != null && instance.NodeType == ExpressionType.Dynamic;
		}

		static Type GetNonNullableType(Type type)
		{
			return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
		}

		static string GetTypeName(Type type)
		{
			var baseType = GetNonNullableType(type);
			var s = baseType.Name;
			if (type != baseType) s += '?';
			return s;
		}

		//static bool IsNumericType(Type type)
		//{
		//	return GetNumericTypeKind(type) != 0;
		//}

		static bool IsSignedIntegralType(Type type)
		{
			return GetNumericTypeKind(type) == 2;
		}

		static bool IsUnsignedIntegralType(Type type)
		{
			return GetNumericTypeKind(type) == 3;
		}

		static int GetNumericTypeKind(Type type)
		{
			type = GetNonNullableType(type);
			if (type.IsEnum) return 0;
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Char:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return 1;
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
					return 2;
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return 3;
				default:
					return 0;
			}
		}

		//static bool IsEnumType(Type type)
		//{
		//	return GetNonNullableType(type).IsEnum;
		//}

		void CheckAndPromoteOperand(Type signatures, string opName, ref Expression expr, int errorPos)
		{
			var args = new [] { expr };

			args = PrepareOperandArguments(signatures, args);

			expr = args[0];
		}

		void CheckAndPromoteOperands(Type signatures, string opName, ref Expression left, ref Expression right, int errorPos)
		{
			var args = new [] { left, right };

			args = PrepareOperandArguments(signatures, args);

			left = args[0];
			right = args[1];
		}

		Expression[] PrepareOperandArguments(Type signatures, Expression[] args)
		{
			var applicableMethods = FindMethods(signatures, "F", false, args);
			if (applicableMethods.Length == 1)
				return applicableMethods[0].PromotedParameters;

			return args;
		}

		MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
		{
			var flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
					(staticAccess ? BindingFlags.Static : BindingFlags.Instance) | _bindingCase;

			foreach (var t in SelfAndBaseTypes(type))
			{
				var members = t.FindMembers(MemberTypes.Property | MemberTypes.Field, flags, _memberFilterCase, memberName);
				if (members.Length != 0)
					return members[0];
			}
			return null;
		}

		MethodData[] FindMethods(Type type, string methodName, bool staticAccess, Expression[] args)
		{
			//var exactMethod = type.GetMethod(methodName, args.Select(p => p.Type).ToArray());
			//if (exactMethod != null)
			//{
			//	return new MethodData[] { new MethodData(){ MethodBase = exactMethod, Parameters = exactMethod.GetParameters(), PromotedParameters = args} };
			//}

			var flags = BindingFlags.Public | BindingFlags.DeclaredOnly |
					(staticAccess ? BindingFlags.Static : BindingFlags.Instance) | _bindingCase;
			foreach (var t in SelfAndBaseTypes(type))
			{
				var members = t.FindMembers(MemberTypes.Method, flags, _memberFilterCase, methodName);
				var applicableMethods = FindBestMethod(members.Cast<MethodBase>(), args);

				if (applicableMethods.Length > 0)
					return applicableMethods;
			}

			return new MethodData[0];
		}

		MethodData[] FindExtensionMethods(Type type, string methodName, Expression[] args)
		{
			var matchMethods = _arguments.GetExtensionMethods(methodName);

			return FindBestMethod(matchMethods, args);
		}

		MethodData[] FindIndexer(Type type, Expression[] args)
		{
			foreach (var t in SelfAndBaseTypes(type))
			{
				MemberInfo[] members = t.GetDefaultMembers();
				if (members.Length != 0)
				{
					IEnumerable<MethodBase> methods = members.
							OfType<PropertyInfo>().
							Select(p => (MethodBase)p.GetGetMethod()).
							Where(m => m != null);

					var applicableMethods = FindBestMethod(methods, args);
					if (applicableMethods.Length > 0)
						return applicableMethods;
				}
			}

			return new MethodData[0];
		}

		static IEnumerable<Type> SelfAndBaseTypes(Type type)
		{
			if (type.IsInterface)
			{
				var types = new List<Type>();
				AddInterface(types, type);

				types.Add(typeof(object));

				return types;
			}
			return SelfAndBaseClasses(type);
		}

		static IEnumerable<Type> SelfAndBaseClasses(Type type)
		{
			while (type != null)
			{
				yield return type;
				type = type.BaseType;
			}
		}

		static void AddInterface(List<Type> types, Type type)
		{
			if (!types.Contains(type))
			{
				types.Add(type);
				foreach (Type t in type.GetInterfaces())
				{
					AddInterface(types, t);
				}
			}
		}

		class MethodData
		{
			public MethodBase MethodBase;
			public ParameterInfo[] Parameters;
			public Expression[] PromotedParameters;
			public bool HasParamsArray;
		}

		MethodData[] FindBestMethod(IEnumerable<MethodBase> methods, Expression[] args)
		{
			var applicable = methods.
					Select(m => new MethodData { MethodBase = m, Parameters = m.GetParameters() }).
					Where(m => CheckIfMethodIsApplicableAndPrepareIt(m, args)).
					ToArray();
			if (applicable.Length > 1)
			{
				applicable = applicable.
						Where(m => applicable.All(n => m == n || MethodHasPriority(args, m, n))).
						ToArray();
			}

			return applicable;
		}

		bool CheckIfMethodIsApplicableAndPrepareIt(MethodData method, Expression[] args)
		{
			if (method.Parameters.Length > args.Length)
				return false;

			var promotedArgs = new List<Expression>();
			var declaredWorkingParameters = 0;

			Type paramsArrayTypeFound = null;
			List<Expression> paramsArrayPromotedArgument = null;

			foreach (var currentArgument in args)
			{
				Type parameterType;

				if (paramsArrayTypeFound != null)
				{
					parameterType = paramsArrayTypeFound;
				}
				else
				{
					if (declaredWorkingParameters >= method.Parameters.Length)
					{
						return false;
					}

					var parameterDeclaration = method.Parameters[declaredWorkingParameters];
					if (parameterDeclaration.IsOut)
					{
						return false;
					}

					parameterType = parameterDeclaration.ParameterType;

					if (HasParamsArrayType(parameterDeclaration))
					{
						paramsArrayTypeFound = parameterType;
					}

					declaredWorkingParameters++;
				}

				if (paramsArrayPromotedArgument == null)
				{
					if (parameterType.IsGenericParameter)
					{
						promotedArgs.Add(currentArgument);
						continue;
					}
					else
					{
						var promoted = PromoteExpression(currentArgument, parameterType, true);
						if (promoted != null)
						{
							promotedArgs.Add(promoted);
							continue;
						}
					}
				}

				if (paramsArrayTypeFound != null)
				{
					var promoted = PromoteExpression(currentArgument, paramsArrayTypeFound.GetElementType(), true);
					if (promoted != null)
					{
						paramsArrayPromotedArgument = paramsArrayPromotedArgument ?? new List<Expression>();
						paramsArrayPromotedArgument.Add(promoted);
						continue;
					}
				}

				return false;
			}

			if (paramsArrayPromotedArgument != null)
			{
				method.HasParamsArray = true;
				promotedArgs.Add(Expression.NewArrayInit(paramsArrayTypeFound.GetElementType(), paramsArrayPromotedArgument));
			}

			method.PromotedParameters = promotedArgs.ToArray();

			if (method.MethodBase.IsGenericMethodDefinition &&
					method.MethodBase is MethodInfo)
			{
				var methodInfo = ((MethodInfo)method.MethodBase);

				var genericArgsType = ExtractActualGenericArguments(
									method.Parameters.Select(p => p.ParameterType).ToArray(),
									method.PromotedParameters.Select(p => p.Type).ToArray());

				method.MethodBase = methodInfo.MakeGenericMethod(genericArgsType.ToArray());
			}

			return true;
		}

		List<Type> ExtractActualGenericArguments(Type[] requestedParameters, Type[] actualParameters)
		{
			var extractedGenericTypes = new List<Type>();

			for (var i = 0; i < requestedParameters.Length; i++)
			{
				var requestedType = requestedParameters[i];
				var actualType = actualParameters[i];

				if (requestedType.IsGenericParameter)
				{
					extractedGenericTypes.Add(actualType);
				}
				else if (requestedType.ContainsGenericParameters)
				{
					var innerGenericTypes = ExtractActualGenericArguments(requestedType.GetGenericArguments(), actualType.GetGenericArguments());

					extractedGenericTypes.AddRange(innerGenericTypes);
				}
			}

			return extractedGenericTypes;
		}

		Expression PromoteExpression(Expression expr, Type type, bool exact)
		{
			if (expr.Type == type) return expr;
			if (expr is ConstantExpression)
			{
				var ce = (ConstantExpression)expr;
				if (ce == ParserConstants.NULL_LITERAL_EXPRESSION)
				{
					if (!type.IsValueType || IsNullableType(type))
						return Expression.Constant(null, type);
				}
			}

			if (type.IsGenericType)
			{
				var genericType = FindAssignableGenericType(expr.Type, type.GetGenericTypeDefinition());
				if (genericType != null)
					return Expression.Convert(expr, genericType);
			}

			if (IsCompatibleWith(expr.Type, type))
			{
				if (type.IsValueType || exact)
				{
					return Expression.Convert(expr, type);
				}
				return expr;
			}

			return null;
		}

		//object ParseNumber(string text, Type type)
		//{
		//	switch (Type.GetTypeCode(GetNonNullableType(type)))
		//	{
		//		case TypeCode.SByte:
		//			sbyte sb;
		//			if (sbyte.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out sb)) return sb;
		//			break;
		//		case TypeCode.Byte:
		//			byte b;
		//			if (byte.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out b)) return b;
		//			break;
		//		case TypeCode.Int16:
		//			short s;
		//			if (short.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out s)) return s;
		//			break;
		//		case TypeCode.UInt16:
		//			ushort us;
		//			if (ushort.TryParse(text, ParseLiteralUnsignedNumberStyle, ParseCulture, out us)) return us;
		//			break;
		//		case TypeCode.Int32:
		//			int i;
		//			if (int.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out i)) return i;
		//			break;
		//		case TypeCode.UInt32:
		//			uint ui;
		//			if (uint.TryParse(text, ParseLiteralUnsignedNumberStyle, ParseCulture, out ui)) return ui;
		//			break;
		//		case TypeCode.Int64:
		//			long l;
		//			if (long.TryParse(text, ParseLiteralNumberStyle, ParseCulture, out l)) return l;
		//			break;
		//		case TypeCode.UInt64:
		//			ulong ul;
		//			if (ulong.TryParse(text, ParseLiteralUnsignedNumberStyle, ParseCulture, out ul)) return ul;
		//			break;
		//		case TypeCode.Single:
		//			float f;
		//			if (float.TryParse(text, ParseLiteralDecimalNumberStyle, ParseCulture, out f)) return f;
		//			break;
		//		case TypeCode.Double:
		//			double d;
		//			if (double.TryParse(text, ParseLiteralDecimalNumberStyle, ParseCulture, out d)) return d;
		//			break;
		//		case TypeCode.Decimal:
		//			decimal e;
		//			if (decimal.TryParse(text, ParseLiteralDecimalNumberStyle, ParseCulture, out e)) return e;
		//			break;
		//	}
		//	return null;
		//}

		//static object ParseEnum(string name, Type type)
		//{
		//	if (type.IsEnum)
		//	{
		//		MemberInfo[] memberInfos = type.FindMembers(MemberTypes.Field,
		//				BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static,
		//				Type.FilterNameIgnoreCase, name);
		//		if (memberInfos.Length != 0) return ((FieldInfo)memberInfos[0]).GetValue(null);
		//	}
		//	return null;
		//}

		static bool IsCompatibleWith(Type source, Type target)
		{
			if (source == target)
			{
				return true;
			}

			if (!target.IsValueType)
			{
				return target.IsAssignableFrom(source);
			}
			var st = GetNonNullableType(source);
			var tt = GetNonNullableType(target);
			if (st != source && tt == target) return false;
			var sc = st.IsEnum ? TypeCode.Object : Type.GetTypeCode(st);
			var tc = tt.IsEnum ? TypeCode.Object : Type.GetTypeCode(tt);
			switch (sc)
			{
				case TypeCode.SByte:
					switch (tc)
					{
						case TypeCode.SByte:
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Byte:
					switch (tc)
					{
						case TypeCode.Byte:
						case TypeCode.Int16:
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int16:
					switch (tc)
					{
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt16:
					switch (tc)
					{
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int32:
					switch (tc)
					{
						case TypeCode.Int32:
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt32:
					switch (tc)
					{
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Int64:
					switch (tc)
					{
						case TypeCode.Int64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.UInt64:
					switch (tc)
					{
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return true;
					}
					break;
				case TypeCode.Single:
					switch (tc)
					{
						case TypeCode.Single:
						case TypeCode.Double:
							return true;
					}
					break;
				default:
					if (st == tt) return true;
					break;
			}
			return false;
		}

		// from http://stackoverflow.com/a/1075059/209727
		static Type FindAssignableGenericType(Type givenType, Type genericTypeDefinition)
		{
			var interfaceTypes = givenType.GetInterfaces();

			foreach (var it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericTypeDefinition)
				{
					return it;
				}
			}

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericTypeDefinition)
			{
				return givenType;
			}

			Type baseType = givenType.BaseType;
			if (baseType == null) return null;

			return FindAssignableGenericType(baseType, genericTypeDefinition);
		}

		static bool HasParamsArrayType(ParameterInfo parameterInfo)
		{
			return parameterInfo.IsDefined(typeof(ParamArrayAttribute), false);
		}

		static Type GetParameterType(ParameterInfo parameterInfo)
		{
			bool isParamsArray = HasParamsArrayType(parameterInfo);
			Type type = isParamsArray ? parameterInfo.ParameterType.GetElementType() : parameterInfo.ParameterType;
			return type;
		}

		static bool MethodHasPriority(Expression[] args, MethodData method, MethodData otherMethod)
		{
			if (method.HasParamsArray == false && otherMethod.HasParamsArray)
				return true;
			if (method.HasParamsArray && otherMethod.HasParamsArray == false)
				return false;

			//if (m1.Parameters.Length > m2.Parameters.Length)
			//	return true;
			//else if (m1.Parameters.Length < m2.Parameters.Length)
			//	return false;

			bool better = false;
			for (int i = 0; i < args.Length; i++)
			{
				ParameterInfo methodParam = method.Parameters[i];
				ParameterInfo otherMethodParam = otherMethod.Parameters[i];
				Type methodParamType = GetParameterType(methodParam);
				Type otherMethodParamType = GetParameterType(otherMethodParam);
				int c = CompareConversions(args[i].Type, methodParamType, otherMethodParamType);
				if (c < 0) return false;
				if (c > 0) better = true;
				if (HasParamsArrayType(methodParam) || HasParamsArrayType(otherMethodParam))
				{
					break;
				}
			}
			return better;
		}

		// Return 1 if s -> t1 is a better conversion than s -> t2
		// Return -1 if s -> t2 is a better conversion than s -> t1
		// Return 0 if neither conversion is better
		static int CompareConversions(Type s, Type t1, Type t2)
		{
			if (t1 == t2) return 0;
			if (s == t1) return 1;
			if (s == t2) return -1;

			bool assignableT1 = t1.IsAssignableFrom(s);
			bool assignableT2 = t2.IsAssignableFrom(s);
			if (assignableT1 && !assignableT2) return 1;
			if (assignableT2 && !assignableT1) return -1;

			bool compatibleT1t2 = IsCompatibleWith(t1, t2);
			bool compatibleT2t1 = IsCompatibleWith(t2, t1);
			if (compatibleT1t2 && !compatibleT2t1) return 1;
			if (compatibleT2t1 && !compatibleT1t2) return -1;

			if (IsSignedIntegralType(t1) && IsUnsignedIntegralType(t2)) return 1;
			if (IsSignedIntegralType(t2) && IsUnsignedIntegralType(t1)) return -1;

			return 0;
		}

		Expression GenerateEqual(Expression left, Expression right)
		{
			return Expression.Equal(left, right);
		}

		Expression GenerateNotEqual(Expression left, Expression right)
		{
			return Expression.NotEqual(left, right);
		}

		Expression GenerateGreaterThan(Expression left, Expression right)
		{
			if (left.Type == typeof(string))
			{
				return Expression.GreaterThan(
						GenerateStaticMethodCall("Compare", left, right),
						Expression.Constant(0)
				);
			}
			return Expression.GreaterThan(left, right);
		}

		Expression GenerateGreaterThanEqual(Expression left, Expression right)
		{
			if (left.Type == typeof(string))
			{
				return Expression.GreaterThanOrEqual(
						GenerateStaticMethodCall("Compare", left, right),
						Expression.Constant(0)
				);
			}
			return Expression.GreaterThanOrEqual(left, right);
		}

		Expression GenerateLessThan(Expression left, Expression right)
		{
			if (left.Type == typeof(string))
			{
				return Expression.LessThan(
						GenerateStaticMethodCall("Compare", left, right),
						Expression.Constant(0)
				);
			}
			return Expression.LessThan(left, right);
		}

		Expression GenerateLessThanEqual(Expression left, Expression right)
		{
			if (left.Type == typeof(string))
			{
				return Expression.LessThanOrEqual(
						GenerateStaticMethodCall("Compare", left, right),
						Expression.Constant(0)
				);
			}
			return Expression.LessThanOrEqual(left, right);
		}

		Expression GenerateAdd(Expression left, Expression right)
		{
			if (left.Type == typeof(string) && right.Type == typeof(string))
			{
				return GenerateStaticMethodCall("Concat", left, right);
			}
			return Expression.Add(left, right);
		}

		Expression GenerateSubtract(Expression left, Expression right)
		{
			return Expression.Subtract(left, right);
		}

		Expression GenerateStringConcat(Expression left, Expression right)
		{
			return Expression.Call(
					null,
					typeof(string).GetMethod("Concat", new[] { typeof(object), typeof(object) }),
					new[] { left, right });
		}

		MethodInfo GetStaticMethod(string methodName, Expression left, Expression right)
		{
			return left.Type.GetMethod(methodName, new[] { left.Type, right.Type });
		}

		Expression GenerateStaticMethodCall(string methodName, Expression left, Expression right)
		{
			return Expression.Call(null, GetStaticMethod(methodName, left, right), new[] { left, right });
		}

		void SetTextPos(int pos)
		{
			_parsePosition = pos;
			_parseChar = _parsePosition < _expressionTextLength ? _expressionText[_parsePosition] : '\0';
		}

		void NextChar()
		{
			if (_parsePosition < _expressionTextLength)
				_parsePosition++;

			_parseChar = _parsePosition < _expressionTextLength ? _expressionText[_parsePosition] : '\0';
		}

		void PreviousChar()
		{
			SetTextPos(_parsePosition - 1);
		}

		void NextToken()
		{
			while (char.IsWhiteSpace(_parseChar))
				NextChar();

			TokenId t;
			var tokenPos = _parsePosition;
			switch (_parseChar)
			{
				case '!':
					NextChar();
					if (_parseChar == '=')
					{
						NextChar();
						t = TokenId.ExclamationEqual;
					}
					else
					{
						t = TokenId.Exclamation;
					}
					break;
				case '%':
					NextChar();
					t = TokenId.Percent;
					break;
				case '&':
					NextChar();
					if (_parseChar == '&')
					{
						NextChar();
						t = TokenId.DoubleAmphersand;
					}
					else
					{
						throw CreateParseException(_parsePosition, ErrorMessages.InvalidCharacter, _parseChar);
					}
					break;
				case '(':
					NextChar();
					t = TokenId.OpenParen;
					break;
				case ')':
					NextChar();
					t = TokenId.CloseParen;
					break;
				case '*':
					NextChar();
					t = TokenId.Asterisk;
					break;
				case '+':
					NextChar();
					t = TokenId.Plus;
					break;
				case ',':
					NextChar();
					t = TokenId.Comma;
					break;
				case '-':
					NextChar();
					t = TokenId.Minus;
					break;
				case '.':
					NextChar();
					t = TokenId.Dot;
					break;
				case '/':
					NextChar();
					t = TokenId.Slash;
					break;
				case ':':
					NextChar();
					t = TokenId.Colon;
					break;
				case '<':
					NextChar();
					if (_parseChar == '=')
					{
						NextChar();
						t = TokenId.LessThanEqual;
					}
					else
					{
						t = TokenId.LessThan;
					}
					break;
				case '=':
					NextChar();
					if (_parseChar == '=')
					{
						NextChar();
						t = TokenId.DoubleEqual;
					}
					else
					{
						t = TokenId.Equal;
					}
					break;
				case '>':
					NextChar();
					if (_parseChar == '=')
					{
						NextChar();
						t = TokenId.GreaterThanEqual;
					}
					else
					{
						t = TokenId.GreaterThan;
					}
					break;
				case '?':
					NextChar();
					t = TokenId.Question;
					break;
				case '[':
					NextChar();
					t = TokenId.OpenBracket;
					break;
				case ']':
					NextChar();
					t = TokenId.CloseBracket;
					break;
				case '|':
					NextChar();
					if (_parseChar == '|')
					{
						NextChar();
						t = TokenId.DoubleBar;
					}
					else
					{
						t = TokenId.Bar;
					}
					break;
				case '"':
					NextChar();
					bool isEscapeS = false;
					bool isEndS = _parseChar == '\"';
					while (_parsePosition < _expressionTextLength && !isEndS)
					{
						isEscapeS = _parseChar == '\\' && !isEscapeS;
						NextChar();
						isEndS = (_parseChar == '\"' && !isEscapeS);
					}

					if (_parsePosition == _expressionTextLength)
						throw CreateParseException(_parsePosition, ErrorMessages.UnterminatedStringLiteral);

					NextChar();

					t = TokenId.StringLiteral;
					break;
				case '\'':
					NextChar();
					bool isEscapeC = false;
					bool isEndC = false;
					while (_parsePosition < _expressionTextLength && !isEndC)
					{
						isEscapeC = _parseChar == '\\' && !isEscapeC;
						NextChar();
						isEndC = (_parseChar == '\'' && !isEscapeC);
					}

					if (_parsePosition == _expressionTextLength)
						throw CreateParseException(_parsePosition, ErrorMessages.UnterminatedStringLiteral);

					NextChar();

					t = TokenId.CharLiteral;
					break;
				default:

					if (Char.IsLetter(_parseChar) || _parseChar == '@' || _parseChar == '_')
					{
						do
						{
							NextChar();
						} while (Char.IsLetterOrDigit(_parseChar) || _parseChar == '_');
						t = TokenId.Identifier;
						break;
					}

					if (Char.IsDigit(_parseChar))
					{
						t = TokenId.IntegerLiteral;
						do
						{
							NextChar();
						} while (Char.IsDigit(_parseChar));

						if (_parseChar == '.')
						{
							NextChar();
							if (Char.IsDigit(_parseChar))
							{
								t = TokenId.RealLiteral;
								do
								{
									NextChar();
								} while (Char.IsDigit(_parseChar));
							}
							else
							{
								PreviousChar();
								break;
							}
						}

						if (_parseChar == 'E' || _parseChar == 'e')
						{
							t = TokenId.RealLiteral;
							NextChar();
							if (_parseChar == '+' || _parseChar == '-')
								NextChar();
							ValidateDigit();
							do
							{
								NextChar();
							} while (Char.IsDigit(_parseChar));
						}
						if (_parseChar == 'F' || _parseChar == 'f' || _parseChar == 'M' || _parseChar == 'm')
							NextChar();
						break;
					}
					if (_parsePosition == _expressionTextLength)
					{
						t = TokenId.End;
						break;
					}
					throw CreateParseException(_parsePosition, ErrorMessages.InvalidCharacter, _parseChar);
			}
			_token.id = t;
			_token.text = _expressionText.Substring(tokenPos, _parsePosition - tokenPos);
			_token.pos = tokenPos;
		}

		//bool TokenIdentifierIs(string id)
		//{
		//	return _token.id == TokenId.Identifier && String.Equals(id, _token.text, StringComparison.OrdinalIgnoreCase);
		//}

		string GetIdentifier()
		{
			ValidateToken(TokenId.Identifier, ErrorMessages.IdentifierExpected);
			string id = _token.text;
			if (id.Length > 1 && id[0] == '@')
				id = id.Substring(1);
			return id;
		}

		void ValidateDigit()
		{
			if (!char.IsDigit(_parseChar))
				throw CreateParseException(_parsePosition, ErrorMessages.DigitExpected);
		}

		void ValidateToken(TokenId t, string errorMessage)
		{
			if (_token.id != t)
				throw CreateParseException(_token.pos, errorMessage);
		}

		void ValidateToken(TokenId t)
		{
			if (_token.id != t)
				throw CreateParseException(_token.pos, ErrorMessages.SyntaxError);
		}

		Exception CreateParseException(int pos, string format, params object[] args)
		{
			return new ParseException(string.Format(format, args), pos);
		}
	}
}
