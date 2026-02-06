-- Migration script to update existing Empresas table with new fields
-- Run this on each existing tenant database

-- Add new columns to Empresas table
ALTER TABLE Empresas ADD CNPJ NVARCHAR(20) NULL;
ALTER TABLE Empresas ADD Email NVARCHAR(150) NULL;
ALTER TABLE Empresas ADD Telefone NVARCHAR(20) NULL;
ALTER TABLE Empresas ADD CEP NVARCHAR(10) NULL;
ALTER TABLE Empresas ADD Logradouro NVARCHAR(255) NULL;
ALTER TABLE Empresas ADD Numero NVARCHAR(20) NULL;
ALTER TABLE Empresas ADD Complemento NVARCHAR(100) NULL;
ALTER TABLE Empresas ADD Bairro NVARCHAR(100) NULL;
ALTER TABLE Empresas ADD UF NVARCHAR(2) NULL;

-- Drop old columns that were replaced
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Empresas') AND name = 'Endereco')
    ALTER TABLE Empresas DROP COLUMN Endereco;

-- Update existing records with default values (adjust as needed)
UPDATE Empresas 
SET 
    CNPJ = COALESCE(CNPJ, '00000000000000'),
    Email = COALESCE(Email, 'contato@empresa.com'),
    Telefone = COALESCE(Telefone, '0000000000'),
    CEP = COALESCE(CEP, '00000000'),
    Logradouro = COALESCE(Logradouro, 'Não informado'),
    Numero = COALESCE(Numero, 'S/N'),
    Bairro = COALESCE(Bairro, 'Não informado'),
    Cidade = COALESCE(Cidade, 'Não informado'),
    UF = COALESCE(UF, 'XX')
WHERE CNPJ IS NULL OR Email IS NULL OR Telefone IS NULL;