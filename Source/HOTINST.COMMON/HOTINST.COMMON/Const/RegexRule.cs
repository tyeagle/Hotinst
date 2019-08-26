﻿namespace HOTINST.COMMON.Const
{
	public static class RegexRule
	{
		/// <summary>
		/// IP地址的正则验证表达式
		/// </summary>
		public const string IP = @"^(?:(?:(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d))))$";

		/// <summary>
		/// 端口号的正则验证表达式, 0 ~ 65535
		/// </summary>
		public const string Port = @"^([0-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]{1}|6553[0-5])$";

		/// <summary>
		/// IP地址加端口号的正则验证表达式, 0.0.0.0:0, 分组名: ip, port
		/// </summary>
		public const string IPColonPort = @"^(?<ip>(?:(?:(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d))))):(?<port>[0-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]{1}|6553[0-5])$";
	}
}