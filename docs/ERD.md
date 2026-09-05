erDiagram
    User ||--o{ Enrolment : makes
    Event ||--o{ EventCategory : contains
    Category ||--o{ EventCategory : has
    EventCategory ||--o{ Enrolment : receives
    Enrolment ||--o| Result : produces
    Event ||--o{ Route : has

    User {
        int UserId PK
        string Name
        string Email
    }

    Event {
        int EventId PK
        string EventName
        date EventDate
    }

    Category {
        int CategoryId PK
        string CategoryName
    }

    EventCategory {
        int EventCategoryId PK
        int EventId FK
        int CategoryId FK
    }

    Enrolment {
        int EnrolmentId PK
        int ParticipantId FK
        int EventCategoryId FK
    }

    Result {
        int ResultId PK
        int EnrolmentId FK
        string FinishTime
    }

    Route {
        int RouteId PK
        int EventId FK
        string RouteName
    }