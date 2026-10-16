namespace RaceDay.API.Models
{
    public class Enrolment
    {
        public int EnrolmentId { get; set; }
        public int ParticipantId { get; set; }
        public int EventCategoryId { get; set; }
        public DateOnly EnrolmentDate { get; set; }
        public string EnrolmentStatus { get; set; } = string.Empty;

        public User? Participant { get; set; }
        public EventCategory? EventCategory { get; set; }

        public ICollection<Result> Results { get; set; }
         = new List<Result>();
    }
}

