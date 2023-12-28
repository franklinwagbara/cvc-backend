CREATE TABLE [dbo].[COQCertificates] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [COQId]        INT            NOT NULL,
    [ElpsId]       INT            NOT NULL,
    [ExpireDate]   DATETIME2 (7)  NOT NULL,
    [IssuedDate]   DATETIME2 (7)  NOT NULL,
    [CertifcateNo] NVARCHAR (MAX) NOT NULL,
    [Signature]    NVARCHAR (MAX) NOT NULL,
    [QRCode]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_COQCertificates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_COQCertificates_CoQs_COQId] FOREIGN KEY ([COQId]) REFERENCES [dbo].[CoQs] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_COQCertificates_COQId]
    ON [dbo].[COQCertificates]([COQId] ASC);

