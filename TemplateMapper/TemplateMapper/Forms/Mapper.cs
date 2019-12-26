using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TemplateMapper.Azure.SQL;
using TemplateMapper.OCR;

namespace TemplateMapper.Forms
{
    public partial class Mapper : Form
    {
        //Local variables
        List<String> lEvents = new List<String>();
        public PictureBox pbImage = null; Image i = null;
        private Boolean _canDraw1 = false; private Boolean _canDraw2 = false;
        private int _startX; private int _startY;
        public Dictionary<int, RectPair> lPairs = new Dictionary<int, RectPair>();
        private RectPair _cPair;

        public Mapper()
        {
            InitializeComponent();
        }

        private Dictionary<int, string> GetComboData(String queryString)
        {
            Dictionary<int, string> dicLocal = new Dictionary<int, string>();
            try
            {
                dicLocal.Add(0, "Select from list...");
                using (SqlCommand command = new SqlCommand(queryString, DataSummit.conDS))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dicLocal.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
            return dicLocal;
        }
        
        private void fmTemplater_Load(object sender, EventArgs e)
        {
            try
            {
                //Open database connection
                if (DataSummit.conDS == null) DataSummit.Connect();
                if (Azure.SQL.DataSummit.conDS.State == ConnectionState.Open)
                {
                    Dictionary<int, string> lCompanies = GetComboData(
                        "SELECT [CompanyId], [Name] FROM [dbo].[Companies] ORDER BY [Name]");

                    //Detaches event trigger on initial "Select from list..." option
                    cbCompany.SelectedIndexChanged -= new System.EventHandler(this.cbCompany_SelectedIndexChanged);
                    cbCompany.DataSource = new BindingSource(lCompanies, null);
                    cbCompany.DisplayMember = "Value";
                    cbCompany.ValueMember = "Key";

                    //Attaches event trigger
                    cbCompany.SelectedIndexChanged += new System.EventHandler(this.cbCompany_SelectedIndexChanged);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (gbProject.Visible == false) gbProject.Visible = true;
                if (cbProject.Visible == false) cbProject.Visible = true;
                if (cbCompany.SelectedText != "Select from list...")
                {
                    Dictionary<int, string> lProjects = GetComboData(
                        "SELECT [ProjectId], [Projects].[Name] FROM [dbo].[Projects] " +
                        "INNER JOIN[dbo].[Companies] ON [dbo].[Companies].[CompanyId] = [dbo].[Projects].[CompanyId] " +
                        "WHERE[dbo].[Companies].[Name] = '" + cbCompany.Text.ToString() + "' " +
                        "ORDER BY[dbo].[Projects].[Name]");

                    //Detaches event trigger on initial "Select from list..." option
                    cbProject.SelectedIndexChanged -= new System.EventHandler(this.cbProject_SelectedIndexChanged);
                    cbProject.DataSource = new BindingSource(lProjects, null);
                    cbProject.DisplayMember = "Value";
                    cbProject.ValueMember = "Key";
                    //Attaches event trigger
                    cbProject.SelectedIndexChanged += new System.EventHandler(this.cbProject_SelectedIndexChanged);
                }

            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void cbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbProject.Text != "Select from list...")
                {
                    if (btnCollaspse.Visible == false) btnCollaspse.Visible = true;
                    if (dgvAttributes.Visible == false) dgvAttributes.Visible = true;
                    if (btnAddImage.Visible == false) btnAddImage.Visible = true;
                }
                else
                {
                    if (btnCollaspse.Visible == true) btnCollaspse.Visible = false;
                    if (dgvAttributes.Visible == true) dgvAttributes.Visible = false;
                    if (btnAddImage.Visible == true) btnAddImage.Visible = false;
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnCollaspse_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnCollaspse.Tag.ToString() == "Collapse")
                {
                    scDataImage.Panel1Collapsed = true;
                    btnCollaspse.Image = Properties.Resources.Expand;
                    btnCollaspse.Tag = "Expand";
                }
                else if (btnCollaspse.Tag.ToString() == "Expand")
                {
                    scDataImage.Panel1Collapsed = false;
                    btnCollaspse.Image = Properties.Resources.Collapse;
                    btnCollaspse.Tag = "Collapse";
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnAddAttributes_Click(object sender, EventArgs e)
        {
            try
            {
                _canDraw1 = true; //MouseDown event will trigger correct values
                _canDraw2 = false;
                _cPair = new RectPair();

                lEvents = TemplateMapper.Events.FindAll(this);
                pbImage.Focus();
                pbImage.MouseDown += new MouseEventHandler(pbImage_MouseDown);
                pbImage.PreviewKeyDown += new PreviewKeyDownEventHandler(pbImage_PreviewKeyDown);
                pbImage.MouseMove -= new MouseEventHandler(pbImage_MouseMove);

                lEvents = TemplateMapper.Events.FindAll(this);

            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                FileDialog fd = new OpenFileDialog();
                fd.Title = "Add Image for Mapping";
                fd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    pbImage = new PictureBox();
                    i = Image.FromFile(fd.FileName.ToString());
                    pbImage.Image = (Image)i.Clone();
                    pbImage.SizeMode = PictureBoxSizeMode.AutoSize;

                    panInner.AutoScroll = true;
                    panInner.Controls.Add(pbImage);
                }
                if (btnAddAttributes.Visible == false) btnAddAttributes.Visible = true;
                if (btnRefresh.Visible == false) btnRefresh.Visible = true;
                if (btnDelImage.Visible == false) btnDelImage.Visible = true;
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnDelImage_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in panInner.Controls)
                {
                    panInner.Controls.Remove(c);
                }

                //Remove mapping event functionality
                pbImage.MouseDown -= new MouseEventHandler(pbImage_MouseDown);
                pbImage.MouseMove -= new MouseEventHandler(pbImage_MouseMove);
                pbImage.MouseUp -= new MouseEventHandler(pbImage_MouseUp);
                pbImage.Paint -= new PaintEventHandler(pbImage_Paint);
                pbImage = null;

                if (btnDelImage.Visible == true) btnDelImage.Visible = false;
                if (btnRefresh.Visible == true) btnRefresh.Visible = false;
                if (btnAddAttributes.Visible == true) btnAddAttributes.Visible = false;
                if (btnUpload.Visible == true) btnUpload.Visible = false;

                dgvAttributes.Rows.Clear();
                panInner.AutoScroll = false;
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void pbImage_MouseDown(object sender, MouseEventArgs e)
        {
            //The system is now allowed to draw rectangles
            //if (_canDraw1 == false && _canDraw2 == false)
            //{
                //This is used if "Add Attribute" is not used as the initiator
            //}
            if (_canDraw1 == true && _canDraw2 == false)
            {
                //_cPair = new RectPair();
                //_canDraw1 = true;
                //_canDraw2 = false;
                //Initialize and keep track of the start position
                _startX = e.X;
                _startY = e.Y;
                pbImage.MouseMove += new MouseEventHandler(pbImage_MouseMove);
                pbImage.MouseUp += new MouseEventHandler(pbImage_MouseUp);
                pbImage.Paint += new PaintEventHandler(pbImage_Paint);
            }
            else if (_canDraw1 == true && _canDraw2 == true)
            {
                _canDraw1 = false;
                _canDraw2 = true;
                _startX = e.X;
                _startY = e.Y;
            }
        }

        private void pbImage_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //The system is now allowed to draw rectangles
                if (_canDraw1 == true && _canDraw2 == false)
                {
                    //ImageSnippet fmSnip = new ImageSnippet();
                    OCR fmRes = new OCR();
                    Bitmap bitImage = new Bitmap(pbImage.Image);
                    fmRes.ImageData = bitImage.Clone(_cPair.Rect1, bitImage.PixelFormat);

                    String strTag = String.Empty;
                    if(fmRes.ShowDialog() == DialogResult.OK)
                    {
                        strTag = fmRes.TagResult;
                    }

                    if(strTag != String.Empty) AddDGVRow(strTag);
                    //pbImage.Refresh();
                    _canDraw1 = true;      //Remains true to indicate transition, MouseDown Event needs to catch this
                    _canDraw2 = true;

                    //fmSnip.Snippet = bitImage.Clone(_cPair.Rect1, bitImage.PixelFormat);
                    //fmSnip.ShowDialog();
                }
                else if (_canDraw1 == false && _canDraw2 == true)
                {
                    DataGridViewTextBoxCell dtc = (DataGridViewTextBoxCell)_cPair.Row.Cells[dgvAttributes.Columns["colValGeo"].Index];
                    dtc.Value = "{" + _cPair.Rect2.Left.ToString() + "," + _cPair.Rect2.Top.ToString() + "," +
                                      _cPair.Rect2.Width.ToString() + "," + _cPair.Rect2.Height.ToString() + "}";
                    lPairs.Add(lPairs.Count + 1, _cPair);

                    //pbImage.Refresh();
                    _canDraw1 = false;
                    _canDraw2 = false;
                    _cPair = new RectPair();
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void pbImage_MouseMove(object sender, MouseEventArgs e)
        {
            //If we are not allowed to draw, simply return and disregard the rest of the code
            if (_canDraw1 == false && _canDraw2 == false)
            { return; }
            else if (_canDraw1 == true && _canDraw2 == false)
            {
                if ((lPairs.Count(r => r.Value.Rect1.IntersectsWith(_cPair.Rect1)) +
                     lPairs.Count(r => r.Value.Rect2.IntersectsWith(_cPair.Rect1))) > 0)
                {
                    bool isEmp = _cPair.Rect1.IsEmpty;
                }
                //The x-value of our rectangle should be the minimum between the start x-value and the current x-position
                int x = Math.Min(_startX, e.X);
                //The y-value of our rectangle should also be the minimum between the start y-value and current y-value
                int y = Math.Min(_startY, e.Y);

                //The width of our rectangle should be the maximum between the start x-position and current x-position minus
                //the minimum of start x-position and current x-position
                int width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);

                //For the hight value, it's basically the same thing as above, but now with the y-values:
                int height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);
                _cPair.Rect1 = new Rectangle(x, y, width, height);
                //Refresh the form and draw the rectangle
                pbImage.Refresh();

            }
            else if (_canDraw1 == false && _canDraw2 == true)
            {
                if ((lPairs.Count(r => r.Value.Rect1.IntersectsWith(_cPair.Rect2)) +
                     lPairs.Count(r => r.Value.Rect2.IntersectsWith(_cPair.Rect2))) > 0)
                {
                    bool isEmp = _cPair.Rect2.IsEmpty;
                }
                //The x-value of our rectangle should be the minimum between the start x-value and the current x-position
                int x = Math.Min(_startX, e.X);
                //The y-value of our rectangle should also be the minimum between the start y-value and current y-value
                int y = Math.Min(_startY, e.Y);

                //The width of our rectangle should be the maximum between the start x-position and current x-position minus
                //the minimum of start x-position and current x-position
                int width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);

                //For the hight value, it's basically the same thing as above, but now with the y-values:
                int height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);
                _cPair.Rect2 = new Rectangle(x, y, width, height);
                _cPair.Connection = new Line(new Point(_cPair.Rect1.X + (_cPair.Rect1.Width / 2), _cPair.Rect1.Y + (_cPair.Rect1.Height / 2)),
                                             new Point(_cPair.Rect2.X + (_cPair.Rect2.Width / 2), _cPair.Rect2.Y + (_cPair.Rect2.Height / 2)));
                //Refresh the form and draw the rectangle
                pbImage.Refresh();
            }
            else if (_canDraw1 == true && _canDraw2 == true)
            {
                _cPair.Connection = new Line(new Point(_cPair.Rect1.X + (_cPair.Rect1.Width / 2), _cPair.Rect1.Y + (_cPair.Rect1.Height / 2)),
                                             new Point(e.X, e.Y));
                //Refresh the form and draw the rectangle
                pbImage.Refresh();
            }
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {

            try
            {

                //Create a new 'pen' to draw our rectangle with, give it the color red and a width of 2
                if (_canDraw1 == true && _canDraw2 == false)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), _cPair.Rect1);
                }
                if (_canDraw1 == true && _canDraw2 == true)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), _cPair.Rect1);
                    e.Graphics.DrawLine(new Pen(Color.Gold, 2), _cPair.Connection.Start, _cPair.Connection.End);
                }
                if (_canDraw1 == false && _canDraw2 == true)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), _cPair.Rect1);
                    e.Graphics.DrawLine(new Pen(Color.Gold, 2), _cPair.Connection.Start, _cPair.Connection.End);
                    e.Graphics.DrawRectangle(new Pen(Color.Green, 2), _cPair.Rect2);
                }
                //Automatically adds existing shapes
                foreach (RectPair rp in lPairs.Select(f => f.Value).ToList())
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), rp.Rect1);
                    e.Graphics.DrawLine(new Pen(Color.Gold, 2), rp.Connection.Start, rp.Connection.End);
                    e.Graphics.DrawRectangle(new Pen(Color.Green, 2), rp.Rect2);
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void fmTemplater_Resize(object sender, EventArgs e)
        {
            Control fm = (Control)sender;
            scDataImage.Width = fm.Width - 18;
            scDataImage.Height = fm.Height - 113;
            btnAddImage.Location = new Point(fm.Width - 70, btnAddImage.Location.Y);
            btnDelImage.Location = new Point(fm.Width - 116, btnDelImage.Location.Y);
            scDataImage.SplitterDistance = 770;
        }

        private void AddDGVRow(String Val)
        {
            try
            {
                DataGridViewRow dr = new DataGridViewRow();
                DataGridViewTextBoxCell tbcName = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell tbcNameGeo = new DataGridViewTextBoxCell();
                DataGridViewTextBoxCell tbcValueGeo = new DataGridViewTextBoxCell();
                DataGridViewImageCell ticDel = new DataGridViewImageCell();

                tbcName.Value = Val;
                tbcNameGeo.Value = "{" + _cPair.Rect1.Left.ToString() + "," + _cPair.Rect1.Top.ToString() + "," +
                                         _cPair.Rect1.Width.ToString() + "," + _cPair.Rect1.Height.ToString() + "}";

                dr.Cells.Add(tbcName);
                dr.Cells.Add(tbcNameGeo);
                dr.Cells.Add(tbcValueGeo);
                dr.Cells.Add(ticDel);

                dgvAttributes.Rows.Add(dr);
                _cPair.Row = dr;

                if (dgvAttributes.Rows.Count > 0 && btnUpload.Visible == false)
                { btnUpload.Visible = true; }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void dgvAttributes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewTextBoxCell dtc; //DataGridViewImageCell dic;
                DataGridViewRow dr = dgvAttributes.Rows[e.RowIndex];
                if (dgvAttributes.Columns[e.ColumnIndex].GetType() == typeof(DataGridViewTextBoxColumn))
                {
                    DataGridViewTextBoxColumn dc = (DataGridViewTextBoxColumn)dgvAttributes.Columns[e.ColumnIndex];
                    if (dc.Name == "colName")
                    {
                        dtc = (DataGridViewTextBoxCell)dr.Cells[e.ColumnIndex];
                    }
                }
                else
                {
                    //Delete attribute function activated
                    int iDel = lPairs.First(f => f.Value.Row == dr).Key;
                    lPairs.Remove(iDel);
                    dgvAttributes.Rows.Remove(dr);
                    if (dgvAttributes.Rows.Count == 0 && btnUpload.Visible == true) btnUpload.Visible = false;
                    ReDraw();
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void ReDraw([Optional] Point loc)
        {
            try
            {
                pbImage.MouseDown -= new MouseEventHandler(pbImage_MouseDown);
                pbImage.MouseMove -= new MouseEventHandler(pbImage_MouseMove);
                pbImage.MouseUp -= new MouseEventHandler(pbImage_MouseUp);
                pbImage.Paint -= new PaintEventHandler(pbImage_Paint);

                panInner.AutoScroll = false;
                foreach(Control c in panInner.Controls)
                { panInner.Controls.Remove(c); }

                pbImage = new PictureBox();
                pbImage.Image = (Image)i.Clone();

                pbImage.SizeMode = PictureBoxSizeMode.AutoSize;
                panInner.AutoScroll = true;
                panInner.Controls.Add(pbImage);

                Graphics g = pbImage.CreateGraphics();
                foreach (RectPair rp in lPairs.Select(f => f.Value).ToList())
                {
                    g.DrawRectangle(new Pen(Color.Red, 2), rp.Rect1);
                    g.DrawLine(new Pen(Color.Gold, 2), rp.Connection.Start, rp.Connection.End);
                    g.DrawRectangle(new Pen(Color.Green, 2), rp.Rect2);
                }

                //if (loc != null) panInner.AutoScrollPosition = loc;

                pbImage.Refresh();
                panInner.Refresh();

                //pbImage.MouseDown += new MouseEventHandler(pbImage_MouseDown);
                //pbImage.MouseMove += new MouseEventHandler(pbImage_MouseMove);
                //pbImage.MouseUp += new MouseEventHandler(pbImage_MouseUp);
                pbImage.Paint += new PaintEventHandler(pbImage_Paint);
                
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void pbImage_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            try
            {
                Control fm = (Control)sender;
                Point p = panInner.AutoScrollPosition;
                if (e.KeyCode ==  System.Windows.Forms.Keys.Escape)
                {
                    lEvents = TemplateMapper.Events.FindAll(this);
                    if (_canDraw1 == true || _canDraw2 == true)
                    {
                        pbImage.MouseDown -= new MouseEventHandler(pbImage_MouseDown);
                        pbImage.MouseMove -= new MouseEventHandler(pbImage_MouseMove);
                        pbImage.MouseUp -= new MouseEventHandler(pbImage_MouseUp);
                        pbImage.Paint -= new PaintEventHandler(pbImage_Paint);
                        pbImage.PreviewKeyDown -= new PreviewKeyDownEventHandler(pbImage_PreviewKeyDown);

                        _cPair = null;
                        _canDraw1 = false;
                        _canDraw2 = false;
                        _startX = 0;
                        _startY = 0;
                        lEvents = TemplateMapper.Events.FindAll(this);
                        ReDraw(p);
                        panInner.Focus();
                        panInner.AutoScrollPosition = p;
                    }  
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReDraw();
        }
    }
    public class CustomPanel : System.Windows.Forms.Panel
    {
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            String strControlType = activeControl.GetType().ToString();
            String strControlName = activeControl.Name.ToString();
            // Returning the current location prevents the panel from
            // scrolling to the active control when the panel loses and regains focus
            return DisplayRectangle.Location;
        }
    }
}
