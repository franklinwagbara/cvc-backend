CREATE TABLE [dbo].[TransferDetail] (
    [Id]               INT          IDENTITY (1, 1) NOT NULL,
    [IMONumber]        VARCHAR (50) NULL,
    [VesselName]       VARCHAR (50) NULL,
    [CreatedDate]      DATETIME     NULL,
    [ProductId]        INT          NULL,
    [OfftakeVolume]    FLOAT (53)   NULL,
    [TransferRecordId] INT          NULL,
    CONSTRAINT [PK_TransferDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

