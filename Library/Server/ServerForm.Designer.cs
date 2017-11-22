/*
 * Created by SharpDevelop.
 * User: chepchip
 * Date: 11/11/2016
 * Time: 11:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Library
{
	partial class ServerForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIpAddress = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLogConnect = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbNumberClient = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 81);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 44);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // tbIpAddress
            // 
            this.tbIpAddress.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tbIpAddress.Location = new System.Drawing.Point(103, 17);
            this.tbIpAddress.Margin = new System.Windows.Forms.Padding(6);
            this.tbIpAddress.Name = "tbIpAddress";
            this.tbIpAddress.Size = new System.Drawing.Size(206, 31);
            this.tbIpAddress.TabIndex = 2;
            this.tbIpAddress.Text = "127.0.0.1";
            // 
            // tbPort
            // 
            this.tbPort.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tbPort.Location = new System.Drawing.Point(103, 81);
            this.tbPort.Margin = new System.Windows.Forms.Padding(6);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(206, 31);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "13000";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStart.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnStart.Location = new System.Drawing.Point(354, 182);
            this.btnStart.Margin = new System.Windows.Forms.Padding(6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(160, 129);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.StartClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLogConnect);
            this.groupBox1.Location = new System.Drawing.Point(21, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(587, 228);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Control";
            // 
            // tbLogConnect
            // 
            this.tbLogConnect.Location = new System.Drawing.Point(14, 37);
            this.tbLogConnect.Margin = new System.Windows.Forms.Padding(6);
            this.tbLogConnect.Multiline = true;
            this.tbLogConnect.Name = "tbLogConnect";
            this.tbLogConnect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLogConnect.Size = new System.Drawing.Size(552, 141);
            this.tbLogConnect.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tbPort);
            this.panel1.Controls.Add(this.tbIpAddress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(21, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 129);
            this.panel1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe Script", 28.125F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(332, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(306, 124);
            this.label3.TabIndex = 8;
            this.label3.Text = "Server";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(21, 464);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(618, 267);
            this.panel2.TabIndex = 9;
            // 
            // lbNumberClient
            // 
            this.lbNumberClient.AutoSize = true;
            this.lbNumberClient.Location = new System.Drawing.Point(285, 370);
            this.lbNumberClient.Name = "lbNumberClient";
            this.lbNumberClient.Size = new System.Drawing.Size(24, 25);
            this.lbNumberClient.TabIndex = 10;
            this.lbNumberClient.Text = "0";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(650, 743);
            this.Controls.Add(this.lbNumberClient);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ServerForm";
            this.Text = "LibraryManager";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.TextBox tbLogConnect;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.TextBox tbIpAddress;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbNumberClient;
    }
}
