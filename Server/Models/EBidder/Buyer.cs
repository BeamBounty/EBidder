using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBidderWeb.Server.Models.EBidder
{
    [Table("Buyers", Schema = "dbo")]
    public partial class Buyer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int SSN { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] Password { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuyerID { get; set; }

    }
}