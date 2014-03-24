using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace KabuUriKai
{
    public class JudgeDataList
    {
        public ObservableCollection<JudgeData> DataList { get; set; }
        public JudgeDataList()
        {
            DataList = new ObservableCollection<JudgeData>
            {
                new JudgeData { Code = "10", Name="TOPIX", Judge="買い", Avg="10", Owarine = "11" },
                new JudgeData { Code = "11", Name="TOPIXコア30", Judge="売り", Avg="20", Owarine="25" }
            };
        }

        public void CreateDataList()
        {
            /// ローカルから売買判定ファイル(judgeFile.csv)を取得し、
            /// DataListへ設定する
            /// 

            var plines = File.ReadAllLines( @"C:\Users\atx01\Documents\GitHub\KabuUrikai\judge\judgeFile.csv", Encoding.GetEncoding("Shift_JIS"));

            var query =
                from pline in plines.Skip(1)
                let p = pline.Split(',')
                select new JudgeData { 
                    Code = p[0], 
                    Name = p[1], 
                    Judge = p[2], 
                    Avg = p[3], 
                    Owarine = p[4] 
                };

            DataList = new ObservableCollection<JudgeData>();
            foreach (var data in query)
            {
                DataList.Add(data);
            }
        }
    }
}
