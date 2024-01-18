CREATE TABLE [dbo].[COQCertificates] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [COQId]        INT            NULL,
    [ProductId]    INT            NULL,
    [ElpsId]       INT            NOT NULL,
    [ExpireDate]   DATETIME2 (7)  NOT NULL,
    [IssuedDate]   DATETIME2 (7)  NOT NULL,
    [CertifcateNo] NVARCHAR (MAX) NOT NULL,
    [Signature]    NVARCHAR (MAX) NOT NULL,
    [QRCode]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_COQCertificates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_COQCertificates_CoQs_COQId] FOREIGN KEY ([COQId]) REFERENCES [dbo].[CoQs] ([Id]),
    CONSTRAINT [FK_COQCertificates_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_COQCertificates_COQId]
    ON [dbo].[COQCertificates]([COQId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_COQCertificates_ProductId]
    ON [dbo].[COQCertificates]([ProductId] ASC);

