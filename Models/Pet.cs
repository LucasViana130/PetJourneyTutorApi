using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBPET")]
public class Pet
{
    [Key]
    [Column("IDPET")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPet { get; set; }

    [Required(ErrorMessage = "O nome do pet é obrigatório.")]
    [MaxLength(100)]
    [Column("NMPET")]
    public string NmPet { get; set; } = string.Empty;

    [Column("DTNASCIMENTO")]
    public DateTime? DtNascimento { get; set; }

    [Required(ErrorMessage = "O sexo do pet é obrigatório.")]
    [MaxLength(1)]
    [Column("DSSEXO")]
    public string DsSexo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tutor do pet é obrigatório.")]
    [Column("IDTUTOR")]
    public int IdTutor { get; set; }

    [Column("IDESPECIE")]
    public int? IdEspecie { get; set; }

    [Column("IDRACA")]
    public int? IdRaca { get; set; }

    // A clínica é opcional. Se estiver nulo, o tutor usa o sistema sem vínculo com clínica.
    [Column("IDCLINICA")]
    public int? IdClinica { get; set; }
}
