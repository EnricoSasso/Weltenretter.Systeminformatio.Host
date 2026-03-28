using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weltenretter.Systeminformationen.Host.Models
{
    /// <summary>
    /// Repräsentiert eine Person für Testzwecke im Hostprojekt.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Technischer Primärschlüssel (Identity).
        /// </summary>
        [Key] // Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Vorname der Person.
        /// </summary>
        [Required]
        public string Vorname { get; set; } = string.Empty;

        /// <summary>
        /// Nachname der Person.
        /// </summary>
        [Required]
        public string Nachname { get; set; } = string.Empty;
    }
}
