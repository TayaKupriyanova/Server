-- Script Date: 18.12.2021 2:37  - ErikEJ.SqlCeScripting version 3.5.2.90
CREATE TABLE [Clients] (
  [Id] INTEGER NOT NULL
, [login] TEXT NOT NULL
, [password] TEXT NOT NULL
, [publicKey] TEXT NOT NULL
, [privateKey] TEXT NOT NULL
, CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
);
