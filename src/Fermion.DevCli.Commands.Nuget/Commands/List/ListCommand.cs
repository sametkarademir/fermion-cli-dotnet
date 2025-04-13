using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Services;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Nuget.Commands.List;

public class ListCommand(INugetService nugetService) : BaseCommand
{
    public override string Name => "list";
    public override string Description => "List all installed NuGet packages";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        command.SetHandler(async () =>
        {
            await nugetService.GetListAsync();
        });
        return command;
    }
}