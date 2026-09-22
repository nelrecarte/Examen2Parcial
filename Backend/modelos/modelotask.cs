namespace TaskFlow.Api.Models
{
    public class Task
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int PriorityLevel { get; set; }
        public string? Notes { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}