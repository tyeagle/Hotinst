/****************************************************************
* �� �� �ƣ�AccessDBHelperClass
* �����ռ䣺HOTINST.COMMON.Access
* �� �� ����AccessDBHelperClass.cs
* ����ʱ�䣺2016-3-31
* ��    �ߣ�����
* ˵    ������AccessDBHelper������ʽ��װ��������ʹ�á�
* �� �� ��: 
* �޸�ʱ�䣺
* �� �� �ˣ�
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// Access�����ࣨAccessDBHelper�����װ��ʽ��
    /// </summary>
    public class AccessDBHelperClass : IDisposable
    {
        #region ˽�г�Ա

        private OleDbConnection objDbConnection;
        private string m_AccessFile;
        private string m_User;
        private string m_Password;

        #endregion

        #region ���켰�ͷź���

        /// <summary>
        /// �ͷ����ö������Դ
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
        /// ���캯��
        /// </summary>
        public AccessDBHelperClass()
        {
            objDbConnection = null;
            m_AccessFile = "";
            m_User = "";
            m_Password = "";
        }

        #endregion

        #region ������

        /// <summary>
        /// [����]Access���ݿ��ļ�����·����
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
        /// [��ѡ]Access���ݿ��û���Ĭ��Ϊ�գ�
        /// </summary>
        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        /// <summary>
        /// [��ѡ]Access���ݿ��û����루Ĭ��Ϊ�գ�
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// [ֻ��]Access���ݿ⵱ǰ��״̬
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
        /// [ֻ��]�����ַ���
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

        #region �෽��

        /// <summary>
        /// ����Access���ݿ�
        /// </summary>
        /// <returns>����AccessFileΪ�ջ�������ݿ��Ѵ��ڻ򴴽����ݿ����ʱ������false�������ɹ�����true</returns>
        public bool CreateDatabase()
        {
            return AccessDBHelper.CreateDatabase(m_AccessFile, m_User, m_Password);
        }

        /// <summary>
        /// ΪAccess���ݿⴴ����
        /// </summary>
        /// <param name="TableName">������</param>
        /// <param name="FieldList">�ֶ��б�</param>
        /// <returns>����AccessFileΪ�ջ�������ݿⲻ���ڡ�������Ϊ�ա����Ѵ��ڡ���¼ʧ�ܻ򴴽������ʱ������false�������ɹ�����true</returns>
        public bool CreateTable(string TableName, ref List<ColumnStruct> FieldList)
        {
            return AccessDBHelper.CreateTable(m_AccessFile, TableName, ref FieldList, m_User, m_Password);
        }

        /// <summary>
        /// Access���ݿ����Ƿ����ָ����
        /// </summary>
        /// <param name="IsExists">����ֵ�����Ѿ����ڷ���true�������ڷ���false</param>
        /// <param name="TableName">������</param>
        /// <returns>����AccessFileΪ�ջ���󡢱�����Ϊ�ա����ݿⲻ���ڡ���¼ʧ��ʱ������-1������ִ����ɷ���0</returns>
        public int TableExists(out bool IsExists, string TableName)
        {
            return AccessDBHelper.TableExists(out IsExists, m_AccessFile, TableName, m_User, m_Password);
        }

        /// <summary>
        /// �����ݿ�
        /// </summary>
        /// <returns>�ɹ���ʱ����true,ʧ�ܷ���false</returns>
        public bool OpenDatabase()
        {
            objDbConnection = AccessDBHelper.OpenDatabase(m_AccessFile, m_User, m_Password);
            if (objDbConnection != null && objDbConnection.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        /// <returns>�ɹ��رշ���true���رճ�����false</returns>
        public bool CloseDatabase()
        {
            return AccessDBHelper.CloseDatabase(ref objDbConnection);
        }

        /// <summary>
        /// ִ��SQL��ѯ
        /// </summary>
        /// <param name="SqlString">SQL��ѯ���</param>
        /// <returns>ִ�гɹ�����DataSet����ִ�г�����'null'</returns>
        public DataSet ExecuteSqlQuery(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlQuery(ref objDbConnection, SqlString, null, 0, 0);
        }

        /// <summary>
        /// ִ��SQL��ѯ����ȡָ����¼�ŷ�Χ���ݷ�ҳ�ã�
        /// </summary>
        /// <param name="SqlString">SQL��ѯ���</param>
        /// <param name="TableName">���ڱ�ӳ���Դ������</param>
        /// <param name="StartIndex">��ʼ��¼��</param>
        /// <param name="PageSize">����¼��</param>
        /// <returns>ִ�гɹ�����DataSet����ִ�г�����'null'</returns>
        public DataSet ExecuteSqlQuery(string SqlString, string TableName, int StartIndex, int PageSize)
        {
            if (string.IsNullOrWhiteSpace(TableName) || PageSize == 0)
                return null;

            return AccessDBHelper.ExecuteSqlQuery(ref objDbConnection, SqlString, TableName, StartIndex, PageSize);
        }

        /// <summary>
        /// ִ��SQL����
        /// </summary>
        /// <param name="SqlString">SQL�������</param>
        /// <returns>ִ�гɹ�������Ӱ��������ִ�г�����-1</returns>
        public int ExecuteSqlNoneQuery(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlNoneQuery(ref objDbConnection, SqlString);
        }

        /// <summary>
        /// ִ��SQL��������������ƣ�
        /// </summary>
        /// <param name="SqlString">SQL�������</param>
        /// <returns>ִ�гɹ�������Ӱ��������ִ�г�����-1</returns>
        public int ExecuteSqlNoneQueryEx(string SqlString)
        {
            return AccessDBHelper.ExecuteSqlNoneQueryEx(ref objDbConnection, SqlString);
        }

        /// <summary>
        /// ִ�ж���SQL��������������ƣ�
        /// </summary>
        /// <param name="SqlStringList">SQL��������б�</param>
        /// <param name="EffectLines">����ֵ��ÿ���������Ӱ�������</param>
        /// <returns>ִ�гɹ�����0��ִ�г�����-1</returns>
        public int ExecuteSqlNoneQueryEx(List<string> SqlStringList, out int[] EffectLines)
        {
            return AccessDBHelper.ExecuteSqlNoneQueryEx(ref objDbConnection, SqlStringList, out EffectLines);
        }

        #endregion

    }
}
