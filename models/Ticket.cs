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
    public required string Priority {get; set;}
    //Foreign Keys
    //[ForeignKey ("PC")]
    public required Pc Pc {get; set;}
    //[ForeignKey ("Area")]
    public required Area Area {get; set;}
}
