CREATE TABLE [dbo].[ProductColors] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [ProductColorName] NVARCHAR (50) NOT NULL,
    [ProductColorCode] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

