namespace TemplateMapper.Forms
{
    partial class OCR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OCR));
            this.tbEditor = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.gbAmazon = new System.Windows.Forms.GroupBox();
            this.btnAmazonSubmit = new System.Windows.Forms.Button();
            this.btnAmazonEditor = new System.Windows.Forms.Button();
            this.tbAmazon = new System.Windows.Forms.TextBox();
            this.gbGoogle = new System.Windows.Forms.GroupBox();
            this.btnGoogleSubmit = new System.Windows.Forms.Button();
            this.btnGoogleEditor = new System.Windows.Forms.Button();
            this.tbGoogle = new System.Windows.Forms.TextBox();
            this.gbAzure = new System.Windows.Forms.GroupBox();
            this.btnAzureSubmit = new System.Windows.Forms.Button();
            this.btnAzureEditor = new System.Windows.Forms.Button();
            this.tbAzure = new System.Windows.Forms.TextBox();
            this.pnSnippet = new System.Windows.Forms.Panel();
            this.gbCoords = new System.Windows.Forms.GroupBox();
            this.rbExisting = new System.Windows.Forms.RadioButton();
            this.pnAzureColor = new System.Windows.Forms.Panel();
            this.rbAzure = new System.Windows.Forms.RadioButton();
            this.pnGoogleColor = new System.Windows.Forms.Panel();
            this.rbGoogle = new System.Windows.Forms.RadioButton();
            this.pnAmazonColor = new System.Windows.Forms.Panel();
            this.rbAmazon = new System.Windows.Forms.RadioButton();
            this.gbAmazon.SuspendLayout();
            this.gbGoogle.SuspendLayout();
            this.gbAzure.SuspendLayout();
            this.gbCoords.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbEditor
            // 
            this.tbEditor.Location = new System.Drawing.Point(409, 12);
            this.tbEditor.Multiline = true;
            this.tbEditor.Name = "tbEditor";
            this.tbEditor.Size = new System.Drawing.Size(250, 50);
            this.tbEditor.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(665, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(125, 50);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(799, 12);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(125, 50);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // gbAmazon
            // 
            this.gbAmazon.Controls.Add(this.btnAmazonSubmit);
            this.gbAmazon.Controls.Add(this.btnAmazonEditor);
            this.gbAmazon.Controls.Add(this.tbAmazon);
            this.gbAmazon.Location = new System.Drawing.Point(409, 68);
            this.gbAmazon.Name = "gbAmazon";
            this.gbAmazon.Size = new System.Drawing.Size(515, 59);
            this.gbAmazon.TabIndex = 5;
            this.gbAmazon.TabStop = false;
            this.gbAmazon.Text = "Amazon";
            // 
            // btnAmazonSubmit
            // 
            this.btnAmazonSubmit.Location = new System.Drawing.Point(388, 18);
            this.btnAmazonSubmit.Name = "btnAmazonSubmit";
            this.btnAmazonSubmit.Size = new System.Drawing.Size(121, 30);
            this.btnAmazonSubmit.TabIndex = 8;
            this.btnAmazonSubmit.Text = "Submit";
            this.btnAmazonSubmit.UseVisualStyleBackColor = true;
            this.btnAmazonSubmit.Click += new System.EventHandler(this.btnAmazonSubmit_Click);
            // 
            // btnAmazonEditor
            // 
            this.btnAmazonEditor.Location = new System.Drawing.Point(256, 18);
            this.btnAmazonEditor.Name = "btnAmazonEditor";
            this.btnAmazonEditor.Size = new System.Drawing.Size(125, 30);
            this.btnAmazonEditor.TabIndex = 7;
            this.btnAmazonEditor.Text = "To Editor";
            this.btnAmazonEditor.UseVisualStyleBackColor = true;
            this.btnAmazonEditor.Click += new System.EventHandler(this.btnAmazonEditor_Click);
            // 
            // tbAmazon
            // 
            this.tbAmazon.Location = new System.Drawing.Point(7, 22);
            this.tbAmazon.Name = "tbAmazon";
            this.tbAmazon.ReadOnly = true;
            this.tbAmazon.Size = new System.Drawing.Size(243, 22);
            this.tbAmazon.TabIndex = 0;
            // 
            // gbGoogle
            // 
            this.gbGoogle.Controls.Add(this.btnGoogleSubmit);
            this.gbGoogle.Controls.Add(this.btnGoogleEditor);
            this.gbGoogle.Controls.Add(this.tbGoogle);
            this.gbGoogle.Location = new System.Drawing.Point(409, 133);
            this.gbGoogle.Name = "gbGoogle";
            this.gbGoogle.Size = new System.Drawing.Size(515, 59);
            this.gbGoogle.TabIndex = 9;
            this.gbGoogle.TabStop = false;
            this.gbGoogle.Text = "Google";
            // 
            // btnGoogleSubmit
            // 
            this.btnGoogleSubmit.Location = new System.Drawing.Point(388, 18);
            this.btnGoogleSubmit.Name = "btnGoogleSubmit";
            this.btnGoogleSubmit.Size = new System.Drawing.Size(121, 30);
            this.btnGoogleSubmit.TabIndex = 8;
            this.btnGoogleSubmit.Text = "Submit";
            this.btnGoogleSubmit.UseVisualStyleBackColor = true;
            this.btnGoogleSubmit.Click += new System.EventHandler(this.btnGoogleSubmit_Click);
            // 
            // btnGoogleEditor
            // 
            this.btnGoogleEditor.Location = new System.Drawing.Point(256, 18);
            this.btnGoogleEditor.Name = "btnGoogleEditor";
            this.btnGoogleEditor.Size = new System.Drawing.Size(125, 30);
            this.btnGoogleEditor.TabIndex = 7;
            this.btnGoogleEditor.Text = "To Editor";
            this.btnGoogleEditor.UseVisualStyleBackColor = true;
            this.btnGoogleEditor.Click += new System.EventHandler(this.btnGoogleEditor_Click);
            // 
            // tbGoogle
            // 
            this.tbGoogle.Location = new System.Drawing.Point(7, 22);
            this.tbGoogle.Name = "tbGoogle";
            this.tbGoogle.ReadOnly = true;
            this.tbGoogle.Size = new System.Drawing.Size(243, 22);
            this.tbGoogle.TabIndex = 0;
            // 
            // gbAzure
            // 
            this.gbAzure.Controls.Add(this.btnAzureSubmit);
            this.gbAzure.Controls.Add(this.btnAzureEditor);
            this.gbAzure.Controls.Add(this.tbAzure);
            this.gbAzure.Location = new System.Drawing.Point(409, 198);
            this.gbAzure.Name = "gbAzure";
            this.gbAzure.Size = new System.Drawing.Size(515, 59);
            this.gbAzure.TabIndex = 10;
            this.gbAzure.TabStop = false;
            this.gbAzure.Text = "Azure";
            // 
            // btnAzureSubmit
            // 
            this.btnAzureSubmit.Location = new System.Drawing.Point(388, 18);
            this.btnAzureSubmit.Name = "btnAzureSubmit";
            this.btnAzureSubmit.Size = new System.Drawing.Size(121, 30);
            this.btnAzureSubmit.TabIndex = 8;
            this.btnAzureSubmit.Text = "Submit";
            this.btnAzureSubmit.UseVisualStyleBackColor = true;
            this.btnAzureSubmit.Click += new System.EventHandler(this.btnAzureSubmit_Click);
            // 
            // btnAzureEditor
            // 
            this.btnAzureEditor.Location = new System.Drawing.Point(256, 18);
            this.btnAzureEditor.Name = "btnAzureEditor";
            this.btnAzureEditor.Size = new System.Drawing.Size(125, 30);
            this.btnAzureEditor.TabIndex = 7;
            this.btnAzureEditor.Text = "To Editor";
            this.btnAzureEditor.UseVisualStyleBackColor = true;
            this.btnAzureEditor.Click += new System.EventHandler(this.btnAzureEditor_Click);
            // 
            // tbAzure
            // 
            this.tbAzure.Location = new System.Drawing.Point(7, 22);
            this.tbAzure.Name = "tbAzure";
            this.tbAzure.ReadOnly = true;
            this.tbAzure.Size = new System.Drawing.Size(243, 22);
            this.tbAzure.TabIndex = 0;
            // 
            // pnSnippet
            // 
            this.pnSnippet.Location = new System.Drawing.Point(12, 10);
            this.pnSnippet.Name = "pnSnippet";
            this.pnSnippet.Size = new System.Drawing.Size(236, 247);
            this.pnSnippet.TabIndex = 11;
            // 
            // gbCoords
            // 
            this.gbCoords.Controls.Add(this.rbExisting);
            this.gbCoords.Controls.Add(this.pnAzureColor);
            this.gbCoords.Controls.Add(this.rbAzure);
            this.gbCoords.Controls.Add(this.pnGoogleColor);
            this.gbCoords.Controls.Add(this.rbGoogle);
            this.gbCoords.Controls.Add(this.pnAmazonColor);
            this.gbCoords.Controls.Add(this.rbAmazon);
            this.gbCoords.Location = new System.Drawing.Point(255, 10);
            this.gbCoords.Name = "gbCoords";
            this.gbCoords.Size = new System.Drawing.Size(148, 247);
            this.gbCoords.TabIndex = 12;
            this.gbCoords.TabStop = false;
            this.gbCoords.Text = "Coordinate Selector";
            // 
            // rbExisting
            // 
            this.rbExisting.AutoSize = true;
            this.rbExisting.Location = new System.Drawing.Point(6, 31);
            this.rbExisting.Name = "rbExisting";
            this.rbExisting.Size = new System.Drawing.Size(77, 21);
            this.rbExisting.TabIndex = 4;
            this.rbExisting.TabStop = true;
            this.rbExisting.Text = "Existing";
            this.rbExisting.UseVisualStyleBackColor = true;
            // 
            // pnAzureColor
            // 
            this.pnAzureColor.BackColor = System.Drawing.Color.Blue;
            this.pnAzureColor.Location = new System.Drawing.Point(29, 206);
            this.pnAzureColor.Name = "pnAzureColor";
            this.pnAzureColor.Size = new System.Drawing.Size(113, 30);
            this.pnAzureColor.TabIndex = 3;
            // 
            // rbAzure
            // 
            this.rbAzure.AutoSize = true;
            this.rbAzure.Location = new System.Drawing.Point(6, 213);
            this.rbAzure.Name = "rbAzure";
            this.rbAzure.Size = new System.Drawing.Size(17, 16);
            this.rbAzure.TabIndex = 2;
            this.rbAzure.TabStop = true;
            this.rbAzure.UseVisualStyleBackColor = true;
            // 
            // pnGoogleColor
            // 
            this.pnGoogleColor.BackColor = System.Drawing.Color.Red;
            this.pnGoogleColor.Location = new System.Drawing.Point(29, 141);
            this.pnGoogleColor.Name = "pnGoogleColor";
            this.pnGoogleColor.Size = new System.Drawing.Size(113, 30);
            this.pnGoogleColor.TabIndex = 3;
            // 
            // rbGoogle
            // 
            this.rbGoogle.AutoSize = true;
            this.rbGoogle.Location = new System.Drawing.Point(6, 148);
            this.rbGoogle.Name = "rbGoogle";
            this.rbGoogle.Size = new System.Drawing.Size(17, 16);
            this.rbGoogle.TabIndex = 2;
            this.rbGoogle.TabStop = true;
            this.rbGoogle.UseVisualStyleBackColor = true;
            // 
            // pnAmazonColor
            // 
            this.pnAmazonColor.BackColor = System.Drawing.Color.Gold;
            this.pnAmazonColor.Location = new System.Drawing.Point(29, 76);
            this.pnAmazonColor.Name = "pnAmazonColor";
            this.pnAmazonColor.Size = new System.Drawing.Size(113, 30);
            this.pnAmazonColor.TabIndex = 1;
            // 
            // rbAmazon
            // 
            this.rbAmazon.AutoSize = true;
            this.rbAmazon.Location = new System.Drawing.Point(6, 83);
            this.rbAmazon.Name = "rbAmazon";
            this.rbAmazon.Size = new System.Drawing.Size(17, 16);
            this.rbAmazon.TabIndex = 0;
            this.rbAmazon.TabStop = true;
            this.rbAmazon.UseVisualStyleBackColor = true;
            // 
            // OCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 269);
            this.Controls.Add(this.gbCoords);
            this.Controls.Add(this.pnSnippet);
            this.Controls.Add(this.gbAzure);
            this.Controls.Add(this.gbGoogle);
            this.Controls.Add(this.gbAmazon);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OCR";
            this.Text = " ";
            this.Load += new System.EventHandler(this.OCR_Load);
            this.gbAmazon.ResumeLayout(false);
            this.gbAmazon.PerformLayout();
            this.gbGoogle.ResumeLayout(false);
            this.gbGoogle.PerformLayout();
            this.gbAzure.ResumeLayout(false);
            this.gbAzure.PerformLayout();
            this.gbCoords.ResumeLayout(false);
            this.gbCoords.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbEditor;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.GroupBox gbAmazon;
        private System.Windows.Forms.Button btnAmazonSubmit;
        private System.Windows.Forms.Button btnAmazonEditor;
        private System.Windows.Forms.TextBox tbAmazon;
        private System.Windows.Forms.GroupBox gbGoogle;
        private System.Windows.Forms.Button btnGoogleSubmit;
        private System.Windows.Forms.Button btnGoogleEditor;
        private System.Windows.Forms.TextBox tbGoogle;
        private System.Windows.Forms.GroupBox gbAzure;
        private System.Windows.Forms.Button btnAzureSubmit;
        private System.Windows.Forms.Button btnAzureEditor;
        private System.Windows.Forms.TextBox tbAzure;
        private System.Windows.Forms.Panel pnSnippet;
        private System.Windows.Forms.GroupBox gbCoords;
        private System.Windows.Forms.RadioButton rbExisting;
        private System.Windows.Forms.Panel pnAzureColor;
        private System.Windows.Forms.RadioButton rbAzure;
        private System.Windows.Forms.Panel pnGoogleColor;
        private System.Windows.Forms.RadioButton rbGoogle;
        private System.Windows.Forms.Panel pnAmazonColor;
        private System.Windows.Forms.RadioButton rbAmazon;
    }
}