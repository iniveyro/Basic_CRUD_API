using System.ComponentModel.DataAnnotations;

namespace Api.Models;
public class PC
{
    [Key]
    public int NumInv {get; set;}
    public int? NumSer {get; set;}
    public string? Description {get;set;}
    public ICollection<Ticket>? Tickets { get; set; }
}