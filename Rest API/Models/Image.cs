using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Rest_API.Models
{
    public partial class Image
    {
        [Key] // Specifies that this property is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specifies that the database generates a value for this property
        public int ImageId { get; set; }

        [Required] // Specifies that this property is required
        public byte[] ImageData { get; set; }

        [Required(ErrorMessage = "FileName is required")] // Specifies that this property is required and provides a custom error message if not provided
        public string FileName { get; set; }

        [Required(ErrorMessage = "ContentType is required")] // Specifies that this property is required and provides a custom error message if not provided
        public string ContentType { get; set; }

        [ForeignKey("User")] // Specifies the foreign key relationship with the User entity
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
