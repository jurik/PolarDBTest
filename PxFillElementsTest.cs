using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;
using System.Collections.Generic;

namespace PolarDbTest
{
    [TestClass]
    public class PxFillElementsTest
    {
        private PType seqtriplets;
        private object[] testdb;

        [TestInitialize]
        public void PxCellInit()
        {
            string testpacfilename = "test.pxc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }

            seqtriplets = new PTypeSequence(
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

            testdb = new object[] {
                new object[] { 1, new object[] {"a", "b", "c"}},
                new object[] { 1, new object[] {"a1", "b1", "c1"}},
                new object[] { 2, new object[] {"da", "db", "dc", "lang"}}
            };
        }

        [TestCleanup]
        public void PxCellCleanUp()
        {
            string testpacfilename = "test.pxc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
        }

        [TestMethod]
        public void FillTest()
        {
            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            try
            {
                xCell.Fill(testdb);
                Assert.AreEqual(3, xCell.Root.Count());
                var e = (object[])xCell.Root.Element(2).Get().Value;
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
                xCell.Close();
            }
        }

        [TestMethod]
        public void Fill2Test()
        {
            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);
            try
            {
                xCell.Fill2(testdb);
                Assert.AreEqual(3, xCell.Root.Count());
                var e = (object[])xCell.Root.Element(2).Get().Value;
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
                xCell.Close();
            }
        }

        [TestMethod]
        public void TestFillPxCell2()
        {
            seqtriplets = new PTypeSequence(
                   new PTypeRecord(
                            new NamedType("test1", new PType(PTypeEnumeration.sstring)),
                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                            new NamedType("lang", new PType(PTypeEnumeration.sstring))
                    )
           );

            testdb = new object[] {
                new object[] {
                        "a",
                        "1L",
                        "dalang"},
                new object[] {
                        "b",
                        "2L",
                        "dalang"},
                new object[] {
                        "c",
                        "3L",
                        "dalang"}
            };


            string testpacfilename = "test.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

            try
            {
                xCell.Fill2(testdb);
                var i = xCell.Root.Count();
                var e = (object[])xCell.Root.Element(0).Get().Value;
            }
            finally
            {
                xCell.Close();
            }

        }

      [TestMethod]
        public void TestFillPxCellWhenRecordHasLongValue()
        {
            seqtriplets = new PTypeSequence(
                new PTypeUnion(
                    new NamedType("op",
                        new PTypeRecord(
                            new NamedType("test1", new PType(PTypeEnumeration.sstring)))),
                    new NamedType("testindex", new PType(PTypeEnumeration.longinteger)),
                    new NamedType("dp",
                        new PTypeRecord(
                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                            new NamedType("lang", new PType(PTypeEnumeration.sstring)))))
                            );

            testdb = new object[] {
                new object[] { 0, new object[] {"a"}},
                new object[] { 1, 4L},
                new object[] { 2, new object[] {"da", "lang"}}
            };


            string testpacfilename = "test1.pxc";
            PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

            try
            {
                xCell.Fill2(testdb);
                Assert.AreEqual(3, xCell.Root.Count());
                var e = (object[])xCell.Root.Element(0).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(((object[])e[1])[0], "a");
                var e1 = (object[])xCell.Root.Element(1).Get().Value;
                Assert.AreEqual(e1.Length, 2);
                Assert.AreEqual(e1[0], 1);
                Assert.AreEqual(e1[1], 4L);

                var e2 = (object[])xCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e2.Length, 2);
                Assert.AreEqual(((object[])e2[1])[0], "da");
                Assert.AreEqual(((object[])e2[1])[1], "lang");
            }
            finally
            {
                xCell.Close();
            }

        }

      [TestMethod]
      public void TestFillPxCellWhenRecordHasInt32Value()
      {
          seqtriplets = new PTypeSequence(
              new PTypeUnion(
                  new NamedType("op",
                      new PTypeRecord(
                          new NamedType("test1", new PType(PTypeEnumeration.sstring)))),
                  new NamedType("testindex", new PType(PTypeEnumeration.integer)),
                  new NamedType("dp",
                      new PTypeRecord(
                          new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                          new NamedType("lang", new PType(PTypeEnumeration.sstring)))))
                          );

          testdb = new object[] {
                new object[] { 0, new object[] {"a"}},
                new object[] { 1, 4},
                new object[] { 2, new object[] {"da", "lang"}}
            };


          string testpacfilename = "test.pxc";
          PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

          try
          {
              xCell.Fill2(testdb);
              Assert.AreEqual(3, xCell.Root.Count());
              var e = (object[])xCell.Root.Element(0).Get().Value;
              Assert.AreEqual(e.Length, 2);
              Assert.AreEqual(((object[])e[1])[0], "a");
              var e1 = (object[])xCell.Root.Element(1).Get().Value;
              Assert.AreEqual(e1.Length, 2);
              Assert.AreEqual(e1[0], 1);
              Assert.AreEqual(e1[1], 4);

              var e2 = (object[])xCell.Root.Element(2).Get().Value;
              Assert.AreEqual(e2.Length, 2);
              Assert.AreEqual(((object[])e2[1])[0], "da");
              Assert.AreEqual(((object[])e2[1])[1], "lang");
          }
          finally
          {
              xCell.Close();
          }
      }

      [TestMethod]
      public void TestFillPxCellWhenRecordHasCharValue()
      {
          seqtriplets = new PTypeSequence(
              new PTypeUnion(
                  new NamedType("op",
                      new PTypeRecord(
                          new NamedType("test1", new PType(PTypeEnumeration.sstring)))),
                  new NamedType("testindex", new PType(PTypeEnumeration.character)),
                  new NamedType("dp",
                      new PTypeRecord(
                          new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                          new NamedType("lang", new PType(PTypeEnumeration.sstring)))))
                          );

          testdb = new object[] {
                new object[] { 0, new object[] {"a"}},
                new object[] { 1, '4'},
                new object[] { 2, new object[] {"da", "lang"}}
            };


          string testpacfilename = "test.pxc";
          PxCell xCell = new PxCell(seqtriplets, testpacfilename, false);

          try
          {
              xCell.Fill2(testdb);
              Assert.AreEqual(3, xCell.Root.Count());
              var e = (object[])xCell.Root.Element(0).Get().Value;
              Assert.AreEqual(e.Length, 2);
              Assert.AreEqual(((object[])e[1])[0], "a");
              var e1 = (object[])xCell.Root.Element(1).Get().Value;
              Assert.AreEqual(e1.Length, 2);
              Assert.AreEqual(e1[0], 1);
              Assert.AreEqual(e1[1], '4');

              var e2 = (object[])xCell.Root.Element(2).Get().Value;
              Assert.AreEqual(e2.Length, 2);
              Assert.AreEqual(((object[])e2[1])[0], "da");
              Assert.AreEqual(((object[])e2[1])[1], "lang");
          }
          finally
          {
              xCell.Close();
          }
      }
    }
}
