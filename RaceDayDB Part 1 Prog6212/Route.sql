CREATE TABLE Route(
    RouteId INT PRIMARY KEY IDENTITY(1,1),
    EventId INT NOT NULL,
    DistanceKM DECIMAL(6,2) NOT NULL,
    RouteDescription VARCHAR(500),
    RouteMapURL VARCHAR(500),

    FOREIGN KEY (EventId) REFERENCES Event(EventId)
);