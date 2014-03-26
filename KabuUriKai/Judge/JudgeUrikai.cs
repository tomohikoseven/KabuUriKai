using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace KabuUriKai.Judge
{
    public class JudgeUrikai : URIData
    {
        private static int CODE = 0;        // コード
        private static int MEIGARA = 1;     // 銘柄名
        private static int OWARINE = 7;     // 終値
        private static string SHIFT_JIS = "Shift_JIS";

        private static int HEADERSKIP = 2;  // 株データファイル内ヘッダスキップ数

        public JudgeUrikai()
        {
        }

        /// ファイル出力する
        /// 
        public void CreateJudgeFile()
        {
            /// data配下のすべてのファイルパスを取得する
            /// 
            List<string> filePathList = GetAllFilePathOnDataFolder();

            /// 全Codeを取得する
            /// 
            List<JudgeUrikaiData> judgeUrikaiDataList = getCodeList(filePathList);

            /// 各Codeに対して、平均値、売買判定をする
            /// 
            CalcJudgeUrikai(filePathList, judgeUrikaiDataList);

            /// ファイルに出力する
            /// 
            OutputJudgeFile(judgeUrikaiDataList);

        }

        /// ファイル出力する
        /// 
        private void OutputJudgeFile(List<JudgeUrikaiData> judgeUrikaiDataList)
        {
            if (!Directory.Exists( judgeFolderName ))
            {
                Directory.CreateDirectory(judgeFolderName);
            }

            /// ファイルを開く
            /// 
            using (StreamWriter writer = new StreamWriter(judgeFolderName + "judgeFile.csv",false, Encoding.GetEncoding(SHIFT_JIS)) )
            {
                /// ヘッダを書き込む
                ///
                WriteHeader(writer);

                /// データリストを書き込む
                ///
                WriteData(writer, judgeUrikaiDataList);
            }

        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("Code,Name,Judge,Avg,Owarine");
        }

        private void WriteData(StreamWriter writer, List<JudgeUrikaiData> judgeUrikaiDataList)
        {
            foreach (var data in judgeUrikaiDataList)
            {
                writer.WriteLine("{0},{1},{2},{3},{4}", data.Code, data.Meigara, data.Urikai, data.Avg, data.Owarine );
            }
        }

        /// <summary>
        /// data配下の全ファイルパス取得
        /// </summary>
        /// <returns>全ファイルパスのリスト(ソート済み)</returns>
        private List<string> GetAllFilePathOnDataFolder()
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
                select new JudgeUrikaiData( p[CODE],p[MEIGARA],p[OWARINE] );

            // 取得したデータをListへ格納
            foreach (var pref in prefs)
            {
                judgeUrikaiDataList.Add(pref);
            }

            return judgeUrikaiDataList;
        }

        /// <summary>
        /// 25日移動平均、売買判定を計算し、judgeUrikaiDataListへ代入する
        /// </summary>
        /// <param name="filePathList">株データのファイルパスリスト</param>
        /// <param name="judgeUrikaiDataList">売買判定データリスト</param>
        private void CalcJudgeUrikai(List<string> filePathList, List<JudgeUrikaiData> judgeUrikaiDataList)
        {
            /// 各コードに対し、平均値を計算する
            /// 
            foreach( var judgeUrikaiData in judgeUrikaiDataList )
            {
                var code = judgeUrikaiData.Code;

                // 平均値を計算する
                var query =
                        from filePath in filePathList
                        let p = ReadLineOfCode(filePath, code).Split(',')
                        select p[OWARINE];
                List<string> list = query.ToList();
                judgeUrikaiData.Avg = Average( list );


                // 売買判定をする(true:買い、false:売り)
                judgeUrikaiData.Urikai = Judgement(judgeUrikaiData.Avg, judgeUrikaiData.Owarine);

            }
            
        }

        private string Judgement(double avg, string owarine )
        {
            double result = 0.0;

            if (double.TryParse(owarine, out result))
            {
                return avg > result ? "True" : "False";
            }

            return "-";
        }

        private double Average(List<string> list)
        {
            int count = 0;
            double sum = 0.0;

            foreach (var t in list)
            {
                double result = 0.0;
                if (double.TryParse(t, out result ))
                {
                    sum += result;
                    count++;
                }
            }

            if (count == 0)
            {
                return 0;
            }

            return sum / count;
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
                    var codeOnLine = line.Split(',').ElementAt(0);
                    if (code == codeOnLine)
                    {
                        return line;
                    }
                }
            }

            // 対象コードが見つからない場合
            return  code + ",-,-,-,-,-,-,-,-,-";
        }

    }
}
