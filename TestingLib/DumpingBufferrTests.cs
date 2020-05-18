using Common;
using NUnit.Framework;
using System;

namespace DumpingBufferLib.Tests
{
    [TestFixture()]
    public class DumpingBufferrTests
    {
        private DumpingBufferr dumpingBufferr;

        [SetUp()]
        public void SetUp()
        {
            dumpingBufferr = new DumpingBufferr();
        }

        [Test()]
        [TestCase("analog", "digital")]
        [TestCase("custom", "limitset")]
        [TestCase("singlenode", "multiplenode")]
        [TestCase("consumer", "source")]
        [TestCase("motion", "sensor")]
        public void ReceiveValuesTestAdd(string c1, string c2)
        {
            DeltaCD ret = null;
            var code1 = GetCode(c1);
            var code2 = GetCode(c2);
            for (int i = 0; i < 5; i++)
            {
                dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code1, new Value(DateTime.Now, "Novi Sad", 22)));
                ret = dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code2, new Value(DateTime.Now, "Novi Sad", 22)));
            }

            Assert.NotNull(ret);
        }

        [Test()]
        [TestCase("analog", "digital")]
        [TestCase("custom", "limitset")]
        [TestCase("singlenode", "multiplenode")]
        [TestCase("consumer", "source")]
        [TestCase("motion", "sensor")]
        public void ReceiveValuesTestUpdate(string c1, string c2)
        {
            DeltaCD ret = null;
            var code1 = GetCode(c1);
            var code2 = GetCode(c2);
            for (int i = 0; i < 5; i++)
            {
                dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code1, new Value(DateTime.Now, "Novi Sad", 22)));
                dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code2, new Value(DateTime.Now, "Novi Sad", 22)));
            }
            for (int i = 0; i < 5; i++)
            {
                dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code1, new Value(DateTime.Now, "Novi Sad", 440)));
                ret = dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code2, new Value(DateTime.Now, "Novi Sad", 440)));
            }

            Assert.NotNull(ret);
        }

        [Test()]
        [TestCase("analog")]
        [TestCase("custom")]
        [TestCase("singlenode")]
        [TestCase("consumer")]
        [TestCase("digital")]
        [TestCase("limitset")]
        [TestCase("multiplenode")]
        [TestCase("source")]
        [TestCase("sensor")]
        [TestCase("motion")]
        public void ReceiveValuesTestReturnsNull(string c1)
        {
            var code = GetCode(c1);
            var ret = dumpingBufferr.ReceiveValues(new Tuple<Code, Value>(code, new Value(DateTime.Now, "Novi Sad", 440)));

            Assert.IsNull(ret);
        }

        private Code GetCode(string codeString)
        {
            switch (codeString)
            {
                case "analog":
                    return Code.CODE_ANALOG;
                case "digital":
                    return Code.CODE_DIGITAL;
                case "custom":
                    return Code.CODE_CUSTOM;
                case "limitset":
                    return Code.CODE_LIMITSET;
                case "singlenode":
                    return Code.CODE_SINGLENOE;
                case "multiplenode":
                    return Code.CODE_MULTIPLENODE;
                case "consumer":
                    return Code.CODE_CONSUMER;
                case "source":
                    return Code.CODE_SOURCE;
                case "motion":
                    return Code.CODE_MOTION;
                case "sensor":
                    return Code.CODE_SENSOR;
                default:
                    return new Code();
            }
        }
    }
}