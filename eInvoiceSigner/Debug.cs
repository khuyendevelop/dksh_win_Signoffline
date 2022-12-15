using DKSH.Core.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace eInvoice.Signer
{

    public class FolderInfor
    {
        public static string Source
        {
            get { return Fnc.GetString(ConfigurationManager.AppSettings["Source"]); }
        }
        public static string Target
        {
            get { return Fnc.GetString(ConfigurationManager.AppSettings["Target"]); }
        }
        public static string Backup
        {
            get { return Fnc.GetString(ConfigurationManager.AppSettings["Backup"]); }
        }
    }

    public class Logger
    {
        private static void CreateFolder()
        {
            List<string> ret = new List<string>();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Logger";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void WriteDebug(string text)
        {
            CreateFolder();

            #region Write log file with any acction: log, error, send ok, ...
            try
            {
                //Write log to current app path
                bool logError = Fnc.GetBool(ConfigurationManager.AppSettings["LogDebug"].ToString());
                string err = "{0}\t{1}\t{2}";
                if (logError)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    path = path + @"\Logger\DebugLog_" + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt";
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(string.Format(err, "Debug:", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"), text));
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("Error on WriteDebug: {0} " + ex.Message + ex.StackTrace);
            }
            #endregion
        }

        public static void WriteError(string text)
        {
            CreateFolder();

            #region Write log file with any acction: log, error, send ok, ...
            try
            {
                //Write log to current app path
                bool logError = Fnc.GetBool(ConfigurationManager.AppSettings["LogError"].ToString());
                string err = "{0}\t{1}\t{2}";
                if (logError)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    path = path + @"\Logger\ErrorLog_" + DateTime.Now.Date.ToString("yyyyMMdd") + ".txt";
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(string.Format(err, "Error:", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"), text));
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError("Error on WriteError: {0} " + ex.Message + ex.StackTrace);
            }
            #endregion
        }

    }
}
