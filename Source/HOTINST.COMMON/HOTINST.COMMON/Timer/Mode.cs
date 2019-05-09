/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	Mode
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
using System;
namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// 只运行一次
        /// </summary>
        OnceOnly = 0,
        /// <summary>
        /// 反复执行
        /// </summary>
        Repeats = 1
    }
}
