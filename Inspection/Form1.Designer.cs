namespace eFormTester
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtApi_Id = new System.Windows.Forms.TextBox();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.btnSendSample = new System.Windows.Forms.Button();
            this.btnCheckId = new System.Windows.Forms.Button();
            this.btnFetchId = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnSendXml = new System.Windows.Forms.Button();
            this.btnCreateXml = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tbxRequest = new System.Windows.Forms.TextBox();
            this.tbxResponse = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCreateId = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
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
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Server address:";
            // 
            // txtApi_Id
            // 
            this.txtApi_Id.Location = new System.Drawing.Point(54, 19);
            this.txtApi_Id.Name = "txtApi_Id";
            this.txtApi_Id.Size = new System.Drawing.Size(225, 20);
            this.txtApi_Id.TabIndex = 8;
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(54, 13);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(225, 20);
            this.txtToken.TabIndex = 11;
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(93, 33);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(186, 20);
            this.txtServerAddress.TabIndex = 12;
            // 
            // btnSendSample
            // 
            this.btnSendSample.Location = new System.Drawing.Point(332, 19);
            this.btnSendSample.Name = "btnSendSample";
            this.btnSendSample.Size = new System.Drawing.Size(188, 23);
            this.btnSendSample.TabIndex = 13;
            this.btnSendSample.Text = "Send Sample XML";
            this.toolTip1.SetToolTip(this.btnSendSample, "Sends an auto. gen. XML to the API, and gets a return message");
            this.btnSendSample.UseVisualStyleBackColor = true;
            this.btnSendSample.Click += new System.EventHandler(this.btnSendSample_Click);
            // 
            // btnCheckId
            // 
            this.btnCheckId.Location = new System.Drawing.Point(429, 77);
            this.btnCheckId.Name = "btnCheckId";
            this.btnCheckId.Size = new System.Drawing.Size(91, 23);
            this.btnCheckId.TabIndex = 14;
            this.btnCheckId.Text = "Check Id";
            this.toolTip1.SetToolTip(this.btnCheckId, "Sends Id from the box, and get the return status, and shows response");
            this.btnCheckId.UseVisualStyleBackColor = true;
            this.btnCheckId.Click += new System.EventHandler(this.btnCheckId_Click);
            // 
            // btnFetchId
            // 
            this.btnFetchId.Location = new System.Drawing.Point(429, 106);
            this.btnFetchId.Name = "btnFetchId";
            this.btnFetchId.Size = new System.Drawing.Size(91, 23);
            this.btnFetchId.TabIndex = 15;
            this.btnFetchId.Text = "Fetch Id";
            this.toolTip1.SetToolTip(this.btnFetchId, "Sends Id from the box, and get the return data, and shows response");
            this.btnFetchId.UseVisualStyleBackColor = true;
            this.btnFetchId.Click += new System.EventHandler(this.btnFetchId_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtApi_Id);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(16, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 55);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "API Id";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtToken);
            this.groupBox3.Controls.Add(this.txtServerAddress);
            this.groupBox3.Location = new System.Drawing.Point(16, 82);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 64);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Server";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnCreateId);
            this.groupBox7.Controls.Add(this.btnSendXml);
            this.groupBox7.Controls.Add(this.btnCreateXml);
            this.groupBox7.Controls.Add(this.btnDelete);
            this.groupBox7.Controls.Add(this.groupBox3);
            this.groupBox7.Controls.Add(this.btnSendSample);
            this.groupBox7.Controls.Add(this.btnCheckId);
            this.groupBox7.Controls.Add(this.btnFetchId);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(5, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(860, 201);
            this.groupBox7.TabIndex = 20;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Config (step 1)";
            // 
            // btnSendXml
            // 
            this.btnSendXml.Location = new System.Drawing.Point(429, 48);
            this.btnSendXml.Name = "btnSendXml";
            this.btnSendXml.Size = new System.Drawing.Size(91, 23);
            this.btnSendXml.TabIndex = 25;
            this.btnSendXml.Text = "Send XML";
            this.toolTip1.SetToolTip(this.btnSendXml, "Sends XML from the box, and shows response");
            this.btnSendXml.UseVisualStyleBackColor = true;
            this.btnSendXml.Click += new System.EventHandler(this.btnSendXml_Click);
            // 
            // btnCreateXml
            // 
            this.btnCreateXml.Location = new System.Drawing.Point(332, 48);
            this.btnCreateXml.Name = "btnCreateXml";
            this.btnCreateXml.Size = new System.Drawing.Size(91, 23);
            this.btnCreateXml.TabIndex = 23;
            this.btnCreateXml.Text = "Sample XML";
            this.toolTip1.SetToolTip(this.btnCreateXml, "Creates auto. gen. XML");
            this.btnCreateXml.UseVisualStyleBackColor = true;
            this.btnCreateXml.Click += new System.EventHandler(this.btnCreateXml_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(429, 135);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 23);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "Delete Id";
            this.toolTip1.SetToolTip(this.btnDelete, "Sends Id from the box, and deletes the Id\'s data, and shows response");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tbxRequest
            // 
            this.tbxRequest.Location = new System.Drawing.Point(21, 247);
            this.tbxRequest.Multiline = true;
            this.tbxRequest.Name = "tbxRequest";
            this.tbxRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxRequest.Size = new System.Drawing.Size(407, 359);
            this.tbxRequest.TabIndex = 21;
            // 
            // tbxResponse
            // 
            this.tbxResponse.Location = new System.Drawing.Point(437, 247);
            this.tbxResponse.Multiline = true;
            this.tbxResponse.Name = "tbxResponse";
            this.tbxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxResponse.Size = new System.Drawing.Size(407, 359);
            this.tbxResponse.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Message sent to Microting (the box)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 231);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Message recieved from Microting";
            // 
            // btnCreateId
            // 
            this.btnCreateId.Location = new System.Drawing.Point(332, 77);
            this.btnCreateId.Name = "btnCreateId";
            this.btnCreateId.Size = new System.Drawing.Size(91, 81);
            this.btnCreateId.TabIndex = 28;
            this.btnCreateId.Text = "Sample Id";
            this.toolTip1.SetToolTip(this.btnCreateId, "Creates auto. gen. XML, sends it, gets the Id, and creates sample Id");
            this.btnCreateId.UseVisualStyleBackColor = true;
            this.btnCreateId.Click += new System.EventHandler(this.btnCreateId_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 628);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxResponse);
            this.Controls.Add(this.tbxRequest);
            this.Controls.Add(this.groupBox7);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Microting eForm C# example";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtApi_Id;
        private System.Windows.Forms.TextBox txtToken;
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
        private System.Windows.Forms.Button btnCreateId;
    }
}

