using System;
using System.IO;
using System.Web;

namespace System.Web.Mvc
{
    public static class Log
    {

        static string FILE = "";
        static object sync = new object();

        private static void WriteTime()
        {
            WriteTime(false);
        }

        private static void WriteTime(bool errorconsole)
        {
            lock (sync)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (errorconsole)
                    {
                        Console.Error.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    else
                    {
                        Console.Write(DateTime.Now.ToString().PadRight(20) + " ");
                    }
                    Console.ResetColor();
                }
                catch
                {
                }
            }
        }

        public static void Info(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();
                try
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        public static void Success(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime();
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(string.Format(message, parameters));
                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        public static void Error(string message, params object[] parameters)
        {
            lock (sync)
            {
                WriteTime(true);
                try
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();

                    Save(string.Format(message, parameters));

                }
                catch
                {
                }
            }
        }

        public static void Warning(string message, params object[] parameters)
        {
            {
                WriteTime(true);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(string.Format(message, parameters));
                    Console.ResetColor();
                }
                catch
                {
                }
                Save(message);
            }
        }

        public static void Exception(Exception ex, string message, params object[] parameters)
        {
            lock (sync)
            {

                if (ex is System.Threading.ThreadAbortException)
                    return;
                Error(message);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("ERROR : " + ex.Message);
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Save(message);
                }
                catch
                {
                }
            }
        }

        private static void Save(string str)
        {

            if (string.IsNullOrEmpty(str)) { return; }

            var path = HttpContext.Current.Server.MapPath("\\Log");


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            if (string.IsNullOrEmpty(FILE))
            {
                FILE = string.Format("{0}\\{1}-SYSTEM-LOG.txt", path, DateTime.Now.ToString("yyyy-MM-dd"));
            }

            if (!File.Exists(FILE))
            {
                System.IO.File.WriteAllText(FILE, DateTime.Now.ToString());
            }

            using (StreamWriter sw = File.AppendText(FILE))
            {
                sw.WriteLine(DateTime.Now + "        " + str);
            }


        }

    }
}