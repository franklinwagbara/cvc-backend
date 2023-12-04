CREATE TABLE [dbo].[Companies] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [AddressId]        INT            NULL,
    [Address]          NVARCHAR (MAX) NULL,
    [StateId]          INT            NULL,
    [YearIncorporated] NVARCHAR (MAX) NULL,
    [CountryId]        INT            NULL,
    [RcNumber]         NVARCHAR (MAX) NULL,
    [TinNumber]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

