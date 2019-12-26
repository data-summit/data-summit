using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateMapper.Forms
{
    public partial class OCR : Form
    {
        public String TagResult = String.Empty;
        public Image ImageData = null;
        private TemplateMapper.OCR.Consolidated.ConsolidatedOCR allRes = new TemplateMapper.OCR.Consolidated.ConsolidatedOCR();
        private int borderSize = 50;
        //private TemplateMapper.OCR.Azure.AzureOCR azRes = new TemplateMapper.OCR.Azure.AzureOCR();
        public OCR()
        {
            InitializeComponent();
        }

        private void OCR_Load(object sender, EventArgs e)
        {
            try
            {
                PictureBox pbCur = new PictureBox();
                //pbCur.Dock = DockStyle.Fill;
                pbCur.SizeMode = PictureBoxSizeMode.AutoSize;
                pnSnippet.AutoScroll = true;
                pbCur.Image = ImageData;

                pnSnippet.Controls.Add(pbCur);
                pnSnippet.Refresh();

                //TemplateMapper.OCR.Amazon.AmazonOCR amRes = new TemplateMapper.OCR.Amazon.AmazonOCR();
                //String sAmazonRes = String.Empty;
                //Task tAmazon = Task.Factory.StartNew(() => sAmazonRes = amRes.Run(ImageData));

                TemplateMapper.OCR.Azure.AzureOCR azRes = new TemplateMapper.OCR.Azure.AzureOCR();
                String sAzureRes = String.Empty;
                Task tAzure = Task.Factory.StartNew(async () => sAzureRes = await azRes.Run(ImageData, borderSize));
                tAzure.Wait();
                TagResult = sAzureRes;

                //TemplateMapper.OCR.Google.GoogleOCR gRes = new TemplateMapper.OCR.Google.GoogleOCR();
                //String sGoogleRes = String.Empty;
                //Task tGoogle = Task.Factory.StartNew(() => sGoogleRes = gRes.Run(new List<System.Drawing.Image> { ImageData }));

                //Task.WaitAll(tGoogle, tAzure);
                //Task.WaitAll(tAmazon, tGoogle, tAzure);


                //allRes.FromAmazon(amRes);
                allRes.FromAzure(azRes, borderSize);
                //allRes.FromGoogle(gRes.responses);

                //tbAmazon.Text = sAmazonRes;
                tbAzure.Text = sAzureRes;
                //tbGoogle.Text = sGoogleRes;
                
                //if(sAmazonRes == sGoogleRes)
                //{
                //    TagResult = sAmazonRes;
                //    DialogResult = DialogResult.OK;
                //    Close();
                //}

                //if (azRes.Regions.Count > 0)
                //{
                //    Graphics g = pbCur.CreateGraphics();

                //    pbCur.Refresh();
                //    pbCur.Update();
                //    pnSnippet.Refresh();
                //    pnSnippet.Update();
                //}
                pbCur.Paint += new PaintEventHandler(pbCur_Paint);
                pbCur.Refresh();
                //pbCur.Paint -= new PaintEventHandler(pbCur_Paint);
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            TagResult = tbEditor.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAmazonSubmit_Click(object sender, EventArgs e)
        {
            TagResult = tbAmazon.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAmazonEditor_Click(object sender, EventArgs e)
        {
            tbEditor.Text = tbAmazon.Text;
        }

        private void btnGoogleSubmit_Click(object sender, EventArgs e)
        {
            TagResult = tbGoogle.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnGoogleEditor_Click(object sender, EventArgs e)
        {
            tbEditor.Text = tbGoogle.Text;
        }

        private void btnAzureSubmit_Click(object sender, EventArgs e)
        {
            TagResult = tbAzure.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAzureEditor_Click(object sender, EventArgs e)
        {
            tbEditor.Text = tbAzure.Text;
        }

        private void pbCur_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if(allRes.Sentences.Count > 0)
                {
                    foreach(TemplateMapper.OCR.Consolidated.Sentence s in allRes.Sentences)
                    {
                        Rectangle r = new Rectangle((int)s.Rectangle.Left, (int)s.Rectangle.Top, (int)s.Rectangle.Width, (int)s.Rectangle.Height);
                        if (s.Vendor == "Amazon") e.Graphics.DrawRectangle(new Pen(pnAmazonColor.BackColor, 2), r);
                        if (s.Vendor == "Azure") e.Graphics.DrawRectangle(new Pen(pnAzureColor.BackColor, 2), r);
                        if (s.Vendor == "Google") e.Graphics.DrawRectangle(new Pen(pnGoogleColor.BackColor, 2), r);

                    }
                }
                //if (azRes.Regions.Count > 0)
                //{
                //    Rectangle azRect = new Rectangle(azRes.Regions[0].Rectangle.Left - azRes.borderSize, azRes.Regions[0].Rectangle.Top - azRes.borderSize,
                //                                          azRes.Regions[0].Rectangle.Width, azRes.Regions[0].Rectangle.Height);
                //    e.Graphics.DrawRectangle(new Pen(pnAzureColor.BackColor, 2), azRect);
                //}
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
            }
        }
    }
}
