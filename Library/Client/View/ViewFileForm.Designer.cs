namespace Server
{
    partial class ViewFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewFile));
            this.view1 = new AxAcroPDFLib.AxAcroPDF();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.view1)).BeginInit();
            this.SuspendLayout();
            // 
            // view1
            // 
            this.view1.Enabled = true;
            this.view1.Location = new System.Drawing.Point(12, 74);
            this.view1.Name = "view1";
            this.view1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("view1.OcxState")));
            this.view1.Size = new System.Drawing.Size(1300, 891);
            this.view1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(547, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(231, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ViewFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1324, 977);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.view1);
            this.Name = "ViewFile";
            this.Text = "ViewFile";
            this.Load += new System.EventHandler(this.ViewFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.view1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF view1;
        private System.Windows.Forms.Button button1;
    }
}