using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HOTINST.COMMON.License
{
    class RSAHelper
    {
        public static string RSAEncrypt(string xmlPublicKey, string encryptString)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
            int MaxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (PlainTextBArray.Length <= MaxBlockSize)
            {
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
            }
            else
            {
                using (MemoryStream PlaiStream = new MemoryStream(PlainTextBArray))
                using (MemoryStream CrypStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] ToEncrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

                        Byte[] Cryptograph = rsa.Encrypt(ToEncrypt, false);
                        CrypStream.Write(Cryptograph, 0, Cryptograph.Length);

                        BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                    }

                    Result = Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                }
            }
            return Result;
        }
        public static string RSADecrypt(string xmlPrivateKey, string decryptString)
        {
            byte[] PlainTextBArray = null;
            byte[] DypherTextBArray = null;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            DypherTextBArray = Convert.FromBase64String(decryptString);

            int MaxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制
            if (DypherTextBArray.Length <= MaxBlockSize)
            {
                PlainTextBArray = rsa.Decrypt(DypherTextBArray, false);
                Result = (new UnicodeEncoding()).GetString(PlainTextBArray);
            }
            else
            {
                using (MemoryStream CrypStream = new MemoryStream(DypherTextBArray))
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);

                        Byte[] Plaintext = rsa.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);

                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }

                    Result = (new UnicodeEncoding()).GetString(PlaiStream.ToArray());
                }
            }

            return Result;
        }
        public static string MD5Hash(string strSource)
        {
            String strHashData = String.Empty;
            //从字符串中取得Hash描述   
            byte[] Buffer;
            byte[] HashData;
            HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
            Buffer = Encoding.GetEncoding("GB2312").GetBytes(strSource);
            HashData = MD5.ComputeHash(Buffer);
            strHashData = Convert.ToBase64String(HashData);

            return strHashData;
        }
    }
}
