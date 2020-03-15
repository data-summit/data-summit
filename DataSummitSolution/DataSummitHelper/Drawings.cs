using DataSummitModels.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitHelper
{
    public class Drawings
    {
        DataSummitDbContext dataSummitDbContext;

        public Drawings(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public long DrawingCount()
        {
            if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
            return dataSummitDbContext.Drawings.Count();
        }

        public List<DataSummitModels.DB.Drawings> GetAllCompanyDrawings(int projectId)
        {
            List<DataSummitModels.DB.Drawings> drawings = new List<DataSummitModels.DB.Drawings>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                drawings = dataSummitDbContext.Drawings.Where(e => e.ProjectId == projectId).ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return drawings;
        }
        public long CreateDrawing(List<DataSummitModels.DB.Drawings> drawings)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                foreach (DataSummitModels.DB.Drawings drawing in drawings)
                {
                    dataSummitDbContext.Drawings.Add(drawing);
                    dataSummitDbContext.SaveChanges();
                    returnid = drawing.DrawingId;
                }
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return returnid;
        }

        public void UpdateDrawing(long id, DataSummitModels.DB.Drawings drawing)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.Drawings.Update(drawing);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
        public void DeleteDrawing(long drawingId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.Drawings drawing = dataSummitDbContext.Drawings.First(p => p.DrawingId == drawingId);
                dataSummitDbContext.Drawings.Remove(drawing);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }
    }
}