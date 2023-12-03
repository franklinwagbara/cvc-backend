CREATE TABLE [dbo].[LGAs] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [StateId] INT            NOT NULL,
    [Name]    NVARCHAR (MAX) NOT NULL,
    [Code]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_LGAs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LGAs_States_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_LGAs_StateId]
    ON [dbo].[LGAs]([StateId] ASC);

