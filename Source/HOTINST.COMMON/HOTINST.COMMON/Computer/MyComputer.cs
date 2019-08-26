/****************************************************************
 * 类 名 称：MyComputer
 * 命名空间：HOTINST.COMMON.Computer
 * 文 件 名：MyComputer.cs
 * 创建时间：2016-5-4
 * 作    者：汪锋
 * 说    明：用于获取计算机的状态及执行操作（可按需扩展）
 * 修改历史：
 *          
 *****************************************************************/
using System;

namespace HOTINST.COMMON.Computer
{
    /// <summary>
    /// 获取计算机状态及执行操作
    /// </summary>
    public static partial class MyComputer
    {
        /// <summary>
        /// 获取计算机的名称
        /// </summary>
        /// <returns>返回计算机名</returns>
        public static string MachineName()
        {
            return Environment.MachineName;
        }
    }
}
