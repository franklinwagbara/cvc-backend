﻿CREATE TABLE [dbo].[COQTanks] (
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [CoQId]  INT NOT NULL,
    [TankId] INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_COQTanks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

