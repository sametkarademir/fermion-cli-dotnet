namespace Fermion.DevCli.Core.Extensions;

public static class ConfigurationResourcesExtensions
{
    private static string GetBasePath()
    {
        var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".devcli");
        if (!Directory.Exists(baseDir))
        {
            Directory.CreateDirectory(baseDir);
        }
        return baseDir;
    }

    public static string GetNugetConfigFilePath(string directoryName, string fileName)
    {
        var nugetDir = Path.Combine(GetBasePath(), directoryName);
        if (!Directory.Exists(nugetDir))
        {
            Directory.CreateDirectory(nugetDir);
        }
        var configFile = Path.Combine(nugetDir, fileName);

        if (!File.Exists(configFile))
        {
            File.Create(configFile).Close();
        }

        return configFile;
    }
}