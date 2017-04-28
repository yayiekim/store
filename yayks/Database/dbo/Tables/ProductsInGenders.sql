CREATE TABLE [dbo].[ProductsInGenders] (
    [Gender]    NVARCHAR (50)  NOT NULL,
    [ProductId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_ProductsInGenders] PRIMARY KEY CLUSTERED ([Gender] ASC, [ProductId] ASC),
    CONSTRAINT [FK_ProductsInGenders_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);

