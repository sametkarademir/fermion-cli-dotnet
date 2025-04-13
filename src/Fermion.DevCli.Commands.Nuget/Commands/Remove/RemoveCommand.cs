using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Services;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Nuget.Commands.Remove;

public class RemoveCommand(INugetService nugetService) : BaseCommand
{
    public override string Name => "remove";
    public override string Description => "Remove a NuGet token for a given source.";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        var nameArgument = new Argument<string>("name", "NuGet API source name");
        command.AddArgument(nameArgument);
        command.SetHandler(async (name) =>
        {
            await nugetService.RemoveTokenAsync(name);
        }, nameArgument);

        return command;
    }
}