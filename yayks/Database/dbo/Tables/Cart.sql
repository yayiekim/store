CREATE TABLE [dbo].[Cart] (
    [Id]           NVARCHAR (128) NOT NULL,
    [AspNetUserId] NVARCHAR (128) NOT NULL,
    [ProductId]    NVARCHAR (128) NOT NULL,
    [Quantity]     INT            NOT NULL,
    [DateCreated]  DATETIME       NOT NULL,
    [IsSelected]   BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cart_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Cart_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

