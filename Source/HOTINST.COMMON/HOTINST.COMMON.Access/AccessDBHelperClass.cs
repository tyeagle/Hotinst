/****************************************************************
* 类 名 称：AccessDBHelperClass
* 命名空间：HOTINST.COMMON.Access
* 文 件 名：AccessDBHelperClass.cs
* 创建时间：2016-3-31
* 作    者：汪锋
* 说    明：对AccessDBHelper的类形式封装，更方便使用。
* 待 补 充: 
* 修改时间：
* 修 改 人：
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// Access操作类（AccessDBHelper的类封装形式）
    /// </summary>
    public class AccessDBHelperClass : IDisposable
    {
        #region 私有成员

        private OleDbConnection objDbConnection;
        private string m_AccessFile;
        private string m_User;
        private string m_Password;

        #endregion

        #region 构造及释放函数

        /// <summary>
        /// 释放引用对象的资源
        /// </summary>
        public void Dispose()
        {
            if (objDbConnection != null)
            {
                objDbConnection.Dispose();
                objDbConnection = null;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccessDBHelperClass()
        {
            objDbConnection = null;
            m_AccessFile = "";
            m_User = "";
            m_Password = "";
        }

        #endregion

        #region 类属性

        /// <summary>
        /// [必须]Access数据库文件（含路径）
        /// </summary>
        public string AccessFile
        {
            get { return m_AccessFile; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    m_AccessFile = "";
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(value)))
                        m_AccessFile = "";
                }
                catch (Exception ex)
                {
                    m_AccessFile = "";
                    System.Diagnostics.Debug.Print(ex.Message);
                }
                m_AccessFile = value;
            }
        }

        /// <summary>
        /// [可选]Access数据库用户（默认为空）
        /// </summary>
        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        /// <summary>
        /// [可选]Access数据库用户密码（默认为空）
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// [只读]Access数据库当前打开状态
        /// </summary>
        public bool IsOpen
        {
            get
            {
                if (objDbConnection != null && objDbConnection.State != ConnectionState.Closed)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// [只读]连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (objDbConnection != null)
                    return objDbConnection.ConnectionString;
                else
                    return string.Empty;
            }
        }

        #endregion

        #region 类方法

        /// <summary>
        /// 创建Access数据库
        /// </summary>
        /// <returns>属性AccessFile为空或错误、数据库已存在或创建数据库出错时均返回false，创建成功返回true</returns>
        public bool CreateDatabase()
        {
            return AccessDBHelper.CreateDatabase(m_AccessFile, m_User, m_Password);
        }

        /// <summary>
        /// 为Access数据库创建表
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <param name="FieldList">字段列表</param>
        /// <returns>属性AccessFile为空或错误，数据库不存在、表名称为空、表已存在、登录失败或创建表出错时均返回false，创建成功返回true</returns>
        public bool CreateTable(string TableName, ref List<ColumnStruct> FieldList)
        {
            return AccessDBHelper.CreateTable(m_AccessFile, TableName, ref FieldList, m_User, m_Password);
        }

        /// <summary>
        /// Access数据库中是否存在指定表
        /// </summary>
        /// <param name="IsExists">返回值，表已经存在返回true，表不存在返回false</param>
        /// <param name="TableName">表名称</param>
        /// <returns>属性AccessFile为空或错误、表名称为空、数据库不存在、登录失败时均返回-1，正常执行完成返回0</returns>
        public int TableExists(out bool IsExists, string TableName)
        {
            return AccessDBHelper.TableExists(out IsExists, m_AccessFile, TableName, m_User, m_Password);
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <returns>成功打开时返回true,失败返回false</returns>
        public bool OpenDatabase()
        {
            objDbConnection = AccessDBHelper.OpenDatabase(m_AccessFile, m_User, m_Password);
            if (objDbConnection != null && objDbConnection.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <returns>成功关闭返回true，关闭出错返回false</returns>
        public bool CloseDatabase()
        {
            return AccessDBHelper.CloseDatabase(ref objDbConnection);
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="SqlString">SQL查询语句</param>
        /// <returns>执行成功返回DataSet对象，执行出错返回'null'</returns>
        public DataSet ExecuteSqlQuery(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlQuery(ref objDbConnection, SqlString, null, 0, 0);
        }

        /// <summary>
        /// 执行SQL查询（获取指定记录号范围数据分页用）
        /// </summary>
        /// <param name="SqlString">SQL查询语句</param>
        /// <param name="TableName">用于表映射的源表名称</param>
        /// <param name="StartIndex">起始记录号</param>
        /// <param name="PageSize">最大记录号</param>
        /// <returns>执行成功返回DataSet对象，执行出错返回'null'</returns>
        public DataSet ExecuteSqlQuery(string SqlString, string TableName, int StartIndex, int PageSize)
        {
            if (string.IsNullOrWhiteSpace(TableName) || PageSize == 0)
                return null;

            return AccessDBHelper.ExecuteSqlQuery(ref objDbConnection, SqlString, TableName, StartIndex, PageSize);
        }

        /// <summary>
        /// 执行SQL操作
        /// </summary>
        /// <param name="SqlString">SQL操作语句</param>
        /// <returns>执行成功返回受影响行数，执行出错返回-1</returns>
        public int ExecuteSqlNoneQuery(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlNoneQuery(ref objDbConnection, SqlString);
        }

        /// <summary>
        /// 执行SQL操作（带事务机制）
        /// </summary>
        /// <param name="SqlString">SQL操作语句</param>
        /// <returns>执行成功返回受影响行数，执行出错返回-1</returns>
        public int ExecuteSqlNoneQueryEx(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlNoneQueryEx(ref objDbConnection, SqlString);
        }

        /// <summary>
        /// 执行多项SQL操作（带事务机制）
        /// </summary>
        /// <param name="SqlStringList">SQL操作语句列表</param>
        /// <param name="EffectLines">返回值，每项操作所受影响的行数</param>
        /// <returns>执行成功返回0，执行出错返回-1</returns>
        public int ExecuteSqlNoneQueryEx(List<string> SqlStringList, out int[] EffectLines)
        {
            return AccessDBHelper.ExecuteSqlNoneQueryEx(ref objDbConnection, SqlStringList, out EffectLines);
        }

        #endregion

    }
}
