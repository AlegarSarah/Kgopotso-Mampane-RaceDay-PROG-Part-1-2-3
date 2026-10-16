namespace RaceDay.API.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int EnrolmentId { get; set; }
        public TimeOnly? FinishTime { get; set; }
        public int? Position { get; set; }
        public string? ResultStatus { get; set; }
        public DateOnly? RecordDate { get; set; }

        public Enrolment? Enrolment { get; set; }
    }
}

