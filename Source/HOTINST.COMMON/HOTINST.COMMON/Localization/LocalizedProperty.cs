using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace HOTINST.COMMON.Localization
{
    /// <summary>
    /// Contains information about a localized property.
    /// </summary>
    public abstract class LocalizedProperty
    {
        #region Public properties

        /// <summary>
        /// The convert to use to convert the retrieved resource to a value suitable
        /// for the property.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// The parameter to pass to the converter.
        /// </summary>
        public object ConverterParameter { get; set; }

        #endregion

        #region Protected properties

        /// <summary>
        /// The object to which the property belongs.
        /// </summary>
        protected internal DependencyObject Object => (DependencyObject)_object.Target;

	    /// <summary>
        /// The localized property.
        /// </summary>
        protected internal object Property { get; private set; }

        #endregion

        #region Internal properties

        /// <summary>
        /// Indicates if the object to the property belongs has been garbage collected.
        /// </summary>
        internal bool IsAlive => _object.IsAlive;

	    /// <summary>
        /// Indicates if the object is in design mode.
        /// </summary>
        internal bool IsInDesignMode
        {
            get
            {
                var obj = _object.Target as DependencyObject;

                return obj != null && DesignerProperties.GetIsInDesignMode(obj);
            }
        }

        #endregion

        #region Fields

	    private readonly WeakReference _object;

	    private readonly int _hashCode;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalizedProperty"/> class.
		/// </summary>
		/// <param name="obj">The owner of the property.</param>
		/// <param name="property"></param>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="property"/> is <c>null</c>.</exception>
		protected LocalizedProperty(DependencyObject obj, object property)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

			_object = new WeakReference(obj);

            Property = property ?? throw new ArgumentNullException(nameof(property));

            _hashCode = obj.GetHashCode() ^ property.GetHashCode();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <returns>The value of the property.</returns>
        protected internal abstract object GetValue();

        /// <summary>
        /// Sets the value of the property.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected internal abstract void SetValue(object value);

        /// <summary>
        /// Gets the type of the value of the property.
        /// </summary>
        /// <returns>The type of the value of the property.</returns>
        protected internal abstract Type GetValueType();

        #endregion

        #region Attached properties

        /// <summary>
        /// Gets the <see cref="ResourceManager"/> set for the object.
        /// </summary>
        /// <returns>
        /// A <see cref="ResourceManager"/> or null if no explicit value is set for the object.
        /// </returns>
        public ResourceManager GetResourceManager()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            ResourceManager result;

            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetResourceManager(obj);
            }
            else
            {
                result = (ResourceManager)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetResourceManager((DependencyObject)x)), obj);
            }

	        return result ?? (result = LocalizationManager.DefaultResourceManager);
        }

        /// <summary>
        /// Gets the culture set for the object.
        /// </summary>
        /// <returns>
        /// A <see cref="CultureInfo"/> or null if no explicit value is set for the object.
        /// </returns>
        public CultureInfo GetCulture()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            CultureInfo result;

            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetCulture(obj);
            }
            else
            {
                result = (CultureInfo)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetCulture((DependencyObject)x)), obj);
            }

	        return result ?? (result = obj.Dispatcher.Thread.CurrentCulture);
        }

        /// <summary>
        /// Gets the UI culture set for the object.
        /// </summary>
        /// <returns>
        /// A <see cref="CultureInfo"/> or null if no explicit value is set for the object.
        /// </returns>
        public CultureInfo GetUICulture()
        {
            var obj = Object;

            if (obj == null)
            {
                return null;
            }

            CultureInfo result;

            if (obj.CheckAccess())
            {
                result = LocalizationScope.GetUICulture(obj);
            }
            else
            {
                result = (CultureInfo)obj.Dispatcher.Invoke(new DispatcherOperationCallback(x => LocalizationScope.GetUICulture((DependencyObject)x)), obj);
            }

	        return result ?? (result = obj.Dispatcher.Thread.CurrentUICulture);
        }

        #endregion

        #region Hash code & equals

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            var instance = obj as LocalizedDependencyProperty;

	        if (_hashCode != instance?._hashCode)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var targetObject = Object;

	        return targetObject != null && ReferenceEquals(targetObject, instance.Object) && ReferenceEquals(Property, instance.Property);
        }

        #endregion
    }
}
