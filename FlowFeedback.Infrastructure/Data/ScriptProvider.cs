using System.Reflection;

namespace FlowFeedback.Infrastructure.Data;

public static class ScriptProvider
{
    public static string GetTenantSchemaScript()
    {
        return GetScript("02_TenantSchema.sql");
    }

    public static string GetMasterSchemaScript()
    {
        return GetScript("01_MasterSchema.sql");
    }

    private static string GetScript(string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();
        // O nome do resource é: Namespace.Pasta.Arquivo
        var resourceName = $"FlowFeedback.Infrastructure.Scripts.{filename}";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new Exception($"Script não encontrado: {resourceName}. Verifique se está como Embedded Resource.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}