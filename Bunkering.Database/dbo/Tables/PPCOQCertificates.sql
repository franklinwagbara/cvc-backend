CREATE TABLE [dbo].[PPCOQCertificates] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [COQId]        INT            NULL,
    [ProductId]    INT            NULL,
    [ElpsId]       INT            NOT NULL,
    [ExpireDate]   DATETIME2 (7)  NOT NULL,
    [IssuedDate]   DATETIME2 (7)  NOT NULL,
    [CertifcateNo] NVARCHAR (MAX) NOT NULL,
    [Signature]    NVARCHAR (MAX) NOT NULL,
    [QRCode]       NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_PPCOQCertificates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PPCOQCertificates_ProcessingPlantCOQs_COQId] FOREIGN KEY ([COQId]) REFERENCES [dbo].[ProcessingPlantCOQs] ([ProcessingPlantCOQId]),
    CONSTRAINT [FK_PPCOQCertificates_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);



GO
CREATE NONCLUSTERED INDEX [IX_PPCOQCertificates_ProductId]
    ON [dbo].[PPCOQCertificates]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PPCOQCertificates_COQId]
    ON [dbo].[PPCOQCertificates]([COQId] ASC);

