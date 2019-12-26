using DataSummitModels;
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

        public List<DataSummitModels.PaperSizes> GetAllPaperSizes()
        {
            List<DataSummitModels.PaperSizes> PaperSizes = new List<DataSummitModels.PaperSizes>();
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

        public DataSummitModels.PaperSizes GetPaperSizesById(int paperSizeId)
        {
            DataSummitModels.PaperSizes PaperSizes = new DataSummitModels.PaperSizes();
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

        public long CreatePaperSize(DataSummitModels.PaperSizes paperSize)
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

        public void UpdatePaperSize(int id, DataSummitModels.PaperSizes paperSize)
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
                DataSummitModels.PaperSizes PaperSizes = dataSummitDbContext.PaperSizes.First(p => p.PaperSizeId == paperSizeId);
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
