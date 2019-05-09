using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 枚举的Xaml标记扩展
	/// </summary>
	public class EnumerationExtension : MarkupExtension
	{
		private Type _enumType;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="enumType"></param>
		public EnumerationExtension(Type enumType)
		{
			EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
		}

		/// <summary>
		/// 
		/// </summary>
		public Type EnumType
		{
			get => _enumType;
			private set
			{
				if(_enumType == value)
					return;

				var enumType = Nullable.GetUnderlyingType(value) ?? value;

				if(enumType.IsEnum == false)
					throw new ArgumentException("Type must be an Enum.");

				_enumType = value;
			}
		}
		
		private string GetDescription(object enumValue)
		{
			var descriptionAttribute = EnumType
				.GetField(enumValue.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.FirstOrDefault() as DescriptionAttribute;
			
			return descriptionAttribute != null
				? descriptionAttribute.Description
				: enumValue.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		public class EnumerationMember
		{
			/// <summary>
			/// 描述
			/// </summary>
			public string Description { get; set; }
			/// <summary>
			/// 值
			/// </summary>
			public object Value { get; set; }
		}

		#region Overrides of MarkupExtension

		/// <summary>在派生类中实现时，返回一个对象，此对象被设置为此标记扩展的目标属性的值。</summary>
		/// <returns>要在应用了扩展的属性上设置的对象值。</returns>
		/// <param name="serviceProvider">可以为标记扩展提供服务的对象。</param>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var enumValues = Enum.GetValues(EnumType);

			return (from object enumValue in enumValues select new EnumerationMember
			{
				Value = enumValue,
				Description = GetDescription(enumValue)
			}).ToArray();
		}

		#endregion
	}
}