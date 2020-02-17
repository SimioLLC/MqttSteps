namespace MqttSteps
{
    partial class FormMqttStatus
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textUrl = new System.Windows.Forms.TextBox();
            this.textReceivedPayload = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textPublishPayload = new System.Windows.Forms.TextBox();
            this.buttonSubscribe = new System.Windows.Forms.Button();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.textTopic = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 17);
            this.label5.TabIndex = 26;
            this.label5.Text = "MQTT Server Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "MQTT Server Address";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(192, 55);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(193, 22);
            this.textPort.TabIndex = 24;
            this.textPort.Text = "1883";
            this.textPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textUrl
            // 
            this.textUrl.Location = new System.Drawing.Point(192, 27);
            this.textUrl.Name = "textUrl";
            this.textUrl.Size = new System.Drawing.Size(193, 22);
            this.textUrl.TabIndex = 23;
            this.textUrl.Text = "localhost";
            this.textUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textReceivedPayload
            // 
            this.textReceivedPayload.Location = new System.Drawing.Point(264, 210);
            this.textReceivedPayload.Multiline = true;
            this.textReceivedPayload.Name = "textReceivedPayload";
            this.textReceivedPayload.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textReceivedPayload.Size = new System.Drawing.Size(268, 181);
            this.textReceivedPayload.TabIndex = 27;
            this.textReceivedPayload.Text = " ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(117, -35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 17);
            this.label6.TabIndex = 33;
            this.label6.Text = "Topic";
            // 
            // textPublishPayload
            // 
            this.textPublishPayload.Location = new System.Drawing.Point(27, 210);
            this.textPublishPayload.Multiline = true;
            this.textPublishPayload.Name = "textPublishPayload";
            this.textPublishPayload.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textPublishPayload.Size = new System.Drawing.Size(193, 129);
            this.textPublishPayload.TabIndex = 32;
            this.textPublishPayload.Text = "The time has come, the walrus said, to speak of many things";
            // 
            // buttonSubscribe
            // 
            this.buttonSubscribe.Location = new System.Drawing.Point(264, 131);
            this.buttonSubscribe.Name = "buttonSubscribe";
            this.buttonSubscribe.Size = new System.Drawing.Size(193, 46);
            this.buttonSubscribe.TabIndex = 31;
            this.buttonSubscribe.Text = "Subscribe";
            this.buttonSubscribe.UseVisualStyleBackColor = true;
            // 
            // buttonPublish
            // 
            this.buttonPublish.Location = new System.Drawing.Point(27, 345);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(193, 46);
            this.buttonPublish.TabIndex = 30;
            this.buttonPublish.Text = "Publish";
            this.buttonPublish.UseVisualStyleBackColor = true;
            // 
            // textTopic
            // 
            this.textTopic.Location = new System.Drawing.Point(27, 130);
            this.textTopic.Name = "textTopic";
            this.textTopic.Size = new System.Drawing.Size(193, 22);
            this.textTopic.TabIndex = 29;
            this.textTopic.Text = "test/topic";
            this.textTopic.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(27, 158);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(193, 46);
            this.buttonConnect.TabIndex = 28;
            this.buttonConnect.Text = "Connect Publisher";
            this.buttonConnect.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 35;
            this.label1.Text = "Topic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 36;
            this.label2.Text = "Payload";
            // 
            // FormMqttStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 429);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textPublishPayload);
            this.Controls.Add(this.buttonSubscribe);
            this.Controls.Add(this.buttonPublish);
            this.Controls.Add(this.textTopic);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textReceivedPayload);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textPort);
            this.Controls.Add(this.textUrl);
            this.Name = "FormMqttStatus";
            this.Text = "MQTT Status";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textUrl;
        private System.Windows.Forms.TextBox textReceivedPayload;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPublishPayload;
        private System.Windows.Forms.Button buttonSubscribe;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.TextBox textTopic;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}