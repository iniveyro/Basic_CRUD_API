using System.ComponentModel.DataAnnotations;

public class Ticket
{
    [Key]
    public int NumId { get; set; }
    public string? Description { get; set; }
    public bool Status { get; set; }
}