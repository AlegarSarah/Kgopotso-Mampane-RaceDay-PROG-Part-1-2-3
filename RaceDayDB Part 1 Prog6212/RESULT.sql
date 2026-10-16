CREATE TABLE Result(
ResultId INT PRIMARY KEY IDENTITY(1,1),
EnrolmentId INT NOT NULL,
FinishTime TIME,
Position INT,
ResultStatus VARCHAR(30),
RecordDate DATE,

FOREIGN KEY (EnrolmentId) REFERENCES Enrolment(EnrolmentId)
);