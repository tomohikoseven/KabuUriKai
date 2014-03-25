using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using KabuUriKai.Common;
using KabuUriKai.Log;

namespace KabuUriKai.Download
{
    public class DownloadKabuData : URIData
    {

        public DownloadKabuData() : base()
        {
        }

        /// <summary>
        /// 当日含め２５日分の株リストをダウンロードする
        /// </summary>
        public void downloadKabuList25Days()
        {
            MyLog.Debug("Start.");
            List<DateTime> torihikibiList;

            /// ----------------
            /// 株リストを削除する
            /// ----------------
            fileDeleteAll();

            // 25日分の日付リスト
            // -----------------
            // ２５日分の取引日（平日）リストを取得する
            // -----------------
            torihikibiList = GetTorihikibiList();

            // -----------------
            // ２５日分の株リストを取得する
            // -----------------
            torihikibiList.ForEach(t => { download(t); });
            MyLog.Debug("End.");
        }

        /// <summary>
        /// Dataフォルダ配下のファイルをすべて削除する
        /// </summary>
        private void fileDeleteAll()
        {
            MyLog.Debug("Start.");
            string[] files = null;
            files = getAllFilePathOnDataFolder();

            if (files != null && files.Count() != 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            MyLog.Debug("End.");
        }


        /// <summary>
        /// 指定された日付の株リストを取得する
        /// </summary>
        /// <param name="date">日付</param>
        private void download( DateTime date )
        {
            MyLog.Debug("Start.");
            MyLog.Debug("param[ date = {0} ].", date );

            // ------------------
            // 引数の日付の株リストをダウンロードする
            // ------------------
            var targetDate = date.ToString(FORMAT);
            var saveFileName = saveFolderName + targetDate + ".dat";
            var uri = new Uri(targetUri + targetDate);
            var client = new WebClient();

            // フォルダがなければ、作成する。
            if (!Directory.Exists(saveFolderName))
            {
                Directory.CreateDirectory( saveFolderName );
                MyLog.Debug("Create saveFolder = {0}.", saveFolderName);
            }

            try
            {
                // ダウンロードする
                client.DownloadFile(uri, saveFileName);
                MyLog.Debug("Download file = {0}", saveFileName);
            }
            catch (WebException we)
            {
                MyLog.logger.Error(we);
                MyLog.logger.Error( uri.ToString() + "のダウンロードに失敗した。" );
            }
            MyLog.Debug("End.");
        }

        /// <summary>
        /// 今日より前の25日分の取引日を計算し、リストで返却する
        /// </summary>
        /// <returns>今日以前の取引日リスト</returns>
        private List<DateTime> GetTorihikibiList()
        {
            var today = DateTime.Today.AddDays(-1);
            var count = 25;
            var cnt = 0;
            var dateList = new List<DateTime>();

            // 25日分の取引日リストを作成
            for (var t = today; cnt != count; t = t.AddDays(-1) )
            {
                if (IsHeijitsu(t))
                {
                    dateList.Add(t);
                    cnt++;
                }
            }

            return dateList;
            
        }

        /// <summary>
        /// 指定された日にちが平日かどうか判定する。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>True：平日、False:土日祝祭日</returns>
        private bool IsHeijitsu(DateTime date)
        {
            MyLog.Debug("Start.");
            MyLog.Debug("param[ date = {0} ].", date);

            var ret = false;

            // 平日の場合、true
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday :
                    ret = false;
                    break;
                case DayOfWeek.Monday :
                    ret = true;
                    break;
                case DayOfWeek.Tuesday :
                    ret = true;
                    break;
                case DayOfWeek.Wednesday :
                    ret = true;
                    break;
                case DayOfWeek.Thursday :
                    ret = true;
                    break;
                case DayOfWeek.Friday :
                    ret = true;
                    break;
                case DayOfWeek.Saturday :
                    ret = false;
                    break;
                default:
                    throw new NotImplementedException();

            }

            MyLog.Debug("return = {0}.", ret);
            MyLog.Debug("End.");
            return ret;
        }

    }
}
