using System.ComponentModel.DataAnnotations;

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

public class PC
{
    [Key]
    public int NumInv {get; set;}
    public int? NumSer {get; set;}
    public string? Description {get;set;}
    public ICollection<Ticket>? Tickets { get; set; }
}

public class Area
{
    [Key]
    public required string Name {get; set;}
    public ICollection<Ticket>? Tickets { get; set; }
}