CREATE TABLE [dbo].[ValidatiionResponses] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserId]    NVARCHAR (MAX) NOT NULL,
    [LicenseNo] NVARCHAR (MAX) NOT NULL,
    [Response]  NVARCHAR (MAX) NOT NULL,
    [IsUsed]    BIT            NOT NULL,
    CONSTRAINT [PK_ValidatiionResponses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

