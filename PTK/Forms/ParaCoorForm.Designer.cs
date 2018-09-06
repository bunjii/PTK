namespace PTK
{
    partial class ParaCoorForm
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
            this.webBrowserForParaCood = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserForParaCood
            // 
            this.webBrowserForParaCood.Location = new System.Drawing.Point(2, 1);
            this.webBrowserForParaCood.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserForParaCood.Name = "webBrowserForParaCood";
            this.webBrowserForParaCood.ScrollBarsEnabled = false;
            this.webBrowserForParaCood.Size = new System.Drawing.Size(880, 662);
            this.webBrowserForParaCood.TabIndex = 0;
            this.webBrowserForParaCood.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowserForParaCood.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserForParaCood_DocumentCompleted);
            // 
            // ParaCoorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.webBrowserForParaCood);
            this.Name = "ParaCoorForm";
            this.Text = "ParaCoorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParaCoorForm_FormClosing);
            this.Load += new System.EventHandler(this.ParaCoorForm_Load);
            this.Shown += new System.EventHandler(this.ParaCoorForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserForParaCood;
    }
}