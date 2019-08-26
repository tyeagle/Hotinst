/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	FormatCheck
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/12 9:59:29
*	文件描述:  使用正则表达式判断字符串的格式是否满足数据要求：身份证、电话、邮件
*
***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HOTINST.COMMON.Data
{
    public class StringFormat
    {
        /// <summary>
        /// 判断是否为十六进制字符串，以0x打头(不是必须)
        /// 其余字符都是数字或a-f,A-F组成的字符串
        /// </summary>
        /// <param name="value">待判断的字符串</param>
        /// <returns>是十六制字符串返回true,不是返回false</returns>
        public static bool IsHexString(string value)
        {
            Regex objRegex = new Regex(@"^[\da-fA-F]+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为二进制字符串，所有字符只能为[0,1]两种构成的字符串
        /// </summary>
        /// <param name="value">待判断的字符串</param>
        /// <returns>是二进制字符串返回true,不是返回false</returns>
        public static bool IsBinString(string value)
        {
            Regex objRegex = new Regex(@"^[01]+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为数值
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是数值返回true，不是数值返回false</returns>
        public static bool IsNumeric(string value)
        {
            Regex objRegex = new Regex(@"^[+-]?\d*[.]?\d+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为无符号数值
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是无符号数值返回true，不是返回false</returns>
        public static bool IsUnsignNumeric(string value)
        {
            Regex objRegex = new Regex(@"^\d*[.]?\d+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为整型数值
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是整型数值返回true，不是返回false</returns>
        public static bool IsIntNumeric(string value)
        {
            Regex objRegex = new Regex(@"^[+-]?\d+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为无符号整型数值
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是无符号整型数值返回true，不是返回false</returns>
        public static bool IsUnsignIntNumeric(string value)
        {
            Regex objRegex = new Regex(@"^\d+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为日期时间字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是日期时间字符串返回true，不是返回false</returns>
        public static bool IsDateTimeString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            DateTime dt;
            if (DateTime.TryParse(value, out dt))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断是否为身份证字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是身份证字符串返回true，不是返回false</returns>
        public static bool IsPersonalIDCardString(string value)
        {
            Regex objRegex = new Regex(@"(^\d{15}$)|(^\d{17}([0-9]|X)$)");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为Email格式字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是Email格式字符串返回true，不是返回false</returns>
        public static bool IsEmailString(string value)
        {
            Regex objRegex = new Regex(@"^([\w\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为手机号码
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是手机号码格式字符串返回true，不是返回false</returns>
        public static bool IsMobilePhoneString(string value)
        {
            Regex objRegex = new Regex(@"^((\(\d{3}\))|(\d{3}\-))?(1[358]\d{9})$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为座机号码
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是座机号码格式字符串返回true，不是返回false</returns>
        public static bool IsFixedTelephoneString(string value)
        {
            Regex objRegex = new Regex(@"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为中国邮政编码
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是中国邮政编码格式字符串返回true，不是返回false</returns>
        public static bool IsChinaPostCode(string value)
        {
            Regex objRegex = new Regex(@"^[1-9]\d{5}(?!\d)$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为IP地址字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是IP地址字符串返回true，不是返回false</returns>
        public static bool IsIPAddressString(string value)
        {
            Regex objRegex = new Regex(@"^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为组织机构代码字字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是组织机构代码字符串返回true，不是返回false</returns>
        public static bool IsOrganizationCodeString(string value)
        {
            Regex objRegex = new Regex(@"^[A-Za-z0-9]{8}\-[A-Za-z0-9]{1}$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为字母和数字组合的字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是字母和数字组合的字符串返回true，不是返回false</returns>
        public static bool IsLetterAndNumberString(string value)
        {
            Regex objRegex = new Regex(@"^[A-Za-z0-9]+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为字母和汉字组合的字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是字母和汉字组合的字符串返回true，不是返回false</returns>
        public static bool IsLetterAndChineseString(string value)
        {
            Regex objRegex = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
            return objRegex.IsMatch(value);
        }

        /// <summary>
        /// 判断是否为字母、数字和汉字组合的字符串
        /// </summary>
        /// <param name="value">待判断字符串</param>
        /// <returns>是字母、数字和汉字组合的字符串返回true，不是返回false</returns>
        public static bool IsLetterNumberAndChineseString(string value)
        {
            Regex objRegex = new Regex(@"^[A-Za-z0-9\u4e00-\u9fa5]+$");
            return objRegex.IsMatch(value);
        }
    }
}
