namespace Backend.DTOs;

public class TaskDto
{
    public string Title { get; set; } = string.Empty;
    public int PriorityLevel { get; set; }
    public string? Notes { get; set; }
}
