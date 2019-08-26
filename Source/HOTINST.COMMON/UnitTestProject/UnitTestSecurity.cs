using System;
using System.Diagnostics;
using HOTINST.COMMON.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestSecurity
    {

        [TestMethod]
        public void TestCrc8()
        {
            byte[] aryBuffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };

            var crc8 = CyclicRedundancyCheck.Crc8(aryBuffer, 0, aryBuffer.Length, 0x07, 0, 0, false, false);
            var crcCalc = FastCyclicRdeundancyCheck8.Crc8;
            var crc8Check = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc8 == crc8Check);

            var crc8Itu = CyclicRedundancyCheck.Crc8(aryBuffer, 0, aryBuffer.Length, 0x07, 0, 0x55, false, false);
            crcCalc = FastCyclicRdeundancyCheck8.Crc8Itu;
            var crc8ItuCheck = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc8Itu == crc8ItuCheck);

            var crc8Rohc = CyclicRedundancyCheck.Crc8(aryBuffer, 0, aryBuffer.Length, 0x07, 0xFF, 0, true, true);
            crcCalc = FastCyclicRdeundancyCheck8.Crc8Rohc;
            var crc8RohcCheck = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc8Rohc == crc8RohcCheck);

            var crc8Maxim = CyclicRedundancyCheck.Crc8(aryBuffer, 0, aryBuffer.Length, 0x31, 0, 0, true, true);
            crcCalc = FastCyclicRdeundancyCheck8.Crc8Maxim;
            var crc8MaximCheck = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc8Maxim == crc8MaximCheck);


           int i = 0;
        }
        [TestMethod]
        public void TestCrc16()
        {
            byte[] aryBuffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };

            var crc16IBMCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x8005, 0, 0, true, true);
            var crcCalc = FastCyclicRdeundancyCheck16.Crc16Ibm;
            var crc16IBM = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc16IBM == crc16IBMCheck);

            crcCalc = FastCyclicRdeundancyCheck16.Crc16Maxim;
            var crc16Maxim = crcCalc.CrcCode(aryBuffer);
            var crc16MaximCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x8005, 0, 0xFFFF, true, true);
            Debug.Assert(crc16MaximCheck == crc16Maxim);


            crcCalc = FastCyclicRdeundancyCheck16.Crc16Usb;
            var crc16USB = crcCalc.CrcCode(aryBuffer);
            var crc16USBCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x8005, 0xFFFF, 0xFFFF, true, true);
            Debug.Assert(crc16USBCheck == crc16USB);


            crcCalc = FastCyclicRdeundancyCheck16.Crc16Modbus;
            var Crc16MODBUS = crcCalc.CrcCode(aryBuffer);
            var crc16MODBUSCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x8005, 0xFFFF, 0, true, true);
            Debug.Assert(crc16MODBUSCheck == Crc16MODBUS);


            crcCalc = FastCyclicRdeundancyCheck16.Crc16Ccit;
            var crc16Ccit = crcCalc.CrcCode(aryBuffer);
            var crc16CCITCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x1021, 0, 0, true, true);
            Debug.Assert(crc16CCITCheck == crc16Ccit);

            crcCalc = FastCyclicRdeundancyCheck16.Crc16CcitFalce;
            var crc16CcitFalse = crcCalc.CrcCode(aryBuffer);
            var crc16CCITFalseCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x1021, 0xFFFF, 00, false, false);
            Debug.Assert(crc16CCITFalseCheck == crc16CcitFalse);


            crcCalc = FastCyclicRdeundancyCheck16.Crc16XModem;
            var crc16XModem = crcCalc.CrcCode(aryBuffer);
            var crc16XModemCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x1021, 0, 0, false, false);
            Debug.Assert(crc16XModemCheck == crc16XModem);

            crcCalc = FastCyclicRdeundancyCheck16.Crc16X25;
            var crc16X25 = crcCalc.CrcCode(aryBuffer);
            var crc16X25Check = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x1021, 0xFFFF, 0xFFFF, true, true);
            Debug.Assert(crc16X25Check == crc16X25);

            //var crc16DNP = CyclicRedundancyCheck.Crc16Dnp(aryBuffer, 0, aryBuffer.Length);
            var crc16DNPCheck = CyclicRedundancyCheck.Crc16(aryBuffer, 0, aryBuffer.Length, 0x3D65, 0, 0xFFFF, true, true);







            //CRCAgrithm crc = new CRCAgrithm(CRCAgrithm.CrcCode.CrcCcitt);
            //// ReSharper disable once UnusedVariable
            //ushort ustCheckCode = crc.CalcCrcitt(aryBuffer);


        }
        [TestMethod]
        public void TestCrc32()
        {
            byte[] aryBuffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };

            var crc32 = CyclicRedundancyCheck.Crc32(aryBuffer, 0, aryBuffer.Length, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF,
                true, true);
            var crcCalc = FastCyclicRdeundancyCheck32.Crc32;
            var crc32Check = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc32 == crc32Check);

            var crc32MpegCheck = CyclicRedundancyCheck.Crc32(aryBuffer, 0x04C11DB7, 0xFFFFFFFF, 0x0, false, false);
            crcCalc = FastCyclicRdeundancyCheck32.Crc32Mpeg2;
            var crc32Mpeg = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc32Mpeg == crc32MpegCheck);


            var crc32JamcrcCheck = CyclicRedundancyCheck.Crc32(aryBuffer, 0xED888320, 0xFFFFFFFF, 0x0, true, false);
            crcCalc = FastCyclicRdeundancyCheck32.Crc32Jmacrc;
            var crc32Jamcrc = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc32Jamcrc == crc32JamcrcCheck);

            var crc32BzipCheck = CyclicRedundancyCheck.Crc32(aryBuffer, 0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, false, false);
            crcCalc = FastCyclicRdeundancyCheck32.Crc32BZip;
            var crc32Bzip = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(crc32Bzip == crc32BzipCheck);

            var Crc32SctpCheck = CyclicRedundancyCheck.Crc32(aryBuffer, 0x1EDC6F41, 0xFFFFFFFF, 0xFFFFFFFF, false, false);
            crcCalc = FastCyclicRdeundancyCheck32.Crc32Sctp;
            var Crc32Sctp = crcCalc.CrcCode(aryBuffer);
            Debug.Assert(Crc32Sctp == Crc32SctpCheck);


            int i = 0;
        }

        [TestMethod]
        public void TestFastCrc32()
        {
            //非0初始值， 输入不反转，输出不反转
            byte[] aryBuffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90 };
            var crc32 = new FastCyclicRdeundancyCheck32(0x12345678, 0x1234, 0x5678, false, false);
            var crc32Code = crc32.CrcCode(aryBuffer);
            var crc32Check = CyclicRedundancyCheck.Crc32(aryBuffer, 0x12345678, 0x1234, 0x5678, false, false);
            Debug.Assert(crc32Code == crc32Check);


            crc32 = new FastCyclicRdeundancyCheck32(0x12345678, 0xFFFFFFFF, 0x5678, true, false);
            crc32Code = crc32.CrcCode(aryBuffer);
            crc32Check = CyclicRedundancyCheck.Crc32(aryBuffer, 0x12345678, 0xFFFFFFFF, 0x5678, true, false);
            Debug.Assert(crc32Code == crc32Check);

            ///初值0xFFFFFFFF, 输入反转，输出反转
            crc32 = new FastCyclicRdeundancyCheck32(0x12345678, 0xFFFFFFFF, 0x5678, true, true);
            crc32Code = crc32.CrcCode(aryBuffer);
            crc32Check = CyclicRedundancyCheck.Crc32(aryBuffer, 0x12345678, 0xFFFFFFFF, 0x5678, true, true);
            Debug.Assert(crc32Code == crc32Check);

        }



        [TestMethod]
        public void TestCreateTable()
        {
            var crc8Table = CyclicRedundancyCheck.CreateCrcTable(0x07, false);
            var crc8Table1 = CyclicRedundancyCheck.CreateCrcTable(0x31, false);

            CheckTableDump(crc8Table);
            CheckTableDump(crc8Table1);



            var table16 = CyclicRedundancyCheck.CreateCrcTable(0x8005, true);

            var crc16Table1201Revers = CyclicRedundancyCheck.CreateCrcTable(0x1021, true);

            var crc16Table1201 = CyclicRedundancyCheck.CreateCrcTable(0x1021, false);

            var crc32Table = CyclicRedundancyCheck.CreateCrcTable(0xEDB88320, false);
            var crc32TableRevers = CyclicRedundancyCheck.CreateCrcTable(0xEDB88320, true);

            var crc32Table2Revers = CyclicRedundancyCheck.CreateCrcTable(0x04C11DB7, true);
            var crc32Table2 = CyclicRedundancyCheck.CreateCrcTable(0x04C11DB7, false);

            int i = 0;

        }


        private void CheckTableDump(byte[] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = i + 1; j < table.Length; j++)
                {
                    if (i != j)
                    {
                        Debug.Assert(table[i] != table[j]);
                    }
                }
            }
        }
    }
}
