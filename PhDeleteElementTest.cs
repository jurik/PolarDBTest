using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolarDB;
using System.Collections.Generic;
using System.Linq;

namespace PolarDbTest
{
    [TestClass]
    public class PhDeleteElementTest
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
        public void DeleteAtomTest()
        {
            var seqtriplets = new PTypeSequence(new PType(PTypeEnumeration.integer));

            var testdb = new object[] { 1, 2, 3, 5, 15, 19, 21 };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            hCell.Fill(testdb);
            try
            {
                Assert.AreEqual(7, hCell.Root.Count());
                Assert.AreEqual(5, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(15, (int)hCell.Root.Element(4).Get().Value);
                Assert.AreEqual(19, (int)hCell.Root.Element(5).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(6).Get().Value);

                hCell.DeleteElement(4);
                Assert.AreEqual(6, hCell.Root.Count());
                Assert.AreEqual(5, (int)hCell.Root.Element(3).Get().Value);
                Assert.AreEqual(19, (int)hCell.Root.Element(4).Get().Value);
                Assert.AreEqual(21, (int)hCell.Root.Element(5).Get().Value);
            }
            finally
            {
                hCell.Close();
            }
        }

        [TestMethod]
        public void DeleteInnerElementTest()
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

                hCell.DeleteElement(4);

                Assert.AreEqual(hCell.Root.Count(), 6);

                e = (object[])hCell.Root.Element(2).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 1);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a21");
                Assert.AreEqual(o[1], 22);

                e = (object[])hCell.Root.Element(4).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a15");
                Assert.AreEqual(o[1], "b15");

                e = (object[])hCell.Root.Element(5).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a16");
                Assert.AreEqual(o[1], "b16");

                hCell.DeleteElement(5);

                Assert.AreEqual(hCell.Root.Count(), 5);

                e = (object[])hCell.Root.Element(4).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a15");
                Assert.AreEqual(o[1], "b15");

            }
            finally
            {
                hCell.Close();
            }

        }


        [TestMethod]
        public void DeleteOutterElementTest()
        // добавляю элемент в следующий сегмент последовательности
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
                new object[] { 1, new object[] {"a21", 22}},
           };

            PhCell hCell = new PhCell(seqtriplets, string.Empty, "test");
            hCell.Fill(testdb);
            try
            {
                Assert.AreEqual(hCell.Root.Count(), 2);
                var e = (object[])hCell.Root.Element(1).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 1);
                var o = (object[])e[1];
                Assert.AreEqual(o[0], "a21");
                Assert.AreEqual(o[1], 22);

                int step = 17359;
                for (int i = 2; i < step; i++)
                {
                    hCell.AppendElement(new object[] { 0, new object[] { "a" + i, "b" + i } });
                    Assert.AreEqual(hCell.Root.Count(), i + 1);
                    e = (object[])hCell.Root.Element(i).Get().Value;
                    Assert.AreEqual(e.Length, 2);
                    Assert.AreEqual(e[0], 0);
                    o = (object[])e[1];
                    Assert.AreEqual(o[0], "a" + i);
                    Assert.AreEqual(o[1], "b" + i);
                }


                hCell.AppendElement(new object[] { 0, new object[] { "a" + step, "b" + step } });
                Assert.AreEqual(hCell.Root.Count(), step + 1);

                hCell.AppendElement(new object[] { 0, new object[] { "a" + (step + 1), "b" + (step + 1) } });
                Assert.AreEqual(hCell.Root.Count(), step + 2);

                hCell.AppendElement(new object[] { 0, new object[] { "a" + (step + 2), "b" + (step + 2) } });
                Assert.AreEqual(hCell.Root.Count(), step + 3);

                hCell.DeleteElement(step + 1);
                Assert.AreEqual(hCell.Root.Count(), step + 2);
                e = (object[])hCell.Root.Element(step - 1).Get().Value;
                Assert.AreEqual(e.Length, 2);
                Assert.AreEqual(e[0], 0);
                o = (object[])e[1];
                Assert.AreEqual(o[0], "a" + (step - 1));
                Assert.AreEqual(o[1], "b" + (step - 1));

                var e1 = (object[])hCell.Root.Element(step).Get().Value;
                Assert.AreEqual(e1.Length, 2);
                Assert.AreEqual(e1[0], 0);
                o = (object[])e1[1];
                Assert.AreEqual(o[0], "a" + step);
                Assert.AreEqual(o[1], "b" + step);

                hCell.DeleteElement(step);
                Assert.AreEqual(hCell.Root.Count(), step + 1);
                e = (object[])hCell.Root.Element(step - 2).Get().Value;
                e1 = (object[])hCell.Root.Element(step).Get().Value;

                //проверяем работу списка после добавления элемента
                var s = (object[])hCell.Root.Elements().Last().Get().Value;
                Assert.AreEqual(s.Length, 2);
                Assert.AreEqual(s[0], 0);
                o = (object[])s[1];
                Assert.AreEqual(o[0], "a" + (step + 2));
                Assert.AreEqual(o[1], "b" + (step + 2));


            }
            finally
            {
                hCell.Close();
            }
        }
    }
}
