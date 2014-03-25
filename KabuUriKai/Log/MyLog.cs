using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

namespace KabuUriKai.Log
{
    public class MyLog
    {
        public static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        MyLog()
        {
        }

    }
}
