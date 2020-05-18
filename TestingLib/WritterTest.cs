using Common;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace WritterLib.Tests
{
    [TestFixture]
    public class WritterTest
    {
        private Writter writter;

        [SetUp]
        public void SetUp()
        {
            writter = new Writter();
        }


        [Test]
        public void WriteToDumpingBufferTest()
        {
            var mock = new Mock<IRandom>();
            mock.SetReturnsDefault(1);
            var retVal = writter.WriteToDumpingBuffer(mock.Object);
            var value = new Value(DateTime.Now, "Beograd", 1);
            Assert.AreEqual(new Tuple<Code, Value>(Enum.GetValues(typeof(Code)).Cast<Code>().ElementAt(1), value), retVal);
        }

        [Test]
        public void ManualWriteToHistoryTest_TestAllInputsAreGood()
        {
            var sw = new StringWriter();
            var sr = new StringReader("CODE_ANALOG" + Environment.NewLine + "Beograd" + Environment.NewLine + "1" + Environment.NewLine);

            Console.SetOut(sw);
            Console.SetIn(sr);

            var retVal = writter.ManualWriteToHistory();

            Assert.AreEqual(retVal.Item1, Code.CODE_ANALOG);
            Assert.AreEqual(retVal.Item2, new Value(DateTime.Now, "Beograd", 1));
        }


        [Test]
        [TestCase("CODE", "Beograd", "1")]
        [TestCase("CODE_ANALOG", "Beograd", "1A")]
        [TestCase("CODE_ANALOG", "Beograd", "-10")]
        public void ManualWriteToHistoryTest_TestAllInputsAreBad(string code, string area, string consumption)
        {
            var sw = new StringWriter();
            var sr = new StringReader(code + Environment.NewLine + area + Environment.NewLine + consumption + Environment.NewLine);

            Console.SetOut(sw);
            Console.SetIn(sr);

            var retVal = writter.ManualWriteToHistory();

            Assert.AreEqual(retVal, null);

        }
    }
}