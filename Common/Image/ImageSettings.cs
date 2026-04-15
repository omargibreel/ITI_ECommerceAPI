using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Image
{
    public class ImageSettings
    {
        public string UploadBasePath { get; set; } = null!;
        public long MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5 MB
        public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".webp" };
    }
}
