CREATE TABLE [dbo].[Configuration] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [ConfigurationTypeId] INT           NOT NULL,
    [Value]               VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Configuration_ConfigurationType] FOREIGN KEY ([ConfigurationTypeId]) REFERENCES [dbo].[ConfigurationType] ([Id])
);

