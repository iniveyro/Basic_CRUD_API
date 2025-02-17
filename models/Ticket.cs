using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;
public class Ticket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NumId { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public required PriorityLevel Priority {get; set;}
    public required Pc Pc {get; set;}
    public required Area Area {get; set;}
}
    public enum PriorityLevel
    {
        Urgente,
        Moderada,
        Baja
    }