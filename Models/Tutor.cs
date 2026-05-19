using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBTUTOR")]
public class Tutor
{
    [Key]
    [Column("IDTUTOR")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdTutor { get; set; }

    [Required(ErrorMessage = "O nome do tutor é obrigatório.")]
    [MaxLength(200)]
    [Column("NMTUTOR")]
    public string NmTutor { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [MaxLength(150)]
    [Column("DSEMAIL")]
    public string DsEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [MaxLength(20)]
    [Column("NRTELEFONE")]
    public string NrTelefone { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("DSPLANO")]
    public string? DsPlano { get; set; } = "FREE";

    [Column("DTCADASTRO")]
    public DateTime DtCadastro { get; set; } = DateTime.Now;
}
