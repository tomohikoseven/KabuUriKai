using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace KabuUriKai.Judge
{
    public class JudgeUrikai
    {
        private static int CODE = 0;        // コード
        private static int MEIGARA = 1;     // 銘柄名
        private static int OWARINE = 7;     // 終値
        private static string SHIFT_JIS = "Shift_JIS";

        private static int HEADERSKIP = 2;  // 株データファイル内ヘッダスキップ数

        public JudgeUrikai()
        {
        }

        /// <summary>
        /// data配下の全ファイルパス取得
        /// </summary>
        /// <returns>全ファイルパスのリスト(ソート済み)</returns>
        private List<string> getAllFilePathOnDataFolder()
        {
            // URIdataのgetAllFilePathOnDataFolder()
            var files = URIData.getAllFilePathOnDataFolder();
            var fileList = new List<string>(files);
            fileList.Sort();
        
            return fileList;
        }


        /// 1ファイルから全Code取得
        /// 
        private List<JudgeUrikaiData> getCodeList( List<string> filePathList )
        {
            // Code別の売り買い判定データ
            var judgeUrikaiDataList = new List<JudgeUrikaiData>();

            // ファイルパス取得(最新のdatファイルパス)
            var filePath = filePathList.Last();
         
            // ファイル内データ（Code/銘柄名/終値）取得
            var plines = File.ReadAllLines(filePath, Encoding.GetEncoding(SHIFT_JIS));
            var prefs = 
                from pline in plines.Skip(HEADERSKIP)
                    let p = pline.Split(',')
                select new JudgeUrikaiData( p[CODE],p[MEIGARA],double.Parse(p[OWARINE]) );

            // 取得したデータをListへ格納
            foreach (var pref in prefs)
            {
                judgeUrikaiDataList.Add(pref);
            }

            return judgeUrikaiDataList;
        }

        /// 各Codeに対して平均値、売買判定を計算する
        /// 

        /// ファイル出力する
        /// 
        public void CreateJudgeFile()
        {
            /// data配下のすべてのファイルパスを取得する
            /// 
            List<string> filePathList = getAllFilePathOnDataFolder();

            /// 全Codeを取得する
            /// 
            List<JudgeUrikaiData> judgeUrikaiDataList =  getCodeList( filePathList );

            /// 各Codeに対して、平均値、売買判定をする
            /// 
            CalcJudgeUrikai(filePathList, ref judgeUrikaiDataList);

            /// ファイルに出力する
            /// 

            throw new NotImplementedException();
        }

        /// <summary>
        /// 25日移動平均、売買判定を計算し、judgeUrikaiDataListへ代入する
        /// </summary>
        /// <param name="filePathList">株データのファイルパスリスト</param>
        /// <param name="judgeUrikaiDataList">売買判定データリスト</param>
        private void CalcJudgeUrikai(List<string> filePathList, ref List<JudgeUrikaiData> judgeUrikaiDataList)
        {
            /// 各株データファイルパスに対し、平均値を計算する
            /// 
            foreach( var judgeUrikaiData in judgeUrikaiDataList )
            {
                var code = judgeUrikaiData.Code;

                // 平均値を計算する
                var query =
                        from filePath in filePathList
                        let p = ReadLineOfCode(filePath, code).Split(',')
                        select double.Parse(p[OWARINE]);
                List<double> list = query.ToList();
                judgeUrikaiData.Avg = list.Average();


                // 売買判定をする(true:買い、false:売り)
                judgeUrikaiData.Urikai =
                    judgeUrikaiData.Avg > judgeUrikaiData.Owarine ? true : false ;
            }
            
        }

        /// <summary>
        /// 株データファイルから、引数codeを含む行を返す
        /// </summary>
        /// <param name="filePath">株データファイルパス</param>
        /// <param name="code">コード</param>
        /// <returns></returns>
        private string ReadLineOfCode(string filePath, string code)
        {
            string line = null;

            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(SHIFT_JIS)))
            {
                while (sr.EndOfStream == false)
                {
                    line = sr.ReadLine();
                    if (line.Contains(code))
                    {
                        return line;
                    }
                }
            }

            throw new NotImplementedException();
        }

    }
}
