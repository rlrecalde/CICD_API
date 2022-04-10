CREATE TABLE [dbo].[Project] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [UserId]        INT           NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [RelativePath]  VARCHAR (255) NOT NULL,
    [DotnetVersion] VARCHAR (5)   NOT NULL,
    [Test]          BIT           NOT NULL,
    [Deploy]        BIT           NOT NULL,
    [DeployPort]    INT           NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Project_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

