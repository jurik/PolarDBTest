using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;

namespace PolarDbTest
{
    [TestClass]
    public class PhUpdateElementTest
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
        public void UpdateStringsTest()
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

        [TestMethod]
        public void UpdateUnionTest()
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
                var e = (object[])hCell.Root.Element(1).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 1);
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                var o = (object[])e[1];
                Assert.AreEqual(o.Length, 3);
                Assert.AreEqual(o[0], "a1");
                Assert.AreEqual(o[1], "b1");
                Assert.AreEqual(o[2], "c1");

                PhEntry entry = hCell.Root.Element(1);
                entry.Set(new object[] { 1, new object[] { "a2", "b2", "c134" } });

                e = (object[])hCell.Root.Element(1).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 1);
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                o = (object[])e[1];
                Assert.AreEqual(o.Length, 3);
                Assert.AreEqual(o[0], "a2");
                Assert.AreEqual(o[1], "b2");
                Assert.AreEqual(o[2], "c134");


            }
            finally
            {
                hCell.Close();
            }
        }


        [TestMethod]
        public void UpdateRecordsTest()
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

                PhEntry el = hCell.Root.Element(0);
                el.Set(new object[] { "a3", "b45", "c45" });

                e = (object[])hCell.Root.Element(0).Get().Value;
                Assert.AreEqual(e.Length, 3);
                Assert.AreEqual(e[0], "a3");
                Assert.AreEqual(e[1], "b45");
                Assert.AreEqual(e[2], "c45");

            }
            finally
            {
                hCell.Close();
            }

        }

        [TestMethod]
        public void UpdateAtomsTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));

            var testdb = new object[] { 1, 4, 6, 8, 13, 88};

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            try
            {
                hCell.Fill(testdb);
                Assert.AreEqual(6, hCell.Root.Count());
                var e = (int)hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e, 6);
                Assert.AreEqual((int)hCell.Root.Element(0).Get().Value, 1);
                Assert.AreEqual((int)hCell.Root.Element(2).Get().Value, 6);
                Assert.AreEqual((int)hCell.Root.Element(5).Get().Value, 88);

                var entry = (PhEntry)hCell.Root.Element(2);
                entry.Set(987);
                Assert.AreEqual((int)hCell.Root.Element(2).Get().Value, 987);                
            }
            finally
            {
                hCell.Close();
            }
        }

    }
}
