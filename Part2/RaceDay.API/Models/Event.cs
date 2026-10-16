namespace RaceDay.API.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int OrganiserId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public User? Organiser { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }
            = new List<EventCategory>();

        public ICollection<Route> Routes { get; set; }
            = new List<Route>();
    }
}

