using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.OCR.Google.Response
{
    [Serializable]
    public class Blocks
    {
        public Property property { get; set; }
        public BoundingBox boundingBox { get; set; }
        public List<Paragraphs> paragraphs = new List<Paragraphs>();
        public string blockType { get; set; }

        public Blocks()
        {
            blockType = BlockType.TEXT.ToString();
        }

        public enum BlockType
        {
            TEXT = 1
        }
    }
}
