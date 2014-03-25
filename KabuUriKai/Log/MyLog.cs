using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;
using System.IO;
using System.Diagnostics;

namespace KabuUriKai.Log
{
    public static class MyLog
    {
        private static readonly string _logFolderPath = @"log";

        public static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void CreateLogFolder()
        {
            if (!Directory.Exists(_logFolderPath))
            {
                string stCurrentDir = System.IO.Directory.GetCurrentDirectory();
                Directory.CreateDirectory(_logFolderPath);
            }
        }

        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Output("Debug", message);
        }

        public static void Info(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Output("Info", message);
        }

        public static void Fatal(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Output("Fatal", message);
        }

        public static void Error(string format, params object[] args)
        {
            var message = string.Format(format, args);
            Output("Error", message);
        }

        private static void Output(string mode, string message)
        {
            switch (mode)
            {
                case("Info"):
                    logger.Info(message);
                    break;
                case("Error"):
                    logger.Error(message);
                    break;
                case("Fatal"):
                    logger.Fatal(message);
                    break;
                case("Debug"):
                    logger.Debug(message);
                    break;
            }
        }
    }
}
