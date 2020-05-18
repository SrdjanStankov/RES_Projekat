using Common;
using DumpingBufferLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HistoricalLib.Tests
{
    [TestFixture()]
    public class HistoricalTests
    {
        private Historical historical;

        [SetUp]
        public void SetUp()
        {
            historical = new Historical();
        }

        [Test()]
        [TestCase(1, "Analog", "Digital")]
        [TestCase(2, "Custom", "Limitset")]
        [TestCase(3, "Singlenode", "Multiplenode")]
        [TestCase(4, "Consumer", "Source")]
        [TestCase(5, "Motion", "Sensor")]
        public void ReceiveValueTestGood(int dataSet, string key, string key2)
        {
            var addCollectionDescription = GetObject(key, key2, dataSet);
            var updateCollectionDescription = GetObject(key, key2, dataSet);
            var removeCollectionDescription = GetObject(key, key2, dataSet);

            var cd = new DeltaCD(1, addCollectionDescription, updateCollectionDescription, removeCollectionDescription);

            historical.ReceiveValue(cd);
        }

        [Test()]
        [TestCase(1, "Analog", "Limitset")]
        [TestCase(2, "Custom", "Multiplenode")]
        [TestCase(3, "Singlenode", "Source")]
        [TestCase(4, "Consumer", "Sensor")]
        [TestCase(5, "Motion", "Digital")]
        public void ReceiveValueTestValidateDataSetsFail(int dataSet, string key, string key2)
        {
            var addCollectionDescription = GetObject(key, key2, dataSet);
            var updateCollectionDescription = GetObject(key, key2, dataSet);
            var removeCollectionDescription = GetObject(key, key2, dataSet);

            var cd = new DeltaCD(1, addCollectionDescription, updateCollectionDescription, removeCollectionDescription);

            Assert.Throws<ArgumentException>(() => historical.ReceiveValue(cd));
        }

        [Test()]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public void ReceiveValueTestPackDescriptionGood(bool add, bool update, bool remove)
        {
            CollectionDescription addCD = null;
            CollectionDescription updateCD = null;
            CollectionDescription removeCD = null;
            var dumpProp = new List<DumpingProperty>();
            AddCodeToDumpingProperty("Analog", dumpProp);
            AddCodeToDumpingProperty("Digital", dumpProp);
            if (add)
            {
                addCD = new CollectionDescription(1, 1, dumpProp);
            }
            if (update)
            {
                updateCD = new CollectionDescription(1, 1, dumpProp);
            }
            if (remove)
            {
                removeCD = new CollectionDescription(1, 1, dumpProp);
            }

            var cd = new DeltaCD(1, addCD, updateCD, removeCD);
            historical.ReceiveValue(cd);
        }



        private CollectionDescription GetObject(string key, string key2, int dataSet)
        {
            var dumpProp = new List<DumpingProperty>();
            AddCodeToDumpingProperty(key, dumpProp);
            AddCodeToDumpingProperty(key2, dumpProp);

            return new CollectionDescription(1, dataSet, dumpProp);
        }

        private static void AddCodeToDumpingProperty(string key, List<DumpingProperty> dumpProp)
        {
            var val = new Value(DateTime.Now, "Novi Sad", 123);
            switch (key)
            {
                case "Analog":
                    dumpProp.Add(new DumpingProperty(Code.CODE_ANALOG, val));
                    break;
                case "Digital":
                    dumpProp.Add(new DumpingProperty(Code.CODE_DIGITAL, val));
                    break;
                case "Custom":
                    dumpProp.Add(new DumpingProperty(Code.CODE_CUSTOM, val));
                    break;
                case "Limitset":
                    dumpProp.Add(new DumpingProperty(Code.CODE_LIMITSET, val));
                    break;
                case "Singlenode":
                    dumpProp.Add(new DumpingProperty(Code.CODE_SINGLENOE, val));
                    break;
                case "Multiplenode":
                    dumpProp.Add(new DumpingProperty(Code.CODE_MULTIPLENODE, val));
                    break;
                case "Consumer":
                    dumpProp.Add(new DumpingProperty(Code.CODE_CONSUMER, val));
                    break;
                case "Source":
                    dumpProp.Add(new DumpingProperty(Code.CODE_SOURCE, val));
                    break;
                case "Motion":
                    dumpProp.Add(new DumpingProperty(Code.CODE_MOTION, val));
                    break;
                case "Sensor":
                    dumpProp.Add(new DumpingProperty(Code.CODE_SENSOR, val));
                    break;
            }
        }
    }
}