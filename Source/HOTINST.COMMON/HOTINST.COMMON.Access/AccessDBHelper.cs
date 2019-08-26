/****************************************************************
* 类 名 称：AccessDBHelper
* 命名空间：HOTINST.COMMON.Access
* 文 件 名：AccessDBHelper.cs
* 创建时间：2016-3-31
* 作    者：汪锋
* 说    明：动态创建Access的数据库及数据表，操作访问数据库。
* 待 补 充: ADOX还有UserClass和GroupClass可以用来管理数据库的用
*           户、组和权限，可以扩展该部分功能。
* 修改时间：
* 修 改 人：
*****************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using ADOX;

namespace HOTINST.COMMON.Access
{

    /// <summary>
    /// Access操作函数库
    /// </summary>
    public static class AccessDBHelper
    {
        #region Access数据库直接文件操作

        /// <summary>
        /// 创建一个Access数据库(mdb)文件
        /// </summary>
        /// <param name="FilePath">数据库文件名（含路径）</param>
        /// <param name="UserID">要创建的数据库登录用户名，可以省略</param>
        /// <param name="Password">要创建的数据库登录密码，可以省略</param>
        /// <returns>文件名错误、文件已存在或创建数据库出错时均返回false，创建成功返回true</returns>
        public static bool CreateDatabase(string FilePath, string UserID = "", string Password = "")
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                return false;
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                return false;
            if (File.Exists(FilePath))
                return false;

            try
            {
                Catalog objCatalog = new Catalog();
                objCatalog.Create(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5;User Id={1}; Password={2}", FilePath, UserID, Password));
                Marshal.ReleaseComObject(objCatalog);
                objCatalog = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 为指定Access数据库创建表
        /// </summary>
        /// <param name="DatabaseFilePath">数据库文件名（含路径）</param>
        /// <param name="TableName">表名</param>
        /// <param name="ColumnList">表中的列</param>
        /// <param name="UserID">数据库登录用户名，如果有必须提供，没有可以省略</param>
        /// <param name="Password">数据库登录密码，如果有必须提供，没有可以省略</param>
        /// <returns>数据库文件名错误、表名空、文件不存在、表已存在、用户名错误、密码错误或创建表出错时均返回false，创建成功返回true</returns>
        public static bool CreateTable(string DatabaseFilePath, string TableName, ref List<ColumnStruct> ColumnList, string UserID = "", string Password = "")
        {
            if (string.IsNullOrWhiteSpace(DatabaseFilePath) || string.IsNullOrWhiteSpace(TableName))
                return false;
            if (!File.Exists(DatabaseFilePath))
                return false;
            if (ColumnList.Count < 1)
                return false;

            //创建连接对象
            ADODB.Connection objConn = new ADODB.Connection();
            try
            {
                //打开连接
                objConn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabaseFilePath, UserID, Password, -1);
                //创建Catalog并设置活动链接
                Catalog objCatalog = new Catalog();
                objCatalog.ActiveConnection = objConn;
                //检查表是否已存在
                if (objCatalog.Tables.Count > 0)
                {
                    for (int cnt = 0; cnt < objCatalog.Tables.Count; cnt++)
                    {
                        if (objCatalog.Tables[cnt].Name == TableName)
                        {
                            objConn.Close();
                            return false;
                        }
                    }
                }
                //创建表对象并命名
                Table objTable = new Table();
                objTable.ParentCatalog = objCatalog;
                objTable.Name = TableName;
                //添加所有列
                for (int cnt = 0; cnt < ColumnList.Count; cnt++)
                {
                    //构建列
                    Column objColumn = new Column();
                    objColumn.ParentCatalog = objCatalog;
                    objColumn.Name = ColumnList[cnt].ColName;
                    objColumn.Type = AccessDataTypeToADOXDataType(ColumnList[cnt].ColType);
                    objColumn.DefinedSize = ColumnList[cnt].ColDefinedSize;
                    objColumn.Properties["Jet OLEDB:Allow Zero Length"].Value = ColumnList[cnt].ColEnableEmpty;
                    objColumn.Properties["AutoIncrement"].Value = ColumnList[cnt].ColAutoIncrement;
                    //添加列到表
                    objTable.Columns.Append(objColumn);
                    //添加表主键
                    if (ColumnList[cnt].ColPriamryKey)
                        objTable.Keys.Append(string.Format("{0}PrimaryKey{1}", objTable.Name, cnt.ToString().Trim()), KeyTypeEnum.adKeyPrimary, objColumn, null, null);
                }
                //添加表到Catalog
                objCatalog.Tables.Append(objTable);
                //关闭连接
                objConn.Close();
                //释放资源
                Marshal.ReleaseComObject(objTable);
                Marshal.ReleaseComObject(objConn);
                Marshal.ReleaseComObject(objCatalog);
                objTable = null;
                objConn = null;
                objCatalog = null;
                //GC.WaitForPendingFinalizers();
                //GC.Collect();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
            finally
            {
                if (objConn != null)
                {
                    if (objConn.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        objConn.Close();
                    objConn = null;
                }
            }
        }

        /// <summary>
        /// 判断指定的表是否存在
        /// </summary>
        /// <param name="IsExists">返回值，为true表示指定的表存在，false表示不存在</param>
        /// <param name="DatabaseFilePath">数据库文件名（含路径）</param>
        /// <param name="TableName">表名</param>
        /// <param name="UserID">数据库登录用户名，如果有必须提供，没有可以省略</param>
        /// <param name="Password">数据库登录密码，如果有必须提供，没有可以省略</param>
        /// <returns>数据库文件名错误、表名空、文件不存在、用户名错误、密码错误时均返回-1，正常执行完成返回0</returns>
        public static int TableExists(out bool IsExists, string DatabaseFilePath, string TableName, string UserID = "", string Password = "")
        {
            IsExists = false;

            if (string.IsNullOrWhiteSpace(DatabaseFilePath) || string.IsNullOrWhiteSpace(TableName))
                return -1;
            if (!File.Exists(DatabaseFilePath))
                return -1;

            //创建连接对象
            ADODB.Connection objConn = new ADODB.Connection();
            try
            {
                //打开连接
                objConn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DatabaseFilePath, UserID, Password, -1);
                //创建Catalog并设置活动链接
                Catalog objCatalog = new Catalog();
                objCatalog.ActiveConnection = objConn;
                //检查表是否已存在
                if (objCatalog.Tables.Count > 0)
                {
                    for (int cnt = 0; cnt < objCatalog.Tables.Count; cnt++)
                    {
                        if (objCatalog.Tables[cnt].Name == TableName)
                        {
                            IsExists = true;
                            break;
                        }
                    }
                }
                //关闭连接
                objConn.Close();
                //释放资源
                Marshal.ReleaseComObject(objConn);
                Marshal.ReleaseComObject(objCatalog);
                objConn = null;
                objCatalog = null;
                //GC.WaitForPendingFinalizers();
                //GC.Collect();

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return -1;
            }
            finally
            {
                if (objConn != null)
                {
                    if (objConn.State != (int)ADODB.ObjectStateEnum.adStateClosed)
                        objConn.Close();
                    objConn = null;
                }
            }
        }

        private static DataTypeEnum AccessDataTypeToADOXDataType(AccessDataTypeEnum enmADODataType)
        {
            switch(enmADODataType)
            {
                //case AccessDataTypeEnum.BigInt:
                //    return DataTypeEnum.adBigInt;
                //case AccessDataTypeEnum.Binary:
                //    return DataTypeEnum.adBinary;
                case AccessDataTypeEnum.Boolean:
                    return DataTypeEnum.adBoolean;
                //case AccessDataTypeEnum.BSTR:
                //    return DataTypeEnum.adBSTR;
                //case AccessDataTypeEnum.Chapter:
                //    return DataTypeEnum.adChapter;
                //case AccessDataTypeEnum.Char:
                //    return DataTypeEnum.adChar;
                case AccessDataTypeEnum.Currency:
                    return DataTypeEnum.adCurrency;
                case AccessDataTypeEnum.Date:
                    return DataTypeEnum.adDate;
                //case AccessDataTypeEnum.DBDate:
                //    return DataTypeEnum.adDBDate;
                //case AccessDataTypeEnum.DBTime:
                //    return DataTypeEnum.adDBTime;
                case AccessDataTypeEnum.DBTimeStamp:
                    return DataTypeEnum.adDBTimeStamp;
                //case AccessDataTypeEnum.Decimal:
                //    return DataTypeEnum.adDecimal;
                case AccessDataTypeEnum.Double:
                    return DataTypeEnum.adDouble;
                //case AccessDataTypeEnum.Empty:
                //    return DataTypeEnum.adEmpty;
                //case AccessDataTypeEnum.Error:
                //    return DataTypeEnum.adError;
                //case AccessDataTypeEnum.FileTime:
                //    return DataTypeEnum.adFileTime;
                case AccessDataTypeEnum.GUID:
                    return DataTypeEnum.adGUID;
                //case AccessDataTypeEnum.IDispatch:
                //    return DataTypeEnum.adIDispatch;
                case AccessDataTypeEnum.Integer:
                    return DataTypeEnum.adInteger;
                //case AccessDataTypeEnum.IUnknown:
                //    return DataTypeEnum.adIUnknown;
                case AccessDataTypeEnum.LongVarBinary:
                    return DataTypeEnum.adLongVarBinary;
                case AccessDataTypeEnum.LongVarChar:
                    return DataTypeEnum.adLongVarChar;
                case AccessDataTypeEnum.LongVarWChar:
                    return DataTypeEnum.adLongVarWChar;
                case AccessDataTypeEnum.Numeric:
                    return DataTypeEnum.adNumeric;
                //case AccessDataTypeEnum.PropVariant:
                //    return DataTypeEnum.adPropVariant;
                case AccessDataTypeEnum.Single:
                    return DataTypeEnum.adSingle;
                case AccessDataTypeEnum.SmallInt:
                    return DataTypeEnum.adSmallInt;
                //case AccessDataTypeEnum.TinyInt:
                //    return DataTypeEnum.adTinyInt;
                //case AccessDataTypeEnum.UnsignedBigInt:
                //    return DataTypeEnum.adUnsignedBigInt;
                //case AccessDataTypeEnum.UnsignedInt:
                //    return DataTypeEnum.adUnsignedInt;
                //case AccessDataTypeEnum.UnsignedSmallInt:
                //    return DataTypeEnum.adUnsignedSmallInt;
                case AccessDataTypeEnum.UnsignedTinyInt:
                    return DataTypeEnum.adUnsignedTinyInt;
                //case AccessDataTypeEnum.UserDefined:
                //    return DataTypeEnum.adUserDefined;
                case AccessDataTypeEnum.VarBinary:
                    return DataTypeEnum.adVarBinary;
                case AccessDataTypeEnum.VarChar:
                    return DataTypeEnum.adVarChar;
                //case AccessDataTypeEnum.Variant:
                //    return DataTypeEnum.adVariant;
                //case AccessDataTypeEnum.VarNumeric:
                //    return DataTypeEnum.adVarNumeric;
                case AccessDataTypeEnum.VarWChar:
                    return DataTypeEnum.adVarWChar;
                //case AccessDataTypeEnum.WChar:
                //    return DataTypeEnum.adWChar;
                default:
                    return DataTypeEnum.adError;
            }
        }

        #endregion

        #region Access数据库访问

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="FilePath">数据库文件名（含路径）</param>
        /// <param name="UserID">数据库登录用户名，如果有必须提供，没有可以省略</param>
        /// <param name="Password">数据库登录密码，如果有必须提供，没有可以省略</param>
        /// <returns>成功打开时返回OleDbConnection对象，打开失败时返回'null'</returns>
        public static OleDbConnection OpenDatabase(string FilePath, string UserID = "", string Password = "")
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                return null;
            if (!File.Exists(FilePath))
                return null;

            string sConnString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0}; User Id={1}; Password={2}", FilePath, UserID, Password);
            try
            {
                OleDbConnection objConn = new OleDbConnection() { ConnectionString = sConnString };
                objConn.Open();
                return objConn;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="Connection">数据库连接对象</param>
        /// <returns>成功关闭返回true，关闭出错返回false</returns>
        public static bool CloseDatabase(ref OleDbConnection Connection)
        {
            if (Connection == null)
                return true;
            if (Connection.State == ConnectionState.Closed)
                return true;

            try
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 执行SQL查询（可分页获取数据）
        /// </summary>
        /// <param name="Connection">数据库连接对象</param>
        /// <param name="SqlString">SQL查询语句</param>
        /// <param name="TableName">需要分页获取数据时，用于表映射的源表名称。若为空，
        /// 则忽略TableName、StartIndex、PageSize参数，返回SQL语句执行后完整的DataSet</param>
        /// <param name="StartIndex">需要分页获取数据时，起始记录号</param>
        /// <param name="PageSize">需要分页获取数据时，最大记录号。若为0，则忽略
        /// TableName、StartIndex、PageSize参数，返回SQL语句执行后完整的DataSet</param>
        /// <returns>执行成功返回DataSet对象，执行出错返回'null'</returns>
        public static DataSet ExecuteSqlQuery(ref OleDbConnection Connection, string SqlString, string TableName = null, int StartIndex = 0, int PageSize = 0)
        {
            if (Connection == null)
                return null;
            if (string.IsNullOrWhiteSpace(SqlString))
                return null;

            try
            {
                OleDbConnection objConn = Connection;
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = SqlString;
                DataSet objDS = new DataSet();
                OleDbDataAdapter objAdp = new OleDbDataAdapter();
                objAdp.SelectCommand = objCmd;
                if (string.IsNullOrWhiteSpace(TableName) || PageSize == 0)
                    objAdp.Fill(objDS);
                else
                    objAdp.Fill(objDS, StartIndex, PageSize, TableName);

                return objDS;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 执行SQL操作
        /// </summary>
        /// <param name="Connection">数据库连接对象</param>
        /// <param name="SqlString">SQL操作语句</param>
        /// <returns>执行成功返回受影响行数，执行出错返回-1</returns>
        public static int ExecuteSqlNoneQuery(ref OleDbConnection Connection, string SqlString)
        {
            if (Connection == null)
                return -1;
            if (string.IsNullOrWhiteSpace(SqlString))
                return -1;

            try
            {
                OleDbConnection objConn = Connection;
                OleDbCommand objCmd = new OleDbCommand() { Connection = objConn, CommandText = SqlString };
                return objCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 执行SQL操作（带事务机制）
        /// </summary>
        /// <param name="Connection">数据库连接对象</param>
        /// <param name="SqlString">SQL操作语句</param>
        /// <returns>执行成功返回受影响行数，执行出错返回-1</returns>
        public static int ExecuteSqlNoneQueryEx(ref OleDbConnection Connection,string SqlString)
        {
            if (Connection == null)
                return -1;
            if (string.IsNullOrWhiteSpace(SqlString))
                return -1;

            OleDbConnection objConn = Connection;
            OleDbTransaction objTrans = objConn.BeginTransaction();
            try
            {
                OleDbCommand objCmd = new OleDbCommand() { Connection = objConn, CommandText = SqlString, Transaction = objTrans };
                int iRet = objCmd.ExecuteNonQuery();
                objTrans.Commit();
                return iRet;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                System.Diagnostics.Debug.Print(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 执行多项SQL操作（带事务机制）
        /// </summary>
        /// <param name="Connection">数据库连接对象</param>
        /// <param name="SqlStringList">SQL操作语句列表</param>
        /// <param name="EffectLines">返回值，每项操作所受影响的行数</param>
        /// <returns>执行成功返回0，执行出错返回-1</returns>
        public static int ExecuteSqlNoneQueryEx(ref OleDbConnection Connection, List<string> SqlStringList, out int[] EffectLines)
        {
            EffectLines = null;

            if (Connection == null)
                return -1;
            if (SqlStringList.Count < 1)
                return -1;
            for (int cnt = 0; cnt < SqlStringList.Count; cnt++)
            {
                if (string.IsNullOrWhiteSpace(SqlStringList[cnt]))
                    return -1;
            }

            OleDbConnection objConn = Connection;
            OleDbTransaction objTrans = objConn.BeginTransaction();
            try
            {
                int[] aryEffectLines = new int[SqlStringList.Count];
                for (int cnt = 0; cnt < SqlStringList.Count; cnt++)
                {
                    OleDbCommand objCmd = new OleDbCommand() { Connection = objConn, CommandText = SqlStringList[cnt], Transaction = objTrans };
                    aryEffectLines[cnt] = objCmd.ExecuteNonQuery();
                }
                objTrans.Commit();
                EffectLines = aryEffectLines;

                return 0;
            }
            catch (Exception ex)
            {
                objTrans.Rollback();
                System.Diagnostics.Debug.Print(ex.Message);
                return -1;
            }
        }

        #endregion

    }
}
