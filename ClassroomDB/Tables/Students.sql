CREATE TABLE [dbo].[Students]
(
    [StudentId]  INT           NOT NULL IDENTITY(1,1),
    [FirstName]  NVARCHAR(50)  NOT NULL,
    [LastName]   NVARCHAR(50)  NOT NULL,
    [Email]      NVARCHAR(100) NOT NULL,
    [DateOfBirth] DATE         NULL,
    [EnrolledAt] DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([StudentId] ASC),
    CONSTRAINT [UQ_Students_Email] UNIQUE ([Email])
);
