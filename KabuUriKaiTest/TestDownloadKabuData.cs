using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KabuUriKai;
using NUnit.Framework;
using System.Reflection;
using System.IO;

namespace KabuUriKaiTest
{
    /// <summary>
    /// テストクラス
    /// </summary>
    [TestFixture]
    public class TestDownloadKabuData
    {

        [Test]
        public void TestIsHeijitsuTrue()
        {
            var cls1 = CreateDownloadKabuData();  
            Type type = cls1.GetType();

            MethodInfo methodInfo = type.GetMethod("IsHeijitsu", BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = (bool)methodInfo.Invoke( cls1, new object[] { new DateTime( 2014 ,3 ,14 ) } );
            var expected = true;

            Assert.AreEqual( expected, actual );
        }

        [Test]
        public void TestIsHeijitsuFalse()
        {
            var cls1 = CreateDownloadKabuData();
            Type type = cls1.GetType();

            MethodInfo methodInfo = type.GetMethod("IsHeijitsu", BindingFlags.NonPublic | BindingFlags.Instance);

            var actual = (bool)methodInfo.Invoke(cls1, new object[] { new DateTime(2014, 3, 15) });
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// DownloadKabuData()のテスト
        /// </summary>
        [Test]
        public void TestGetTorihikibiList()
        { 
            var cls1 = CreateDownloadKabuData();  
            Type type = cls1.GetType();

            MethodInfo methodInfo = type.GetMethod("GetTorihikibiList", BindingFlags.NonPublic | BindingFlags.Instance);
            

            var actualList = (List<DateTime>)methodInfo.Invoke(cls1, new object[] { new DateTime(2014, 3, 14) });
            var expectedList = CreateHeijitsuDateList();
 

            // ---------------
            // テスト
            // ---------------

            // リストが同数か確認
            Assert.AreEqual(expectedList.Count, actualList.Count);

            // 中身の日付が同じか確認
            var expected = true; 
            expectedList.ForEach(t => 
            {
                var actual = actualList.Exists(x => x.ToString() == t.ToString());
                Assert.AreEqual(expected, actual);
            } );
              
        }

        /// <summary>
        /// ファイルダウンロードテスト
        /// </summary>
        [Test]
        public void Testdownload()
        {
            var cls1 = CreateDownloadKabuData();
            Type type = cls1.GetType();

            MethodInfo methodInfo = type.GetMethod("download", BindingFlags.NonPublic | BindingFlags.Instance);

            var date = new DateTime(2014, 3, 13);

            methodInfo.Invoke(cls1, new object[] { date });
            var expected = true;

            Assert.AreEqual(expected, File.Exists(@"C:\Users\tomohiko\Documents\01_kabu\tool\data\2014-03-13.dat"));

        }

        [Test]
        public void TestdownloadKabuList25Days()
        {
            var cls1 = CreateDownloadKabuData();
            cls1.downloadKabuList25Days();

            Assert.AreEqual(true, true);
        }

        private DownloadKabuData CreateDownloadKabuData()
        {
            return new DownloadKabuData();
        }

        private Type CreateDataAndType()
        {
            var data = CreateDownloadKabuData();
            return data.GetType();
        }

        private MethodInfo GetMethodInfo( string method )
        {
            Type type = CreateDataAndType();
            return type.GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private List<DateTime> CreateHeijitsuDateList()
        {
            var expectedList = new List<DateTime>();
            expectedList.Add(new DateTime(2014, 3, 14));
            expectedList.Add(new DateTime(2014, 3, 13));
            expectedList.Add(new DateTime(2014, 3, 12));
            expectedList.Add(new DateTime(2014, 3, 11));
            expectedList.Add(new DateTime(2014, 3, 10));

            expectedList.Add(new DateTime(2014, 3, 7));
            expectedList.Add(new DateTime(2014, 3, 6));
            expectedList.Add(new DateTime(2014, 3, 5));
            expectedList.Add(new DateTime(2014, 3, 4));
            expectedList.Add(new DateTime(2014, 3, 3));

            expectedList.Add(new DateTime(2014, 2, 28));
            expectedList.Add(new DateTime(2014, 2, 27));
            expectedList.Add(new DateTime(2014, 2, 26));
            expectedList.Add(new DateTime(2014, 2, 25));
            expectedList.Add(new DateTime(2014, 2, 24));

            expectedList.Add(new DateTime(2014, 2, 21));
            expectedList.Add(new DateTime(2014, 2, 20));
            expectedList.Add(new DateTime(2014, 2, 19));
            expectedList.Add(new DateTime(2014, 2, 18));
            expectedList.Add(new DateTime(2014, 2, 17));

            expectedList.Add(new DateTime(2014, 2, 14));
            expectedList.Add(new DateTime(2014, 2, 13));
            expectedList.Add(new DateTime(2014, 2, 12));
            expectedList.Add(new DateTime(2014, 2, 11));
            expectedList.Add(new DateTime(2014, 2, 10));

            return expectedList;
        }
    }
}
