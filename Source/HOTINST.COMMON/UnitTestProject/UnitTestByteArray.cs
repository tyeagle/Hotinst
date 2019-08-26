using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HOTINST.COMMON.License;
using HOTINST.COMMON.Bitwise;
using HOTINST.COMMON.Security;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestByteArray
    {
        /// <summary>
        /// 本用例用于验证无符号数据
        /// </summary>
        [TestMethod]
        public void TestUIntAndUlong()
        {
            byte[] buffer = new byte[128];
            ulong ulLittleEndian = 0x8765432187654321;
            ulong ulRead = 0;
            ByteArrayHelper.SetValue(buffer, 0, 0, 64, ulLittleEndian);
            ByteArrayHelper.GetValue(buffer, 0, 0, 64, out ulRead);
            Assert.AreEqual(ulRead, ulLittleEndian);

            ulong ulBigEndian = 0x2143658721436587;
            ByteArrayHelper.SetValue(buffer, 8, 0, 64, ulBigEndian, Endian.BigEndian);
            ByteArrayHelper.GetValue(buffer, 8, 0, 64, out ulRead, Endian.BigEndian);
            Assert.AreEqual(ulRead, ulBigEndian);

            uint usLittleEndian = 0x88778877;
            uint usRead = 0;
            ByteArrayHelper.SetValue(buffer, 16, 0, 32, usLittleEndian);
            ByteArrayHelper.GetValue(buffer, 16, 0, 32, out usRead);
            Assert.AreEqual(usLittleEndian, usRead);

            uint usBigEndian = 0x77887788;
            ByteArrayHelper.SetValue(buffer, 16, 0, 32, usBigEndian, Endian.BigEndian);
            ByteArrayHelper.GetValue(buffer, 16, 0, 32, out usRead, Endian.BigEndian);
            Assert.AreEqual(usBigEndian, usRead);
        }

        [TestMethod]
        public void TestByteArrayHelper()
        {
            byte[] buf = new byte[64];
            
            // ox11111001
            ByteArrayHelper.SetValue(buf, 0, 0, 2, (byte)1);
            ByteArrayHelper.SetValue(buf, 0, 2, 2, (byte)2);
            ByteArrayHelper.SetValue(buf, 0, 4, 2, (byte)3);
            ByteArrayHelper.SetValue(buf, 0, 6, 2, (byte)3);

            byte btVal = 0;
            ByteArrayHelper.GetValue(buf, 0, 0, 8, out btVal);
            Assert.AreEqual(btVal, 249);

            //0x534B
            short shVal = 0;
            ByteArrayHelper.SetValue(buf, 1, 0, 3, (short)3);
            ByteArrayHelper.SetValue(buf, 1, 3, 1, (short)1);
            ByteArrayHelper.SetValue(buf, 1, 4, 8, (short)0x34);
            ByteArrayHelper.SetValue(buf, 1, 12, 4, (short)0x05);
            ByteArrayHelper.GetValue(buf, 1, 0, 16, out shVal);
            Assert.AreEqual(shVal, 0x534B);
            short shValBe = 0;
            ByteArrayHelper.GetValue(buf, 1, 0, 16, out shValBe, Endian.BigEndian);
            Assert.AreEqual(shValBe, 0x4B53);

            //0x1B87534B
            int iVal = 0;
            ByteArrayHelper.SetValue(buf, 3, 0, 3, (int)3);
            ByteArrayHelper.SetValue(buf, 3, 3, 1, (int)1);
            ByteArrayHelper.SetValue(buf, 3, 4, 8, (int)0x34);
            ByteArrayHelper.SetValue(buf, 3, 12, 4, (int)0x05);
            ByteArrayHelper.SetValue(buf, 3, 16, 8, (int)0x087);
            ByteArrayHelper.SetValue(buf, 3, 24, 3, (int)0x3);
            ByteArrayHelper.SetValue(buf, 3, 27, 5, (int)0xA3);

            ByteArrayHelper.GetValue(buf, 3, 0, 32, out iVal);
            Assert.AreEqual(iVal, 0x1B87534B);
            uint uiValBE = 0;
            ByteArrayHelper.GetValue(buf, 3, 0, 32, out uiValBE, Endian.BigEndian);
            Assert.AreEqual<uint>(uiValBE, 0x4b53871b);

            //0x1B87534B1B87534B
            long lVal = 0;
            ByteArrayHelper.SetValue(buf, 7, 0, 3, (long)3);
            ByteArrayHelper.SetValue(buf, 7, 3, 1, (long)1);
            ByteArrayHelper.SetValue(buf, 7, 4, 8, (long)0x34);
            ByteArrayHelper.SetValue(buf, 7, 12, 4, (long)0x05);
            ByteArrayHelper.SetValue(buf, 7, 16, 8, (long)0x087);
            ByteArrayHelper.SetValue(buf, 7, 24, 3, (long)0x3);
            ByteArrayHelper.SetValue(buf, 7, 27, 5, (long)0xA3);
            ByteArrayHelper.SetValue(buf, 11, 0, 32, (long)0x1B87534B);

            ByteArrayHelper.GetValue(buf, 7, 0, 64, out lVal);
            Assert.AreEqual(lVal, 0x1B87534B1B87534B);

            ulong ulValBE = 0;
            ByteArrayHelper.GetValue(buf, 7, 0, 64, out ulValBE, Endian.BigEndian);
            Assert.AreEqual<ulong>(ulValBE, 0x4b53871b4b53871b);

        }

        [TestMethod]
        public void ReverseBit()
        {
            byte byteReverse = BitwiseOperator.ReverseBit(0xff, 0x55);
            Debug.Assert(byteReverse == 0xaa);
            Debug.Assert(BitwiseOperator.ReverseBit(byteReverse, 0x55) == 0xff);

            ushort ushortReverse = BitwiseOperator.ReverseBit(0xffff, 0xaaaa);
            Debug.Assert(ushortReverse == 0x5555);
            Debug.Assert(BitwiseOperator.ReverseBit(ushortReverse, 0xaaaa) == 0xffff);

            uint uReverse = BitwiseOperator.ReverseBit(0xffffffff, 0x55555555);
            Debug.Assert(uReverse == 0xaaaaaaaa);
            
        }

        [TestMethod]
        public void ReverseBitOrder()
        {
            byte byteValue = 0x35;
            byte byteReverse = BitwiseOperator.ReverseBitOrder(byteValue);

            Debug.Assert(byteReverse == 0xac);

            ushort usReverse = BitwiseOperator.ReverseBitOrder(0x359a);
            Debug.Assert(usReverse == 0x59ac);

            uint uReverse = BitwiseOperator.ReverseBitOrder(0x359bea01);
            Debug.Assert(uReverse ==0x8057d9ac);
        }

        [TestMethod]
        public void TestSecurity()
        {
            byte[] CheckData = new byte[80];

            for (byte i = 0; i < 80; i++)
            {
                CheckData[i] = i;
            }

            byte CheckCode = SecurityHelper.CheckCRC8(CheckData, 0, 80);

        }


        [TestMethod]
        public void TestRSA()
        { 
            string product = "Datalab";
            string verson = "1.0.0";

            string computer =                                                                                                                        License.ComputerIdentify();

            string license = License.CreateLicenseXml(computer, product, verson, AuthorizationType.AuthorizationByTime);

            bool ok = License.ValidateLicense(license, product, verson);

            Debug.Assert(ok);

        }


        [TestMethod]
        public void TestBitHelper()
        {
            ushort byteasc = 123;
            ushort bytedes = BitwiseOperator.ReverseBit(byteasc, 0xFF);

        }
    }
}
