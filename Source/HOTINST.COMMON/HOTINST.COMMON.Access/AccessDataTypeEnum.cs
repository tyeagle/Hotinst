namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// ADO��������
    /// </summary>
    public enum AccessDataTypeEnum
    {
        //BigInt,
        //Binary,
        /// <summary>
        /// ������
        /// </summary>
        Boolean,        //��Ӧ��YesNo
        //BSTR,
        //Chapter,
        //Char,
        /// <summary>
        /// ������
        /// </summary>
        Currency,       //��Ӧ��Currency
        /// <summary>
        /// ������
        /// </summary>
        Date,           //��Ӧ��Date
        //DBDate,
        //DBTime,
        /// <summary>
        /// ʱ���
        /// </summary>
        DBTimeStamp,    //��Ӧ��DateTime (Access 97 (ODBC))
        //Decimal,
        /// <summary>
        /// ˫���ȸ�����
        /// </summary>
        Double,         //��Ӧ��Double
        //Empty,
        //Error,
        //FileTime,
        /// <summary>
        /// ΨһGUID
        /// </summary>
        GUID,           //��Ӧ��ReplicationID (Access 97 (OLEDB)), (Access 2000 (OLEDB))
        //IDispatch,
        /// <summary>
        /// ����
        /// </summary>
        Integer,        //��Ӧ��AutoNumber,Integer,Long
        //IUnknown,
        /// <summary>
        /// �ɱ䳤����������
        /// </summary>
        LongVarBinary,  //��Ӧ��OLEObject
        /// <summary>
        /// �ɱ䳤���ַ���
        /// </summary>
        LongVarChar,    //��Ӧ��Memo (Access 97),Hyperlink (Access 97)
        /// <summary>
        /// �ɱ䳤���ַ��ͣ�Unicode��
        /// </summary>
        LongVarWChar,   //��Ӧ��Memo (Access 2000 (OLEDB)),Hyperlink (Access 2000 (OLEDB))
        /// <summary>
        /// ����
        /// </summary>
        Numeric,        //��Ӧ��Decimal (Access 2000 (OLEDB))
        //PropVariant,
        /// <summary>
        /// �����ȸ�����
        /// </summary>
        Single,         //��Ӧ��Single
        /// <summary>
        /// ������
        /// </summary>
        SmallInt,       //��Ӧ��Integer
        //TinyInt,
        //UnsignedBigInt,
        //UnsignedInt,
        //UnsignedSmallInt,
        /// <summary>
        /// �޷���΢����
        /// </summary>
        UnsignedTinyInt,//��Ӧ��Byte
        //UserDefined,
        /// <summary>
        /// �ɱ䳤��������
        /// </summary>
        VarBinary,      //��Ӧ��ReplicationID (Access 97)
        /// <summary>
        /// �ɱ䳤�ַ���
        /// </summary>
        VarChar,        //��Ӧ��Text (Access 97)
        //Variant,
        //VarNumeric,
        /// <summary>
        /// �ɱ䳤�ַ��ͣ�Unicode��
        /// </summary>
        VarWChar,       //��Ӧ��Text (Access 2000 (OLEDB))
        //WChar
    }
}
