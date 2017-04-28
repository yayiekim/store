CREATE TABLE [dbo].[Purchases] (
    [Id]            NVARCHAR (128) NOT NULL,
    [DatePurchased] DATETIME       NOT NULL,
    [AspNetUserId]  NVARCHAR (128) NOT NULL,
    [ReferenceNo]   NVARCHAR (50)  NULL,
    [Description]   NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Table_AspNeUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

