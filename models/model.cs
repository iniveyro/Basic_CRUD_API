using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int NumId { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
    public required string Priority {get; set;}
    //Foreign Keys
    [ForeignKey ("NumInv")]
    public required PC NumInv {get; set;}
    [ForeignKey ("Name")]
    public required Area Name {get; set;}

}

public class PC
{
    [Key]
    public int NumInv {get; set;}
    public int? NumSer {get; set;}
    public string? Description {get;set;}
}

public class Area
{
    [Key]
    public required string Name {get; set;}
}