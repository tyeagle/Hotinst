/**
 * ==============================================================================
 *
 * ClassName: ConversionExtensions
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:28:57
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Globalization;

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 
	/// </summary>
	public static class ConversionExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static long? ConvertToInt64Null(this string text)
		{
			long value;
			return long.TryParse(text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value) ? new long?(value) : null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="culture"></param>
		/// <param name="numberformat"></param>
		/// <returns></returns>
		public static double? ConvertToDoubleNull(this string text, CultureInfo culture, NumberFormatInfo numberformat)
		{
			double? result;
			if(numberformat != null && culture != null)
			{
				double value;
				result = (double.TryParse(text, NumberStyles.Any, numberformat, out value) ? new double?(value) : null);
			}
			else
			{
				NumberFormatInfo provider = (culture == null) ? NumberFormatInfo.InvariantInfo : culture.NumberFormat;
				double value;
				result = (double.TryParse(text, NumberStyles.Any, provider, out value) ? new double?(value) : null);
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static decimal? ConvertToDecimalNull(this string text)
		{
			decimal value;
			return decimal.TryParse(text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out value) ? new decimal?(value) : null;
		}
	}
}