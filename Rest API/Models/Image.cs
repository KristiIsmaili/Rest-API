using System;
using System.Collections.Generic;

#nullable disable

namespace Rest_API.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public byte[] ImageData { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
