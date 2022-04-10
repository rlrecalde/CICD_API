CREATE TABLE [dbo].[Commit] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [BranchId]       INT           NOT NULL,
    [Sha]            VARCHAR (255) NOT NULL,
    [CommitterLogin] VARCHAR (50)  NOT NULL,
    [CommitterName]  VARCHAR (255) NOT NULL,
    [Date]           DATETIME      NOT NULL,
    [Message]        VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Commit] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Commit_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id])
);

