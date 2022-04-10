CREATE TABLE [dbo].[Branch] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [ProjectId] INT          NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [Deleted]   BIT          NOT NULL,
    CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Branch_Project] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Project] ([Id])
);

