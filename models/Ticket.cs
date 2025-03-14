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
    public int Priority {get; set;}
    public required int Pc {get; set;}
    public required string Area {get; set;}
}
    