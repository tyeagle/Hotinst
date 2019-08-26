using System;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace HOTINST.COMMON.Localization
{
	/// <summary>
	/// LocalizedNonDependencyProperty
	/// </summary>
	public class LocalizedNonDependencyProperty : LocalizedProperty
    {
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="property"></param>
        public LocalizedNonDependencyProperty(DependencyObject obj, PropertyInfo property)
            : base(obj, property)
        {
        }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>The value of the property.</returns>
        protected internal override object GetValue()
        {
            var targetObject = Object;

	        return targetObject != null ? ((PropertyInfo)Property).GetValue(targetObject, null) : null;
        }

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected internal override void SetValue(object value)
        {
            var targetObject = Object;

            if (targetObject != null)
            {
                if (targetObject.CheckAccess())
                {
                    ((PropertyInfo)Property).SetValue(targetObject, value, null);
                }
                else
                {
                    targetObject.Dispatcher.Invoke(new SendOrPostCallback(SetValue), value);
                }
            }
        }

        /// <summary>
        /// Gets the type of the value of the property.
        /// </summary>
        /// <returns>The type of the value of the property.</returns>
        protected internal override Type GetValueType()
        {
            return ((PropertyInfo)Property).PropertyType;
        }
    }
}
