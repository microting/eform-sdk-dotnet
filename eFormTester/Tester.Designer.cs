namespace eFormTester
{
    partial class Tester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tester));
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtApiId = new System.Windows.Forms.TextBox();
            this.txtServerToken = new System.Windows.Forms.TextBox();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.btnSendSample = new System.Windows.Forms.Button();
            this.btnCheckId = new System.Windows.Forms.Button();
            this.btnFetchId = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOrganizationId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNotificationToken = new System.Windows.Forms.TextBox();
            this.txtNotificationAddress = new System.Windows.Forms.TextBox();
            this.btnUnsub = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSendExtXml = new System.Windows.Forms.Button();
            this.cbxDropDown = new System.Windows.Forms.ComboBox();
            this.btnSub = new System.Windows.Forms.Button();
            this.btnRetriveId = new System.Windows.Forms.Button();
            this.btnSendXml = new System.Windows.Forms.Button();
            this.btnCreateXml = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.tbxRequest = new System.Windows.Forms.TextBox();
            this.tbxResponse = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnVerifyXmlResponse = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "API Id:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Token:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Address:";
            // 
            // txtApiId
            // 
            this.txtApiId.Location = new System.Drawing.Point(54, 13);
            this.txtApiId.Name = "txtApiId";
            this.txtApiId.Size = new System.Drawing.Size(225, 20);
            this.txtApiId.TabIndex = 8;
            // 
            // txtServerToken
            // 
            this.txtServerToken.Location = new System.Drawing.Point(54, 13);
            this.txtServerToken.Name = "txtServerToken";
            this.txtServerToken.Size = new System.Drawing.Size(225, 20);
            this.txtServerToken.TabIndex = 11;
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(54, 33);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(225, 20);
            this.txtServerAddress.TabIndex = 12;
            // 
            // btnSendSample
            // 
            this.btnSendSample.BackColor = System.Drawing.Color.White;
            this.btnSendSample.Location = new System.Drawing.Point(311, 19);
            this.btnSendSample.Name = "btnSendSample";
            this.btnSendSample.Size = new System.Drawing.Size(91, 23);
            this.btnSendSample.TabIndex = 13;
            this.btnSendSample.Text = "Send Sample XML";
            this.toolTip1.SetToolTip(this.btnSendSample, "Sends an auto. gen. XML to the API, and gets a return message");
            this.btnSendSample.UseVisualStyleBackColor = false;
            this.btnSendSample.Click += new System.EventHandler(this.btnSendSample_Click);
            // 
            // btnCheckId
            // 
            this.btnCheckId.BackColor = System.Drawing.Color.Yellow;
            this.btnCheckId.Location = new System.Drawing.Point(408, 106);
            this.btnCheckId.Name = "btnCheckId";
            this.btnCheckId.Size = new System.Drawing.Size(91, 23);
            this.btnCheckId.TabIndex = 14;
            this.btnCheckId.Text = "Check Id";
            this.toolTip1.SetToolTip(this.btnCheckId, "Sends Id from the box, and get the return status, and shows response");
            this.btnCheckId.UseVisualStyleBackColor = false;
            this.btnCheckId.Click += new System.EventHandler(this.btnCheckId_Click);
            // 
            // btnFetchId
            // 
            this.btnFetchId.BackColor = System.Drawing.Color.Yellow;
            this.btnFetchId.Location = new System.Drawing.Point(311, 106);
            this.btnFetchId.Name = "btnFetchId";
            this.btnFetchId.Size = new System.Drawing.Size(91, 23);
            this.btnFetchId.TabIndex = 15;
            this.btnFetchId.Text = "Fetch Id";
            this.toolTip1.SetToolTip(this.btnFetchId, "Sends Id from the box, and get the return data, and shows response");
            this.btnFetchId.UseVisualStyleBackColor = false;
            this.btnFetchId.Click += new System.EventHandler(this.btnFetchId_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOrganizationId);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtApiId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(19, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 61);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Id";
            // 
            // txtOrganizationId
            // 
            this.txtOrganizationId.Location = new System.Drawing.Point(93, 33);
            this.txtOrganizationId.Name = "txtOrganizationId";
            this.txtOrganizationId.Size = new System.Drawing.Size(186, 20);
            this.txtOrganizationId.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtOrganizationId, "ONLY needed if using Entity Select or Entity Search data items");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Organization Id:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtServerToken);
            this.groupBox3.Controls.Add(this.txtServerAddress);
            this.groupBox3.Location = new System.Drawing.Point(19, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 62);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Server";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.groupBox2);
            this.groupBox7.Controls.Add(this.btnUnsub);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.btnSendExtXml);
            this.groupBox7.Controls.Add(this.cbxDropDown);
            this.groupBox7.Controls.Add(this.btnSub);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.btnRetriveId);
            this.groupBox7.Controls.Add(this.btnSendXml);
            this.groupBox7.Controls.Add(this.btnCreateXml);
            this.groupBox7.Controls.Add(this.btnDelete);
            this.groupBox7.Controls.Add(this.btnSendSample);
            this.groupBox7.Controls.Add(this.btnCheckId);
            this.groupBox7.Controls.Add(this.btnFetchId);
            this.groupBox7.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(5, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(854, 173);
            this.groupBox7.TabIndex = 20;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Config (first step)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtNotificationToken);
            this.groupBox2.Controls.Add(this.txtNotificationAddress);
            this.groupBox2.Location = new System.Drawing.Point(547, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 62);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notification Subscriber";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Token:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Address:";
            // 
            // txtNotificationToken
            // 
            this.txtNotificationToken.Location = new System.Drawing.Point(54, 13);
            this.txtNotificationToken.Name = "txtNotificationToken";
            this.txtNotificationToken.Size = new System.Drawing.Size(225, 20);
            this.txtNotificationToken.TabIndex = 11;
            // 
            // txtNotificationAddress
            // 
            this.txtNotificationAddress.Location = new System.Drawing.Point(54, 33);
            this.txtNotificationAddress.Name = "txtNotificationAddress";
            this.txtNotificationAddress.Size = new System.Drawing.Size(225, 20);
            this.txtNotificationAddress.TabIndex = 12;
            // 
            // btnUnsub
            // 
            this.btnUnsub.BackColor = System.Drawing.Color.Red;
            this.btnUnsub.Location = new System.Drawing.Point(743, 116);
            this.btnUnsub.Name = "btnUnsub";
            this.btnUnsub.Size = new System.Drawing.Size(91, 23);
            this.btnUnsub.TabIndex = 35;
            this.btnUnsub.Text = "Unsubscribe";
            this.btnUnsub.UseVisualStyleBackColor = false;
            this.btnUnsub.Click += new System.EventHandler(this.btnUnsub_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Basic Features";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(405, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Advanced Features";
            // 
            // btnSendExtXml
            // 
            this.btnSendExtXml.BackColor = System.Drawing.Color.LawnGreen;
            this.btnSendExtXml.Location = new System.Drawing.Point(408, 77);
            this.btnSendExtXml.Name = "btnSendExtXml";
            this.btnSendExtXml.Size = new System.Drawing.Size(91, 23);
            this.btnSendExtXml.TabIndex = 32;
            this.btnSendExtXml.Text = "Send Ext. XML";
            this.toolTip1.SetToolTip(this.btnSendExtXml, "Sends Extended XML from the box, and shows response. ONLY needed if complex XML e" +
        "lements are included");
            this.btnSendExtXml.UseVisualStyleBackColor = false;
            this.btnSendExtXml.Click += new System.EventHandler(this.btnSendExtXml_Click);
            // 
            // cbxDropDown
            // 
            this.cbxDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDropDown.FormattingEnabled = true;
            this.cbxDropDown.Location = new System.Drawing.Point(408, 49);
            this.cbxDropDown.Name = "cbxDropDown";
            this.cbxDropDown.Size = new System.Drawing.Size(130, 21);
            this.cbxDropDown.TabIndex = 31;
            this.cbxDropDown.SelectedIndexChanged += new System.EventHandler(this.cbxDropDown_SelectedIndexChanged);
            // 
            // btnSub
            // 
            this.btnSub.BackColor = System.Drawing.Color.LawnGreen;
            this.btnSub.Location = new System.Drawing.Point(743, 87);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(91, 23);
            this.btnSub.TabIndex = 30;
            this.btnSub.Text = "Subscribe";
            this.btnSub.UseVisualStyleBackColor = false;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnRetriveId
            // 
            this.btnRetriveId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRetriveId.Location = new System.Drawing.Point(505, 77);
            this.btnRetriveId.Name = "btnRetriveId";
            this.btnRetriveId.Size = new System.Drawing.Size(33, 81);
            this.btnRetriveId.TabIndex = 29;
            this.btnRetriveId.Text = "Get Id";
            this.toolTip1.SetToolTip(this.btnRetriveId, "Retrives the Id from the response, and moves it to the box");
            this.btnRetriveId.UseVisualStyleBackColor = false;
            this.btnRetriveId.Click += new System.EventHandler(this.btnRetriveId_Click);
            // 
            // btnSendXml
            // 
            this.btnSendXml.BackColor = System.Drawing.Color.LawnGreen;
            this.btnSendXml.Location = new System.Drawing.Point(311, 77);
            this.btnSendXml.Name = "btnSendXml";
            this.btnSendXml.Size = new System.Drawing.Size(91, 23);
            this.btnSendXml.TabIndex = 25;
            this.btnSendXml.Text = "Send XML";
            this.toolTip1.SetToolTip(this.btnSendXml, "Sends XML from the box, and shows response");
            this.btnSendXml.UseVisualStyleBackColor = false;
            this.btnSendXml.Click += new System.EventHandler(this.btnSendXml_Click);
            // 
            // btnCreateXml
            // 
            this.btnCreateXml.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCreateXml.Location = new System.Drawing.Point(311, 48);
            this.btnCreateXml.Name = "btnCreateXml";
            this.btnCreateXml.Size = new System.Drawing.Size(91, 23);
            this.btnCreateXml.TabIndex = 23;
            this.btnCreateXml.Text = "Generate XML";
            this.toolTip1.SetToolTip(this.btnCreateXml, "Creates auto. gen. XML");
            this.btnCreateXml.UseVisualStyleBackColor = false;
            this.btnCreateXml.Click += new System.EventHandler(this.btnCreateXml_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Red;
            this.btnDelete.Location = new System.Drawing.Point(311, 135);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 23);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "Delete Id";
            this.toolTip1.SetToolTip(this.btnDelete, "Sends Id from the box, marks the Id\'s for later deletion, and shows the response");
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(337, 184);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(91, 23);
            this.btnVerify.TabIndex = 30;
            this.btnVerify.Text = "Verify XML";
            this.toolTip1.SetToolTip(this.btnVerify, "Checks if the XML request is convertable.");
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerifyXmlRequest_Click);
            // 
            // tbxRequest
            // 
            this.tbxRequest.Location = new System.Drawing.Point(21, 205);
            this.tbxRequest.Multiline = true;
            this.tbxRequest.Name = "tbxRequest";
            this.tbxRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxRequest.Size = new System.Drawing.Size(407, 359);
            this.tbxRequest.TabIndex = 21;
            this.tbxRequest.Text = "575789";
            // 
            // tbxResponse
            // 
            this.tbxResponse.Location = new System.Drawing.Point(437, 205);
            this.tbxResponse.Multiline = true;
            this.tbxResponse.Name = "tbxResponse";
            this.tbxResponse.ReadOnly = true;
            this.tbxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxResponse.Size = new System.Drawing.Size(407, 359);
            this.tbxResponse.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Message sent to Microting (the box)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 189);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Message recieved from Microting";
            // 
            // btnVerifyXmlResponse
            // 
            this.btnVerifyXmlResponse.Location = new System.Drawing.Point(753, 184);
            this.btnVerifyXmlResponse.Name = "btnVerifyXmlResponse";
            this.btnVerifyXmlResponse.Size = new System.Drawing.Size(91, 23);
            this.btnVerifyXmlResponse.TabIndex = 31;
            this.btnVerifyXmlResponse.Text = "Verify XML Response";
            this.toolTip1.SetToolTip(this.btnVerifyXmlResponse, "Checks if the XML response is convertable.");
            this.btnVerifyXmlResponse.UseVisualStyleBackColor = true;
            this.btnVerifyXmlResponse.Click += new System.EventHandler(this.btnVerifyXmlResponse_Click);
            // 
            // Tester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 588);
            this.Controls.Add(this.btnVerifyXmlResponse);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxResponse);
            this.Controls.Add(this.tbxRequest);
            this.Controls.Add(this.groupBox7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Tester";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Microting eForm C# example";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtApiId;
        private System.Windows.Forms.TextBox txtServerToken;
        private System.Windows.Forms.TextBox txtServerAddress;
        private System.Windows.Forms.Button btnSendSample;
        private System.Windows.Forms.Button btnCheckId;
        private System.Windows.Forms.Button btnFetchId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox tbxRequest;
        private System.Windows.Forms.TextBox tbxResponse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnCreateXml;
        private System.Windows.Forms.Button btnSendXml;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRetriveId;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnVerifyXmlResponse;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.TextBox txtOrganizationId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxDropDown;
        private System.Windows.Forms.Button btnSendExtXml;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUnsub;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNotificationToken;
        private System.Windows.Forms.TextBox txtNotificationAddress;
    }
}

