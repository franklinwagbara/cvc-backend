CREATE TABLE [dbo].[Payments] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId]      INT            NULL,
    [COQId]              INT            NULL,
    [ApplicationTypeId]  INT            NULL,
    [OrderId]            NVARCHAR (MAX) NOT NULL,
    [ExtraPaymentId]     INT            NULL,
    [PaymentType]        NVARCHAR (MAX) NOT NULL,
    [TransactionDate]    DATETIME2 (7)  NOT NULL,
    [PaymentDate]        DATETIME2 (7)  NULL,
    [TransactionId]      NVARCHAR (MAX) NULL,
    [RRR]                NVARCHAR (MAX) NULL,
    [Description]        NVARCHAR (MAX) NOT NULL,
    [AppReceiptId]       NVARCHAR (MAX) NULL,
    [Amount]             FLOAT (53)     NOT NULL,
    [Arrears]            FLOAT (53)     NULL,
    [ServiceCharge]      FLOAT (53)     NOT NULL,
    [TxnMessage]         NVARCHAR (MAX) NULL,
    [RetryCount]         INT            NULL,
    [LastRetryDate]      DATETIME2 (7)  NULL,
    [Account]            NVARCHAR (MAX) NULL,
    [BankCode]           NVARCHAR (MAX) NULL,
    [LateRenewalPenalty] FLOAT (53)     NULL,
    [NonRenewalPenalty]  FLOAT (53)     NULL,
    [Status]             NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC)
);








































GO
CREATE NONCLUSTERED INDEX [IX_Payments_ApplicationId]
    ON [dbo].[Payments]([ApplicationId] ASC);


GO


