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

        public List<BlockPositions> GetAllCompanyBlockPositions(int blockPositionId)
        {
            List<BlockPositions> BlockPositions = new List<BlockPositions>();
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

        public BlockPositions GetBlockPositionById(int blockPositionId)
        {
            BlockPositions BlockPositions = new BlockPositions();
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

        public long CreateBlockPosition(BlockPositions blockPositions)
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

        public void UpdateBlockPosition(int id, BlockPositions blockPositions)
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
                BlockPositions BlockPositions = dataSummitDbContext.BlockPositions.First(p => p.BlockPositionId == blockPositionsId);
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