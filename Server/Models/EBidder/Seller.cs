using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBidderWeb.Server.Models.EBidder
{
    [Table("Seller", Schema = "dbo")]
    public partial class Seller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int SSN { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SellerID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

    }
}