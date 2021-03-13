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

        public List<DataSummitModels.DB.PaperSize> GetAllPaperSizes()
        {
            List<DataSummitModels.DB.PaperSize> PaperSizes = new List<DataSummitModels.DB.PaperSize>();
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

        public DataSummitModels.DB.PaperSize GetPaperSizesById(int paperSizeId)
        {
            DataSummitModels.DB.PaperSize PaperSizes = new DataSummitModels.DB.PaperSize();
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

        public long CreatePaperSize(DataSummitModels.DB.PaperSize paperSize)
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

        public void UpdatePaperSize(int id, DataSummitModels.DB.PaperSize paperSize)
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
                DataSummitModels.DB.PaperSize PaperSizes = dataSummitDbContext.PaperSizes.First(p => p.PaperSizeId == paperSizeId);
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
