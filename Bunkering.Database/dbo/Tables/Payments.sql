﻿CREATE TABLE [dbo].[Payments] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId]      INT            NULL,
    [COQId]              INT            NULL,
    [ApplicationTypeId]  INT            NULL,
    [OrderId]            NVARCHAR (MAX) NOT NULL,
    [ExtraPaymentId]     INT            NULL,
    [PaymentType]        NVARCHAR (MAX) NOT NULL,
    [TransactionDate]    DATETIME2 (7)  NOT NULL,
    [PaymentDate]        DATETIME2 (7)  NOT NULL,
    [TransactionId]      NVARCHAR (MAX) NULL,
    [RRR]                NVARCHAR (MAX) NOT NULL,
    [Description]        NVARCHAR (MAX) NOT NULL,
    [AppReceiptId]       NVARCHAR (MAX) NOT NULL,
    [Amount]             FLOAT (53)     NOT NULL,
    [Arrears]            FLOAT (53)     NULL,
    [ServiceCharge]      FLOAT (53)     NOT NULL,
    [TxnMessage]         NVARCHAR (MAX) NULL,
    [RetryCount]         INT            NOT NULL,
    [LastRetryDate]      DATETIME2 (7)  NOT NULL,
    [Account]            NVARCHAR (MAX) NOT NULL,
    [BankCode]           NVARCHAR (MAX) NOT NULL,
    [LateRenewalPenalty] FLOAT (53)     NOT NULL,
    [NonRenewalPenalty]  FLOAT (53)     NOT NULL,
    [Status]             NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC)
);




































GO
CREATE NONCLUSTERED INDEX [IX_Payments_ApplicationId]
    ON [dbo].[Payments]([ApplicationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Payments_ExtraPaymentId]
    ON [dbo].[Payments]([ExtraPaymentId] ASC);

