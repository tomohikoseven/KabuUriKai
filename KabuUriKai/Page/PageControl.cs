﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using KabuUriKai.Log;

namespace KabuUriKai.Page
{
    public class PageControl : JudgeDataList
    {
        private int start = 0;
        private int itemCount = 20;
        private string sortColumn = "Code";
        private bool ascending = true;
        private int totalItems = 0;
        private ICommand firstCommand;
        private ICommand previousCommand;
        private ICommand nextCommand;
        private ICommand lastCommand;

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
        public virtual void Sort(string sortColumn, bool ascending)
        {
            this.sortColumn = sortColumn;
            this.ascending = ascending;
            RefreshProducts();
        }

        /// <summary>
        /// Refreshes the list of products. Called by navigation commands.
        /// </summary>
        public virtual void RefreshProducts()
        {
            MyLog.Debug( System.Reflection.MethodBase.GetCurrentMethod().Name + " Start.");

            DataList = GetJudgeDataList(start, itemCount, sortColumn, ascending, out totalItems);

            NotifyPropertyChanged("Start");
            NotifyPropertyChanged("End");
            NotifyPropertyChanged("TotalItems");

            MyLog.Debug(System.Reflection.MethodBase.GetCurrentMethod().Name + " End.");
        }
 
    }
}
