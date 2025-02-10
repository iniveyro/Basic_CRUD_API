using System.ComponentModel.DataAnnotations;

namespace Api.Models;
public class Area
{
    [Key]
    public required string Name {get; set;}
    public string? Location {get; set;}
}