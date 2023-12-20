CREATE TABLE [dbo].[ApplicationDepots]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [DepotId] INT NOT NULL, 
    [AppId] INT NOT NULL, 
    [Volume] DECIMAL NOT NULL, 
    [ProductId] INT NOT NULL
)
