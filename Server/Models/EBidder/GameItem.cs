using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBidderWeb.Server.Models.EBidder
{
    [Table("GameItems", Schema = "dbo")]
    public partial class GameItem
    {
        [Key]
        [Required]
        public int GameID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GameName { get; set; }

    }
}