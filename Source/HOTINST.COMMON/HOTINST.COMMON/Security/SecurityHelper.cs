/****************************************************************
 * 类 名 称：SecurityHelper
 * 命名空间：HOTINST.COMMON.Security
 * 文 件 名：SecurityHelper.cs
 * 创建时间：2016-5-5
 * 作    者：汪锋
 * 说    明：与安全的相关操作
 * 修改历史：
 *          2016-10-17 汪锋 新增CRC和MD5校验的多个重载，支持对
 *          数组中的某一部分进行计算校验值
 *          2016-10-18 汪锋 修改CheckXor并新增多个重载，支持对
 *          数组中的某一部分进行计算校验值
 *          2016-10-18 汪锋 重写CheckSum并新增多个重载，支持对
 *          数组中的某一部分进行计算校验值
 * 
 *****************************************************************/
using HOTINST.COMMON.Bitwise;
using HOTINST.COMMON.Data;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HOTINST.COMMON.Security
{
    /// <summary>
    /// 安全相关Helper类
    /// </summary>
    public static class SecurityHelper
    {

        #region 余式表
        private static readonly ushort[] AryCrCtable16 = new ushort[]
            {
                0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
                0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
                0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
                0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
                0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
                0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
                0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
                0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
                0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
                0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
                0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
                0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
                0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
                0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
                0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
                0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
                0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
                0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
                0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
                0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
                0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
                0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
                0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
                0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
                0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
                0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
                0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
                0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
                0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
                0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
                0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
                0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
            };
        private static uint[] aryCRCtable32 =
            {
                0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 0x706AF48F, 0xE963A535, 0x9E6495A3,
                0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91,
                0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE, 0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
                0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9, 0xFA0F3D63, 0x8D080DF5,
                0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
                0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
                0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 0xB8BDA50F,
                0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924, 0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D,
                0x76DC4190, 0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
                0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
                0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457,
                0x65B0D9C6, 0x12B7E950, 0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
                0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB,
                0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9,
                0x5005713C, 0x270241AA, 0xBE0B1010, 0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
                0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD,
                0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683,
                0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
                0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB, 0x196C3671, 0x6E6B06E7,
                0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
                0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
                0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 0x4669BE79,
                0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236, 0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F,
                0xC5BA3BBE, 0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
                0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
                0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21,
                0x86D3D2D4, 0xF1D4E242, 0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
                0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45,
                0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB,
                0xAED16A4A, 0xD9D65ADC, 0x40DF0B66, 0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
                0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693, 0x54DE5729, 0x23D967BF,
                0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
            };
        //创建余式表
        private static ushort[] aryCRCtableCCITT = new ushort[]
        {
                0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
                0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
                0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
                0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
                0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
                0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
                0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
                0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
                0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
                0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
                0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
                0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
                0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
                0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
                0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
                0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
                0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
                0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
                0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
                0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
                0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
                0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
                0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
                0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
                0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
                0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
                0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
                0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
                0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
                0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
                0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
                0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
        };
        #endregion

        #region CRC校验
        /* *************************************************************
         * CRC初始值为零；CRC校验多项式:
         * CRC8=X8+X5+X4+1
         * CRC-CCITT=X16+X12+X5+1
         * CRC16=X16+X15+X5+1
         * CRC12=X12+X11+X3+X2+1
         * CRC32=X32+X26+X23+X22+X16+X12+X11+X10+X8+X7+X5+X4+X2+X1+1
         * *************************************************************/

        /// <summary>
        /// 计算十六进制字符串的CRC8校验值
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>是有效的十六制字符串返回CRC8校验值,否则返回空字符串</returns>
        public static string CheckCRC8(string HexString)
        {
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckCRC8(aryCheckData).ToString("X2");
        }

        /// <summary>
        /// 计算Byte数组的CRC8校验值
        /// </summary>
        /// <param name="buffer">待计算校验的byte数组</param>
        /// <returns>返回计算后的校验值</returns>
        public static byte CheckCRC8(byte[] buffer)
        {
            return CheckCRC8(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 计算Byte数组的CRC8校验值
        /// </summary>
        /// <param name="buffer">待计算校验的byte数组</param>
        /// <param name="offset">待计算的起始位置索引</param>
        /// <param name="len">待计算的连续字节长度</param>
        /// <returns>返回计算后的校验值</returns>
        public static byte CheckCRC8(byte[] buffer, int offset, int len)
        {
            byte checkCode = 0;

            for (int i = 0; i < len; i++)
            {
                checkCode ^= buffer[i + offset];
                for (int j = 0; j < 8; j++)
                {
                    if ((checkCode & 0x01) != 0)
                    {
                        checkCode >>= 1;
                        checkCode ^= 0x8C;
                    }
                    else
                    {
                        checkCode >>= 1;
                    }
                }
            }
            return checkCode;
        }

        /// <summary>
        /// 计算十六进制字符串的CRC16校验值
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>是有效的十六制字符串返回CRC16校验值,否则返回空字符串</returns>
        public static string CheckCRC16(string HexString)
        {
            //转换传入字符串为对应byte数组
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckCRC16(aryCheckData).ToString("X4");
        }
        /// <summary>
        /// 计算Byte数组的CRC16校验值
        /// </summary>
        /// <param name="CheckData">待计算校验的byte数组</param>
        /// <returns></returns>
        public static ushort CheckCRC16(byte[] CheckData)
        {
            return CheckCRC16(CheckData, 0, CheckData.Length);
        }
        
        /// <summary>
        /// 计算Byte数组的CRC16校验值
        /// </summary>
        /// <param name="buffer">待计算校验的byte数组</param>
        /// <param name="offset">待计算的起始位置索引</param>
        /// <param name="len">待计算的连续字节长度</param>
        /// <returns>返回计算后的校验值</returns>
        public static ushort CheckCRC16(byte[] buffer, int offset, int len)
        {
            ushort CheckCode = 0x0000;
            //计算CRC
            for (int Cnt = 0; Cnt < len; Cnt++)
                CheckCode = Convert.ToUInt16(((CheckCode >> 8) & 0xFF) ^ AryCrCtable16[CheckCode & 0xFF ^ buffer[Cnt + offset]]);

            return CheckCode;
        }

        /// <summary>
        /// 计算十六进制字符串的CRC32校验值
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>是有效的十六制字符串返回CRC32校验值,否则返回空字符串</returns>
        public static string CheckCRC32(string HexString)
        {
            //转换传入字符串为对应byte数组
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckCRC32(aryCheckData).ToString("X8");
        }

   
        /// <summary>
        /// 计算Byte数组的CRC32校验值
        /// </summary>
        /// <param name="CheckData">待计算校验的byte数组</param>
        /// <returns></returns>
        public static UInt32 CheckCRC32(byte[] CheckData)
        {
            return CheckCRC32(CheckData, 0, CheckData.Length);
        }

        /// <summary>
        /// 计算Byte数组的CRC32校验值
        /// </summary>
        /// <param name="buffer">待计算校验的byte数组</param>
        /// <param name="offset">待计算的起始位置索引</param>
        /// <param name="len">待计算的连续字节长度</param>
        /// <returns>返回计算后的校验值</returns>
        public static UInt32 CheckCRC32(byte[] buffer, int offset, int len)
        {
            UInt32 CheckCode = 0xFFFFFFFF;
            //计算CRC
            for (int Cnt = 0; Cnt < len; Cnt++)
                CheckCode = (CheckCode >> 8) ^ aryCRCtable32[(CheckCode ^ buffer[Cnt + offset]) & 0xFF];

            CheckCode = ~CheckCode;

            return CheckCode;
        }

        /// <summary>
        /// 计算十六进制字符串CCITT的CRC校验值
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>是有效的十六制字符串返回CRC16校验值,否则返回空字符串</returns>
        public static string CheckCRC_CCITT(string HexString)
        {
            //转换传入字符串为对应byte数组
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckCRC_CCITT(aryCheckData).ToString("X4");
        }

        /// <summary>
        /// 计算Byte数组CCITT的CRC校验值
        /// </summary>
        /// <param name="CheckData">待计算校验的byte数组</param>
        /// <returns></returns>
        public static ushort CheckCRC_CCITT(byte[] CheckData)
        {
            return CheckCRC_CCITT(CheckData, 0, CheckData.Length);
        }

        /// <summary>
        /// 计算Byte数组CCITT的CRC校验值
        /// </summary>
        /// <param name="buffer">待计算校验的byte数组</param>
        /// <param name="offset">待计算的起始位置索引</param>
        /// <param name="len">待计算的连续字节长度</param>
        /// <returns></returns>
        public static ushort CheckCRC_CCITT(byte[] buffer, int offset, int len)
        {
            ushort CheckCode = 0xFFFF;
           
            //计算CRC
            ushort ustTemp;
            for (int Cnt = 0; Cnt < len; Cnt++)
            {
                ustTemp = CheckCode;
                CheckCode = aryCRCtableCCITT[(ustTemp >> 8) ^ buffer[Cnt + offset]];
                CheckCode = (ushort)(CheckCode ^ (ustTemp << 8));
            }
            return CheckCode;
        }

        #endregion

        #region MD5校验

        /// <summary>
        /// 计算MD5校验值
        /// </summary>
        /// <param name="CheckString">待计算校验码的字符串</param>
        /// <returns>计算成功返回定长32位十六进制字符串校验码，失败返回空字符串</returns>
        public static string CheckMD5(string CheckString)
        {
            if (string.IsNullOrWhiteSpace(CheckString))
                return string.Empty;

            byte[] aryCheckData = UTF8Encoding.ASCII.GetBytes(CheckString);

            return CheckMD5(aryCheckData);
        }

        /// <summary>
        /// 计算MD5校验值
        /// </summary>
        /// <param name="CheckData">待计算校验码的byte数组</param>
        /// <returns>计算成功返回定长32位十六进制字符串校验码，失败返回空字符串</returns>
        public static string CheckMD5(byte[] CheckData)
        {
            byte[] aryCheckCode = null;
            if (CheckMD5(CheckData, out aryCheckCode))
                return BitConverter.ToString(aryCheckCode, 0).Replace("-", "");
            else
                return string.Empty;
        }

        /// <summary>
        /// 计算MD5校验值
        /// </summary>
        /// <param name="CheckData">待计算校验码的byte数组</param>
        /// <param name="CheckCode">输出值。返回计算后的校验值</param>
        /// <returns>传入的参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckMD5(byte[] CheckData, out byte[] CheckCode)
        {
            CheckCode = null;

            if (CheckData == null || CheckData.Length < 1)
                return false;

            MD5CryptoServiceProvider objMD5Crypto = new MD5CryptoServiceProvider();
            CheckCode = objMD5Crypto.ComputeHash(CheckData);

            return true;
        }

        /// <summary>
        /// 计算MD5校验值
        /// </summary>
        /// <param name="CheckData">待计算校验码的byte数组</param>
        /// <param name="StartIndex">待计算的起始位置索引</param>
        /// <param name="Length">待计算的连续字节长度</param>
        /// <param name="CheckCode">输出值。返回计算后的校验值</param>
        /// <returns>传入的参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckMD5(byte[] CheckData, int StartIndex, int Length, out byte[] CheckCode)
        {
            CheckCode = null;

            if (CheckData == null || CheckData.Length < 1)
                return false;
            if (StartIndex < 0 || Length < 1)
                return false;
            if (StartIndex + Length > CheckData.Length)
                return false;

            MD5CryptoServiceProvider objMD5Crypto = new MD5CryptoServiceProvider();
            CheckCode = objMD5Crypto.ComputeHash(CheckData, StartIndex, Length);

            return true;
        }

        #endregion

        #region 异或校验(CheckXor)

        #region CheckXor8

        /// <summary>
        /// 计算十六进制字符串的异或校验值（8位）
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor8(string HexString)
        {
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckXor8(aryCheckData);
        }

        /// <summary>
        /// 计算字节数组的异或校验值（8位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor8(byte[] CheckData)
        {
            byte bytCheckCode = 0;
            if (CheckXor8(CheckData, out bytCheckCode))
                return bytCheckCode.ToString("X2");
            else
                return string.Empty;
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（8位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor8(byte[] CheckData, out byte CheckCode)
        {
            return CheckXor8(CheckData, 0, CheckData.Length, out CheckCode);
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（8位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="StartIndex">待计算的起始位置索引值</param>
        /// <param name="Length">待计算的连续字节长度</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor8(byte[] CheckData, int StartIndex, int Length, out byte CheckCode)
        {
            bool result = false;
            try
            {
                CheckCode = CheckXor8(CheckData, StartIndex, Length);
                result = true;
            }
            catch(Exception)
            {
                result = false;
                CheckCode = 0;
            }
            return result;
        }

        #endregion

        #region CheckXor16

        /// <summary>
        /// 计算十六进制字符串的异或校验值（16位）
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor16(string HexString)
        {
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);

            return CheckXor16(aryCheckData);
        }

        /// <summary>
        /// 计算字节数组的异或校验值（16位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor16(byte[] CheckData)
        {
            ushort ushtCheckCode = 0;
            if (CheckXor16(CheckData, out ushtCheckCode))
                return ushtCheckCode.ToString("X4");
            else
                return string.Empty;
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（16位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor16(byte[] CheckData, out ushort CheckCode)
        {
            return CheckXor16(CheckData, 0, CheckData.Length, out CheckCode);
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（16位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="StartIndex">待计算的起始位置索引值</param>
        /// <param name="Length">待计算的连续字节长度</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor16(byte[] CheckData, int StartIndex, int Length, out ushort CheckCode)
        {
            bool result = false;
            try
            {
                CheckCode = CheckXor16(CheckData, StartIndex, Length);
                result = true;
            }
            catch(Exception)
            {
                CheckCode = 0;
                result = false;
            }
            return result;
        }

        #endregion

        #region CheckXor32

        /// <summary>
        /// 计算十六进制字符串的异或校验值（32位）
        /// </summary>
        /// <param name="HexString">待计算校验的十六进制字符串</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor32(string HexString)
        {
            byte[] aryCheckData = StringHelper.HexStringToByteArray(HexString, Endian.LittleEndian);
            return CheckXor32(aryCheckData);
        }

        /// <summary>
        /// 计算字节数组的异或校验值（32位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <returns>计算成功返回异或校验值的十六进制形式字符串，否则返回空字符串</returns>
        public static string CheckXor32(byte[] CheckData)
        {
            uint uinCheckCode = 0;
            if (CheckXor32(CheckData, out uinCheckCode))
                return uinCheckCode.ToString("X8");
            else
                return string.Empty;
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（32位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor32(byte[] CheckData, out uint CheckCode)
        {
            return CheckXor32(CheckData, 0, CheckData.Length, out CheckCode);
        }

        /// <summary>
        /// 计算字节数组指定部分的异或校验值（32位）
        /// </summary>
        /// <param name="CheckData">待计算校验值的字节数组</param>
        /// <param name="StartIndex">待计算的起始位置索引值</param>
        /// <param name="Length">待计算的连续字节长度</param>
        /// <param name="CheckCode">输出值。返回计算的异或校验值</param>
        /// <returns>如果传入参数正确并计算成功返回true，否则返回false</returns>
        public static bool CheckXor32(byte[] CheckData, int StartIndex, int Length, out uint CheckCode)
        {
            bool result = false;
            try
            {
                CheckCode = CheckXor32(CheckData, StartIndex, Length);
                result = true;
            }
            catch(Exception)
            {
                CheckCode = 0;
                result = false;
            }            

            return result;
        }

        #endregion

        #region CheckXor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Offset"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public unsafe static byte CheckXor8(byte[] Buffer, int Offset, int Len)
        {
            if (Offset + Len > Buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"Buffer.Length[{Buffer.Length}], Offset[{Offset}], Len[{Len}], Len is too small or out of buffer.");
            }

            Byte checkSum = 0;
            for(int i = Offset; i < Offset + Len; ++i)
            {
                checkSum += Buffer[i];
            }
            return checkSum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Offset"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public unsafe static UInt16 CheckXor16(byte[] Buffer, int Offset, int Len)
        {
            int numberOfUint = Len / 2;
            int remainder = Len % 2;

            if (Offset + Len > Buffer.Length || numberOfUint <= 0)
            {
                throw new ArgumentOutOfRangeException($"Buffer.Length[{Buffer.Length}], Offset[{Offset}], Len[{Len}], Len is too small or out of buffer.");
            }

            if (0 != remainder)
            {
                throw new ArgumentException($"Len[{Len}] can not be divisible by 2");
            }

            UInt16 checkSum = 0;
            fixed (byte* pByte = Buffer)
            {
	            UInt16* pBeginOfUint = (UInt16*)(pByte + Offset);
				for (UInt16* pUint = pBeginOfUint; pUint < pBeginOfUint + numberOfUint; ++pUint)
                {
                    checkSum += *pUint;
                }
            }
            return checkSum;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Offset"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public unsafe static UInt32 CheckXor32(byte[] Buffer, int Offset, int Len)
        {
            int numberOfUint = Len / 4;

            if (Offset + Len > Buffer.Length || numberOfUint <= 0)
            {
                throw new ArgumentOutOfRangeException($"Buffer.Length[{Buffer.Length}], Offset[{Offset}], Len[{Len}], Len is too small or out of buffer.");
            }

            if( 0 != Len % 4)
            {
                throw new ArgumentException($"Len[{Len}] can not be divisible by 4");
            }
                        
            UInt32 checkSum = 0;
            fixed(byte* pByte = Buffer)
            {
                UInt32* pBeginOfUint = (UInt32*)(pByte + Offset);
                for(UInt32* pUint = pBeginOfUint; pUint < pBeginOfUint + numberOfUint; ++pUint)
                {
                    checkSum ^= *pUint;
                }
            }
            return checkSum;
        }

        #endregion

        #endregion

        #region 累加和校验（CheckSum）

        /* **********************************************************************************************************************************************
         * 生成校验码 算法：
         * CheckSum8：计算所有指定字节数组中元素的数值之和，与256取模，再取反，再加1，得出校验码；
         * CheckSum16：计算所有指定字节数组中元素的数值之和，与65536取模，再取反，再加1，得出校验码；
         * CheckSum32：计算所有指定字节数组中元素的数值之和，与4294967296取模，再取反，再加1，得出校验码；
         * 判断校验 算法：
         * CheckSum8IsRight：计算与CheckSum8相同的指定字节数组中元素的数值之和，再加上要判断的校码相加，截取结果低1字节，如果为零表示正确，非零错误；
         * CheckSum16IsRight：计算与CheckSum16相同的指定字节数组中元素的数值之和，再加上要判断的校码相加，截取结果低2字节，如果为零表示正确，非零错误；
         * CheckSum32IsRight：计算与CheckSum32相同的指定字节数组中元素的数值之和，再加上要判断的校码相加，截取结果低4字节，如果为零表示正确，非零错误；
         * **********************************************************************************************************************************************/

        #region CheckSum
        
        /// <summary>
        /// 求和，返回和的补码（取反，加1）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte CheckSum8(byte[] buffer, int offset, int len)
        {
            byte sumOfArray = Sum(buffer, offset, len);

            byte checkSum = TwosComplement(sumOfArray);

            return checkSum;
        }
        /// <summary>
        /// 求和，返回和的补码（取反，加1）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe UInt16 CheckSum16(byte[] buffer, int offset, int len)
        {
            int numberOfUint = len / 2;
            int remainder = len % 2;

            if (offset + len > buffer.Length || numberOfUint <= 0)
            {
                throw new ArgumentOutOfRangeException($"Buffer.Length[{buffer.Length}], Offset[{offset}], Len[{len}], Len is too small or out of buffer.");
            }

            if (0 != remainder)
            {
                throw new ArgumentException($"数组长度不能被4整除.");
            }

            UInt16 sumOfArray = 0;
            fixed (byte* pByte = buffer)
			{
				UInt16* pBeginOfUint = (UInt16*)(pByte + offset);
                for (UInt16* pUint = pBeginOfUint; pUint < pBeginOfUint + numberOfUint; ++pUint)
                {
                    sumOfArray += *pUint;
                }
            }

            UInt16 checkSumOfArray = TwosComplement(sumOfArray);

            return checkSumOfArray;
        }

        public static UInt16 CheckSum(UInt16[]buffer, int offset, int len)
        {
            UInt16 sum = Sum(buffer, offset, len);

            UInt16 checkSum = TwosComplement(sum);

            return checkSum;
        }
        public static UInt16 CheckSum(Int16[] buffer, int offset, int len)
        {
            Int16 sum = Sum(buffer, offset, len);

            UInt16 checkSum = TwosComplement((UInt16)sum);

            return checkSum;
        }
        /// <summary>
        /// 求和，返回和的补码（取反，加1）
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Offset"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public static unsafe UInt32 CheckSum32(byte[] Buffer, int Offset, int Len)
        {
            int numberOfUint = Len / 4;
            int remainder = Len % 4;

            if (Offset + Len > Buffer.Length || numberOfUint <= 0)
            {
                throw new ArgumentOutOfRangeException($"Buffer.Length[{Buffer.Length}], Offset[{Offset}], Len[{Len}], Len is too small or out of buffer.");
            }

            if (0 != remainder)
            {
                throw new ArgumentException($"数组长度不能被4整除.");
            }

            UInt32 sumOfArray = 0;
            fixed (byte* pByte = Buffer)
            {
	            UInt32* pBeginOfUint = (UInt32*)(pByte + Offset);
				for (UInt32* pUint = pBeginOfUint; pUint < pBeginOfUint + numberOfUint; ++pUint)
                {
                    sumOfArray += *pUint;
                }
            }

            UInt32 checkSumOfArray = TwosComplement(sumOfArray);

            return checkSumOfArray;
        }

        public static UInt32 CheckSum(UInt32[] buffer, int offset, int len)
        {
            UInt32 sum = Sum(buffer, offset, len);

            UInt32 checkSum = TwosComplement(sum);

            return checkSum;
        }
        public static UInt32 CheckSum(Int32[] buffer, int offset, int len)
        {
            Int32 sum = Sum(buffer, offset, len);

            UInt32 checkSum = TwosComplement((UInt32)sum);

            return checkSum;
        }

        #endregion

        #endregion

        #region DES加解密

        #region 加密

        /// <summary>
        /// DES加密(初始化向量和密钥均为内置固定值)
        /// </summary>
        /// <param name="EncryptStringData">待加密的数据</param>
        /// <returns>加密成功返回加密后十六进制字符串，加密失败或参数错误返回空字符串</returns>
        public static string DesEncrypt(string EncryptStringData)
        {
            //DesIV:HOTINSTW；DesKey:WANGFENG；
            return DesEncrypt(EncryptStringData, "484F54494E535457", "57414E4746454E47");
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncryptStringData">待加密源码</param>
        /// <param name="HexIV">初始化向量（定长8字节十六进制字符串）</param>
        /// <param name="HexKey">加密密钥（定长8字节十六进制字符串）</param>
        /// <returns>加密成功返回加密后十六进制字符串，加密失败或参数错误返回空字符串</returns>
        public static string DesEncrypt(string EncryptStringData, string HexIV, string HexKey)
        {
            if (string.IsNullOrWhiteSpace(EncryptStringData) || string.IsNullOrWhiteSpace(HexIV) || string.IsNullOrWhiteSpace(HexKey))
                return string.Empty;

            HexIV = HexIV.ToUpper().PadLeft(16, 'D').Substring(0, 16);
            HexKey = HexKey.ToUpper().PadLeft(16, 'A').Substring(0, 16);

            byte[] aryIV; byte[] aryKey;

            aryIV = StringHelper.HexStringToByteArray(HexIV, Endian.LittleEndian);
            aryKey = StringHelper.HexStringToByteArray(HexKey, Endian.LittleEndian);

            return DesEncrypt(EncryptStringData, aryIV, aryKey);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncryptStringData">待加密源码</param>
        /// <param name="IV">初始化向量（定长8字节）</param>
        /// <param name="Key">加密密钥（定长8字节）</param>
        /// <returns>加密成功返回加密后十六进制字符串，加密失败或参数错误返回空字符串</returns>
        public static string DesEncrypt(string EncryptStringData, byte[] IV, byte[] Key)
        {
            if (string.IsNullOrWhiteSpace(EncryptStringData))
                return string.Empty;
            if (IV == null || IV.Length < 8)
                return string.Empty;
            if (Key == null || Key.Length < 8)
                return string.Empty;

            byte[] aryIV = new byte[8];
            Array.Copy(IV, aryIV, 8);
            byte[] aryKey = new byte[8];
            Array.Copy(Key, aryKey, 8);

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider() { IV = aryIV, Key = aryKey };
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.WriteLine(EncryptStringData);
            objStreamWriter.Close();
            objCryptoStream.Close();
            byte[] aryEncryptResult = objMemoryStream.ToArray();
            objMemoryStream.Close();

            string strEncryptResult = "";
            for (int Cnt = 0; Cnt < aryEncryptResult.Length; Cnt++)
                strEncryptResult += aryEncryptResult[Cnt].ToString("X2");

            return strEncryptResult;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncryptByteArraryData">待加密源码</param>
        /// <param name="IV">初始化向量（定长8字节）</param>
        /// <param name="Key">加密密钥（定长8字节）</param>
        /// <returns>加密成功返回加密后十六进制字符串，加密失败或参数错误返回空字符串</returns>
        public static string DesEncrypt(byte[] EncryptByteArraryData, byte[] IV, byte[] Key)
        {
            if (EncryptByteArraryData == null || EncryptByteArraryData.Length < 1)
                return string.Empty;
            if (IV == null || IV.Length < 8)
                return string.Empty;
            if (Key == null || Key.Length < 8)
                return string.Empty;

            byte[] aryIV = new byte[8];
            Array.Copy(IV, aryIV, 8);
            byte[] aryKey = new byte[8];
            Array.Copy(Key, aryKey, 8);

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider() { IV = aryIV, Key = aryKey };
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.WriteLine(EncryptByteArraryData);
            objStreamWriter.Close();
            objCryptoStream.Close();
            byte[] aryEncryptResult = objMemoryStream.ToArray();
            objMemoryStream.Close();

            string strEncryptResult = "";
            for (int Cnt = 0; Cnt < aryEncryptResult.Length; Cnt++)
                strEncryptResult += aryEncryptResult[Cnt].ToString("X2");

            return strEncryptResult;
        }

        #endregion

        #region 解密

        /// <summary>
        /// DES解密(初始化向量和密钥均为内置固定值)
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <returns>解密成功返回解密后字符串，解密失败或参数错误返回空字符串</returns>
        public static string DesDecrypt(string DecryptHexString)
        {
            return DesDecrypt(DecryptHexString, "484F54494E535457", "57414E4746454E47");
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <param name="HexIV">初始化向量（定长8字节十六进制字符串）</param>
        /// <param name="HexKey">加密密钥（定长8字节十六进制字符串）</param>
        /// <returns>解密成功返回解密后字符串，解密失败或参数错误返回空字符串</returns>
        /// <remarks>解密时的初始化向量和密钥应与加密时的一致才能正确解密</remarks>
        public static string DesDecrypt(string DecryptHexString, string HexIV, string HexKey)
        {
            if (string.IsNullOrWhiteSpace(DecryptHexString) || string.IsNullOrWhiteSpace(HexIV) || string.IsNullOrWhiteSpace(HexKey))
                return string.Empty;

            HexIV = HexIV.ToUpper().PadLeft(16, 'D').Substring(0, 16);
            HexKey = HexKey.ToUpper().PadLeft(16, 'A').Substring(0, 16);

            byte[] aryIV; byte[] aryKey;

            aryIV = StringHelper.HexStringToByteArray(HexIV, Endian.LittleEndian);
            aryKey = StringHelper.HexStringToByteArray(HexKey, Endian.LittleEndian);

            return DesEncrypt(DecryptHexString, aryIV, aryKey);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <param name="IV">初始化向量（定长8字节）</param>
        /// <param name="Key">加密密钥（定长8字节）</param>
        /// <returns>解密成功返回解密后字符串，解密失败或参数错误返回空字符串</returns>
        /// <remarks>解密时的初始化向量和密钥应与加密时的一致才能正确解密</remarks>
        public static string DesDecrypt(string DecryptHexString, byte[] IV, byte[] Key)
        {
            if (string.IsNullOrWhiteSpace(DecryptHexString))
                return string.Empty;
            byte[] aryDecryptByteData = StringHelper.HexStringToByteArray(DecryptHexString, Endian.LittleEndian);

            if (IV == null || IV.Length < 8)
                return string.Empty;
            if (Key == null || Key.Length < 8)
                return string.Empty;

            byte[] aryIV = new byte[8];
            Array.Copy(IV, aryIV, 8);
            byte[] aryKey = new byte[8];
            Array.Copy(Key, aryKey, 8);

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider() { IV = aryIV, Key = aryKey };
            MemoryStream objMemoryStream = new MemoryStream(aryDecryptByteData);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            string strDecryptResult = objStreamReader.ReadLine();
            objStreamReader.Close();
            objCryptoStream.Clear();
            objMemoryStream.Close();

            return strDecryptResult;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <param name="DecryptedData">输出值。解密后的byte数组</param>
        /// <returns>解密成功返回true，解密失败或参数错误返回false</returns>
        public static bool DesDecrypt(string DecryptHexString, out byte[] DecryptedData)
        {
            return DesDecrypt(DecryptHexString, "484F54494E535457", "57414E4746454E47", out DecryptedData);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <param name="HexIV">初始化向量（定长8字节十六进制字符串）</param>
        /// <param name="HexKey">加密密钥（定长8字节十六进制字符串）</param>
        /// <param name="DecryptedData">输出值。解密后的byte数组</param>
        /// <returns>解密成功返回true，解密失败或参数错误返回false</returns>
        /// <remarks>解密时的初始化向量和密钥应与加密时的一致才能正确解密</remarks>
        public static bool DesDecrypt(string DecryptHexString, string HexIV, string HexKey, out byte[] DecryptedData)
        {
            DecryptedData = null;

            if (string.IsNullOrWhiteSpace(DecryptHexString) || string.IsNullOrWhiteSpace(HexIV) || string.IsNullOrWhiteSpace(HexKey))
                return false;

            HexIV = HexIV.ToUpper().PadLeft(16, 'D').Substring(0, 16);
            HexKey = HexKey.ToUpper().PadLeft(16, 'A').Substring(0, 16);

            byte[] aryIV; byte[] aryKey;

            aryIV = StringHelper.HexStringToByteArray(HexIV, Endian.LittleEndian);
            aryKey = StringHelper.HexStringToByteArray(HexKey, Endian.LittleEndian);

            return DesDecrypt(DecryptHexString, aryIV, aryKey, out DecryptedData);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptHexString">待解密十六进制字符串</param>
        /// <param name="IV">初始化向量（定长8字节）</param>
        /// <param name="Key">加密密钥（定长8字节）</param>
        /// <param name="DecryptedData">输出值。解密后的byte数组</param>
        /// <returns>解密成功返回true，解密失败或参数错误返回false</returns>
        /// <remarks>解密时的初始化向量和密钥应与加密时的一致才能正确解密</remarks>
        public static bool DesDecrypt(string DecryptHexString, byte[] IV, byte[] Key, out byte[] DecryptedData)
        {
            DecryptedData = null;

            if (string.IsNullOrWhiteSpace(DecryptHexString))
                return false;
            byte[] aryDecryptByteData = StringHelper.HexStringToByteArray(DecryptHexString, Endian.LittleEndian);
            
            if (IV == null || IV.Length < 8)
                return false;
            if (Key == null || Key.Length < 8)
                return false;

            byte[] aryIV = new byte[8];
            Array.Copy(IV, aryIV, 8);
            byte[] aryKey = new byte[8];
            Array.Copy(Key, aryKey, 8);

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider() { IV = aryIV, Key = aryKey };
            MemoryStream objMemoryStream = new MemoryStream(aryDecryptByteData);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            //string strDecryptResult = objStreamReader.ReadLine();

            //获取解密后原始字节数据
            MemoryStream objMemoryStreamTemp = new MemoryStream();
            objStreamReader.BaseStream.CopyTo(objMemoryStreamTemp);
            DecryptedData = objMemoryStreamTemp.ToArray();
            objMemoryStreamTemp.Close();

            objStreamReader.Close();
            objCryptoStream.Clear();
            objMemoryStream.Close();

            return true;
        }

        #endregion

        #endregion

        #region sum
        /// <summary>
        /// 计算一组byte数组的和
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte Sum(byte[]buffer, int offset, int len)
        {
            byte sum = 0;
            for (int i = offset; i < offset + len; i++)
            {
                sum += buffer[i];
            }
            return sum;
        }
        /// <summary>
        /// 计算一组uint16数组的和
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static ushort Sum(ushort[] buffer, int offset, int len)
        {
            ushort sum = 0;
            for (int i = offset; i < offset + len; i++)
            {
                sum += buffer[i];
            }
            return sum;
        }
        public static Int16 Sum(Int16[] buffer, int offset, int len)
        {
            Int16 sum = 0;
            for (int i = offset; i < offset + len; i++)
            {
                sum += buffer[i];
            }
            return sum;
        }
        /// <summary>
        /// 计算一组Int32数组的和
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static UInt32 Sum(UInt32[] buffer, int offset, int len)
        {
            UInt32 sum = 0;
            for (int i = offset; i < offset + len; i++)
            {
                sum += buffer[i];
            }
            return sum;
        }
        public static Int32 Sum(Int32[] buffer, int offset, int len)
        {
            Int32 sum = 0;
            for (int i = offset; i < offset + len; i++)
            {
                sum += buffer[i];
            }
            return sum;
        }
        #endregion

        #region 反码(One's complement ,对1求补)
        /// <summary>
        /// 计算一个byte数字的1的补码，即计算机原理中的反码
        /// 这里计算反码不区分正负数，所有位取反，包括符号位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte OnesComplement(byte value)
        {
            return (byte)(~value);
        }
        /// <summary>
        /// 计算一个Uint16数据的1的补码，即计算机原理中的反码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort OnesComplement(ushort value)
        {
            return (ushort)(~value);
        }
        /// <summary>
        /// 计算一个Uint32数据的1的补码，即计算机原理中的反码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 OnesComplement(UInt32 value)
        {
            return (ushort)(~value);
        }

        #endregion

        #region 补码(Two's complement, 对2求补)
        /// <summary>
        /// 计算一个byte数字的2的补码，既计算机原理中教授的补码
        /// 这里计算补码不区分正负数，所有位取反后再加1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte TwosComplement(byte value)
        {
            return (byte)(~value + 1);
        }
        /// <summary>
        /// 计算一个Uint16数据的2的补码，既计算机原理中教授的补码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort TwosComplement(ushort value)
        {
            return (ushort)(~value + 1);
        }
        /// <summary>
        /// 计算一个Uint32数据的2的补码，既计算机原理中教授的补码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UInt32 TwosComplement(UInt32 value)
        {
            return (UInt32)(~value + 1);
        }

        #endregion
    }
}
