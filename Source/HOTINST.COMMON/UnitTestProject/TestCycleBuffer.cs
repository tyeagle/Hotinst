using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HOTINST.COMMON;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HOTINST.COMMON.Collection;
using HOTINST.COMMON.Data;

namespace UnitTestProject
{
    /// <summary>
    /// TestCycleBuffer 的摘要说明
    /// </summary>
    [TestClass]
    public class TestCycleBuffer
    {
        public TestCycleBuffer()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestPerformance()
        {
            //
            // TODO:  在此处添加测试逻辑
            //

            ushort[] data = new ushort[1024];
            for (ushort i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }

            CycleBuffer<ushort> cycleBuffer = new CycleBuffer<ushort>(1024 * 1024 * 64);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i= 0; i < 1024 *64; i++)
            {
                cycleBuffer.Write(data, 0, data.Length);      
            }

            for (int i = 0; i < 1024 * 64; i++)
            {
                cycleBuffer.Read(data, 0, data.Length);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);


            List<ushort[]> buffer = new List<ushort[]>();

            sw.Start();
            for (int i = 0; i < 1024 *64; i++)
            { 
                buffer.Add(new ushort[1024]);               
            }
            while (buffer.Count > 0)
            {
                ushort[] ua = buffer[0];
                buffer.RemoveAt(0);
            }
            
            Console.WriteLine(sw.Elapsed);

        }

        [TestMethod]
        public void TestCycle()
        {
            CycleBuffer<byte> buffer = new CycleBuffer<byte>(27);
            byte[] data = new byte[13]{1,2,3,4,5,6,7,8,9,10,11,12,13};

            Debug.Assert(buffer.IsEmpty());

             buffer.Write(data, 0, data.Length);
            Debug.Assert(buffer.Count() == 13);

            buffer.Write(data, 7, data.Length-7);
            Debug.Assert(buffer.Count() == 19);

            var dr = buffer.Read(5);
            Debug.Assert(dr.Length == 5);
            Debug.Assert(buffer.Count()==14);

            buffer.Write(data, 5, data.Length -5);
            Debug.Assert(buffer.Count() == 22 );

            //buffer.Write(data, 0, data.Length);
        }

        public enum SenceType
        {
            /// <summary>
            /// 热电偶
            /// </summary>
            [System.ComponentModel.Description("热电偶")]
            Thermocouple,
            /// <summary>
            /// 热电阻
            /// </summary>
            [System.ComponentModel.Description("热电阻")]
            ResistanceTemperatureDetector,
        }

        [TestMethod]
        public void TestEnumHelper()
        {
            SenceType st = SenceType.Thermocouple;

            var listName = EnumHelper.GetEnumStringFromEnumValue(st.GetType());
            var value0 = listName.GetValues(0);

            int i = (int) st;
            var desc = i.ToEnumDescriptionString(typeof(SenceType));
        }
    }
}
