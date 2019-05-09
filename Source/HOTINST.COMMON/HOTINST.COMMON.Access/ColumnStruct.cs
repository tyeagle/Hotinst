namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// 列描述结构体
    /// </summary>
    public struct ColumnStruct
    {
        //ParentCatalog
        /// <summary>
        /// 列名
        /// </summary>
        public string ColName;
        /// <summary>
        /// 列数据类型
        /// </summary>
        public AccessDataTypeEnum ColType;
        /// <summary>
        /// 列数据长度
        /// </summary>
        public int ColDefinedSize;
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool ColEnableEmpty;
        /// <summary>
        /// 列是否为自动增长
        /// </summary>
        public bool ColAutoIncrement;
        /// <summary>
        /// 列是否为主键
        /// </summary>
        public bool ColPriamryKey;
    }
}
