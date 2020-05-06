using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class PaperSize
    {
        private DataSummitDbContext dataSummitDbContext;

        public PaperSize(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<PaperSizes> GetAllPaperSizes()
        {
            List<PaperSizes> PaperSizes = new List<PaperSizes>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                PaperSizes = dataSummitDbContext.PaperSizes.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return PaperSizes;
        }

        public PaperSizes GetPaperSizesById(int paperSizeId)
        {
            PaperSizes PaperSizes = new PaperSizes();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                PaperSizes = dataSummitDbContext.PaperSizes.First(p => p.PaperSizeId == paperSizeId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return PaperSizes;
        }

        public long CreatePaperSize(PaperSizes paperSize)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.PaperSizes.Add(paperSize);
                dataSummitDbContext.SaveChanges();
                returnid = paperSize.PaperSizeId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdatePaperSize(int id, PaperSizes paperSize)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.PaperSizes.Update(paperSize);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeletePaperSize(int paperSizeId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                PaperSizes PaperSizes = dataSummitDbContext.PaperSizes.First(p => p.PaperSizeId == paperSizeId);
                dataSummitDbContext.PaperSizes.Remove(PaperSizes);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}
