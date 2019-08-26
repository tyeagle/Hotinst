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
            byte[] plainTextBArray;
            byte[] cypherTextBArray;
            string result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            plainTextBArray = (new UnicodeEncoding()).GetBytes(encryptString);
            int maxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (plainTextBArray.Length <= maxBlockSize)
            {
                cypherTextBArray = rsa.Encrypt(plainTextBArray, false);
                result = Convert.ToBase64String(cypherTextBArray);
            }
            else
            {
                using (MemoryStream plaiStream = new MemoryStream(plainTextBArray))
                using (MemoryStream crypStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[maxBlockSize];
                    int BlockSize = plaiStream.Read(Buffer, 0, maxBlockSize);

                    while (BlockSize > 0)
                    {
                        Byte[] toEncrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, toEncrypt, 0, BlockSize);

                        Byte[] cryptograph = rsa.Encrypt(toEncrypt, false);
                        crypStream.Write(cryptograph, 0, cryptograph.Length);

                        BlockSize = plaiStream.Read(Buffer, 0, maxBlockSize);
                    }

                    result = Convert.ToBase64String(crypStream.ToArray(), Base64FormattingOptions.None);
                }
            }
            return result;
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
