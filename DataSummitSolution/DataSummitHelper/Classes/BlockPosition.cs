using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSummitHelper
{
    public class BlockPosition
    {
        private DataSummitDbContext dataSummitDbContext;

        public BlockPosition(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.BlockPosition> GetAllCompanyBlockPositions(int blockPositionId)
        {
            List<DataSummitModels.DB.BlockPosition> BlockPositions = new List<DataSummitModels.DB.BlockPosition>();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                BlockPositions = dataSummitDbContext.BlockPositions.ToList();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return BlockPositions;
        }

        public DataSummitModels.DB.BlockPosition GetBlockPositionById(int blockPositionId)
        {
            DataSummitModels.DB.BlockPosition BlockPositions = new DataSummitModels.DB.BlockPosition();
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                BlockPositions = dataSummitDbContext.BlockPositions.First(e => e.BlockPositionId == blockPositionId);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return BlockPositions;
        }

        public long CreateBlockPosition(DataSummitModels.DB.BlockPosition blockPositions)
        {
            long returnid = 0;
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.BlockPositions.Add(blockPositions);
                dataSummitDbContext.SaveChanges();
                returnid = blockPositions.BlockPositionId;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return returnid;
        }

        public void UpdateBlockPosition(int id, DataSummitModels.DB.BlockPosition blockPositions)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                dataSummitDbContext.BlockPositions.Update(blockPositions);
                dataSummitDbContext.SaveChanges();
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return;
        }

        public void DeleteBlockPosition(int blockPositionsId)
        {
            try
            {
                if (dataSummitDbContext == null) dataSummitDbContext = new DataSummitDbContext();
                DataSummitModels.DB.BlockPosition BlockPositions = dataSummitDbContext.BlockPositions.First(p => p.BlockPositionId == blockPositionsId);
                dataSummitDbContext.BlockPositions.Remove(BlockPositions);
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