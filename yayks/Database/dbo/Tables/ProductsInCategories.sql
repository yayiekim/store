CREATE TABLE [dbo].[ProductsInCategories] (
    [ProductId]  NVARCHAR (128) NOT NULL,
    [CategoryId] INT            NOT NULL,
    CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([ProductId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_ProductsInCategories_ProductCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ProductCategories] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductsInCategories_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);

