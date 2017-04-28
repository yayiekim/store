CREATE TABLE [dbo].[ProductDetailImages] (
    [Id]               NVARCHAR (128) NOT NULL,
    [ProductDetailsId] NVARCHAR (128) NOT NULL,
    [ImageUrl]         NVARCHAR (500) NOT NULL,
    [FileName]         NVARCHAR (500) NOT NULL,
    [ThumbUrl]         NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductDetailImages_ProductDetails] FOREIGN KEY ([ProductDetailsId]) REFERENCES [dbo].[ProductDetails] ([Id]) ON DELETE CASCADE
);

