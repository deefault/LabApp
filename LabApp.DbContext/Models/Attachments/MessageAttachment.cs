using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models.Attachments
{
    public class MessageAttachment : Attachment
    {
        public int? MessageId  { get; set; }

        public virtual Message Message  { get; set; }
    }
}