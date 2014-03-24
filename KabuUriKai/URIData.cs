﻿using System.IO;

namespace KabuUriKai
{
    public class URIData
    {
        // ダウンロードファイルの保存先
        protected static string saveFolderName = @"C:\Users\tomohiko\Documents\01_kabu\tool\data";
        protected static string targetUri = @"http://k-db.com/site/download.aspx?p=all&download=csv&date=";
        protected static string FORMAT = "yyyy-MM-dd";

        /// <summary>
        /// Dataフォルダ配下のファイルのファイルパスを取得する
        /// </summary>
        /// <returns>ファイルパス（文字列）配列</returns>
        public static string[] getAllFilePathOnDataFolder()
        {
            string[] files = null;
            if (Directory.Exists(saveFolderName))
            {
                files = Directory.GetFiles(saveFolderName, "*", SearchOption.AllDirectories);
            }
            return files;
        }
    }
}
