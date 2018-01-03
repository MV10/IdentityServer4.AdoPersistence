CREATE TABLE [dbo].[AppUser] (
    [id]                  INT                IDENTITY (1, 1) NOT NULL,
    [SubjectId]           NVARCHAR (MAX)     NOT NULL,
    [Username]            NVARCHAR (MAX)     NOT NULL,
    [PasswordSalt]        NVARCHAR (MAX)     NOT NULL,
    [PasswordHash]        NVARCHAR (MAX)     NOT NULL,
    [ProviderName]        NVARCHAR (MAX)     NOT NULL,
    [ProviderSubjectId]   NVARCHAR (MAX)     NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[Claim] (
    [id]             INT            IDENTITY (1, 1) NOT NULL,
    [user_id]        INT            NOT NULL,
    [Issuer]         NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [OriginalIssuer] NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Subject]        NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Type]           NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Value]          NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [ValueType]      NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[Grant] (
    [id]           INT            IDENTITY (1, 1) NOT NULL,
    [Key]          NVARCHAR (200) NOT NULL,
    [ClientId]     NVARCHAR (200) NOT NULL,
    [CreationTime] DATETIME2 (7)  NOT NULL,
    [Data]         NVARCHAR (MAX) NOT NULL,
    [Expiration]   DATETIME2 (7)  NULL,
    [SubjectId]    NVARCHAR (200) NOT NULL,
    [Type]         NVARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
