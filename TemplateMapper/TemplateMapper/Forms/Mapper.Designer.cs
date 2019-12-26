namespace TemplateMapper.Forms
{
    partial class Mapper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mapper));
            this.gbCompany = new System.Windows.Forms.GroupBox();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.gbProject = new System.Windows.Forms.GroupBox();
            this.cbProject = new System.Windows.Forms.ComboBox();
            this.scDataImage = new System.Windows.Forms.SplitContainer();
            this.dgvAttributes = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNameGeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValGeo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDel = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnCollaspse = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.btnDelImage = new System.Windows.Forms.Button();
            this.btnAddAttributes = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panInner = new TemplateMapper.Forms.CustomPanel();
            this.gbCompany.SuspendLayout();
            this.gbProject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDataImage)).BeginInit();
            this.scDataImage.Panel1.SuspendLayout();
            this.scDataImage.Panel2.SuspendLayout();
            this.scDataImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // gbCompany
            // 
            this.gbCompany.Controls.Add(this.cbCompany);
            this.gbCompany.Location = new System.Drawing.Point(104, 12);
            this.gbCompany.Name = "gbCompany";
            this.gbCompany.Size = new System.Drawing.Size(363, 50);
            this.gbCompany.TabIndex = 0;
            this.gbCompany.TabStop = false;
            this.gbCompany.Text = "Company";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(6, 18);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(350, 24);
            this.cbCompany.TabIndex = 0;
            this.cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
            // 
            // gbProject
            // 
            this.gbProject.Controls.Add(this.cbProject);
            this.gbProject.Location = new System.Drawing.Point(473, 12);
            this.gbProject.Name = "gbProject";
            this.gbProject.Size = new System.Drawing.Size(363, 50);
            this.gbProject.TabIndex = 1;
            this.gbProject.TabStop = false;
            this.gbProject.Text = "Project";
            this.gbProject.Visible = false;
            // 
            // cbProject
            // 
            this.cbProject.FormattingEnabled = true;
            this.cbProject.Location = new System.Drawing.Point(7, 18);
            this.cbProject.Name = "cbProject";
            this.cbProject.Size = new System.Drawing.Size(350, 24);
            this.cbProject.TabIndex = 2;
            this.cbProject.Visible = false;
            this.cbProject.SelectedIndexChanged += new System.EventHandler(this.cbProject_SelectedIndexChanged);
            // 
            // scDataImage
            // 
            this.scDataImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scDataImage.Location = new System.Drawing.Point(0, 68);
            this.scDataImage.Name = "scDataImage";
            // 
            // scDataImage.Panel1
            // 
            this.scDataImage.Panel1.Controls.Add(this.dgvAttributes);
            this.scDataImage.Panel1Collapsed = true;
            this.scDataImage.Panel1MinSize = 770;
            // 
            // scDataImage.Panel2
            // 
            this.scDataImage.Panel2.Controls.Add(this.panInner);
            this.scDataImage.Size = new System.Drawing.Size(1054, 645);
            this.scDataImage.SplitterDistance = 770;
            this.scDataImage.TabIndex = 2;
            // 
            // dgvAttributes
            // 
            this.dgvAttributes.AllowUserToAddRows = false;
            this.dgvAttributes.AllowUserToDeleteRows = false;
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colNameGeo,
            this.colValGeo,
            this.colDel});
            this.dgvAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttributes.Location = new System.Drawing.Point(0, 0);
            this.dgvAttributes.Name = "dgvAttributes";
            this.dgvAttributes.ReadOnly = true;
            this.dgvAttributes.RowTemplate.Height = 24;
            this.dgvAttributes.Size = new System.Drawing.Size(770, 100);
            this.dgvAttributes.TabIndex = 1;
            this.dgvAttributes.Visible = false;
            this.dgvAttributes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttributes_CellContentClick);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 250;
            // 
            // colNameGeo
            // 
            this.colNameGeo.HeaderText = "Name Geometry";
            this.colNameGeo.Name = "colNameGeo";
            this.colNameGeo.ReadOnly = true;
            this.colNameGeo.Width = 200;
            // 
            // colValGeo
            // 
            this.colValGeo.HeaderText = "Value Geometry";
            this.colValGeo.Name = "colValGeo";
            this.colValGeo.ReadOnly = true;
            this.colValGeo.Width = 200;
            // 
            // colDel
            // 
            this.colDel.HeaderText = "Del";
            this.colDel.Image = global::TemplateMapper.Properties.Resources.Delete;
            this.colDel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.colDel.Name = "colDel";
            this.colDel.ReadOnly = true;
            this.colDel.Width = 40;
            // 
            // btnCollaspse
            // 
            this.btnCollaspse.Image = global::TemplateMapper.Properties.Resources.Expand;
            this.btnCollaspse.Location = new System.Drawing.Point(12, 22);
            this.btnCollaspse.Name = "btnCollaspse";
            this.btnCollaspse.Size = new System.Drawing.Size(40, 40);
            this.btnCollaspse.TabIndex = 1;
            this.btnCollaspse.Tag = "Expand";
            this.btnCollaspse.UseVisualStyleBackColor = true;
            this.btnCollaspse.Visible = false;
            this.btnCollaspse.Click += new System.EventHandler(this.btnCollaspse_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.Image = global::TemplateMapper.Properties.Resources.Add2;
            this.btnAddImage.Location = new System.Drawing.Point(1002, 22);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(40, 40);
            this.btnAddImage.TabIndex = 3;
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Visible = false;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // btnDelImage
            // 
            this.btnDelImage.Image = global::TemplateMapper.Properties.Resources.Delete;
            this.btnDelImage.Location = new System.Drawing.Point(956, 22);
            this.btnDelImage.Name = "btnDelImage";
            this.btnDelImage.Size = new System.Drawing.Size(40, 40);
            this.btnDelImage.TabIndex = 4;
            this.btnDelImage.UseVisualStyleBackColor = true;
            this.btnDelImage.Visible = false;
            this.btnDelImage.Click += new System.EventHandler(this.btnDelImage_Click);
            // 
            // btnAddAttributes
            // 
            this.btnAddAttributes.Image = global::TemplateMapper.Properties.Resources.Add;
            this.btnAddAttributes.Location = new System.Drawing.Point(58, 22);
            this.btnAddAttributes.Name = "btnAddAttributes";
            this.btnAddAttributes.Size = new System.Drawing.Size(40, 40);
            this.btnAddAttributes.TabIndex = 5;
            this.btnAddAttributes.UseVisualStyleBackColor = true;
            this.btnAddAttributes.Visible = false;
            this.btnAddAttributes.Click += new System.EventHandler(this.btnAddAttributes_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Image = global::TemplateMapper.Properties.Resources.Upload;
            this.btnUpload.Location = new System.Drawing.Point(888, 22);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(62, 40);
            this.btnUpload.TabIndex = 6;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Visible = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::TemplateMapper.Properties.Resources.Refresh;
            this.btnRefresh.Location = new System.Drawing.Point(842, 22);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(40, 40);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Visible = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panInner
            // 
            this.panInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panInner.Location = new System.Drawing.Point(0, 0);
            this.panInner.Name = "panInner";
            this.panInner.Size = new System.Drawing.Size(1054, 645);
            this.panInner.TabIndex = 0;
            // 
            // fmTemplater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1054, 713);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnDelImage);
            this.Controls.Add(this.btnAddAttributes);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.btnCollaspse);
            this.Controls.Add(this.scDataImage);
            this.Controls.Add(this.gbProject);
            this.Controls.Add(this.gbCompany);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1026, 300);
            this.Name = "fmTemplater";
            this.Text = "Image Template Mapper";
            this.Load += new System.EventHandler(this.fmTemplater_Load);
            this.Resize += new System.EventHandler(this.fmTemplater_Resize);
            this.gbCompany.ResumeLayout(false);
            this.gbProject.ResumeLayout(false);
            this.scDataImage.Panel1.ResumeLayout(false);
            this.scDataImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scDataImage)).EndInit();
            this.scDataImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCompany;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.GroupBox gbProject;
        private System.Windows.Forms.ComboBox cbProject;
        private System.Windows.Forms.SplitContainer scDataImage;
        private System.Windows.Forms.DataGridView dgvAttributes;
        private System.Windows.Forms.Button btnCollaspse;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.Button btnDelImage;
        private System.Windows.Forms.Button btnAddAttributes;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNameGeo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValGeo;
        private System.Windows.Forms.DataGridViewImageColumn colDel;
        private CustomPanel panInner;
        private System.Windows.Forms.Button btnRefresh;
    }
}

