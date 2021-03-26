namespace TCPClient01
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
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.tbPayload = new System.Windows.Forms.TextBox();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.tbSend = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.num1 = new System.Windows.Forms.TextBox();
            this.num3 = new System.Windows.Forms.TextBox();
            this.num2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbSendBytes = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.SendFile = new System.Windows.Forms.Button();
            this.Browse = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbConsole
            // 
            this.tbConsole.Location = new System.Drawing.Point(8, 15);
            this.tbConsole.Margin = new System.Windows.Forms.Padding(4);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbConsole.Size = new System.Drawing.Size(940, 378);
            this.tbConsole.TabIndex = 0;
            // 
            // tbPayload
            // 
            this.tbPayload.Location = new System.Drawing.Point(725, 422);
            this.tbPayload.Margin = new System.Windows.Forms.Padding(4);
            this.tbPayload.Name = "tbPayload";
            this.tbPayload.Size = new System.Drawing.Size(223, 22);
            this.tbPayload.TabIndex = 1;
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(92, 433);
            this.tbServerPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(132, 22);
            this.tbServerPort.TabIndex = 2;
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(92, 401);
            this.tbServerIP.Margin = new System.Windows.Forms.Padding(4);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(232, 22);
            this.tbServerIP.TabIndex = 3;
            // 
            // tbSend
            // 
            this.tbSend.Location = new System.Drawing.Point(725, 451);
            this.tbSend.Margin = new System.Windows.Forms.Padding(4);
            this.tbSend.Name = "tbSend";
            this.tbSend.Size = new System.Drawing.Size(223, 28);
            this.tbSend.TabIndex = 4;
            this.tbSend.Text = "&Send";
            this.tbSend.UseVisualStyleBackColor = true;
            this.tbSend.Click += new System.EventHandler(this.tbSend_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(232, 430);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(93, 28);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "&Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 431);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Server Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 405);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Server IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(648, 425);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Message:";
            // 
            // num1
            // 
            this.num1.Location = new System.Drawing.Point(120, 24);
            this.num1.Name = "num1";
            this.num1.Size = new System.Drawing.Size(49, 22);
            this.num1.TabIndex = 9;
            this.num1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // num3
            // 
            this.num3.Location = new System.Drawing.Point(120, 80);
            this.num3.Name = "num3";
            this.num3.Size = new System.Drawing.Size(49, 22);
            this.num3.TabIndex = 10;
            this.num3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // num2
            // 
            this.num2.Location = new System.Drawing.Point(120, 52);
            this.num2.Name = "num2";
            this.num2.Size = new System.Drawing.Size(49, 22);
            this.num2.TabIndex = 11;
            this.num2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Position:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Size:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "Step:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbSendBytes);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.num3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.num2);
            this.groupBox1.Controls.Add(this.num1);
            this.groupBox1.Location = new System.Drawing.Point(347, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 118);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send Bytes";
            // 
            // tbSendBytes
            // 
            this.tbSendBytes.Location = new System.Drawing.Point(190, 44);
            this.tbSendBytes.Name = "tbSendBytes";
            this.tbSendBytes.Size = new System.Drawing.Size(94, 38);
            this.tbSendBytes.TabIndex = 16;
            this.tbSendBytes.Text = "Send Bytes";
            this.tbSendBytes.UseVisualStyleBackColor = true;
            this.tbSendBytes.Click += new System.EventHandler(this.tbSendBytes_Click);
            // 
            // btnTime
            // 
            this.btnTime.Location = new System.Drawing.Point(232, 465);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(92, 27);
            this.btnTime.TabIndex = 16;
            this.btnTime.Text = "Get time";
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // SendFile
            // 
            this.SendFile.Location = new System.Drawing.Point(232, 495);
            this.SendFile.Name = "SendFile";
            this.SendFile.Size = new System.Drawing.Size(92, 25);
            this.SendFile.TabIndex = 17;
            this.SendFile.Text = "Send File";
            this.SendFile.UseVisualStyleBackColor = true;
            this.SendFile.Click += new System.EventHandler(this.SendFile_Click);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(137, 465);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(87, 27);
            this.Browse.TabIndex = 17;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(725, 486);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(223, 29);
            this.progressBar1.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(44, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 27);
            this.button1.TabIndex = 18;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 532);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.SendFile);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tbSend);
            this.Controls.Add(this.tbServerIP);
            this.Controls.Add(this.tbServerPort);
            this.Controls.Add(this.tbPayload);
            this.Controls.Add(this.tbConsole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "TCPClient 01";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.TextBox tbPayload;
        private System.Windows.Forms.TextBox tbServerPort;
        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.Button tbSend;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox num1;
        private System.Windows.Forms.TextBox num3;
        private System.Windows.Forms.TextBox num2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button tbSendBytes;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button SendFile;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
    }
}

