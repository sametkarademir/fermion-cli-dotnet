using System.Diagnostics;
using System.Text.Json;
using Fermion.DevCli.Commands.Nuget.Models;
using Fermion.DevCli.Core.Extensions;

namespace Fermion.DevCli.Commands.Nuget.Services;

public interface INugetService
{
    /// <summary>
    /// Gets the list of NuGet tokens.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method reads the NuGet configuration file and prints the tokens to the console.
    /// </remarks>
    Task GetListAsync();

    /// <summary>
    /// Sets the NuGet token for a given source.
    /// </summary>
    /// <param name="token">The NuGet token.</param>
    /// <param name="name">The name of the token.</param>
    /// <param name="source">The NuGet source URL.</param>
    /// <param name="description">An optional description for the token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method saves the token in the configuration file.
    /// </remarks>
    Task SetTokenAsync(string name, string token, string source, string? description = null);

    /// <summary>
    /// Removes the NuGet token for a given source.
    /// </summary>
    /// <param name="name">The name of the token to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method removes the token from the configuration file.
    /// </remarks>
    Task RemoveTokenAsync(string name);

    /// <summary>
    /// Pushes a NuGet package to the specified source.
    /// </summary>
    /// <param name="packagePath">The path to the NuGet package.</param>
    /// <param name="name">The name of the NuGet source.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method uses the dotnet CLI to push the package.
    /// </remarks>
    Task PushPackage(string packagePath, string name);
}

public class NugetService : INugetService
{
    public async Task GetListAsync()
    {
        var nugetConfiguration = ReadConfig();
        if (nugetConfiguration.NuGet.Count == 0)
        {
            ConsoleWriteExtensions.PrintMessage("NuGet tokens not found. Please set a token using 'devcli nuget set-token <name> <token> -s <source> -d <description>'", MessageType.Warning);
            return;
        }

        var i = 1;
        foreach (var kvp in nugetConfiguration.NuGet)
        {
            ConsoleWriteExtensions.PrintMessage($"Source {i}: \n\t * Name: {kvp.Key} \n\t * Source: {kvp.Value.Url}, \n\t * Token: {kvp.Value.Token} \n\t * Description: {kvp.Value.Descripiton}");
            i++;
        }
        await Task.CompletedTask;
    }

    public async Task SetTokenAsync(string name, string token, string source, string? description = null)
    {
        var nugetConfiguration = ReadConfig();
        string nameKey = name.ToLowerInvariant();
        if (nugetConfiguration.NuGet.ContainsKey(nameKey))
        {
            nugetConfiguration.NuGet[nameKey].Token = token;
            nugetConfiguration.NuGet[nameKey].Url = source;
            nugetConfiguration.NuGet[nameKey].Descripiton = description;
        }
        else
        {
            nugetConfiguration.NuGet.Add(nameKey, new Source { Url = source, Token = token, Descripiton = description });
        }
        SaveConfig(nugetConfiguration);
        ConsoleWriteExtensions.PrintMessage("Saved NuGet token successfully!", MessageType.Success);
        ConsoleWriteExtensions.PrintMessage($"NuGet token for {name} has been set.");
        ConsoleWriteExtensions.PrintMessage("You can now push packages using 'devcli nuget push <package_path>'");
        await Task.CompletedTask;
    }

    public async Task RemoveTokenAsync(string name)
    {
        var nugetConfiguration = ReadConfig();
        string nameKey = name.ToLowerInvariant();
        if (nugetConfiguration.NuGet.Remove(nameKey))
        {
            SaveConfig(nugetConfiguration);
            ConsoleWriteExtensions.PrintMessage($"NuGet token for {name} has been removed.", MessageType.Success);
        }
        else
        {
            ConsoleWriteExtensions.PrintMessage($"NuGet token for {name} not found.", MessageType.Warning);
        }
        await Task.CompletedTask;
    }

    public async Task PushPackage(string packagePath, string name)
    {
        if (!File.Exists(packagePath))
        {
            ConsoleWriteExtensions.PrintMessage($"Error: nuget package not found at {packagePath}", MessageType.Error);
            return;
        }

        var nugetConfiguration = ReadConfig();
        string nameKey = name.ToLowerInvariant();
        if (!nugetConfiguration.NuGet.TryGetValue(nameKey, out var source))
        {
            ConsoleWriteExtensions.PrintMessage($"Error: NuGet token not found for {name}", MessageType.Error);
            ConsoleWriteExtensions.PrintMessage("Please set the token first using 'devcli nuget set-token <token>'");
            return;
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"nuget push \"{packagePath}\" --api-key {source.Token} --source {source.Url}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        ConsoleWriteExtensions.PrintMessage($"NuGet package is being pushed: {Path.GetFileName(packagePath)}");
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        if (process.ExitCode == 0)
        {
            ConsoleWriteExtensions.PrintMessage("NuGet package pushed successfully!", MessageType.Success);
            ConsoleWriteExtensions.PrintMessage(output, MessageType.Info);
        }
        else
        {
            ConsoleWriteExtensions.PrintMessage("NuGet push failed!", MessageType.Error);
            ConsoleWriteExtensions.PrintMessage(error, MessageType.Error);
        }
    }

    /// <summary>
    /// Reads the NuGet configuration file and returns the deserialized object.
    /// </summary>
    /// <returns>The NuGet configuration object.</returns>
    /// <remarks>
    /// This method reads the configuration file from the specified path and deserializes it into a NugetConfiguration object.
    /// If the file does not exist or is empty, a new NugetConfiguration object is returned.
    /// </remarks>
    private NugetConfiguration ReadConfig()
    {
        var path = ConfigurationResourcesExtensions.GetNugetConfigFilePath(NugetConstants.NugetDirectory, NugetConstants.NugetConfigFile);
        string json = File.ReadAllText(path);
        if (string.IsNullOrEmpty(json))
        {
            return new NugetConfiguration();
        }
        return JsonSerializer.Deserialize<NugetConfiguration>(json) ?? new NugetConfiguration();
    }

    /// <summary>
    /// Saves the NuGet configuration object to the specified file.
    /// </summary>
    /// <param name="config">The NuGet configuration object to save.</param>
    /// <remarks>
    /// This method serializes the NuGet configuration object to JSON and writes it to the specified file.
    /// If the file does not exist, it will be created.
    /// </remarks>
    private void SaveConfig(NugetConfiguration config)
    {
        var path = ConfigurationResourcesExtensions.GetNugetConfigFilePath(NugetConstants.NugetDirectory, NugetConstants.NugetConfigFile);
        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}