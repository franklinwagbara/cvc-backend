CREATE TABLE [dbo].[vAppPayment] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId]   INT             NOT NULL,
    [AppType]         NVARCHAR (MAX)  NOT NULL,
    [AppReference]    NVARCHAR (MAX)  NOT NULL,
    [Description]     NVARCHAR (MAX)  NOT NULL,
    [OrderId]         NVARCHAR (MAX)  NOT NULL,
    [TransactionDate] DATETIME2 (7)   NOT NULL,
    [PaymentDate]     DATETIME2 (7)   NOT NULL,
    [PaymentType]     NVARCHAR (MAX)  NOT NULL,
    [Account]         NVARCHAR (MAX)  NOT NULL,
    [ServiceCharge]   DECIMAL (18, 2) NOT NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [RRR]             NVARCHAR (MAX)  NOT NULL,
    [TxnMessage]      NVARCHAR (MAX)  NOT NULL,
    [PaymentStatus]   NVARCHAR (MAX)  NOT NULL,
    CONSTRAINT [PK_vAppPayment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

