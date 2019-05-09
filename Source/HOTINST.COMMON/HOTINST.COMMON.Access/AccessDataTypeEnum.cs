namespace HOTINST.COMMON.Access
{
    /// <summary>
    /// ADO数据类型
    /// </summary>
    public enum AccessDataTypeEnum
    {
        //BigInt,
        //Binary,
        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean,        //对应：YesNo
        //BSTR,
        //Chapter,
        //Char,
        /// <summary>
        /// 货币型
        /// </summary>
        Currency,       //对应：Currency
        /// <summary>
        /// 日期型
        /// </summary>
        Date,           //对应：Date
        //DBDate,
        //DBTime,
        /// <summary>
        /// 时间戳
        /// </summary>
        DBTimeStamp,    //对应：DateTime (Access 97 (ODBC))
        //Decimal,
        /// <summary>
        /// 双精度浮点型
        /// </summary>
        Double,         //对应：Double
        //Empty,
        //Error,
        //FileTime,
        /// <summary>
        /// 唯一GUID
        /// </summary>
        GUID,           //对应：ReplicationID (Access 97 (OLEDB)), (Access 2000 (OLEDB))
        //IDispatch,
        /// <summary>
        /// 整型
        /// </summary>
        Integer,        //对应：AutoNumber,Integer,Long
        //IUnknown,
        /// <summary>
        /// 可变长长二进制型
        /// </summary>
        LongVarBinary,  //对应：OLEObject
        /// <summary>
        /// 可变长长字符型
        /// </summary>
        LongVarChar,    //对应：Memo (Access 97),Hyperlink (Access 97)
        /// <summary>
        /// 可变长长字符型（Unicode）
        /// </summary>
        LongVarWChar,   //对应：Memo (Access 2000 (OLEDB)),Hyperlink (Access 2000 (OLEDB))
        /// <summary>
        /// 数字
        /// </summary>
        Numeric,        //对应：Decimal (Access 2000 (OLEDB))
        //PropVariant,
        /// <summary>
        /// 单精度浮点型
        /// </summary>
        Single,         //对应：Single
        /// <summary>
        /// 短整型
        /// </summary>
        SmallInt,       //对应：Integer
        //TinyInt,
        //UnsignedBigInt,
        //UnsignedInt,
        //UnsignedSmallInt,
        /// <summary>
        /// 无符号微整型
        /// </summary>
        UnsignedTinyInt,//对应：Byte
        //UserDefined,
        /// <summary>
        /// 可变长二进制型
        /// </summary>
        VarBinary,      //对应：ReplicationID (Access 97)
        /// <summary>
        /// 可变长字符型
        /// </summary>
        VarChar,        //对应：Text (Access 97)
        //Variant,
        //VarNumeric,
        /// <summary>
        /// 可变长字符型（Unicode）
        /// </summary>
        VarWChar,       //对应：Text (Access 2000 (OLEDB))
        //WChar
    }
}
