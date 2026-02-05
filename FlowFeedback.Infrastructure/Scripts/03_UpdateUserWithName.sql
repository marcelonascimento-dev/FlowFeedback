-- Migração para adicionar a coluna Name na tabela Users
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Name')
BEGIN
    ALTER TABLE Users ADD Name NVARCHAR(200) NOT NULL DEFAULT '';
END
GO
