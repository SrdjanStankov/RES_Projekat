using NUnit.Framework;
using ReaderLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderLib.Tests
{
    [TestFixture()]
    public class ReaderTests
    {
        private Reader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new Reader();
        }

        [Test()]
        [TestCase("01/01/2000", "01/01/2222")]
        public void GetChangesForIntervalTest(DateTime d1, DateTime d2)
        {
            reader.GetChangesForInterval(d1, d2);
            reader.WriteDatabase(d1, d2);
        }
    }
}