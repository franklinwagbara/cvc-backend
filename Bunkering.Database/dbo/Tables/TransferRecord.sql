CREATE TABLE [dbo].[TransferRecord] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [IMONumber]    VARCHAR (50) NOT NULL,
    [MotherVessel] VARCHAR (50) NULL,
    [LoadingPort]  VARCHAR (50) NULL,
    [TotalVolume]  FLOAT (53)   NULL,
    [TransferDate] DATETIME     NULL,
    [VesselName]   VARCHAR (50) NULL,
    [VesselTypeId] INT          NULL,
    CONSTRAINT [PK_TransferRecord] PRIMARY KEY CLUSTERED ([Id] ASC)
);



