using System.ComponentModel.DataAnnotations;

namespace DevOpsTasks.Frontend.Models;

public sealed class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Todo";
    public string Priority { get; set; } = "Medium";
    public DateTimeOffset CreatedAt { get; set; }
}

public sealed class SaveTaskRequest
{
    [Required(ErrorMessage = "Please enter a title.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "Use 3 to 120 characters.")]
    public string Title { get; set; } = "";

    [StringLength(1000, ErrorMessage = "Description must be 1000 characters or fewer.")]
    public string Description { get; set; } = "";

    [Required]
    [RegularExpression("^(Todo|InProgress|Done)$", ErrorMessage = "Choose a valid status.")]
    public string Status { get; set; } = "Todo";

    [Required]
    [RegularExpression("^(Low|Medium|High)$", ErrorMessage = "Choose a valid priority.")]
    public string Priority { get; set; } = "Medium";
}
