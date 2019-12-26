using DataSummitModels;
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

        public List<DataSummitModels.BlockPositions> GetAllCompanyBlockPositions(int blockPositionId)
        {
            List<DataSummitModels.BlockPositions> BlockPositions = new List<DataSummitModels.BlockPositions>();
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

        public DataSummitModels.BlockPositions GetBlockPositionById(int blockPositionId)
        {
            DataSummitModels.BlockPositions BlockPositions = new DataSummitModels.BlockPositions();
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

        public long CreateBlockPosition(DataSummitModels.BlockPositions blockPositions)
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

        public void UpdateBlockPosition(int id, DataSummitModels.BlockPositions blockPositions)
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
                DataSummitModels.BlockPositions BlockPositions = dataSummitDbContext.BlockPositions.First(p => p.BlockPositionId == blockPositionsId);
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