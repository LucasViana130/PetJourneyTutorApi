using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBCLINICA")]
public class Clinic
{
    [Key]
    [Column("IDCLINICA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdClinica { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("NMCLINICA")]
    public string NmClinica { get; set; } = string.Empty;

    [MaxLength(300)]
    [Column("DSENDERECO")]
    public string? DsEndereco { get; set; }

    [MaxLength(20)]
    [Column("NRTELEFONE")]
    public string? NrTelefone { get; set; }

    [MaxLength(150)]
    [Column("DSEMAIL")]
    public string? DsEmail { get; set; }

    [MaxLength(20)]
    [Column("DSSTATUS")]
    public string? DsStatus { get; set; }
}
