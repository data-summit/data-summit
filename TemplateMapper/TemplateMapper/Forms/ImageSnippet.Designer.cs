namespace TemplateMapper.Forms
{
    partial class ImageSnippet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageSnippet));
            this.pnSnippet = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnSnippet
            // 
            this.pnSnippet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSnippet.Location = new System.Drawing.Point(0, 0);
            this.pnSnippet.Name = "pnSnippet";
            this.pnSnippet.Size = new System.Drawing.Size(800, 450);
            this.pnSnippet.TabIndex = 0;
            // 
            // ImageSnippet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnSnippet);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageSnippet";
            this.Text = "Image Snippet";
            this.Load += new System.EventHandler(this.ImageSnippet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSnippet;
    }
}