namespace MqttTest
{
    partial class FormMain
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.textPublishTopic = new System.Windows.Forms.TextBox();
            this.textPublishPayload = new System.Windows.Forms.TextBox();
            this.textConnectHosturl = new System.Windows.Forms.TextBox();
            this.textConnectPort = new System.Windows.Forms.TextBox();
            this.labelBrokerUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonBlaster = new System.Windows.Forms.Button();
            this.updnPublishInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.updnNumberToPublish = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.labelNumberPublished = new System.Windows.Forms.Label();
            this.textLogs = new System.Windows.Forms.TextBox();
            this.labelLogs = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.updnPublishInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnNumberToPublish)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(446, 27);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(128, 32);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonPublish
            // 
            this.buttonPublish.Location = new System.Drawing.Point(446, 116);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(128, 32);
            this.buttonPublish.TabIndex = 1;
            this.buttonPublish.Text = "Publish";
            this.buttonPublish.UseVisualStyleBackColor = true;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
            // 
            // textPublishTopic
            // 
            this.textPublishTopic.Location = new System.Drawing.Point(117, 116);
            this.textPublishTopic.Name = "textPublishTopic";
            this.textPublishTopic.Size = new System.Drawing.Size(304, 20);
            this.textPublishTopic.TabIndex = 2;
            this.textPublishTopic.Text = "SampleTopic";
            // 
            // textPublishPayload
            // 
            this.textPublishPayload.Location = new System.Drawing.Point(117, 168);
            this.textPublishPayload.Multiline = true;
            this.textPublishPayload.Name = "textPublishPayload";
            this.textPublishPayload.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textPublishPayload.Size = new System.Drawing.Size(304, 65);
            this.textPublishPayload.TabIndex = 3;
            this.textPublishPayload.Text = "Some sample payload";
            // 
            // textConnectHosturl
            // 
            this.textConnectHosturl.Location = new System.Drawing.Point(117, 27);
            this.textConnectHosturl.Name = "textConnectHosturl";
            this.textConnectHosturl.Size = new System.Drawing.Size(304, 20);
            this.textConnectHosturl.TabIndex = 4;
            this.textConnectHosturl.Text = "localhost";
            // 
            // textConnectPort
            // 
            this.textConnectPort.Location = new System.Drawing.Point(117, 53);
            this.textConnectPort.Name = "textConnectPort";
            this.textConnectPort.Size = new System.Drawing.Size(304, 20);
            this.textConnectPort.TabIndex = 5;
            this.textConnectPort.Text = "1883";
            // 
            // labelBrokerUrl
            // 
            this.labelBrokerUrl.AutoSize = true;
            this.labelBrokerUrl.Location = new System.Drawing.Point(24, 30);
            this.labelBrokerUrl.Name = "labelBrokerUrl";
            this.labelBrokerUrl.Size = new System.Drawing.Size(63, 13);
            this.labelBrokerUrl.TabIndex = 6;
            this.labelBrokerUrl.Text = "Broker URL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Broker Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Publish Topic";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Publish Payload";
            // 
            // buttonBlaster
            // 
            this.buttonBlaster.Location = new System.Drawing.Point(446, 269);
            this.buttonBlaster.Name = "buttonBlaster";
            this.buttonBlaster.Size = new System.Drawing.Size(128, 32);
            this.buttonBlaster.TabIndex = 10;
            this.buttonBlaster.Text = "ContinuousPublish";
            this.buttonBlaster.UseVisualStyleBackColor = true;
            this.buttonBlaster.Click += new System.EventHandler(this.buttonBlaster_Click);
            // 
            // updnPublishInterval
            // 
            this.updnPublishInterval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.updnPublishInterval.Location = new System.Drawing.Point(209, 274);
            this.updnPublishInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.updnPublishInterval.Name = "updnPublishInterval";
            this.updnPublishInterval.Size = new System.Drawing.Size(99, 20);
            this.updnPublishInterval.TabIndex = 11;
            this.updnPublishInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.updnPublishInterval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 276);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Publish Interval (milliseconds)";
            // 
            // updnNumberToPublish
            // 
            this.updnNumberToPublish.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.updnNumberToPublish.Location = new System.Drawing.Point(209, 300);
            this.updnNumberToPublish.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.updnNumberToPublish.Name = "updnNumberToPublish";
            this.updnNumberToPublish.Size = new System.Drawing.Size(99, 20);
            this.updnNumberToPublish.TabIndex = 13;
            this.updnNumberToPublish.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.updnNumberToPublish.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 302);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Number to Publish";
            // 
            // labelNumberPublished
            // 
            this.labelNumberPublished.AutoSize = true;
            this.labelNumberPublished.Location = new System.Drawing.Point(455, 307);
            this.labelNumberPublished.Name = "labelNumberPublished";
            this.labelNumberPublished.Size = new System.Drawing.Size(105, 13);
            this.labelNumberPublished.TabIndex = 15;
            this.labelNumberPublished.Text = "Number Published=0";
            // 
            // textLogs
            // 
            this.textLogs.Location = new System.Drawing.Point(30, 384);
            this.textLogs.Multiline = true;
            this.textLogs.Name = "textLogs";
            this.textLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLogs.Size = new System.Drawing.Size(752, 144);
            this.textLogs.TabIndex = 16;
            this.textLogs.Text = "(no logs yet)";
            // 
            // labelLogs
            // 
            this.labelLogs.AutoSize = true;
            this.labelLogs.Location = new System.Drawing.Point(27, 368);
            this.labelLogs.Name = "labelLogs";
            this.labelLogs.Size = new System.Drawing.Size(106, 13);
            this.labelLogs.TabIndex = 17;
            this.labelLogs.Text = "Logs (newest on top)";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 540);
            this.Controls.Add(this.labelLogs);
            this.Controls.Add(this.textLogs);
            this.Controls.Add(this.labelNumberPublished);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.updnNumberToPublish);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.updnPublishInterval);
            this.Controls.Add(this.buttonBlaster);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelBrokerUrl);
            this.Controls.Add(this.textConnectPort);
            this.Controls.Add(this.textConnectHosturl);
            this.Controls.Add(this.textPublishPayload);
            this.Controls.Add(this.textPublishTopic);
            this.Controls.Add(this.buttonPublish);
            this.Controls.Add(this.buttonConnect);
            this.Name = "FormMain";
            this.Text = "MQTT Test";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updnPublishInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updnNumberToPublish)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.TextBox textPublishTopic;
        private System.Windows.Forms.TextBox textPublishPayload;
        private System.Windows.Forms.TextBox textConnectHosturl;
        private System.Windows.Forms.TextBox textConnectPort;
        private System.Windows.Forms.Label labelBrokerUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonBlaster;
        private System.Windows.Forms.NumericUpDown updnPublishInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown updnNumberToPublish;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelNumberPublished;
        private System.Windows.Forms.TextBox textLogs;
        private System.Windows.Forms.Label labelLogs;
        private System.Windows.Forms.Timer timer1;
    }
}

