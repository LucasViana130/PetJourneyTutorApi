using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBRACA")]
public class Breed
{
    [Key]
    [Column("IDRACA")]
    public int IdRaca { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NMRACA")]
    public string NmRaca { get; set; } = string.Empty;

    [Required]
    [Column("IDESPECIE")]
    public int IdEspecie { get; set; }
}