using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KabuUriKai;
using NUnit.Framework;
using System.Reflection;


namespace KabuUriKaiTest
{
    [TestFixture]
    public class TestJudgeUrikai
    {
        [Test]
        public void TestgetAllFilePathOnDataFolder()
        {
            var cls = new JudgeUrikai();
            Type type = cls.GetType();
            var expectedList = CreateFilePathData();
            
            MethodInfo methodInfo = type.GetMethod("getAllFilePathOnDataFolder", BindingFlags.NonPublic | BindingFlags.Instance);

            var actualList = (List<string>)methodInfo.Invoke(cls, new object[] {});

            // リストが同数か確認
            Assert.AreEqual(expectedList.Count, actualList.Count);

            // 中身の日付が同じか確認
            var expected = true;
            expectedList.ForEach(t =>
            {
                var actual = actualList.Exists(x => x.ToString() == t.ToString());
                Assert.AreEqual(expected, actual);
            });
        }

        [Test]
        public void TestgetCodeList()
        {
            var cls = new JudgeUrikai();
            Type type = cls.GetType();
            var expectedList = CreateCodeList();

            MethodInfo methodInfo = type.GetMethod("getCodeList", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo methodInfo2 = type.GetMethod("getAllFilePathOnDataFolder", BindingFlags.NonPublic | BindingFlags.Instance);

            var filePathList = (List<string>)methodInfo2.Invoke(cls, new object[] { } );

            var actualList = (List<JudgeUrikaiData>)methodInfo.Invoke(cls, new object[] { filePathList });

            // リストが同数か確認
            Assert.AreEqual(expectedList.Count, actualList.Count);

            // 中身のCodeが同じか確認
            var expected = true;
            expectedList.ForEach(t =>
            {
                var actual = actualList.Exists(x => x.Code == t.Code);
                Assert.AreEqual(expected, actual);
            });
        }

        [Test]
        public void TestCalcJudgeUrikai()
        {
            var cls = new JudgeUrikai();
            Type type = cls.GetType();
            var expectedList = CreateUrikaiDataList();

            MethodInfo methodInfo = type.GetMethod("getCodeList", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo methodInfo2 = type.GetMethod("getAllFilePathOnDataFolder", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo methodInfo3 = type.GetMethod("CalcJudgeUrikai", BindingFlags.NonPublic | BindingFlags.Instance);

            var filePathList = (List<string>)methodInfo2.Invoke(cls, new object[] { });

            var urikaiDataList = (List<JudgeUrikaiData>)methodInfo.Invoke(cls, new object[] { filePathList });

            // Test Target
            methodInfo3.Invoke(cls, new object[] { filePathList, urikaiDataList});

            var actualList = urikaiDataList;

            // リストが同数か確認
            Assert.AreEqual(expectedList.Count, actualList.Count);

            // 中身のCodeが同じか確認
            var expected = true;
            expectedList.ForEach(t =>
            {
                var actual = actualList.Exists(x => x.Code == t.Code);
                Assert.AreEqual(expected, actual);

                var query =
                    from list in actualList
                    where list.Code.Contains(t.Code)
                    select list;

                // 必ず一個になる
                JudgeUrikaiData judge = (JudgeUrikaiData)query;

                Assert.AreEqual(expected, judge.Avg.Equals(t.Avg));
                Assert.AreEqual(expected, judge.Urikai == false);
            });

        }

        private List<JudgeUrikaiData> CreateUrikaiDataList()
        {
            var judgeUrikaiDataList = new List<JudgeUrikaiData>();

            judgeUrikaiDataList.Add(new JudgeUrikaiData("10", "TOPIX", 1164.70, false, 1164.70));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("11", "TOPIXコア30", 611.13,false,611.13));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("12", "TOPIXラージ70",1086.12,false,1086.12));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("13","TOPIX100", 772.48, false, 772.48));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("14", "TOPIXミッド400",1254.95,false,1254.95));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("15","TOPIX500",908.04,false,908.04));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("16", "TOPIXスモール",1358.22,false,1358.22));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("17","TOPIX1000",1101.83,false,1101.83));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("9996-T","サトー商会",934,false,934));
            judgeUrikaiDataList.Add(new JudgeUrikaiData("9997-T","ベルーナ",472,false,472));

            return judgeUrikaiDataList;
        }

        private List<string> CreateFilePathData()
        {
            var ret = new List<string>();

            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-10.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-11.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-12.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-13.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-14.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-17.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-18.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-19.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-20.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-21.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-24.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-25.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-26.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-27.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-02-28.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-03.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-04.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-05.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-06.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-07.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-10.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-11.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-12.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-13.dat");
            ret.Add(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-14.dat");
            
            return ret;
            
        }

        private List<JudgeUrikaiData> CreateCodeList()
        {
            var ret = new List<JudgeUrikaiData>();

            ret.Add(new JudgeUrikaiData("10"));
            ret.Add(new JudgeUrikaiData("11"));
            ret.Add(new JudgeUrikaiData("12"));
            ret.Add(new JudgeUrikaiData("13"));
            ret.Add(new JudgeUrikaiData("14"));
            ret.Add(new JudgeUrikaiData("15"));
            ret.Add(new JudgeUrikaiData("16"));
            ret.Add(new JudgeUrikaiData("17"));
            ret.Add(new JudgeUrikaiData("9996-T"));
            ret.Add(new JudgeUrikaiData("9997-T"));

            return ret;
        }
    }
}
