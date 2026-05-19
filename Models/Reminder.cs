using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetJourneyTutorApi.Models;

[Table("TBLEMBRETE")]
public class Reminder
{
    [Key]
    [Column("IDLEMBRETE")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdLembrete { get; set; }

    [Required(ErrorMessage = "O pet do lembrete é obrigatório.")]
    [Column("IDPET")]
    public int IdPet { get; set; }

    [Required(ErrorMessage = "O tipo do lembrete é obrigatório.")]
    [MaxLength(100)]
    [Column("DSTIPO")]
    public string DsTipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição do lembrete é obrigatória.")]
    [MaxLength(300)]
    [Column("DSDESCRICAO")]
    public string DsDescricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data do lembrete é obrigatória.")]
    [Column("DTLEMBRETE")]
    public DateTime DtLembrete { get; set; }

    [Column("DTNOTIFICADO")]
    public DateTime? DtNotificado { get; set; }

    [Required(ErrorMessage = "O status do lembrete é obrigatório.")]
    [MaxLength(20)]
    [Column("DSSTATUS")]
    public string DsStatus { get; set; } = "PENDENTE";
}
