using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host
{
    [Table("Personen", Schema = "dbo")]
    public partial class Personen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Vorname { get; set; }

        [Required]
        public string Nachname { get; set; }
    }
}