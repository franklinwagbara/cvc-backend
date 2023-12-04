﻿CREATE TABLE [dbo].[Payments] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId]      INT             NOT NULL,
    [ExtraPaymentId]     INT             NULL,
    [PaymentType]        NVARCHAR (MAX)  NOT NULL,
    [TransactionDate]    DATETIME2 (7)   NOT NULL,
    [TransactionId]      NVARCHAR (MAX)  NOT NULL,
    [OrderId]            NVARCHAR (MAX)  NULL,
    [RRR]                NVARCHAR (MAX)  NOT NULL,
    [PaymentDate]        DATETIME2 (7)   NULL,
    [Description]        NVARCHAR (MAX)  NOT NULL,
    [AppReceiptId]       NVARCHAR (MAX)  NOT NULL,
    [Amount]             DECIMAL (18, 2) NOT NULL,
    [Arrears]            DECIMAL (18, 2) NOT NULL,
    [ServiceCharge]      DECIMAL (18, 2) NOT NULL,
    [TxnMessage]         NVARCHAR (MAX)  NOT NULL,
    [RetryCount]         INT             NULL,
    [LastRetryDate]      DATETIME2 (7)   NULL,
    [Account]            NVARCHAR (MAX)  NOT NULL,
    [BankCode]           NVARCHAR (MAX)  NOT NULL,
    [LateRenewalPenalty] DECIMAL (18, 2) NOT NULL,
    [NonRenewalPenalty]  DECIMAL (18, 2) NOT NULL,
    [Status]             NVARCHAR (MAX)  NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payments_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payments_ExtraPayments_ExtraPaymentId] FOREIGN KEY ([ExtraPaymentId]) REFERENCES [dbo].[ExtraPayments] ([Id]) ON DELETE CASCADE
);


















GO
CREATE NONCLUSTERED INDEX [IX_Payments_ApplicationId]
    ON [dbo].[Payments]([ApplicationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Payments_ExtraPaymentId]
    ON [dbo].[Payments]([ExtraPaymentId] ASC);

