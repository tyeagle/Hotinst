using System;

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Dockablz
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationReportException : Exception
    {
		/// <summary>
		/// 
		/// </summary>
        public LocationReportException()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
        public LocationReportException(string message) : base(message)
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
        public LocationReportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}