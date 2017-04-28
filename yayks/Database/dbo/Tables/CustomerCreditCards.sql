CREATE TABLE [dbo].[CustomerCreditCards] (
    [Id]           NVARCHAR (128) NOT NULL,
    [Token]        NVARCHAR (MAX) NULL,
    [AspNetUserId] NVARCHAR (128) NOT NULL,
    [IsDefault]    BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CustomerCreditCards_AspNetUsers] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

