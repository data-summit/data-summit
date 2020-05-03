using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class Drawing
    {
        DataSummitDbContext dataSummitDbContext;

        public Drawing(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public long DrawingCount()
        {
            return dataSummitDbContext.Drawings.Count();
        }

        public List<Drawings> GetAllCompanyDrawings(int projectId)
        {
            var drawings = new List<Drawings>();
            try
            {
                drawings = dataSummitDbContext.Drawings
                    .Where(e => e.ProjectId == projectId)
                    .ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return drawings;
        }

        public long CreateDrawing(List<Drawings> drawings)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                foreach (Drawings drawing in drawings)
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

        public void UpdateDrawing(long id, Drawings drawing)
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
                Drawings drawing = dataSummitDbContext.Drawings.First(p => p.DrawingId == drawingId);
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