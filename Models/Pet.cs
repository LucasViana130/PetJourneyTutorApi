using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBPET")]
public class Pet
{
    [Key]
    [Column("IDPET")]
    public int IdPet { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NMPET")]
    public string NmPet { get; set; } = string.Empty;

    [Column("DTNASCIMENTO")]
    public DateTime? DtNascimento { get; set; }

    [Required]
    [MaxLength(1)]
    [Column("DSSEXO")]
    public string DsSexo { get; set; } = string.Empty;

    [Required]
    [Column("IDTUTOR")]
    public int IdTutor { get; set; }

    [Required]
    [Column("IDESPECIE")]
    public int IdEspecie { get; set; }

    [Column("IDRACA")]
    public int? IdRaca { get; set; }

    [Column("IDCLINICA")]
    public int? IdClinica { get; set; }
}