using DataSummitModels.Enums;

namespace AzureFunctions.DTO
{
    public class ImagePortionDto
    {
        public ImageType ImageType { get; set; }
        public decimal WidthSplit { get; set; }
        public decimal HeightSplit { get; set; }
        public decimal WidthCalculatedSpan { get; set; }
        public decimal HeightCalculatedSpan { get; set; }
        public decimal WidthCalculatedHalfSpan { get; set; }
        public decimal HeightCalculatedHalfSpan { get; set; }
        public int WidthIterations { get; set; }
        public int HeightIterations { get; set; }
        public int WidthPixel { get; set; }
        public int HeightPixel { get; set; }
        public int WidthIndex { get; set; }
        public int HeightIndex { get; set; }
        public string Name { get; set; }
        public decimal AdjustedWith { get => WidthPixel + WidthCalculatedSpan; }
        public decimal AdjustedHeight { get => HeightPixel + HeightCalculatedSpan; }
    }
}