using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KabuUriKai
{
    public class User
    {
        public string Nickname { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Birthplace { get; set; }
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - Birthday.Year;
                if (today.Month < Birthday.Month)
                {
                    age--;
                }
                else if (today.Month == Birthday.Month && today.Day < Birthday.Day)
                {
                    age--;
                }
                return age;
            }
        }
    }
}
