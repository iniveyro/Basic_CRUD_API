using System.ComponentModel.DataAnnotations;

namespace Api.Models;
public class Pc
{
    [Key]
    public int NumInv {get; set;}
    public int? NumSer {get; set;}
    public string? Description {get;set;}
}