using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Models;

public class TodoItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}