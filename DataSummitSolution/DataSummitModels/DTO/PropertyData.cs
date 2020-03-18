using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitModels.DTO
{
    public class PropertyData
    {
        //From ProfileAttribute class
        public long ProfileAttributeId { get; set; }
        public string Name { get; set; }
        public int NameX { get; set; }
        public int NameY { get; set; }
        public short NameWidth { get; set; }
        public short NameHeight { get; set; }
        public byte PaperSizeId { get; set; }
        public byte BlockPositionId { get; set; }
        public int? ProfileVersionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UserId { get; set; }
        public string Value { get; set; }
        public int? ValueX { get; set; }
        public int? ValueY { get; set; }
        public short? ValueWidth { get; set; }
        public short? ValueHeight { get; set; }
        
        //From StandardAttribute class
        public short StandardAttributeId { get; set; }
        public string StandardName { get; set; }

        //From Sentence class
        public Guid SentenceId { get; set; }
        public string Words { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public string Vendor { get; set; }
        public bool IsUsed { get; set; }
        public decimal? Confidence { get; set; }
        public decimal? SlendernessRatio { get; set; }
        public long DrawingId { get; set; }

        //From Properties class
        public long PropertyId { get; set; }

        public PropertyData()
        { }
    }
}
