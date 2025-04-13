namespace Fermion.DevCli.Commands.Nuget.Models;

public class NugetConfiguration
{
    public Dictionary<string, Source> NuGet { get; set; } = new Dictionary<string, Source>();
}

public class Source
{
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? Description { get; set; }
}