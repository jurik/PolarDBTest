using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;

namespace PolarDbTest
{
    [TestClass]
    public class PhSortTest
    {
        [TestInitialize]
        [TestCleanup]
        public void PxCellCleanUp()
        {
            string testpacfilename = "test.phc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
            testpacfilename = "test.ptc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
        }

        [TestMethod]
        public void SortAtomTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));

            var testdb = new object[] { 15, 2, 31, 35, 11, 190, 21 };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");

            hCell.Fill(testdb);
            try
            {
                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual(31, (int)hCell.Root.Element(2).Get().Value);
                Assert.AreEqual(35, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(6).Get().Value);

                hCell.Sort(e =>
                {
                    return (int)e.Get().Value;
                });
                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual(2, (int)hCell.Root.Element(0).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(190, (int)hCell.Root.Element(6).Get().Value);

            }
            finally
            {
                hCell.Close();
            }
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

            PhCell hCell = new PhCell(tp_seq, string.Empty, "test");
            try
            {
                hCell.Fill(testdb);

                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual("Ivan", hCell.Root.Element(2).Field(0).Get().Value.ToString());
                Assert.AreEqual("Jim", hCell.Root.Element(3).Field(0).Get().Value.ToString());
                Assert.AreEqual("Ken", hCell.Root.Element(5).Field(0).Get().Value.ToString());
                Assert.AreEqual("Aby", hCell.Root.Element(6).Field(0).Get().Value.ToString());

                hCell.Sort(e =>
                {
                    return (string)e.Field(0).Get().Value;
                });

                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual("Ivan", hCell.Root.Element(2).Field(0).Get().Value.ToString());
                Assert.AreEqual("Jim", hCell.Root.Element(3).Field(0).Get().Value.ToString());
                Assert.AreEqual("Petr", hCell.Root.Element(5).Field(0).Get().Value.ToString());
                Assert.AreEqual("Wai", hCell.Root.Element(6).Field(0).Get().Value.ToString());
            }
            finally
            {
                hCell.Close();
            }

        }
    }
}
