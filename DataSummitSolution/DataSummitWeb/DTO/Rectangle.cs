

namespace DataSummitWeb.DTO
{
    public class Rectangle
    {
        public Point Tl { get; set; }
        public Point Tr { get; set; }
        public Point Bl { get; set; }
        public Point Br { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public string Fill { get; set; }
        public string Type { get; set; }

        public Rectangle()
        { }
    }
}
