using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace KabuUriKai
{
    public class UserList
    {
        public ObservableCollection<User> Users { get; set; }
        public UserList()
        {
            Users = new ObservableCollection<User>
            {
                new User { Name="芥川 龍之介", Nickname="ryu",
                            Birthday= new DateTime(1950,3,6),Birthplace="東京"},
                new User { Name="夏目 漱石", Nickname="natsu",
                    Birthday = new DateTime(1982,11,13), Birthplace="神奈川" }
            };
        }
    }
}
