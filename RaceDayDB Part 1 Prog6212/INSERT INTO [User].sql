INSERT INTO [User]
(FirstName, LastName, Email, PasswordHash, Role, PhoneNumber)
VALUES('Thabang', 'Mametsa', 'thabang@raceday.co.za', 'hashed_password_1', 'Organiser', '0715689903'),
('Lerato', 'Motsepe', 'lerato@raceday.co.za', 'hashed_password_2', 'Organiser', '0768059920'),
('Kabelo', 'Mashiane', 'kabelo@gmail.com', 'hashed_password_3', 'Participant', '0701711985'),
('Naledi', 'Mokoena', 'naledi@gmail.com', 'hashed_password_4', 'Participant', '0714319970'),
('Sipho', 'Dlamini', 'sipho@gmail.com', 'hashed_password_5', 'Participant', '0834659732'),
('Palesa', 'ndlovu', 'palesa@gmail.com', 'hashed_password_6', 'Participant', '0761089291'),
('Tshepo', 'Molefe', 'tshepo@gmail.com', 'hashed_password_7', 'Participant', '0839791018');

SELECT *
FROM [User]