using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;

namespace PolarDbTest
{
    [TestClass]
    public class PhFillElementsTest
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
        public void FillTest()
        {
            var seqtriplets = new PTypeSequence(
                                new PTypeUnion(
                                    new NamedType("empty", new PType(PTypeEnumeration.none)),
                                    new NamedType("op",
                                        new PTypeRecord(
                                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("predicate", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("obj", new PType(PTypeEnumeration.sstring)))),
                                    new NamedType("dp",
                                        new PTypeRecord(
                                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("predicate", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("data", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("lang", new PType(PTypeEnumeration.sstring)))))
                                );

            var testdb = new object[] {
                new object[] { 1, new object[] {"a", "b", "c"}},
                new object[] { 1, new object[] {"a1", "b1", "c1"}},
                new object[] { 2, new object[] {"da", "db", "dc", "lang"}}
            };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            try
            {
                hCell.Fill(testdb);
                Assert.AreEqual(3, hCell.Root.Count());
                var e = (object[])hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 2);
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                var o = (object[])e[1];
                Assert.AreEqual(o.Length, 4);
                Assert.AreEqual(o[0], "da");
                Assert.AreEqual(o[1], "db");
                Assert.AreEqual(o[2], "dc");
                Assert.AreEqual(o[3], "lang");
            }
            finally
            {
                hCell.Close();
            }
        }

        [TestMethod]
        public void FillRecordsTest()
        {
            var seqtriplets = new PTypeSequence(
                                        new PTypeRecord(
                                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("predicate", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("obj", new PType(PTypeEnumeration.sstring)))
                                );

            var testdb = new object[] {
                new object[] {"a", "b", "c"},
                new object[] {"a1", "b1", "c1"},
                new object[] {"da", "db", "dc"}
            };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            try
            {
                hCell.Fill(testdb);
                Assert.AreEqual(3, hCell.Root.Count());
                var e = (object[])hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e.Length, 3);
                Assert.AreEqual(e[0], "da");
                Assert.AreEqual(e[1], "db");
                Assert.AreEqual(e[2], "dc");

                Assert.AreEqual(hCell.Root.Element(2).Field(2).Get().Value, "dc");
            }
            finally
            {
                hCell.Close();
            }
        }

        [TestMethod]
        public void FillAtomsTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.sstring));

            var testdb = new object[] { "test1", "obj24", "ggg", "ntest45", "nq1"
            };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            try
            {
                hCell.Fill(testdb);
                Assert.AreEqual(5, hCell.Root.Count());
                var e = (string)hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e.Length, 3);
                Assert.AreEqual(e, "ggg");
                Assert.AreEqual((string)hCell.Root.Element(0).Get().Value, "test1");
                Assert.AreEqual((string)hCell.Root.Element(4).Get().Value, "nq1");
            }
            finally
            {
                hCell.Close();
            }
        }

    }
}
