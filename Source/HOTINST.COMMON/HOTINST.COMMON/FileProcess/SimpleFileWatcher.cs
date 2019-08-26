/****************************************************************
* 类 名 称：SimpleFileWatcher
* 命名空间：HOTINST.COMMON.FileProcess
* 文 件 名：SimpleFileWatcher.cs
* 创建时间：2016-4-11
* 作    者：汪锋
* 说    明：简易文件监控类。该监控类只对指定的单个文件进行监控，
*           当所监控的文件写入时间发生改变时触发事件。
* 修改时间：
* 修 改 人：
*****************************************************************/

using System;
using System.IO;
using System.Threading;

namespace HOTINST.COMMON.FileProcess
{
    /// <summary>
    /// 简易文件监控类
    /// </summary>
    public sealed class SimpleFileWatcher : IDisposable
    {
        private string m_FileFullPath;
        private bool m_IsMonitoring;
        private int m_TimeoutMillis;
        private FileSystemWatcher objFileSystemWatcher;
        private System.Threading.Timer objTimer;
        /// <summary>
        /// 写操作触发事件
        /// </summary>
        public event FileSystemEventHandler LastWriteChanged;

        /// <summary>
        /// 释放引用对象资源
        /// </summary>
        public void Dispose()
        {
            if (objFileSystemWatcher != null)
            {
                objFileSystemWatcher.Dispose();
                objFileSystemWatcher = null;
            }
            if (objTimer != null)
            {
                objTimer.Dispose();
                objTimer = null;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleFileWatcher()
        {
            m_FileFullPath = string.Empty;
            m_IsMonitoring = false;
            m_TimeoutMillis = 500;
            objFileSystemWatcher = null;
            objTimer = null;
            LastWriteChanged = null;
        }

        /// <summary>
        /// 要监控的文件全路径（含文件名）
        /// </summary>
        /// <value>不能为空，指定的文件必须存在</value>
        public string FileFullPath
        {
            get { return m_FileFullPath; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    m_FileFullPath = string.Empty;
                if (!File.Exists(value))
                    m_FileFullPath = string.Empty;
                m_FileFullPath = value;
            }
        }

        /// <summary>
        /// 当前是否处于监控状态中
        /// </summary>
        /// <value>监控状态为true，未监控为false</value>
        public bool IsMonitoring
        {
            get { return m_IsMonitoring; }
        }

        /// <summary>
        /// 用于防止文件一次改变操作却连续触发多次事件的微调延时（单位：毫秒）
        /// </summary>
        /// <value>默认值为500毫秒，仅在必要时调整该值</value>
        public int TimeoutMillis
        {
            set { m_TimeoutMillis = value; }
            get { return m_TimeoutMillis; }
        }

        /// <summary>
        /// 开始监控文件
        /// </summary>
        /// <returns>成功启动监控返回true，否则返回false</returns>
        public bool StartMonitoring()
        {
            if (m_IsMonitoring)
                return true;
            if (string.IsNullOrWhiteSpace(m_FileFullPath))
                return false;

            if (objFileSystemWatcher == null)
                objFileSystemWatcher = new FileSystemWatcher();
            if (objTimer == null)
                objTimer = new System.Threading.Timer(new TimerCallback(OnFileChange), null, Timeout.Infinite, Timeout.Infinite);
            try
            {
                objFileSystemWatcher.Path = (Path.GetDirectoryName(m_FileFullPath));
                objFileSystemWatcher.IncludeSubdirectories = false;
                objFileSystemWatcher.Filter = (Path.GetFileName(m_FileFullPath));
                objFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                objFileSystemWatcher.Changed += objFileSystemWatcher_Changed;
                objFileSystemWatcher.EnableRaisingEvents = true;

                m_IsMonitoring = true;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }

        private void objFileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //重新设置定时器的触发间隔，并且仅仅触发一次
            objTimer.Change(m_TimeoutMillis, Timeout.Infinite);
        }

        private void OnFileChange(object state)
        {
            if (LastWriteChanged != null)
                LastWriteChanged(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, Path.GetDirectoryName(m_FileFullPath), Path.GetFileName(m_FileFullPath)));
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopMonitoring()
        {
            if (m_IsMonitoring)
            {
                objFileSystemWatcher.EnableRaisingEvents = false;
                objFileSystemWatcher.Changed -= objFileSystemWatcher_Changed;
                objFileSystemWatcher.Dispose();
                objFileSystemWatcher = null;
                m_IsMonitoring = false;
            }
        }

    }
}
