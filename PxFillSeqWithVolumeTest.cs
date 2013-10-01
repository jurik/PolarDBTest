using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;
using System.Collections.Generic;
using System.Linq;

namespace PolarDbTest
{
    [TestClass]
    public class PxFillSeqWithVolumeTest
    {

        [TestCleanup]
        [TestInitialize]
        public void PxCellCleanUp()
        {
            string testpacfilename = "test.pxc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
        }

        [TestMethod]
        public void FillWholeVolumeSeqUnionTest()
        {
            var seqtriplets = new PTypeSequence(
                    new PTypeUnion(
                        new NamedType("op",
                            new PTypeRecord(
                                new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                new NamedType("char", new PType(PTypeEnumeration.character))
                            )),
                        new NamedType("dp",
                            new PTypeRecord(
                                new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                new NamedType("integer", new PType(PTypeEnumeration.integer)),
                                new NamedType("bool", new PType(PTypeEnumeration.boolean))
                            )
                        )
                    ));

            var testdb = new object[] {
                new object[] { 0, new object[] {"a", 'c'}},
                new object[] { 0, new object[] {"a1", 'c'}},
                new object[] { 1, new object[] {"da", 3, true}},
                new object[] { 0, new object[] {"a1", '1'}},
                new object[] { 1, new object[] {"da1", 7, true}},
                new object[] { 0, new object[] {"a2", '2'}},
                new object[] { 1, new object[] {"da2", 9, false}},
                new object[] { 1, new object[] {"da2", 11, true}},
                new object[] { 1, new object[] {"da3", 13, false}}
            };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

            xCell.Fill(testdb);

            CheckSequence(xCell);
        }

        [TestMethod]
        public void Fill2SeqWithVolumeTest()
        {
            var seqtriplets = new PTypeSequence(
                    new PTypeUnion(
                        new NamedType("op",
                            new PTypeRecord(
                                new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                new NamedType("char", new PType(PTypeEnumeration.character))
                            )),
                        new NamedType("dp",
                            new PTypeRecord(
                                new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                new NamedType("integer", new PType(PTypeEnumeration.integer)),
                                new NamedType("bool", new PType(PTypeEnumeration.boolean))
                            )
                        )
                    ));

            var testdb = new object[] {
                new object[] { 0, new object[] {"a", 'c'}},
                new object[] { 0, new object[] {"a1", 'c'}},
                new object[] { 1, new object[] {"da", 3, true}},
                new object[] { 0, new object[] {"a1", '1'}},
                new object[] { 1, new object[] {"da1", 7, true}},
                new object[] { 0, new object[] {"a2", '2'}},
                new object[] { 1, new object[] {"da2", 9, false}},
                new object[] { 1, new object[] {"da2", 11, true}},
                new object[] { 1, new object[] {"da3", 13, false}}
            };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

            xCell.Fill2(testdb);

            CheckSequence(xCell);
        }

        [TestMethod]
        public void Fill2WholeVolumeSeqUnionTest()
        {

            var seqtriplets = new PTypeSequence(
                                new PTypeUnion(
                                    new NamedType("op",
                                        new PTypeRecord(
                                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("char", new PType(PTypeEnumeration.character))
                                        )),
                                    new NamedType("dp",
                                        new PTypeRecord(
                                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                            new NamedType("integer", new PType(PTypeEnumeration.integer)),
                                            new NamedType("bool", new PType(PTypeEnumeration.boolean))
                                        )
                                    )
                                ));

            var testdb = new object[] {
                new object[] { 0, new object[] {"a", 'c'}},
                new object[] { 0, new object[] {"a1", 'c'}},
                new object[] { 1, new object[] {"da", 3, true}},
                new object[] { 0, new object[] {"a1", '1'}},
                new object[] { 1, new object[] {"da1", 7, true}},
                new object[] { 0, new object[] {"a2", '2'}},
                new object[] { 1, new object[] {"da2", 9, false}},
                new object[] { 1, new object[] {"da2", 11, true}},
                new object[] { 1, new object[] {"da3", 13, false}}
            };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

            xCell.Fill2(testdb);

            CheckSequence(xCell);

        }

        private static void CheckSequence(PxCell xCell)
        {

            try
            {
                Assert.AreEqual(9, xCell.Root.Count());

                var e = (object[])xCell.Root.Element(0).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 0);
                var o = (object[])e[1];
                Assert.AreEqual(o[0], "a");
                Assert.AreEqual(o[1], 'c');

                e = (object[])xCell.Root.Element(2).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "da");
                Assert.AreEqual(o[1], 3);
                Assert.AreEqual(o[2], true);

                e = (object[])xCell.Root.Element(3).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a1");
                Assert.AreEqual(o[1], '1');

                e = (object[])xCell.Root.Element(4).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "da1");
                Assert.AreEqual(o[1], 7);
                Assert.AreEqual(o[2], true);

                e = (object[])xCell.Root.Element(5).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a2");
                Assert.AreEqual(o[1], '2');

                e = (object[])xCell.Root.Element(7).Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "da2");
                Assert.AreEqual(o[1], 11);
                Assert.AreEqual(o[2], true);

                e = (object[])xCell.Root.Elements().ToArray()[0].Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a");
                Assert.AreEqual(o[1], 'c');

                e = (object[])xCell.Root.Elements().ToArray()[5].Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a2");
                Assert.AreEqual(o[1], '2');

                e = (object[])xCell.Root.Elements().ToArray()[7].Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "da2");
                Assert.AreEqual(o[1], 11);
                Assert.AreEqual(o[2], true);

                e = (object[])xCell.Root.Elements().Last().Get().Value;
                Assert.IsInstanceOfType(e[1], typeof(object[]));
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "da3");
                Assert.AreEqual(o[1], 13);
                Assert.AreEqual(o[2], false);

            }
            finally
            {
                xCell.Close();
            }
        }

        [TestMethod]
        public void FillWholeVolumeSeqRecordTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));
            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21, 90 };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            xCell.Fill(testdb);

            try
            {
                Assert.AreEqual(8, xCell.Root.Count());
                Assert.AreEqual(1, (int)xCell.Root.Element(0).Get().Value);
                Assert.AreEqual(3, (int)xCell.Root.Element(2).Get().Value);
                Assert.AreEqual(5, (int)xCell.Root.Element(3).Get().Value);
                Assert.AreEqual(15, (int)xCell.Root.Element(4).Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Element(5).Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Element(7).Get().Value);

                var objects = xCell.Root.Elements().Select(e => e.Get().Value).ToArray();
                Assert.AreEqual(1, (int)xCell.Root.Elements().ToArray()[0].Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Elements().ToArray()[5].Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Elements().ToArray()[7].Get().Value);

            }
            finally
            {
                xCell.Close();
            }
        }

        [TestMethod]
        public void Fill2WholeVolumeSeqRecordTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));
            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21, 90 };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            xCell.Fill2(testdb);

            try
            {
                Assert.AreEqual(8, xCell.Root.Count());
                Assert.AreEqual(1, (int)xCell.Root.Element(0).Get().Value);
                Assert.AreEqual(3, (int)xCell.Root.Element(2).Get().Value);
                Assert.AreEqual(5, (int)xCell.Root.Element(3).Get().Value);
                Assert.AreEqual(15, (int)xCell.Root.Element(4).Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Element(5).Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Element(7).Get().Value);

                var objects = xCell.Root.Elements().Select(e => e.Get().Value).ToArray();
                Assert.AreEqual(1, (int)xCell.Root.Elements().ToArray()[0].Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Elements().ToArray()[5].Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Elements().ToArray()[7].Get().Value);

            }
            finally
            {
                xCell.Close();
            }
        }

        [TestMethod]
        public void FillVolumedSeqRecordWithRestTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));
            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21, 90, 100, 13 };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            xCell.Fill(testdb);

            try
            {
                Assert.AreEqual(10, xCell.Root.Count());
                Assert.AreEqual(1, (int)xCell.Root.Element(0).Get().Value);
                Assert.AreEqual(3, (int)xCell.Root.Element(2).Get().Value);
                Assert.AreEqual(5, (int)xCell.Root.Element(3).Get().Value);
                Assert.AreEqual(15, (int)xCell.Root.Element(4).Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Element(5).Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Element(7).Get().Value);
                Assert.AreEqual(13, (int)xCell.Root.Element(9).Get().Value);

                var objects = xCell.Root.Elements().Select(e => e.Get().Value).ToArray();
                Assert.AreEqual(1, (int)xCell.Root.Elements().ToArray()[0].Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Elements().ToArray()[5].Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Elements().ToArray()[7].Get().Value);
                Assert.AreEqual(100, (int)xCell.Root.Elements().ToArray()[8].Get().Value);

            }
            finally
            {
                xCell.Close();
            }
        }

        [TestMethod]
        public void Fill2VolumedSeqRecordWithRestTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));
            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21, 90, 100, 13 };

            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            xCell.Fill2(testdb);

            try
            {
                Assert.AreEqual(10, xCell.Root.Count());
                Assert.AreEqual(1, (int)xCell.Root.Element(0).Get().Value);
                Assert.AreEqual(3, (int)xCell.Root.Element(2).Get().Value);
                Assert.AreEqual(5, (int)xCell.Root.Element(3).Get().Value);
                Assert.AreEqual(15, (int)xCell.Root.Element(4).Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Element(5).Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Element(7).Get().Value);
                Assert.AreEqual(13, (int)xCell.Root.Element(9).Get().Value);

                var objects = xCell.Root.Elements().Select(e => e.Get().Value).ToArray();
                Assert.AreEqual(1, (int)xCell.Root.Elements().ToArray()[0].Get().Value);
                Assert.AreEqual(19, (int)xCell.Root.Elements().ToArray()[5].Get().Value);
                Assert.AreEqual(90, (int)xCell.Root.Elements().ToArray()[7].Get().Value);
                Assert.AreEqual(100, (int)xCell.Root.Elements().ToArray()[8].Get().Value);

            }
            finally
            {
                xCell.Close();
            }
        }

    }
}
