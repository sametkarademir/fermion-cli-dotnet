using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Services;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Nuget.Commands.SetToken;

public class SetTokenCommand(INugetService nugetService) : BaseCommand
{
    public override string Name => "set-token";
    public override string Description => "NuGet API token set command";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        var nameArgument = new Argument<string>("name", "NuGet API token name");
        var tokenArgument = new Argument<string>("token", "NuGet API token value");
        var sourceOption = new Option<string>(
            aliases: ["--source", "-s"],
            description: "NuGet source URL",
            getDefaultValue: () => "https://api.nuget.org/v3/index.json"
        );
        var descriptionOption = new Option<string>(
            aliases: ["--description", "-d"],
            description: "NuGet API token description",
            getDefaultValue: () => string.Empty
        );
        command.AddArgument(nameArgument);
        command.AddArgument(tokenArgument);
        command.AddOption(sourceOption);
        command.AddOption(descriptionOption);
        command.SetHandler(async (name, token, source, description) =>
        {
            await nugetService.SetTokenAsync(name, token, source, description);
        }, nameArgument, tokenArgument, sourceOption, descriptionOption);
        return command;
    }
}