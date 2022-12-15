using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using DKSH.Core.Utils;
using System.Configuration;
using System.Threading;
using eInvoice.Sign;
using iTextSharp.text.pdf;
using System.Collections;
using iTextSharp.text;
using DKSH.Core.SqlClient;
namespace eInvoice.Signer
{
    public partial class frmSigner : Form
    {
        private int listCount = 0;
        private DirectoryInfo backupFolder = null;
        private string sourceFolder = string.Empty;
        private string targetFolder = string.Empty;
        private string failformatFolder = string.Empty;
        private DateTime lastupdateDT09 = DateTime.Now;
        private DataTable DT09 = null; 
        public frmSigner()
        {
            InitializeComponent();

            TokenSign._timer = timer1;
        }

        private void Start()
        {

            //Sign();
            if (cmdStart.Text.Equals("Start"))
            {
                toolStatusLabel.Text = "Start";

                CheckFolder();

                //Start interval
                timerSign.Enabled = true;
                cbBU.Enabled = false;

                cmdStart.Text = "Stop";
            }
            else
            {
                toolStatusLabel.Text = "Stop";

                //Stop interval
                timerSign.Enabled = false;
                cbBU.Enabled = true;

                cmdStart.Text = "Start";
            }
        }

        private bool CheckFolder()
        {
            bool ret = true;
            try
            {
                string workingPath = txtSource.Text;
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
            catch {
                ret = false;
            }

            return ret;
        }

        private void Sign()
        {
            if (!CheckFolder())
                return;

            string Company = ConfigurationManager.AppSettings["Company"].ToString();//VN82
            string BinhDuongPlan = ConfigurationManager.AppSettings["BinhDuong"].ToString();//_VN13;_VN61;_VN79
            string HaNoiPlan = ConfigurationManager.AppSettings["HaNoi"].ToString();//_VN16;_VN63;_VNS3
            string DaNangPlan = ConfigurationManager.AppSettings["DaNang"].ToString();//_VN14;_VN62;_VNS4
            string TechHNPlan = ConfigurationManager.AppSettings["TechHN"].ToString();//_VN42
            string TechVSIPPlan = ConfigurationManager.AppSettings["TechVSIP"].ToString();//_VN41
            string ALL = ConfigurationManager.AppSettings["ALL"].ToString();//_VN14;_VN62;_VNS4 //Khuyen them de gom lai
            string SignEinvoice = ConfigurationManager.AppSettings["SignEinvoice"].ToString();//<!-- Y: la ky einvoice; N: ky chung tu khac-->
            
            ////Clear Temp
            //string tempPath = AppDomain.CurrentDomain.BaseDirectory + @"\Temp";
            //if (Directory.Exists(tempPath))
            //{
            //    try
            //    {
            //        Directory.Delete(tempPath, true);
            //    }
            //    catch { }
            //}
           
                timerSign.Enabled = false;
                string[] plan = cbBU.Text == "BinhDuong" ? BinhDuongPlan.Split(';') :
                                cbBU.Text == "HaNoi" ? HaNoiPlan.Split(';') :
                                cbBU.Text == "DaNang" ? DaNangPlan.Split(';') :
                                cbBU.Text == "TechHN" ? TechHNPlan.Split(';') :
                                cbBU.Text == "TechVSIP" ? TechVSIPPlan.Split(';') :
                                cbBU.Text == "ALL" ? ALL.Split(';') : null;
                if (SignEinvoice.ToLower() == "y")
                {
                if (plan != null)
                    foreach (string path in Directory.GetFiles(sourceFolder, "*.pdf"))
                    {
                        FileInfo file = new FileInfo(path);
                        ////check trung so erun. Khuyen 22Apr2019, tuy nhien da check trong store insert invoice roi nen cho nay tam thoi chua su dung
                        /*
                        string filenamesg = file.Name;
                        int vt_1 = filenamesg.IndexOf("_");
                        int vt_2 = filenamesg.IndexOf("_", vt_1 + 1);
                        int vt_3 = filenamesg.LastIndexOf("_");
                        int vthuy = filenamesg.ToUpper().IndexOf("HUY_");
                        string billtype = "Sales";
                        string billno = "";
                        if (vthuy > 0)
                        {
                            billtype = "Return";
                            billno = filenamesg.ToUpper().Replace("HUY_", "").Substring(vt_2 + 1, 10);
                        }
                        else
                        {
                            billno = filenamesg.Substring(vt_2 + 1, 10);
                        }
                        if (vt_1 > 0 && vt_2 > vt_1 && vt_3 > vt_2)
                        {
                            string seri = filenamesg.Substring(0, vt_1);

                            string erun = filenamesg.Substring(vt_1 + 1, vt_2 - (vt_1 + 1));

                            string plant = filenamesg.Substring(vt_3 + 1);
                            DataAccessHelper iDAHelper = DataAccessHelper.MasterInstance;
                            DataTable tbkq = iDAHelper.ExecuteDataTableP("SPP_CUST_INVOICE_DUPLICATED_ERUN", seri, erun, plant, billno, billtype);


                        }
                         * */
                        if (cbBU.Text != "ALL")
                        {
                            foreach (string folder in plan)
                                if (file.Name.Contains(folder))
                                {
                                    SignPdfFile(file.FullName, file.Name);

                                    while (lstFileProcess.Items.Count > listCount)
                                        lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                                }
                        }
                        else //phan nay Khuyen them de gom lai 1 may
                        {
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

                                Logger.WriteDebug(string.Format("{0} ({1}) file name {2} serialnumber {3}", "SignPdfFile(file.FullName, file.Name)", file.FullName, file.Name, cer.SerialNumber));
                                SignPdfFile(file.FullName, file.Name);

                                Logger.WriteDebug(string.Format("{0}", "  cer.Reset();"));
                                cer.Reset();
                                // TokenSign.ClearCert();
                                while (lstFileProcess.Items.Count > listCount)
                                    lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            }
                            else
                            {
                                Logger.WriteError(string.Format("{0} ({1})", "Plant chua khai bao trong ALL", file.Name));

                            }
                            //foreach (string folder in plan)
                            //    if (file.Name.Contains(folder))
                            //    {
                            //        TokenSign.ClearCert();
                            //        string Serialtoken = ConfigurationManager.AppSettings[folder].ToString();
                            //        X509Certificate2 cer = TokenSign.SelectCertificate(Serialtoken);
                            //        SignPdfFile(file.FullName, file.Name);
                            //       // TokenSign.ClearCert();
                            //        while (lstFileProcess.Items.Count > listCount)
                            //            lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            //    }
                        }
                    }
            }
            else //ky chung tu khac einvoice
            {
                     if (plan != null)
                    foreach (string path in Directory.GetFiles(sourceFolder, "*.pdf"))
                    {
                        FileInfo file = new FileInfo(path);
                        if (cbBU.Text == "ALL")
                        {
                            int i = 0;
                            while (i < plan.Length && file.Name.Substring(0,5).ToUpper().Contains(plan[i].ToString().ToUpper()) == false)//tim xem file can ky co trong danh sach all (ky tu dong)
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

                                Logger.WriteDebug(string.Format("{0} ({1}) file name {2} serialnumber {3}", "SignPdfFile(file.FullName, file.Name)", file.FullName, file.Name, cer.SerialNumber));
                                SignPdfFile(file.FullName, file.Name);

                                Logger.WriteDebug(string.Format("{0}", "  cer.Reset();"));
                                cer.Reset();
                                // TokenSign.ClearCert();
                                while (lstFileProcess.Items.Count > listCount)
                                    lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            }
                            else
                            {
                                MoveFailFormat(file.FullName, file.Name);
                                Logger.WriteError(string.Format("{0} ({1})", "Plant chua khai bao trong ALL", file.Name));

                            }
                        }
                }
            }
            if ("Stop".Equals(cmdStart.Text))
                timerSign.Enabled = true;

        }
        private void SignWithDate(DataTable DT09)
        {
            if (!CheckFolder())
                return;

            string Company = ConfigurationManager.AppSettings["Company"].ToString();//VN82
            string BinhDuongPlan = ConfigurationManager.AppSettings["BinhDuong"].ToString();//_VN13;_VN61;_VN79
            string HaNoiPlan = ConfigurationManager.AppSettings["HaNoi"].ToString();//_VN16;_VN63;_VNS3
            string DaNangPlan = ConfigurationManager.AppSettings["DaNang"].ToString();//_VN14;_VN62;_VNS4
            string TechHNPlan = ConfigurationManager.AppSettings["TechHN"].ToString();//_VN42
            string TechVSIPPlan = ConfigurationManager.AppSettings["TechVSIP"].ToString();//_VN41
            string ALL = ConfigurationManager.AppSettings["ALL"].ToString();//_VN14;_VN62;_VNS4 //Khuyen them de gom lai
            string SignEinvoice = ConfigurationManager.AppSettings["SignEinvoice"].ToString();//<!-- Y: la ky einvoice; N: ky chung tu khac-->

            timerSign.Enabled = false;
            string[] plan = cbBU.Text == "BinhDuong" ? BinhDuongPlan.Split(';') :
                            cbBU.Text == "HaNoi" ? HaNoiPlan.Split(';') :
                            cbBU.Text == "DaNang" ? DaNangPlan.Split(';') :
                            cbBU.Text == "TechHN" ? TechHNPlan.Split(';') :
                            cbBU.Text == "TechVSIP" ? TechVSIPPlan.Split(';') :
                            cbBU.Text == "ALL" ? ALL.Split(';') : null;
            if (SignEinvoice.ToLower() == "y")
            {
                if (plan != null)
                    foreach (string path in Directory.GetFiles(sourceFolder, "*.pdf"))
                    {
                        FileInfo file = new FileInfo(path);
                      
                        if (cbBU.Text != "ALL")
                        {
                            foreach (string folder in plan)
                                if (file.Name.Contains(folder))
                                {
                                    SignPdfFileWithDate(file.FullName, file.Name,DateTime.Now);

                                    while (lstFileProcess.Items.Count > listCount)
                                        lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                                }
                        }
                        else //phan nay Khuyen them de gom lai 1 may
                        {
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
                                DataRow[] dr = DT09.Select("invoice_no = '" + billno+"' and plant='"+plant+"'");
                                DateTime datesign = DateTime.Now;
                                if (dr.Length > 0)
                                {
                                    DataRow row = dr[0];
                                    string rowValue = row["invoice_date"].ToString();
                                    int yyyy=int.Parse(rowValue.Substring(0,4));
                                    int mm=int.Parse(rowValue.Substring(4,2));
                                    int dd=int.Parse(rowValue.Substring(6,2));
                                    datesign = new DateTime(yyyy, mm, dd, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                                }


                                SignPdfFileWithDate(file.FullName, file.Name, datesign);
                                Logger.WriteDebug(string.Format("{0}", "  cer.Reset();"));
                                cer.Reset();
                                // TokenSign.ClearCert();
                                while (lstFileProcess.Items.Count > listCount)
                                    lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            }
                            
                            {
                                Logger.WriteError(string.Format("{0} ({1})", "Plant chua khai bao trong ALL", file.Name));

                            }
                            //foreach (string folder in plan)
                            //    if (file.Name.Contains(folder))
                            //    {
                            //        TokenSign.ClearCert();
                            //        string Serialtoken = ConfigurationManager.AppSettings[folder].ToString();
                            //        X509Certificate2 cer = TokenSign.SelectCertificate(Serialtoken);
                            //        SignPdfFile(file.FullName, file.Name);
                            //       // TokenSign.ClearCert();
                            //        while (lstFileProcess.Items.Count > listCount)
                            //            lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            //    }
                        }
                    }
            }
            else //ky chung tu khac einvoice
            {
                if (plan != null)
                    foreach (string path in Directory.GetFiles(sourceFolder, "*.pdf"))
                    {
                        FileInfo file = new FileInfo(path);
                        if (cbBU.Text == "ALL")
                        {
                            int i = 0;
                            while (i < plan.Length && file.Name.Substring(0, 5).ToUpper().Contains(plan[i].ToString().ToUpper()) == false)//tim xem file can ky co trong danh sach all (ky tu dong)
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

                                Logger.WriteDebug(string.Format("{0} ({1}) file name {2} serialnumber {3}", "SignPdfFile(file.FullName, file.Name)", file.FullName, file.Name, cer.SerialNumber));
                                SignPdfFile(file.FullName, file.Name);

                                Logger.WriteDebug(string.Format("{0}", "  cer.Reset();"));
                                cer.Reset();
                                // TokenSign.ClearCert();
                                while (lstFileProcess.Items.Count > listCount)
                                    lstFileProcess.Items.RemoveAt(lstFileProcess.Items.Count - 1);
                            }
                            else
                            {
                                MoveFailFormat(file.FullName, file.Name);
                                Logger.WriteError(string.Format("{0} ({1})", "Plant chua khai bao trong ALL", file.Name));

                            }
                        }
                    }
            }
            //if ("Stop".Equals(btSignBackDate.Text))
            //    timerSign.Enabled = true;
            if ("Stop Sign BillDate".Equals(btSignBackDate.Text))
                timerSignBackDate.Enabled = true;

        }
        private void SignPdfFile(string fileFullName, string fileName)
        {
            string sourcePath = fileFullName;
            string targetPath = targetFolder + "\\" + fileName;
            string errmgs = "";
            try
            {
                lstFileProcess.Items.Insert(0, string.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:m:s"), fileName));
                toolStatusLabel.Text = "Processing file: " + fileName;

                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    //Move file to temp folder to process
                    errmgs = string.Format("{0} ({1})", " sourcePath = MoveTemp(sourcePath);", sourcePath);
                    sourcePath = MoveTemp(sourcePath);
                    if (!string.IsNullOrEmpty(sourcePath))
                    {
                        errmgs=string.Format("{0} ({1})", "  !TokenSign.SignHashed(sourcePath, targetPath)", targetPath);
                       
                        if (!TokenSign.SignHashed(sourcePath, targetPath))
                        {
                           errmgs=string.Format("{0} ({1})", "  MoveFail(sourcePath, fileName)", fileName);
                            MoveFail(sourcePath, fileName);
                            //Xoa file ky loi
                            errmgs=string.Format("{0} ({1})", "  File.Delete(targetPath);", targetPath);
                            File.Delete(targetPath);
                        }
                        else
                        {
                            errmgs=string.Format("{0} ({1})", "  File.Delete(sourcePath);", sourcePath);
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


                toolStatusLabel.Text = "Done";
            }
            catch (Exception ex)
            {
                toolStatusLabel.Text = "Error: " + ex.Message;
                Logger.WriteError(string.Format("{0} ({1})", ex.Message, fileName));
                File.Delete(targetPath);
                Logger.WriteDebug(string.Format("{0} ({1})", " MoveFail(sourcePath, fileName);", sourcePath));
                MoveFail(sourcePath, fileName);
            }
            Thread.Sleep(100);
        }
        private void SignPdfFileWithDate(string fileFullName, string fileName,DateTime SignDate)
        {
            string sourcePath = fileFullName;
            string targetPath = targetFolder + "\\" + fileName;
            string errmgs = "";
            try
            {
                lstFileProcess.Items.Insert(0, string.Format("{0}: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:m:s"), fileName));
                toolStatusLabel.Text = "Processing file with date: " + fileName;

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


                toolStatusLabel.Text = "Done singed with date";
            }
            catch (Exception ex)
            {
                toolStatusLabel.Text = "Error: " + ex.Message;
                Logger.WriteError(string.Format("{0} ({1})", ex.Message, fileName));
                File.Delete(targetPath);
                Logger.WriteDebug(string.Format("{0} ({1})", " MoveFail(sourcePath, fileName);", sourcePath));
                MoveFail(sourcePath, fileName);
            }
            Thread.Sleep(100);
        }
        private void SignxlsFile(string fileName)
        {
            string sourcePath = sourceFolder + "\\" + fileName;
            string targetPath = targetFolder + "\\" + fileName;
            try
            {
                lstFileProcess.Items.Insert(0, fileName);
                toolStatusLabel.Text = "Processing file: " + fileName;

                //Thuc hien ky file
                if (File.Exists(sourcePath))
                {
                    //Move file to temp folder to process
                    sourcePath = MoveTemp(sourcePath);

                    if (!TokenSign.AddSignature(sourcePath, targetPath))
                    {
                        MoveFail(sourcePath, fileName);
                        //Xoa file ky loi
                        File.Delete(targetPath);
                    }
                    else
                        File.Delete(sourcePath);
                }

                toolStatusLabel.Text = "Done";
            }
            catch (Exception ex)
            {
                toolStatusLabel.Text = "Error: " + ex.Message;
                Logger.WriteError(string.Format("{0} ({1})", ex.Message, fileName));
                File.Delete(targetPath);
                MoveFail(sourcePath, fileName);
            }
            Thread.Sleep(100);
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
                        
                        string backupbath=backupFolder.FullName + @"\Original\" + f.Name;
                        errmgs = " File.Copy(" + sourcePath + ", " + backupbath + ", true); ";
                        if(File.Exists(backupbath))
                        {
                            File.Delete(backupbath);
                        }
                        File.Copy(sourcePath, backupFolder.FullName + @"\Original\" + f.Name, true);
                    }
                    errmgs = " File.Copy(sourcePath, tempName, true); " + sourcePath;
                    File.Copy(sourcePath, tempName, true);

                    errmgs = "  File.Delete(sourcePath); " + sourcePath;
                    File.Delete(sourcePath);

                    if (".pdf".Equals(Fnc.GetString(f.Extension).ToLower()) && chkChangeFont.Checked)
                        tempName = ChangeFont(tempPath, tempName, f.Extension);
                }
                return tempName;
            }
            catch (Exception ex)
            {
                toolStatusLabel.Text = "MoveTemp Error: " + ex.Message;
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
                toolStatusLabel.Text = "MoveFail Error: " + ex.Message;
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
                toolStatusLabel.Text = "MoveFailFormat Error: " + ex.Message;
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
                    var dictFontInfo = dictFonts.GetAsDict((PdfName) font.Key);

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

        #region Event

        private void cmdSetToken_Click(object sender, EventArgs e)
        {
            TokenSign.ClearCert();
            X509Certificate2 cer = TokenSign.SelectCertificate();
           
            if (cer != null)
            {
                txtTokenName.Text = cer.GetName();
                txtSerial.Text = cer.GetSerialNumberString();
            }
        }

        private void cmdSelectSource_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSource.Text = folderDialog.SelectedPath;
            }
        }

        private void cmdTestSource_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtTest.Text = folderDialog.SelectedPath;
            }
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (timerSignBackDate.Enabled == true)
            {
                MessageBox.Show("Signed with billing date is running, please stop SignBackDate first");
            }
            else
            {
                if (cbBU.Text != "ALL")
                {
                    X509Certificate2 cer = TokenSign.GetCertificate();
                    if (cer == null)
                    {
                        MessageBox.Show("Certificate not found!");
                        return;
                    }

                    if (Directory.Exists(txtSource.Text) == false)
                    {
                        MessageBox.Show("Working Folder is not exists!");
                        return;
                    }
                }

                Start();
            }
        }

        private void timerSign_Tick(object sender, EventArgs e)
        {
            try
            {
                Sign();
            }
            catch(Exception ex)
            {
                Logger.WriteError("Copy file to Fail error: " + ex.Message);

                if ("Stop".Equals(cmdStart.Text))
                    timerSign.Enabled = true;
            }
        }

        private void frmSigner_Load(object sender, EventArgs e)
        {
            timerSign.Interval = Fnc.GetInteger(ConfigurationManager.AppSettings["Interval"]);
            txtSource.Text = Fnc.GetString(ConfigurationManager.AppSettings["Working"]);
            listCount = Fnc.GetInteger(ConfigurationManager.AppSettings["ListCount"]);
        }

        private void btnGetSaveKey_Click(object sender, EventArgs e)
        {
            //byte[] key = TokenSign.GetKey();
            //if (key != null && key.Length > 0)
            //    txtKey.Text = "Has Key";
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //AutoPin.GetWindowList();
            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new frmLog()).Show();
        }

        private void btVerifyxml_Click(object sender, EventArgs e)
        {
            (new EinvoiceVerify()).Show();
        }

        private void btSignBackDate_Click(object sender, EventArgs e)
        {
            if (timerSign.Enabled == true)
            {
                MessageBox.Show("Signed with current date is running, please stop Sign Current date first");
            }
            else
            {


                if (cbBU.Text != "ALL")
                {
                    X509Certificate2 cer = TokenSign.GetCertificate();
                    if (cer == null)
                    {
                        MessageBox.Show("Certificate not found!");
                        return;
                    }

                    if (Directory.Exists(txtSource.Text) == false)
                    {
                        MessageBox.Show("Working Folder is not exists!");
                        return;
                    }
                }

                // CheckFolder();
                //Sign();
                if (btSignBackDate.Text.Equals("Sign BillDate"))
                {
                    DataAccessHelper iDAHelper = DataAccessHelper.OTHInstance;
                    lastupdateDT09 = DateTime.Now.Date;
                    DT09 = iDAHelper.ExecuteDataTableP("Get_DT09");

                    toolStatusLabel.Text = "Start Signed with Billing Date";
                    CheckFolder();
                    btSignBackDate.Text = "Stop Sign BillDate";
                    //Start interval
                    cbBU.Enabled = false;
                    timerSignBackDate.Enabled = true;

                   
                }
                else
                {
                    timerSignBackDate.Enabled = false;
                    toolStatusLabel.Text = "Stop Sign BillDate";
                    //Stop interval
                    cbBU.Enabled = true;
                    btSignBackDate.Text = "Sign BillDate";
                }


            
                //  SignWithDate(DT09);
            }
        }
        private void timerSignBackDate_Tick(object sender, EventArgs e)
        {
            try
            {
                if(DateTime.Now.Date!=lastupdateDT09.Date)
                {
                    DataAccessHelper iDAHelper = DataAccessHelper.OTHInstance;
                    lastupdateDT09 = DateTime.Now.Date;
                    DT09 = iDAHelper.ExecuteDataTableP("Get_DT09");
                }
                SignWithDate(DT09);
            }
            catch (Exception ex)
            {
                Logger.WriteError("Copy file to Fail error: " + ex.Message);

                if ("Stop Sign BillDate".Equals(btSignBackDate.Text))
                    timerSignBackDate.Enabled = true;
            }
        }

    }
}