namespace DataSummitWeb.DTO
{
    public class DrawingUpload
    {
        public int ProjectId { get; set; }
        public int TemplateId { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }

        public DrawingUpload()
        { }
    }
}
