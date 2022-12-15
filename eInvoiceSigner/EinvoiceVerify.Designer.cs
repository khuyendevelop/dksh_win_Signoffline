namespace eInvoice.Signer
{
    partial class EinvoiceVerify
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btverify = new System.Windows.Forms.Button();
            this.txtfilepath = new System.Windows.Forms.TextBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdo2 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.txtX509 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btGetInfoX509 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSetToken = new System.Windows.Forms.Button();
            this.txtTokenName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btSignWToken = new System.Windows.Forms.Button();
            this.btSignwithdate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dtSign = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btverify
            // 
            this.btverify.Location = new System.Drawing.Point(430, 52);
            this.btverify.Name = "btverify";
            this.btverify.Size = new System.Drawing.Size(75, 23);
            this.btverify.TabIndex = 0;
            this.btverify.Text = "Verify";
            this.btverify.UseVisualStyleBackColor = true;
            this.btverify.Click += new System.EventHandler(this.btverify_Click);
            // 
            // txtfilepath
            // 
            this.txtfilepath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtfilepath.Location = new System.Drawing.Point(73, 21);
            this.txtfilepath.Name = "txtfilepath";
            this.txtfilepath.ReadOnly = true;
            this.txtfilepath.Size = new System.Drawing.Size(351, 20);
            this.txtfilepath.TabIndex = 1;
            // 
            // btBrowse
            // 
            this.btBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowse.Location = new System.Drawing.Point(430, 18);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 2;
            this.btBrowse.Text = "Browser";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(57, 231);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(599, 184);
            this.txtResult.TabIndex = 3;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(40, 13);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(42, 20);
            this.txtPrefix.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Prefix";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "XML/Pdf file";
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Checked = true;
            this.rdo1.Location = new System.Drawing.Point(86, 14);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(99, 17);
            this.rdo1.TabIndex = 7;
            this.rdo1.TabStop = true;
            this.rdo1.Text = "Verify Signature";
            this.rdo1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdo2);
            this.groupBox1.Controls.Add(this.rdo1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPrefix);
            this.groupBox1.Location = new System.Drawing.Point(57, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 39);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // rdo2
            // 
            this.rdo2.AutoSize = true;
            this.rdo2.Location = new System.Drawing.Point(189, 13);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(170, 17);
            this.rdo2.TabIndex = 8;
            this.rdo2.Text = "Verify Signature and Certificate";
            this.rdo2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(508, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Sign With Default Certificate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtX509
            // 
            this.txtX509.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtX509.Location = new System.Drawing.Point(57, 175);
            this.txtX509.Multiline = true;
            this.txtX509.Name = "txtX509";
            this.txtX509.Size = new System.Drawing.Size(531, 49);
            this.txtX509.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "X509 Cer";
            // 
            // btGetInfoX509
            // 
            this.btGetInfoX509.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGetInfoX509.Location = new System.Drawing.Point(589, 189);
            this.btGetInfoX509.Name = "btGetInfoX509";
            this.btGetInfoX509.Size = new System.Drawing.Size(75, 23);
            this.btGetInfoX509.TabIndex = 13;
            this.btGetInfoX509.Text = "Get Info";
            this.btGetInfoX509.UseVisualStyleBackColor = true;
            this.btGetInfoX509.Click += new System.EventHandler(this.btGetInfoX509_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Result";
            // 
            // cmdSetToken
            // 
            this.cmdSetToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSetToken.Location = new System.Drawing.Point(500, 91);
            this.cmdSetToken.Name = "cmdSetToken";
            this.cmdSetToken.Size = new System.Drawing.Size(166, 23);
            this.cmdSetToken.TabIndex = 15;
            this.cmdSetToken.Text = "Get Token";
            this.cmdSetToken.UseVisualStyleBackColor = true;
            this.cmdSetToken.Click += new System.EventHandler(this.cmdSetToken_Click);
            // 
            // txtTokenName
            // 
            this.txtTokenName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTokenName.Location = new System.Drawing.Point(57, 93);
            this.txtTokenName.Name = "txtTokenName";
            this.txtTokenName.ReadOnly = true;
            this.txtTokenName.Size = new System.Drawing.Size(437, 20);
            this.txtTokenName.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Cer Name";
            // 
            // txtSerial
            // 
            this.txtSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSerial.Location = new System.Drawing.Point(57, 115);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(437, 20);
            this.txtSerial.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Cer Serial";
            // 
            // btSignWToken
            // 
            this.btSignWToken.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSignWToken.Location = new System.Drawing.Point(500, 114);
            this.btSignWToken.Name = "btSignWToken";
            this.btSignWToken.Size = new System.Drawing.Size(166, 23);
            this.btSignWToken.TabIndex = 20;
            this.btSignWToken.Text = "Sign XML With Selected Token";
            this.btSignWToken.UseVisualStyleBackColor = true;
            this.btSignWToken.Click += new System.EventHandler(this.btSignWToken_Click);
            // 
            // btSignwithdate
            // 
            this.btSignwithdate.Location = new System.Drawing.Point(467, 144);
            this.btSignwithdate.Name = "btSignwithdate";
            this.btSignwithdate.Size = new System.Drawing.Size(197, 23);
            this.btSignwithdate.TabIndex = 21;
            this.btSignwithdate.Text = "Sign pdf with Selected Token by date";
            this.btSignwithdate.UseVisualStyleBackColor = true;
            this.btSignwithdate.Click += new System.EventHandler(this.btSignwithdate_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(232, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Sign date";
            // 
            // dtSign
            // 
            this.dtSign.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtSign.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtSign.Location = new System.Drawing.Point(290, 147);
            this.dtSign.Name = "dtSign";
            this.dtSign.Size = new System.Drawing.Size(134, 20);
            this.dtSign.TabIndex = 23;
            // 
            // EinvoiceVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 427);
            this.Controls.Add(this.dtSign);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btSignwithdate);
            this.Controls.Add(this.btSignWToken);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTokenName);
            this.Controls.Add(this.cmdSetToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btGetInfoX509);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtX509);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.txtfilepath);
            this.Controls.Add(this.btverify);
            this.Name = "EinvoiceVerify";
            this.Text = "EinvoiceVerify";
            this.Load += new System.EventHandler(this.EinvoiceVerify_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btverify;
        private System.Windows.Forms.TextBox txtfilepath;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdo1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdo2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtX509;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btGetInfoX509;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSetToken;
        private System.Windows.Forms.TextBox txtTokenName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btSignWToken;
        private System.Windows.Forms.Button btSignwithdate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtSign;
    }
}