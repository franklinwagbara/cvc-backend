CREATE TABLE [dbo].[COQTanks] (
    [Id]       INT        IDENTITY (1, 1) NOT NULL,
    [CoQId]    INT        NOT NULL,
    [TankName] NCHAR (10) NOT NULL,
    CONSTRAINT [PK_COQTanks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

