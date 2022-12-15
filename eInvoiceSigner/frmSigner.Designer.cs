namespace eInvoice.Signer
{
    partial class frmSigner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSigner));
            this.cmdSetToken = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbBU = new System.Windows.Forms.ComboBox();
            this.btnGetSaveKey = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.txtTokenName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkChangeFont = new System.Windows.Forms.CheckBox();
            this.cmdSelectTest = new System.Windows.Forms.Button();
            this.cmdSelectSource = new System.Windows.Forms.Button();
            this.txtTest = new System.Windows.Forms.TextBox();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstFileProcess = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmdStart = new System.Windows.Forms.Button();
            this.timerSign = new System.Windows.Forms.Timer(this.components);
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.btVerifyxml = new System.Windows.Forms.Button();
            this.btSignBackDate = new System.Windows.Forms.Button();
            this.timerSignBackDate = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdSetToken
            // 
            this.cmdSetToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSetToken.Location = new System.Drawing.Point(368, 70);
            this.cmdSetToken.Name = "cmdSetToken";
            this.cmdSetToken.Size = new System.Drawing.Size(75, 23);
            this.cmdSetToken.TabIndex = 4;
            this.cmdSetToken.Text = "Get Token";
            this.cmdSetToken.UseVisualStyleBackColor = true;
            this.cmdSetToken.Click += new System.EventHandler(this.cmdSetToken_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbBU);
            this.groupBox1.Controls.Add(this.btnGetSaveKey);
            this.groupBox1.Controls.Add(this.txtKey);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSerial);
            this.groupBox1.Controls.Add(this.cmdSetToken);
            this.groupBox1.Controls.Add(this.txtTokenName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Token";
            // 
            // cbBU
            // 
            this.cbBU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbBU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBU.FormattingEnabled = true;
            this.cbBU.Items.AddRange(new object[] {
            "BinhDuong",
            "HaNoi",
            "DaNang",
            "TechVSIP",
            "TechHN",
            "ALL"});
            this.cbBU.Location = new System.Drawing.Point(93, 71);
            this.cbBU.Name = "cbBU";
            this.cbBU.Size = new System.Drawing.Size(188, 21);
            this.cbBU.TabIndex = 8;
            // 
            // btnGetSaveKey
            // 
            this.btnGetSaveKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSaveKey.Location = new System.Drawing.Point(287, 70);
            this.btnGetSaveKey.Name = "btnGetSaveKey";
            this.btnGetSaveKey.Size = new System.Drawing.Size(75, 23);
            this.btnGetSaveKey.TabIndex = 7;
            this.btnGetSaveKey.Text = "Get Key";
            this.btnGetSaveKey.UseVisualStyleBackColor = true;
            this.btnGetSaveKey.Visible = false;
            this.btnGetSaveKey.Click += new System.EventHandler(this.btnGetSaveKey_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(93, 72);
            this.txtKey.Name = "txtKey";
            this.txtKey.ReadOnly = true;
            this.txtKey.Size = new System.Drawing.Size(150, 20);
            this.txtKey.TabIndex = 6;
            this.txtKey.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Location";
            // 
            // txtSerial
            // 
            this.txtSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSerial.Location = new System.Drawing.Point(93, 45);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(350, 20);
            this.txtSerial.TabIndex = 3;
            this.txtSerial.TabStop = false;
            // 
            // txtTokenName
            // 
            this.txtTokenName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTokenName.Location = new System.Drawing.Point(93, 19);
            this.txtTokenName.Name = "txtTokenName";
            this.txtTokenName.ReadOnly = true;
            this.txtTokenName.Size = new System.Drawing.Size(350, 20);
            this.txtTokenName.TabIndex = 1;
            this.txtTokenName.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Serial Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Token Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkChangeFont);
            this.groupBox2.Controls.Add(this.cmdSelectTest);
            this.groupBox2.Controls.Add(this.cmdSelectSource);
            this.groupBox2.Controls.Add(this.txtTest);
            this.groupBox2.Controls.Add(this.txtSource);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(449, 45);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Files container";
            // 
            // chkChangeFont
            // 
            this.chkChangeFont.AutoSize = true;
            this.chkChangeFont.Location = new System.Drawing.Point(93, 72);
            this.chkChangeFont.Name = "chkChangeFont";
            this.chkChangeFont.Size = new System.Drawing.Size(87, 17);
            this.chkChangeFont.TabIndex = 6;
            this.chkChangeFont.Text = "Change Font";
            this.chkChangeFont.UseVisualStyleBackColor = true;
            this.chkChangeFont.Visible = false;
            // 
            // cmdSelectTest
            // 
            this.cmdSelectTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelectTest.Location = new System.Drawing.Point(411, 46);
            this.cmdSelectTest.Name = "cmdSelectTest";
            this.cmdSelectTest.Size = new System.Drawing.Size(32, 21);
            this.cmdSelectTest.TabIndex = 2;
            this.cmdSelectTest.Text = "...";
            this.cmdSelectTest.UseVisualStyleBackColor = true;
            this.cmdSelectTest.Click += new System.EventHandler(this.cmdTestSource_Click);
            // 
            // cmdSelectSource
            // 
            this.cmdSelectSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSelectSource.Location = new System.Drawing.Point(411, 19);
            this.cmdSelectSource.Name = "cmdSelectSource";
            this.cmdSelectSource.Size = new System.Drawing.Size(32, 21);
            this.cmdSelectSource.TabIndex = 2;
            this.cmdSelectSource.Text = "...";
            this.cmdSelectSource.UseVisualStyleBackColor = true;
            this.cmdSelectSource.Click += new System.EventHandler(this.cmdSelectSource_Click);
            // 
            // txtTest
            // 
            this.txtTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTest.Location = new System.Drawing.Point(93, 46);
            this.txtTest.Name = "txtTest";
            this.txtTest.ReadOnly = true;
            this.txtTest.Size = new System.Drawing.Size(312, 20);
            this.txtTest.TabIndex = 1;
            this.txtTest.TabStop = false;
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(93, 20);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(312, 20);
            this.txtSource.TabIndex = 1;
            this.txtSource.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Testing Folder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Working Folder";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lstFileProcess);
            this.groupBox3.Location = new System.Drawing.Point(12, 211);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(449, 229);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sign Information";
            // 
            // lstFileProcess
            // 
            this.lstFileProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFileProcess.FormattingEnabled = true;
            this.lstFileProcess.Location = new System.Drawing.Point(3, 16);
            this.lstFileProcess.Name = "lstFileProcess";
            this.lstFileProcess.Size = new System.Drawing.Size(443, 210);
            this.lstFileProcess.TabIndex = 0;
            this.lstFileProcess.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 443);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(473, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStatusLabel
            // 
            this.toolStatusLabel.Name = "toolStatusLabel";
            this.toolStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(21, 180);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 2;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // timerSign
            // 
            this.timerSign.Interval = 500;
            this.timerSign.Tick += new System.EventHandler(this.timerSign_Tick);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "View Log";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btVerifyxml
            // 
            this.btVerifyxml.Location = new System.Drawing.Point(380, 181);
            this.btVerifyxml.Name = "btVerifyxml";
            this.btVerifyxml.Size = new System.Drawing.Size(75, 23);
            this.btVerifyxml.TabIndex = 6;
            this.btVerifyxml.Text = "IT Check";
            this.btVerifyxml.UseVisualStyleBackColor = true;
            this.btVerifyxml.Click += new System.EventHandler(this.btVerifyxml_Click);
            // 
            // btSignBackDate
            // 
            this.btSignBackDate.Location = new System.Drawing.Point(216, 181);
            this.btSignBackDate.Name = "btSignBackDate";
            this.btSignBackDate.Size = new System.Drawing.Size(158, 23);
            this.btSignBackDate.TabIndex = 7;
            this.btSignBackDate.Text = "Sign BillDate";
            this.btSignBackDate.UseVisualStyleBackColor = true;
            this.btSignBackDate.Click += new System.EventHandler(this.btSignBackDate_Click);
            // 
            // timerSignBackDate
            // 
            this.timerSignBackDate.Interval = 500;
            this.timerSignBackDate.Tick += new System.EventHandler(this.timerSignBackDate_Tick);
            // 
            // frmSigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 465);
            this.Controls.Add(this.btSignBackDate);
            this.Controls.Add(this.btVerifyxml);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSigner";
            this.Text = "SignOnline";
            this.Load += new System.EventHandler(this.frmSigner_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdSetToken;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.TextBox txtTokenName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdSelectSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstFileProcess;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusLabel;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Timer timerSign;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGetSaveKey;
        private System.Windows.Forms.CheckBox chkChangeFont;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbBU;
        private System.Windows.Forms.Button cmdSelectTest;
        private System.Windows.Forms.TextBox txtTest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btVerifyxml;
        private System.Windows.Forms.Button btSignBackDate;
        private System.Windows.Forms.Timer timerSignBackDate;
    }

}