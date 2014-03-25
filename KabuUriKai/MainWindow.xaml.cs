using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace KabuUriKai
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DataGridColumn currentSortColumn;

        private ListSortDirection currentSortDirection;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RefreshProducts();
        }

        //private void BtnJudge_Click(object sender, RoutedEventArgs e)
        //{
        //    var judgeDataList = new JudgeDataList();

        //    /// JudgeFile.csv からデータを取得し、JudgeDataListへデータを格納する
        //    /// 
        //    judgeDataList.CreateDataList();

        //    kabuDataGrid.ItemsSource = judgeDataList.DataList;

        //}

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
                case("Avg"):
                    sortField = "Avg";
                    break;
                case("Owarine"):
                    sortField = "Owarine";
                    break;
            }

            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
              ListSortDirection.Ascending : ListSortDirection.Descending;
            bool sortAscending = direction == ListSortDirection.Ascending;
            Sort(sortField, sortAscending);
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

                public event PropertyChangedEventHandler PropertyChanged;

                private ObservableCollection<JudgeData> judgeDatas;
        private int start = 0;
        private int itemCount = 5;
        private string sortColumn = "Code";
        private bool ascending = true;
        private int totalItems = 0;
        private ICommand firstCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand lastCommand;

        public ObservableCollection<JudgeData> JudgeDatas
        {
            get
            {
                return judgeDatas;
            }
            private set
            {
                if (object.ReferenceEquals(judgeDatas, value) != true)
                {
                    judgeDatas = value;
                    NotifyPropertyChanged("JudgeDatas");
                }
            }
        }

        /// <summary>
        /// Gets the index of the first item in the products list.
        /// </summary>
        public int Start { get { return start + 1; } }

        /// <summary>
        /// Gets the index of the last item in the products list.
        /// </summary>
        public int End { get { return start + itemCount < totalItems ? start + itemCount : totalItems; } }

        /// <summary>
        /// The number of total items in the data store.
        /// </summary>
        public int TotalItems { get { return totalItems; } }

        /// <summary>
        /// Gets the command for moving to the first page of products.
        /// </summary>
        public ICommand FirstCommand
        {
            get
            {
                if (firstCommand == null)
                {
                    firstCommand = new RelayCommand
                    (
                      param =>
                      {
                          start = 0;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start - itemCount >= 0 ? true : false;
                      }
                    );
                }

                return firstCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the previous page of products.
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new RelayCommand
                    (
                      param =>
                      {
                          start -= itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start - itemCount >= 0 ? true : false;
                      }
                    );
                }

                return previousCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the next page of products.
        /// </summary>
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand
                    (
                      param =>
                      {
                          start += itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start + itemCount < totalItems ? true : false;
                      }
                    );
                }

                return nextCommand;
            }
        }

        /// <summary>
        /// Gets the command for moving to the last page of products.
        /// </summary>
        public ICommand LastCommand
        {
            get
            {
                if (lastCommand == null)
                {
                    lastCommand = new RelayCommand
                    (
                      param =>
                      {
                          start = (totalItems / itemCount - 1) * itemCount;
                          start += totalItems % itemCount == 0 ? 0 : itemCount;
                          RefreshProducts();
                      },
                      param =>
                      {
                          return start + itemCount < totalItems ? true : false;
                      }
                    );
                }

                return lastCommand;
            }
        }

        /// <summary>
        /// Sorts the list of products.
        /// </summary>
        /// <param name="sortColumn">The column or member that is the basis for sorting.</param>
        /// <param name="ascending">Set to true if the sort</param>
        public void Sort(string sortColumn, bool ascending)
        {
            this.sortColumn = sortColumn;
            this.ascending = ascending;
            RefreshProducts();
        }

        /// <summary>
        /// Refreshes the list of products. Called by navigation commands.
        /// </summary>
        private void RefreshProducts()
        {
            JudgeDatas = JudgeDataList.GetjudgeDataList(start, itemCount, sortColumn, ascending, out totalItems);

            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
