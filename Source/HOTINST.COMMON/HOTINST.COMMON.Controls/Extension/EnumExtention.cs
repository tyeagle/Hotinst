/**
 * ==============================================================================
 * Classname   : EnumExtention
 * Description : 
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/4/19 19:39:50
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 枚举扩展
	/// </summary>
	public static class EnumExtention
	{
		/// <summary>
		/// 获取枚举各项的描述信息
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetDescriptions(this Type enumType)
		{
			if(!enumType.IsEnum)
				throw new ArgumentException(@"传入的参数必须是枚举类型！", nameof(enumType));

			FieldInfo[] fields = enumType.GetFields();
			if(fields.Length == 0)
				return null;

			return from fieldInfo in fields
				where fieldInfo.FieldType.IsEnum
				select fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
				into attrs where attrs.Length > 0
				select ((DescriptionAttribute)attrs[0]).Description;
		}

		/// <summary>
		/// 获取枚举描述
		/// </summary>
		/// <param name="enumName"></param>
		/// <returns></returns>
		public static string GetDescription(this Enum enumName)
		{
			string description;
			FieldInfo fieldInfo = enumName.GetType().GetField(enumName.ToString());
			DescriptionAttribute[] attributes = fieldInfo.GetDescriptAttr();
			if(attributes != null && attributes.Length > 0)
				description = attributes[0].Description;
			else
				throw new ArgumentException($@"{enumName} 未能找到对应的枚举描述.", nameof(enumName));
			return description;
		}

		/// <summary>
		/// 通过描述获取枚举值
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="description"></param>
		/// <returns></returns>
		public static TEnum GetEnum<TEnum>(string description)
		{
			Type type = typeof(TEnum);
			foreach(FieldInfo field in type.GetFields())
			{
				DescriptionAttribute[] curDesc = field.GetDescriptAttr();
				if(curDesc != null && curDesc.Length > 0)
				{
					if(curDesc[0].Description == description)
						return (TEnum)field.GetValue(null);
				}
				else
				{
					if(field.Name == description)
						return (TEnum)field.GetValue(null);
				}
			}
			throw new ArgumentException($@"{description} 未能找到对应的枚举.", nameof(description));
		}

		/// <summary>
		/// 获取枚举描述属性
		/// </summary>
		/// <param name="fieldInfo"></param>
		/// <returns></returns>
		private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
		{
			return (DescriptionAttribute[])fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false);
		}
	}
}