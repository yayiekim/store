CREATE TABLE [dbo].[Orders] (
    [Id]            NVARCHAR (128) NOT NULL,
    [DateCreated]   DATETIME       NOT NULL,
    [AspNetUserId]  NVARCHAR (128) NOT NULL,
    [QuickBuyToken] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Orders_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

