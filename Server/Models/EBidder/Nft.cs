using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBidderWeb.Server.Models.EBidder
{
    [Table("NFT", Schema = "dbo")]
    public partial class Nft
    {
        [Key]
        [Required]
        public int NftID { get; set; }

        [Required]
        public string Name { get; set; }

    }
}