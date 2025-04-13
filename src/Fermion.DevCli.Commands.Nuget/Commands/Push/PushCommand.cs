using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Services;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Nuget.Commands.Push;

public class PushCommand(INugetService nugetService) : BaseCommand
{
    public override string Name => "push";
    public override string Description => "Nuget package push command";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        var nameArgument = new Argument<string>("name", "Nuget source name. Example: nuget.org");
        var pathArgument = new Argument<string>("path", "Nuget package path. Example: /path/to/package.nupkg");
        command.AddArgument(nameArgument);
        command.AddArgument(pathArgument);
        command.SetHandler(
            (name, path) =>
            {
                nugetService.PushPackage(path, name).GetAwaiter();
            }, nameArgument, pathArgument);
        return command;
    }
}