namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// �������ṹ��
    /// </summary>
    public struct ColumnStruct
    {
        //ParentCatalog
        /// <summary>
        /// ����
        /// </summary>
        public string ColName;
        /// <summary>
        /// ����������
        /// </summary>
        public AccessDataTypeEnum ColType;
        /// <summary>
        /// �����ݳ���
        /// </summary>
        public int ColDefinedSize;
        /// <summary>
        /// �Ƿ�����Ϊ��
        /// </summary>
        public bool ColEnableEmpty;
        /// <summary>
        /// ���Ƿ�Ϊ�Զ�����
        /// </summary>
        public bool ColAutoIncrement;
        /// <summary>
        /// ���Ƿ�Ϊ����
        /// </summary>
        public bool ColPriamryKey;
    }
}
