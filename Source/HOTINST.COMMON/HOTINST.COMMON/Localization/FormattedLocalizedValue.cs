using System;

namespace HOTINST.COMMON.Localization
{
    /// <summary>
    /// Formats a list of objects to produce a string value.
    /// </summary>
    public class FormattedLocalizedValue : LocalizedValue
    {
	    private readonly string _formatString;

	    private readonly object[] _args;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedLocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="formatString">The format string.</param>
        /// <param name="args">The args.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="formatString"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="args"/> is null.</exception>
        public FormattedLocalizedValue(LocalizedProperty property, string formatString, params object[] args)
            : base(property)
        {
			_formatString = formatString ?? throw new ArgumentNullException(nameof(formatString));
            _args = args ?? throw new ArgumentNullException(nameof(args));
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

            return string.Format(culture, _formatString, _args);
        }
    }
}
