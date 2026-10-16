namespace RaceDay.API.Models
{
    public class Route
    {
        public int RouteId { get; set; }
        public int EventId { get; set; }
        public decimal DistanceKM { get; set; }
        public string? RouteDescription { get; set; }
        public string? RouteMapURL { get; set; }

        public Event? Event { get; set; }
    }
}

