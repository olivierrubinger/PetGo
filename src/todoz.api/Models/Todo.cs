namespace todoz.api.Models
{
    public class Todo
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
