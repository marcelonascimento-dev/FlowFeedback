-- Script para remover todas as tabelas do banco Master

-- 1. Tabelas com Dependências (Foreign Keys)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'DispositivoKeys')
    DROP TABLE DispositivoKeys;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'UserTenants')
    DROP TABLE UserTenants;

-- 2. Tabelas Principais
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
    DROP TABLE Users;

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Tenants')
    DROP TABLE Tenants;

PRINT 'Todas as tabelas do banco Master foram removidas com sucesso.';
