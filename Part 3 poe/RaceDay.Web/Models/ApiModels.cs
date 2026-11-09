namespace RaceDay.Web.Models;

public class EventVm
{
    public int EventId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
    public string Location { get; set; } = "";
    public decimal DistanceKm { get; set; }
    public string EventType { get; set; } = "";
    public int OrganiserId { get; set; }
    public string? BannerUrl { get; set; }
}
public class EventSummaryVm
{
    public int EventId { get; set; }
    public string Name { get; set; } = "";
    public DateTime Date { get; set; }
    public string Location { get; set; } = "";
    public string EventType { get; set; } = "";
    public decimal DistanceKm { get; set; }
    public string? BannerUrl { get; set; }
    public int EnrolmentCount { get; set; }
}
public class CategoryVm
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
    public string CategoryType { get; set; } = "";
}
public class EnrolmentVm
{
    public int EnrolmentId { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; } = "";
    public DateTime EventDate { get; set; }
    public string ParticipantName { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public string Status { get; set; } = "";
}
public class MyResultVm
{
    public string EventName { get; set; } = "";
    public DateTime EventDate { get; set; }
    public string CategoryName { get; set; } = "";
    public TimeSpan FinishTime { get; set; }
    public int Position { get; set; }
    public int TotalFinishers { get; set; }
}
public class EventResultVm
{
    public int ResultId { get; set; }
    public int EnrolmentId { get; set; }
    public TimeSpan FinishTime { get; set; }
    public int Position { get; set; }
    public bool IsPublished { get; set; }
}

public class LoginResponse
{
    public string Message { get; set; } = "";

    public string Token { get; set; } = "";

    public DateTime ExpiresAt { get; set; }

    public LoginUserResponse User { get; set; } = new();
}

public class LoginUserResponse
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string Email { get; set; } = "";

    public string Role { get; set; } = "";
}

public class EventDetailsVm
{
    public EventVm Event { get; set; } = new();
    public List<CategoryVm> Categories { get; set; } = new();
    public bool AlreadyEnrolled { get; set; }
}
public class CategoriesVm
{
    public EventVm Event { get; set; } = new();
    public List<CategoryVm> Linked { get; set; } = new();
    public List<CategoryVm> Available { get; set; } = new();
}
public class EnrolmentsPageVm
{
    public EventVm Event { get; set; } = new();
    public List<EnrolmentVm> Enrolments { get; set; } = new();
}
public class ResultRowVm
{
    public EnrolmentVm Enrolment { get; set; } = new();
    public EventResultVm? Result { get; set; }
}
public class ResultsPageVm
{
    public EventVm Event { get; set; } = new();
    public List<ResultRowVm> Rows { get; set; } = new();
}