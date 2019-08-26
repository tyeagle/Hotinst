using System;

namespace HOTINST.COMMON.Localization
{
    /// <summary>
    /// Invaokes a method to obtain a localized value.
    /// </summary>
    public class MethodLocalizedValue : LocalizedValue
    {
	    private readonly LocalizationCallback _method;

	    private readonly object _parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodLocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameter">The parameter to pass to the method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is null.</exception>
        public MethodLocalizedValue(LocalizedProperty property, LocalizationCallback method, object parameter)
            : base(property)
        {
			_method = method ?? throw new ArgumentNullException("method");

            _parameter = parameter;
        }

        /// <summary>
        /// Retrieves the localized value from resources or by other means.
        /// </summary>
        /// <returns>
        /// The localized value.
        /// </returns>
        protected override object GetLocalizedValue()
        {
            var culture = Property.GetCulture();

            var uiCulture = Property.GetUICulture();

            return _method(culture, uiCulture, _parameter);
        }
    }
}
