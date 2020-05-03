using System;
using System.Collections.Generic;

namespace DataSummitFunctions.Models.Google.Response
{
    [Serializable]
    public class Block
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Paragraph> paragraphs { get; set; }
        public string blockType { get; set; }

        public Block()
        {
            //blockType = BlockType.TEXT.ToString();
        }

        public enum BlockType
        {
            TEXT = 1
        }
    }
}
