using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KabuUriKai.Page;
using KabuUriKai.Download;
using KabuUriKai.Judge;

namespace KabuUriKai
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        Paging _page = new Paging();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _page;
            _page.RefreshProducts();
        }

        //private void BtnJudge_Click(object sender, RoutedEventArgs e)
        //{
        //    var judgeDataList = new JudgeDataList();

        //    /// JudgeFile.csv からデータを取得し、JudgeDataListへデータを格納する
        //    /// 
        //    judgeDataList.CreateDataList();

        //    kabuDataGrid.ItemsSource = judgeDataList.DataList;

        //}


        private DataGridColumn currentSortColumn;

        private ListSortDirection currentSortDirection;

        /// <summary>
        /// Custom sort the datagrid since the actual records are stored in the
        /// server, not in the items collection of the datagrid.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Contains the column to be sorted.</param>
        private void kabuDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            string sortField = String.Empty;

            // Use a switch statement to check the SortMemberPath
            // and set the sort column to the actual column name. In this case,
            // the SortMemberPath and column names match.
            switch (e.Column.SortMemberPath)
            {
                case ("Code"):
                    sortField = "Code";
                    break;
                case ("Name"):
                    sortField = "Name";
                    break;
                case ("Judge"):
                    sortField = "Judge";
                    break;
                case ("Avg"):
                    sortField = "Avg";
                    break;
                case ("Owarine"):
                    sortField = "Owarine";
                    break;
            }

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
              ListSortDirection.Ascending : ListSortDirection.Descending;
            bool sortAscending = direction == ListSortDirection.Ascending;
            _page.Sort(sortField, sortAscending);
            currentSortColumn.SortDirection = null;
            e.Column.SortDirection = direction;
            currentSortColumn = e.Column;
            currentSortDirection = direction;
        }

        /// <summary>
        /// Sets the sort direction for the current sorted column since the sort direction
        /// is lost when the DataGrid's ItemsSource property is updated.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Ignored.</param>
        private void kabuDataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (currentSortColumn != null)
            {
                currentSortColumn.SortDirection = currentSortDirection;
            }
        }

        private void kabuDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // The current sorted column must be specified in XAML.
            currentSortColumn = dataGrid.Columns.Where(c => c.SortDirection.HasValue).Single();
            currentSortDirection = currentSortColumn.SortDirection.Value;
        }

        private void btnJudge_Click(object sender, RoutedEventArgs e)
        {
            var download = new DownloadKabuData();
            var judge = new JudgeUrikai();

            /// 株データダウンロード
            /// 
            download.downloadKabuList25Days();

            /// 売買判定ファイル作成
            /// 
            judge.CreateJudgeFile();

            /// Gridへ表示
            /// 
            _page.RefreshProducts();
        }
    }
}
