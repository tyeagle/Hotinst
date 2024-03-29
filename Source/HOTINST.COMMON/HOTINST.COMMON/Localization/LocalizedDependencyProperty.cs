﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace HOTINST.COMMON.Localization
{
	/// <summary>
	/// LocalizedDependencyProperty
	/// </summary>
	public class LocalizedDependencyProperty : LocalizedProperty
    {
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="property"></param>
        public LocalizedDependencyProperty(DependencyObject obj, DependencyProperty property)
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

            if (targetObject != null)
            {
	            return targetObject.CheckAccess() ? targetObject.GetValue((DependencyProperty)Property) : targetObject.Dispatcher.Invoke(new DispatcherOperationCallback(GetValue));
            }

            return null;
        }

	    private object GetValue(object dummy)
        {
            return GetValue();
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
                    targetObject.SetValue((DependencyProperty)Property, value);
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
            return ((DependencyProperty)Property).PropertyType;
        }
    }
}
