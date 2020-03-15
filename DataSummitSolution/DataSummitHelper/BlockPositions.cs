using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSummitHelper
{
    public class BlockPositions
    {
        private DataSummitDbContext dataSummitDbContext;

        public BlockPositions(DataSummitDbContext dbContext)
        {
            dataSummitDbContext = dbContext;
        }

        public List<DataSummitModels.DB.BlockPositions> GetAllCompanyBlockPositions(int blockPositionId)
        {
            List<DataSummitModels.DB.BlockPositions> BlockPositions = new List<DataSummitModels.DB.BlockPositions>();
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

        public DataSummitModels.DB.BlockPositions GetBlockPositionById(int blockPositionId)
        {
            DataSummitModels.DB.BlockPositions BlockPositions = new DataSummitModels.DB.BlockPositions();
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

        public long CreateBlockPosition(DataSummitModels.DB.BlockPositions blockPositions)
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

        public void UpdateBlockPosition(int id, DataSummitModels.DB.BlockPositions blockPositions)
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
                DataSummitModels.DB.BlockPositions BlockPositions = dataSummitDbContext.BlockPositions.First(p => p.BlockPositionId == blockPositionsId);
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