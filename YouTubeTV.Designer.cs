
namespace YouTubeWMC {
    partial class YouTubeTV {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YouTubeTV));
            this.cefSharpBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // cefSharpBrowser
            // 
            this.cefSharpBrowser.ActivateBrowserOnCreation = false;
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.cefSharpBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cefSharpBrowser.Location = new System.Drawing.Point(0, 0);
            this.cefSharpBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cefSharpBrowser.Name = "cefSharpBrowser";
            this.cefSharpBrowser.Size = new System.Drawing.Size(900, 562);
            this.cefSharpBrowser.TabIndex = 0;
            // 
            // YouTubeTV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.cefSharpBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "YouTubeTV";
            this.Text = "YouTube TV";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser cefSharpBrowser;

        public CefSharp.WinForms.ChromiumWebBrowser GetChromiumWebBrowser() {
            return this.cefSharpBrowser;
        }
    }
}

