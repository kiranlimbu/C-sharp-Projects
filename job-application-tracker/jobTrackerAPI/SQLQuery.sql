CREATE DATABASE JobTrackerDB;
GO

USE JobTrackerDB;
GO

CREATE TABLE JobApplication (
	ApplicationID INT NOT NULL IDENTITY (1,1),
	Company VARCHAR (100),
	Position VARCHAR (100),
	Website VARCHAR (500),
	Address VARCHAR (500),
	Contact VARCHAR (100),
	Phone VARCHAR (14),
	DateApplied Date DEFAULT GETDATE(),
	Notes VARCHAR (2000)
);
GO

INSERT INTO JobApplication (Company, Position, Website, Address, Contact, Phone)
	VALUES ('Microsoft', 
		'Customer Engineer', 
		'https://www.microsoft.com/en-us/', 
		'Seattle, WA', 
		'Deidre Munn', 
		'703-468-1063');

INSERT INTO JobApplication (Company, Position, Website, Address, Contact, Phone)
	VALUES ('Amazon', 
		'Software Engineer', 
		'https://www.amazon.com/', 
		'Redmond, WA', 
		'David Young', 
		'253-356-6309');

INSERT INTO JobApplication (Company, Position, Website, Address, Contact, Phone)
	VALUES ('Oracle', 
		'DBA', 
		'https://www.oracle.com/index.html', 
		'Redmond, WA', 
		'Aida Brown', 
		'415-908-1007');
GO

SELECT * FROM JobApplication;
GO

DROP TABLE JobApplication;

IF OBJECT_ID('tempdb..dbo.JobApplication') IS NOT NULL
DROP TABLE JobApplication;
DROP DATABASE JobTrackerDB;