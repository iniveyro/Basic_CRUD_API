using System.ComponentModel.DataAnnotations;

namespace Api.Models;
public class Ticket
{
    [Key]
    public int NumId { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public required string Priority {get; set;}
    //Foreign Keys
    //[ForeignKey ("PC")]
    public required PC PC {get; set;}
    //[ForeignKey ("Area")]
    public required Area Area {get; set;}
}
