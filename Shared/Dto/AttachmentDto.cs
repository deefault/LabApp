namespace LabApp.Shared.Dto
{
    public class AttachmentDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }
        

        /// <summary>
        /// size in bytes
        /// </summary>
        public long Size { get; set; }
    }
}