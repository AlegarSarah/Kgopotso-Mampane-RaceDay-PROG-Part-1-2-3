CREATE TABLE EventCategory(
EventCategoryId INT PRIMARY KEY IDENTITY(1,1), 
EventId INT NOT NULL,
CategoryId INT NOT NULL,
EntryFee DECIMAL(10,5) NOT NULL,
MaximumParticipants INT NOT NULL, 

FOREIGN KEY (EventId ) REFERENCES Event
(EventId),
FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
); 