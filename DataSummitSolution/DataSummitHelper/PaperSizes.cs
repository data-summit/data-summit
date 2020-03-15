using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class PaperSizes
    {
        private DataSummitDbContext dataSummitDbContext;

        public PaperSizes(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.PaperSizes> GetAllPaperSizes()
        {
            List<DataSummitModels.DB.PaperSizes> PaperSizes = new List<DataSummitModels.DB.PaperSizes>();
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

        public DataSummitModels.DB.PaperSizes GetPaperSizesById(int paperSizeId)
        {
            DataSummitModels.DB.PaperSizes PaperSizes = new DataSummitModels.DB.PaperSizes();
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

        public long CreatePaperSize(DataSummitModels.DB.PaperSizes paperSize)
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

        public void UpdatePaperSize(int id, DataSummitModels.DB.PaperSizes paperSize)
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
                DataSummitModels.DB.PaperSizes PaperSizes = dataSummitDbContext.PaperSizes.First(p => p.PaperSizeId == paperSizeId);
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
