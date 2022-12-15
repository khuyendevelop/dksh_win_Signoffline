using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace eInvoice.Signer
{
    public partial class frmLog : Form
    {
        public frmLog()
        {
            InitializeComponent();
        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            lstFiles.DisplayMember = "Name";
            lstFiles.ValueMember = "FullName";

            string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\Logger\";
            DirectoryInfo logFolder = new DirectoryInfo(logPath);
            if (logFolder.Exists)
            {
                foreach (FileInfo f in logFolder.GetFiles())
                {
                    lstFiles.Items.Add(f);
                }
                lstFiles.Sorted = true;
            }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileInfo f = (FileInfo)lstFiles.SelectedItem;
            txtMessage.Text= File.ReadAllText(f.FullName);
        }

    }
}
