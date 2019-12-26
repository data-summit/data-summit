using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
