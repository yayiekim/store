CREATE TABLE [dbo].[PaymentMethods] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [PaymentMethods] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

