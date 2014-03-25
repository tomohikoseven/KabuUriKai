using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;

namespace KabuUriKai.Page
{
    public class JudgeDataList : INotifyPropertyChanged
    {
        private ObservableCollection<JudgeData> dataList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<JudgeData> DataList
        {
            get
            {
                return dataList;
            }
            set
            {
                if (object.ReferenceEquals(dataList, value) != true)
                {
                    dataList = value;
                    NotifyPropertyChanged("DataList");
                }
            }
        }
        
        public JudgeDataList()
        {
            dataList = null;
            //dataList = new ObservableCollection<JudgeData>
            //{
            //    new JudgeData { Code = "10", Name="TOPIX", Judge="買い", Avg="10", Owarine = "11" },
            //    new JudgeData { Code = "11", Name="TOPIXコア30", Judge="売り", Avg="20", Owarine="25" }
            //};
        }



        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        protected virtual ObservableCollection<JudgeData> GetJudgeDataList(int start, int itemCount, string sortColumn, bool ascending, out int totalItems)
        {
            /// データ取得
            /// 
            try
            {
                /// メソッド呼び出し（初回）対処
                if (DataList == null)
                {
                    CreateDataList();
                }

            }
            catch (FileNotFoundException)
            {
                /// 空データ
                var _dataList = new ObservableCollection<JudgeData>() { new JudgeData() };
                DataList = _dataList;
                totalItems = 0;
                return DataList;
            }
            totalItems = DataList.Count;

            /// ソート
            /// 
            var sortedJudgeDataList = sortDataList(sortColumn);
            sortedJudgeDataList = ascending ? sortedJudgeDataList : new ObservableCollection<JudgeData>(sortedJudgeDataList.Reverse());

            /// ページ分のデータにフィルター
            /// 
            var filteredDataList = filterDataList(start, itemCount, totalItems, sortedJudgeDataList);
            return filteredDataList;
        }

        /// <summary>
        /// ページ分のデータを返却する
        /// </summary>
        /// <param name="start"></param>
        /// <param name="itemCount"></param>
        /// <param name="totalItems"></param>
        /// <param name="sortedJudgeDataList"></param>
        /// <returns></returns>
        private static ObservableCollection<JudgeData> filterDataList(int start, int itemCount, int totalItems, ObservableCollection<JudgeData> sortedJudgeDataList)
        {
            ObservableCollection<JudgeData> _filteredDataList = new ObservableCollection<JudgeData>();
            for (int i = start; i < start + itemCount && i < totalItems; i++)
            {
                _filteredDataList.Add(sortedJudgeDataList[i]);
            }
            return _filteredDataList;
        }

        /// <summary>
        /// データを指定したプロパティでソートする
        /// </summary>
        /// <param name="sortColumn"></param>
        /// <returns></returns>
        private ObservableCollection<JudgeData> sortDataList(string sortColumn)
        {
            ObservableCollection<JudgeData> _sortedJudgeDataList = new ObservableCollection<JudgeData>();
            switch (sortColumn)
            {
                case ("Code"):
                    _sortedJudgeDataList = new ObservableCollection<JudgeData>
                    (
                      from p in DataList
                      orderby p.Code
                      select p
                    );
                    break;
                case ("Name"):
                    _sortedJudgeDataList = new ObservableCollection<JudgeData>
                    (
                      from p in DataList
                      orderby p.Name
                      select p
                    );
                    break;
                case ("Judge"):
                    _sortedJudgeDataList = new ObservableCollection<JudgeData>
                    (
                        from p in DataList
                        orderby p.Judge
                        select p
                    );
                    break;
                case ("Avg"):
                    _sortedJudgeDataList = new ObservableCollection<JudgeData>
                    (
                        from p in DataList
                        orderby p.Avg
                        select p
                    );
                    break;
                case ("Owarine"):
                    _sortedJudgeDataList = new ObservableCollection<JudgeData>
                    (
                        from p in DataList
                        orderby p.Owarine
                        select p
                    );
                    break;
            }
            return _sortedJudgeDataList;
        }

        /// <summary>
        /// ローカルから売買判定ファイル(judgeFile.csv)を取得し、DataListへ設定する
        /// </summary>
        private void CreateDataList()
        {
            string[] plines = null;

            /// ローカルから売買判定ファイル(judgeFile.csv)を取得し、DataListへ設定する
            /// 
            try
            {
                plines = File.ReadAllLines(@"C:\Users\atx01\Documents\GitHub\KabuUrikai\judge\judgeFile.csv", Encoding.GetEncoding("Shift_JIS"));
            }
            catch (FileNotFoundException)
            {
                DataList = null;
                throw;
            }

            /// ファイル内を引っこ抜く
            /// 
            var query =
                from pline in plines.Skip(1)
                let p = pline.Split(',')
                select new JudgeData
                {
                    Code = p[0],
                    Name = p[1],
                    Judge = p[2],
                    Avg = p[3],
                    Owarine = p[4]
                };

            /// 格納する
            /// 
            var _dataList = new ObservableCollection<JudgeData>();

            foreach (var data in query)
            {
                _dataList.Add(data);
            }

            DataList = _dataList;

        }
    }
}
