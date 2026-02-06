using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Enums;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowFeedback.Application.Services;

public sealed class SetupService(
    IUserRepository userRepository,
    IUserTenantRepository userTenantRepository,
    ITenantRepository tenantRepository,
    ILogger<SetupService> logger) : ISetupService
{
    public async Task<bool> CreateInitialAdminAsync(string email, string password)
    {
        // 1. Check if any SystemAdmin exists
        var existingAdmin = await userRepository.GetByEmailAsync(email);
        if (existingAdmin != null)
        {
            logger.LogWarning("Tentativa de bootstrap com e-mail já existente: {Email}", email);
            return false;
        }

        // 2. Ensure "System" tenant exists
        var systemTenant = await tenantRepository.GetTenantAsync("system");
        if (systemTenant == null)
        {
            systemTenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "System",
                Slug = "system",
                Status = EnumStatusCadastro.Ativo,
                DbServer = "internal", // Not used for system tenant usually
                DbName = "FlowFeedback",
                DbUser = "sa",
                CreatedAt = DateTime.UtcNow
            };
            await tenantRepository.CadastrarTenantAsync(systemTenant);
        }

        // 3. Create User
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Administrator",
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = true,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CadastrarAsync(user);

        // 4. Link to UserTenant as SystemAdmin
        var userTenant = new UserTenant
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TenantId = systemTenant.Id,
            Role = EnumUserRole.SystemAdmin,
            IsActive = true
        };

        await userTenantRepository.CadastrarAsync(userTenant);

        logger.LogInformation("SystemAdmin criado com sucesso: {Email}", email);
        return true;
    }
}
