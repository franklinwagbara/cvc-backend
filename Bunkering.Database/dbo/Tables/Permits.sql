CREATE TABLE [dbo].[Permits] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT            NOT NULL,
    [ElpsId]        INT            NOT NULL,
    [ExpireDate]    DATETIME2 (7)  NOT NULL,
    [IssuedDate]    DATETIME2 (7)  NOT NULL,
    [PermitNo]      NVARCHAR (MAX) NOT NULL,
    [Signature]     NVARCHAR (MAX) NOT NULL,
    [QRCode]        NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Permits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permits_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[Applications] ([Id]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX_Permits_ApplicationId]
    ON [dbo].[Permits]([ApplicationId] ASC);

