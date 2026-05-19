using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBESPECIE")]
public class Species
{
    [Key]
    [Column("IDESPECIE")]
    public int IdEspecie { get; set; }

    [Required]
    [MaxLength(80)]
    [Column("NMESPECIE")]
    public string NmEspecie { get; set; } = string.Empty;
}