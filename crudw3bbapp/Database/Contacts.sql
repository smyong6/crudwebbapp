CREATE TABLE [dbo].[Contacts]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	[FirstName] TEXT NOT NULL,
	[LastName] TEXT,
	[Email] TEXT,
	[PhoneNumber] TEXT,
	[Company] TEXT,
)