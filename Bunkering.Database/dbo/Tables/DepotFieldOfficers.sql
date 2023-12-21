CREATE TABLE [dbo].[DepotFieldOfficers] (
    [ID]        INT              IDENTITY (1, 1) NOT NULL,
    [DepotID]   INT              NOT NULL,
    [OfficerID] UNIQUEIDENTIFIER NOT NULL,
    [IsDeleted] BIT              NOT NULL,
    CONSTRAINT [PK_DepotFieldOfficers] PRIMARY KEY CLUSTERED ([ID] ASC)
);

