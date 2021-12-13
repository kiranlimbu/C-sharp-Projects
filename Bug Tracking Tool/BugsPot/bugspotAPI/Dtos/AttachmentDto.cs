using Microsoft.AspNetCore.Http;

namespace bugspotAPI.Dtos
{
    public class AttachmentDto
    {
        public string userId { get; set; }

        // attachment file info
        public string imageName { get; set; }
        public byte[] imageData { get; set; }
        public IFormFile formFile { get; set; }
        public string fileExtension { get; set; }
    }
}