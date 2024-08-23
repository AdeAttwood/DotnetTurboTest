using System.ComponentModel.DataAnnotations;

namespace DotnetTurbo.Web.Models;

public class TodoModel
{
    public string ID { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string? Content { get; set; }

    public bool Done { get; set; } = false;
}