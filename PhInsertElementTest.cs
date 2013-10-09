using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;
using System.IO;
using System.Linq;

namespace PolarDbTest
{
    /// <summary>
    /// Summary description for PhInsertElementTest
    /// </summary>
    [TestClass]
    public class PhInsertElementTest
    {
        [TestCleanup]
        [TestInitialize]
        public void PxCellCleanUp()
        {
            string testpacfilename = "test.phc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
            testpacfilename = "test.ptc";
            if (File.Exists(testpacfilename)) { File.Delete(testpacfilename); }
        }

        [TestMethod]
        public void InsertAtomTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));

            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21 };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");

            hCell.Fill(testdb);
            try
            {
                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual(3, (int)hCell.Root.Element(2).Get().Value);
                Assert.AreEqual(5, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(6).Get().Value);

                hCell.InsertElement(0, 12);
                Assert.AreEqual(8, hCell.Root.Count());
                Assert.AreEqual(12, (int)hCell.Root.Element(0).Get().Value);
                Assert.AreEqual(3, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(7).Get().Value);

            }
            finally
            {
                hCell.Close();
            }
        }

        [TestMethod]
        public void InsertElementInnerTest()
        {
            var seqtriplets = new PTypeSequence(
                new PTypeUnion(
                    new NamedType("test1",
                        new PTypeRecord(
                            new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                            new NamedType("obj", new PType(PTypeEnumeration.sstring)))),
                    new NamedType("test2",
                        new PTypeRecord(
                            new NamedType("name", new PType(PTypeEnumeration.sstring)),
                            new NamedType("value", new PType(PTypeEnumeration.integer))))
                 ));

            var testdb = new object[] {
                new object[] { 0, new object[] {"a11", "b11"}},
                new object[] { 0, new object[] {"a12", "b12"}},
                new object[] { 1, new object[] {"a21", 22}},
                new object[] { 0, new object[] {"a13", "b13"}},
                new object[] { 0, new object[] {"a14", "b14"}},
                new object[] { 0, new object[] {"a15", "b15"}},
                new object[] { 0, new object[] {"a16", "b16"}},
            };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            hCell.Fill(testdb);
            try
            {
                Assert.AreEqual(hCell.Root.Count(), 7);
                var e = (object[])hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 1);
                var o = (object[])e[1];
                Assert.AreEqual(o[0], "a21");
                Assert.AreEqual(o[1], 22);

                e = (object[])hCell.Root.Element(3).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a13");
                Assert.AreEqual(o[1], "b13");

                e = (object[])hCell.Root.Element(4).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a14");
                Assert.AreEqual(o[1], "b14");

                hCell.InsertElement(4, new object[] { 0, new object[] { "a44", "b44" } });

                Assert.AreEqual(hCell.Root.Count(), 8);


                e = (object[])hCell.Root.Element(4).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a44");
                Assert.AreEqual(o[1], "b44");

                e = (object[])hCell.Root.Element(5).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a14");
                Assert.AreEqual(o[1], "b14");

                e = (object[])hCell.Root.Element(7).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a16");
                Assert.AreEqual(o[1], "b16");


                hCell.InsertElement(0, new object[] { 0, new object[] { "a55", "b55" } });

                Assert.AreEqual(hCell.Root.Count(), 9);

                e = (object[])hCell.Root.Element(0).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a55");
                Assert.AreEqual(o[1], "b55");

                e = (object[])hCell.Root.Element(5).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a44");
                Assert.AreEqual(o[1], "b44");

                e = (object[])hCell.Root.Element(8).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a16");
                Assert.AreEqual(o[1], "b16");
            }
            finally
            {
                hCell.Close();
            }
        }

        [TestMethod]
        public void InsertElementOutterTest()
        {
            var seqtriplets = new PTypeSequence(
                            new PTypeUnion(
                                new NamedType("test1",
                                    new PTypeRecord(
                                        new NamedType("subject", new PType(PTypeEnumeration.sstring)),
                                        new NamedType("obj", new PType(PTypeEnumeration.sstring)))),
                                new NamedType("test2",
                                    new PTypeRecord(
                                        new NamedType("name", new PType(PTypeEnumeration.sstring)),
                                        new NamedType("value", new PType(PTypeEnumeration.integer))))
                             ));

            var testdb = new object[] {
                new object[] { 0, new object[] {"a11", "b11"}},
            };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            hCell.Fill(testdb);
            try
            {
                hCell.InsertElement(1000, new object[] { 0, new object[] { "a12", "b12" } });
                Assert.AreEqual(hCell.Root.Count(), 2);

                object[] e = (object[])hCell.Root.Element(1).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                object[] o = (object[])e[1];
                Assert.AreEqual(o[0], "a12");
                Assert.AreEqual(o[1], "b12");

                int step = 23725;

                for (int i = 1; i < step - 1; i++)
                {
                    hCell.InsertElement(i, new object[] { 0, new object[] { "a" + i, "b" + i } });
                    Assert.AreEqual(hCell.Root.Count(), i + 2);

                    e = (object[])hCell.Root.Element(i).Get().Value;
                    Assert.AreEqual(e.Length, 2);
                    Assert.AreEqual(e[0], 0);
                    o = (object[])e[1];
                    Assert.AreEqual(o[0], "a" + i);
                    Assert.AreEqual(o[1], "b" + i);
                }

                var str = string.Empty;
                hCell.InsertElement(step - 2, new object[] { 0, new object[] { "a003", "b003" } });
                Assert.AreEqual(hCell.Root.Count(), step + 1);

                e = (object[])hCell.Root.Element(step - 3).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a" + (step - 3));
                Assert.AreEqual(o[1], "b" + (step - 3));

                e = (object[])hCell.Root.Element(step - 2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a003");
                Assert.AreEqual(o[1], "b003");

                var s = (object[])hCell.Root.Elements().Last().Get().Value;
                Assert.AreEqual(s.Length, 2);
                Assert.AreEqual(s[0], 0);
                o = (object[])s[1];
                Assert.AreEqual(o[0], "a12");
                Assert.AreEqual(o[1], "b12");

                hCell.InsertElement(step, new object[] { 0, new object[] { "a005", "b005" } });
                Assert.AreEqual(hCell.Root.Count(), step + 2);

                e = (object[])hCell.Root.Element(step - 3).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a" + (step - 3));
                Assert.AreEqual(o[1], "b" + (step - 3));

                e = (object[])hCell.Root.Element(step - 2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a003");
                Assert.AreEqual(o[1], "b003");

                s = (object[])hCell.Root.Elements().Last().Get().Value;
                Assert.AreEqual(s.Length, 2);
                Assert.AreEqual(s[0], 0);
                o = (object[])s[1];
                Assert.AreEqual(o[0], "a12");
                Assert.AreEqual(o[1], "b12");

                hCell.InsertElement(step + 1, new object[] { 0, new object[] { "a006", "b006" } });
                Assert.AreEqual(hCell.Root.Count(), step + 3);

                e = (object[])hCell.Root.Element(step - 3).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a" + (step - 3));
                Assert.AreEqual(o[1], "b" + (step - 3));

                e = (object[])hCell.Root.Element(step - 2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a003");
                Assert.AreEqual(o[1], "b003");

                s = (object[])hCell.Root.Elements().Last().Get().Value;
                Assert.AreEqual(s.Length, 2);
                Assert.AreEqual(s[0], 0);
                o = (object[])s[1];
                Assert.AreEqual(o[0], "a12");
                Assert.AreEqual(o[1], "b12");
            }
            finally
            {
                hCell.Close();
            }
        }
    }
}
