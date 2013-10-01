using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;
using System.Linq;
using System.Collections.Generic;

namespace PolarDbTest
{
    [TestClass]
    public class PxSortTest
    {
        [TestCleanup]
        [TestInitialize]
        public void PxCellCleanUp()
        {
            string testpacfilename = "test.pxc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
        }

        [TestMethod]
        public void SimpleSortRecordTest()
        {
            PType tp_seq = new PTypeSequence(new PTypeRecord(
                new NamedType("name", new PType(PTypeEnumeration.sstring)),
                new NamedType("id", new PType(PTypeEnumeration.sstring))));

            object[] testdb = new object[] {
                new object[] { "Petr", "0120"},
                new object[] { "Dan", "12"},
                new object[] { "Ivan", "54"},
                new object[] { "Jim", "13"},
                new object[] { "Wai", "1"},
                new object[] { "Ken", "15"},
                new object[] { "Aby", "916"},
            };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(tp_seq, testpacfilename, false);
            try
            {
                xCell.Fill2(testdb);

                Assert.AreEqual(7, xCell.Root.Count());
                Assert.AreEqual("Ivan", xCell.Root.Element(2).Field(0).Get().Value.ToString());
                Assert.AreEqual("Jim", xCell.Root.Element(3).Field(0).Get().Value.ToString());
                Assert.AreEqual("Ken", xCell.Root.Element(5).Field(0).Get().Value.ToString());
                Assert.AreEqual("Aby", xCell.Root.Element(6).Field(0).Get().Value.ToString());

                xCell.Root.Sort(e =>
                {
                    return (string)e.Field(0).Get().Value;
                });

                Assert.AreEqual(7, xCell.Root.Count());
                Assert.AreEqual("Ivan", xCell.Root.Element(2).Field(0).Get().Value.ToString());
                Assert.AreEqual("Jim", xCell.Root.Element(3).Field(0).Get().Value.ToString());
                Assert.AreEqual("Petr", xCell.Root.Element(5).Field(0).Get().Value.ToString());
                Assert.AreEqual("Wai", xCell.Root.Element(6).Field(0).Get().Value.ToString());
            }
            finally            
            {
                xCell.Close();
            }

        }
    }
}
