
CREATE TABLE Enrolment(
EnrolmentId INT PRIMARY KEY IDENTITY(1,1),
ParticipantId INT NOT NULL,
EventCategoryId INT NOT NULL,
EnrolmentDate DATE NOT NULL,
EnrolmentStatus VARCHAR(40) NOT NULL,

FOREIGN KEY (ParticipantId) 
REFERENCES [User](UserId),
FOREIGN KEY (EventCategoryId) 
REFERENCES 
EventCategory(EventCategoryId)
);

