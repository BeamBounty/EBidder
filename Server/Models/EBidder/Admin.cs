using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBidderWeb.Server.Models.EBidder
{
    [Table("Admins", Schema = "dbo")]
    public partial class Admin
    {
        [Required]
        public int SSN { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminID { get; set; }

    }
}