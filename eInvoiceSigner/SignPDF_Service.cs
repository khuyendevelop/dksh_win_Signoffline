using DKSH.Core.SqlClient;
using DKSH.Core.Utils;
using eInvoice.Sign;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;

namespace eInvoice.Signer
{
    partial class SignPDF_Service : ServiceBase
    {
        protected System.Threading.Thread thread;
        public bool _Started = false;
        protected int _Sleep = 60;
        protected string workingfolder = ConfigurationSettings.AppSettings["Working"];
    



        private int listCount = 0;
        private DirectoryInfo backupFolder = null;
        private string sourceFolder = string.Empty;
        private string targetFolder = string.Empty;
        private string failformatFolder = string.Empty;
        private DateTime lastupdateDT09 = DateTime.Now;
        private DataTable DT09 = null; 
        public SignPDF_Service()
        {
            InitializeComponent();
        }
        private bool CheckFolder()
        {
            bool ret = true;
            try
            {
                string workingPath = workingfolder;
                string backupPath = workingPath + FolderInfor.Backup;
                sourceFolder = workingPath + FolderInfor.Source;
                targetFolder = workingPath + FolderInfor.Target;
                failformatFolder = workingPath + @"\FailFile";
                backupFolder = new DirectoryInfo(backupPath);
                if (!backupFolder.Exists)
                    Directory.CreateDirectory(backupPath);

                if (!Directory.Exists(backupPath + @"\Original"))
                    Directory.CreateDirectory(backupPath + @"\Original");
                if (!Directory.Exists(backupPath + @"\FailSigned"))
                    Directory.CreateDirectory(backupPath + @"\FailSigned");
                if (!Directory.Exists(backupPath + @"\Signed"))
                    Directory.CreateDirectory(backupPath + @"\Signed");

                if (!Directory.Exists(sourceFolder))
                    Directory.CreateDirectory(sourceFolder);
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                if (!Directory.Exists(workingPath + @"\FailFile"))
                    Directory.CreateDirectory(workingPath + @"\FailFile");
            }
            catch(Exception ex)
            {
                Logger.WriteError("Check folder fail: " + ex.ToString());
                ret = false;
            }

            return ret;
        }
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            _Started = false;
            X509Certificate2 cer = TokenSign.GetCertificate();
            if (cer == null)
            {
                Logger.WriteError("Certificate not found!");
               
                return;
            }

            if (Directory.Exists(workingfolder) == false)
            {
                Logger.WriteError("Working Folder is not exists!");
              
                return;
            }

            if (CheckFolder() == true)
            {
                if (thread != null && thread.ThreadState == System.Threading.ThreadState.Running)
                {
                    thread.Join(3000);
                    if (thread.ThreadState != System.Threading.ThreadState.Stopped)
                        thread.Abort();
                }
           
           

            try
            {
                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                _Started = true;
                DataAccessHelper iDAHelper = DataAccessHelper.OTHInstance;
                lastupdateDT09 = DateTime.Now.Date;
                DT09 = iDAHelper.ExecuteDataTableP("Get_DT09");
                thread.Start();
            }
            catch (Exception ex)
            {
                this.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                throw ex;
            }
        }
    }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            _Started = false;
            if (thread != null)
            {
                thread.Join(10000);
                if (thread.ThreadState != System.Threading.ThreadState.Stopped)
                    thread.Abort();
            }
        }
        public void Run()
        {

            while (_Started)
            {
                try
                {
                    
                        if (DateTime.Now.Date != lastupdateDT09.Date)
                        {
                            DataAccessHelper iDAHelper = DataAccessHelper.OTHInstance;
                            lastupdateDT09 = DateTime.Now.Date;
                            DT09 = iDAHelper.ExecuteDataTableP("Get_DT09");
                        }
                        SignWithDate(DT09);
                   
                   
                }
                catch (Exception ex)
                {
                    //this.EventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                    Logger.WriteError(ex.ToString());
                }
                System.Threading.Thread.Sleep(_Sleep * 1000);
                //OnStop();
            }
        }

        private void SignWithDate(DataTable DT09)
        {
            
          
            string ALL = ConfigurationManager.AppSettings["ALL"].ToString();//_VN14;_VN62;_VNS4 //Khuyen them de gom lai
            string SignEinvoice = ConfigurationManager.AppSettings["SignEinvoice"].ToString();//<!-- Y: la ky einvoice; N: ky chung tu khac-->

          
            string[] plan =  ALL.Split(';');
            if (SignEinvoice.ToLower() == "y")
            {
                if (plan != null)
                   //  DirectoryInfo folderSourcef = new DirectoryInfo(sourceFolder);//de doc thu muc SVME cho 
                   //   FileInfo[] files = folderSourcef.GetFiles().Where(name => name.Name.ToLower().EndsWith(".pdf")).OrderBy(p => p.CreationTime).Take(100).ToArray();
                   //foreach (FileInfo file in files)
                   //       {
                   //           fileprocess.ReadandWritefile(file, destion, tiento,  bk, isexportBravo);
                   //       }
              
                    foreach (string path in Directory.GetFiles(sourceFolder, "*.pdf"))
                    {
                        FileInfo file = new FileInfo(path);

                        
                            int i = 0;
                            while (i < plan.Length && file.Name.ToUpper().Contains(plan[i].ToString().ToUpper()) == false)//tim xem file can ky co trong danh sach all (ky tu dong)
                            {
                                i++;

                            }
                            if (i < plan.Length)//tim thay trong danh sach ky tu dong
                            {
                                Logger.WriteDebug(string.Format("{0}", "TokenSign.ClearCert();"));
                                TokenSign.ClearCert();

                                Logger.WriteDebug(string.Format("{0} ({1}) plant {2}", "ConfigurationManager.AppSettings[plan[i].ToString()].ToString()", ConfigurationManager.AppSettings[plan[i].ToString()].ToString(), plan[i].ToString()));

                                string Serialtoken = ConfigurationManager.AppSettings[plan[i].ToString()].ToString();

                                Logger.WriteDebug(string.Format("{0} ({1})", "TokenSign.SelectCertificate(Serialtoken)", Serialtoken));
                                X509Certificate2 cer = TokenSign.SelectCertificate(Serialtoken);

                                Logger.WriteDebug(string.Format("{0} ({1}) file name {2} serialnumber {3}", "SignPdfFileWithDate(file.FullName, file.Name)", file.FullName, file.Name, cer.SerialNumber));
                                // SignPdfFile(file.FullName, file.Name);
                                string billno = file.Name.Substring(14, 10);
                                string plant = file.Name.Substring(35, 4);
                                DataRow[] dr = DT09.Select("invoice_no = '" + billno + "' and plant='" + plant + "'");
                                DateTime datesign = DateTime.Now;
                                if (dr.Length > 0)
                                {
                                    DataRow row = dr[0];
                                    string rowValue = row["invoice_date"].ToString();
                                    int yyyy = int.Parse(rowValue.Substring(0, 4));
                                    int mm = int.Parse(rowValue.Substring(4, 2));
                                    int dd = int.Parse(rowValue.Substring(6, 2));
                                    datesign = new DateTime(yyyy, mm, dd, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                                }


                                SignPdfFileWithDate(file.FullName, file.Name, datesign);
                                Logger.WriteDebug(string.Format("{0}", "  cer.Reset();"));
                                cer.Reset();
                                //// TokenSign.ClearCert();
                                //while (lstFileProcess.Items.Count > listCount)
                                //    lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            }

                            {
                                Logger.WriteError(string.Format("{0} ({1})", "Plant chua khai bao trong ALL", file.Name));

                            }
                           
                        }
                    }
        }

        private void SignPdfFileWithDate(string fileFullName, string fileName, DateTime SignDate)
        {
            string sourcePath = fileFullName;
            string targetPath = targetFolder + "\\" + fileName;
            string errmgs = "";
            try
            {
                Logger.WriteDebug(string.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:m:s"), fileName));
             
                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    //Move file to temp folder to process
                    errmgs = string.Format("{0} ({1})", " sourcePath = MoveTemp(sourcePath);", sourcePath);
                    sourcePath = MoveTemp(sourcePath);
                    if (!string.IsNullOrEmpty(sourcePath))
                    {
                        errmgs = string.Format("{0} ({1})", "  !TokenSign.SignHashed(sourcePath, targetPath)", targetPath);

                        if (!TokenSign.SignHashedWithDate(sourcePath, targetPath, SignDate))
                        {
                            errmgs = string.Format("{0} ({1})", "  MoveFail(sourcePath, fileName)", fileName);
                            MoveFail(sourcePath, fileName);
                            //Xoa file ky loi
                            errmgs = string.Format("{0} ({1})", "  File.Delete(targetPath);", targetPath);
                            File.Delete(targetPath);
                        }
                        else
                        {
                            errmgs = string.Format("{0} ({1})", "  File.Delete(sourcePath);", sourcePath);
                            File.Delete(sourcePath);

                            //Neu co dinh nghia Backup thi chuyen file qua backup
                            if (CheckFolder())
                            {
                                string backupbath = backupFolder.FullName + @"\Signed\" + fileName;
                                errmgs = " File.Copy(" + targetPath + ", " + backupbath + ", true)";
                                if (File.Exists(backupbath))
                                {
                                    File.Delete(backupbath);
                                }
                                // Logger.WriteDebug(string.Format("{0} ({1})", "  File.Copy", targetPath));
                                File.Copy(targetPath, backupFolder.FullName + @"\Signed\" + fileName, true);
                            }
                        }
                    }
                    else
                    {
                        Logger.WriteError(errmgs);
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.Message);
              
                Logger.WriteError(string.Format("{0} ({1})", ex.Message, fileName));
                File.Delete(targetPath);
                Logger.WriteDebug(string.Format("{0} ({1})", " MoveFail(sourcePath, fileName);", sourcePath));
                MoveFail(sourcePath, fileName);
            }
           
        }

        private string MoveTemp(string sourcePath)
        {
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + @"\Temp";
            string tempName = tempPath + "\\" + Fnc.GetString(Guid.NewGuid());
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            string errmgs = "";
            try
            {
                //Copy file can xu ly vao folder temp va xoa file
                errmgs = " File.Exists(sourcePath) " + sourcePath;
                if (File.Exists(sourcePath))
                {
                    errmgs = "   FileInfo f = new FileInfo(sourcePath); " + sourcePath;
                    FileInfo f = new FileInfo(sourcePath);
                    tempName = tempName + f.Extension;

                    //Neu co dinh nghia Backup thi chuyen file qua backup
                    if (CheckFolder())
                    {

                        string backupbath = backupFolder.FullName + @"\Original\" + f.Name;
                        errmgs = " File.Copy(" + sourcePath + ", " + backupbath + ", true); ";
                        if (File.Exists(backupbath))
                        {
                            File.Delete(backupbath);
                        }
                        File.Copy(sourcePath, backupFolder.FullName + @"\Original\" + f.Name, true);
                    }
                    errmgs = " File.Copy(sourcePath, tempName, true); " + sourcePath;
                    File.Copy(sourcePath, tempName, true);

                    errmgs = "  File.Delete(sourcePath); " + sourcePath;
                    File.Delete(sourcePath);

                    //if (".pdf".Equals(Fnc.GetString(f.Extension).ToLower()) && chkChangeFont.Checked)
                    //    tempName = ChangeFont(tempPath, tempName, f.Extension);
                }
                return tempName;
            }
            catch (Exception ex)
            {
               
                Logger.WriteError("Copy file to Temp error: " + ex.Message + errmgs);
            }
            return string.Empty;
        }

        private void MoveFail(string sourcePath, string fileName)
        {
            string errmgs = "";
            try
            {
                string failPath = AppDomain.CurrentDomain.BaseDirectory + @"\FailFile";
                if (!Directory.Exists(failPath))
                    Directory.CreateDirectory(failPath);

                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, failPath + "\\" + fileName, true);

                    //Neu co dinh nghia Backup thi chuyen file qua backup
                    if (CheckFolder())
                    {
                        string backupbath = backupFolder.FullName + @"\FailSigned\" + fileName;
                        errmgs = " File.Copy(" + sourcePath + ", " + backupbath + ", true); ";
                        if (File.Exists(backupbath))
                        {
                            File.Delete(backupbath);
                        }
                        File.Copy(sourcePath, backupFolder.FullName + @"\FailSigned\" + fileName, true);
                    }

                    //Xoa file da ky
                    File.Delete(sourcePath);
                }

            }
            catch (Exception ex)
            {
                
                Logger.WriteError("Copy file to Fail error: " + ex.Message + errmgs);
            }
        }
        private void MoveFailFormat(string sourcePath, string fileName)
        {
            string errmgs = "";
            try
            {
                string failPath = failformatFolder;
                if (!Directory.Exists(failPath))
                    Directory.CreateDirectory(failPath);

                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, failPath + "\\" + fileName, true);

                    //Xoa file da ky
                    File.Delete(sourcePath);
                }

            }
            catch (Exception ex)
            {
               
                Logger.WriteError("Copy file to Fail format error: " + ex.Message + errmgs);
            }
        }
        private string ChangeFont(string path, string file, string ext)
        {
            string outputFile = path + "\\" + Fnc.GetString(Guid.NewGuid()) + ext;

            PdfReader pdfReader = new PdfReader(file);
            PdfDictionary cpage = pdfReader.GetPageN(1);
            if (cpage == null)
                return file;

            PdfDictionary dictFonts = cpage.GetAsDict(PdfName.RESOURCES).GetAsDict(PdfName.FONT);
            if (dictFonts != null)
            {
                foreach (DictionaryEntry font in dictFonts)
                {
                    var dictFontInfo = dictFonts.GetAsDict((PdfName)font.Key);

                    if (dictFontInfo != null)
                    {
                        foreach (var f in dictFontInfo)
                        {
                            //Get the font name-optional code
                            var baseFont = dictFontInfo.Get(PdfName.BASEFONT);
                            string strFontName = System.Text.Encoding.ASCII.GetString(baseFont.GetBytes(), 0,
                                                                                      baseFont.Length);
                            string strConvertFont = "Courier New";
                            //if (strFontName.Contains("Courier"))
                            //    strConvertFont = "/Times New Roman";
                            //else
                            //    strConvertFont = strFontName;

                            //if (strFontName.Contains("Courier"))
                            //    strConvertFont = "Times New Roman";
                            //if (strFontName.Contains("CourierNew"))
                            //    strConvertFont = "Courier New";
                            //if (strFontName.Contains("Helvetica"))
                            //    strConvertFont = "Helvetica";
                            //Remove the current font
                            dictFontInfo.Remove(PdfName.BASEFONT);
                            //Set new font eg. Braille, Areal etc
                            dictFontInfo.Put(PdfName.BASEFONT, new PdfString(strConvertFont));
                            break;

                        }
                    }


                }

            }

            //Now create a new document with updated font
            using (FileStream FS = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document Doc = new Document();
                PdfCopy writer = new PdfCopy(Doc, FS);
                Doc.Open();
                for (int j = 1; j <= pdfReader.NumberOfPages; j++)
                {
                    writer.AddPage(writer.GetImportedPage(pdfReader, j));
                }
                Doc.Close();
            }
            pdfReader.Close();

            return outputFile;
            //return file;
        }
    }
}
