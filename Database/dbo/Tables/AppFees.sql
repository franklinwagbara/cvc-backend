CREATE TABLE [dbo].[AppFees] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationTypeId] INT             NOT NULL,
    [ApplicationFee]    DECIMAL (18, 2) NOT NULL,
    [ProcessingFee]     DECIMAL (18, 2) NOT NULL,
    [SerciveCharge]     DECIMAL (18, 2) NOT NULL,
    [NOAFee]            DECIMAL (18, 2) NOT NULL,
    [COQFee]            DECIMAL (18, 2) NOT NULL,
    [IsDeleted]         BIT             NOT NULL,
    CONSTRAINT [PK_AppFees] PRIMARY KEY CLUSTERED ([Id] ASC)
);

