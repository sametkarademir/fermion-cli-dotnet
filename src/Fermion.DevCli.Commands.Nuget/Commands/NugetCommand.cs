using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Commands.List;
using Fermion.DevCli.Commands.Nuget.Commands.Push;
using Fermion.DevCli.Commands.Nuget.Commands.Remove;
using Fermion.DevCli.Commands.Nuget.Commands.SetToken;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Nuget.Commands;

public class NugetCommand(
    SetTokenCommand setTokenCommand,
    RemoveCommand removeCommand,
    PushCommand pushCommand,
    ListCommand listCommand) : BaseCommand
{
    public override string Name => "nuget";
    public override string Description => "nuget management commands";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        command.AddCommand(setTokenCommand.Configure());
        command.AddCommand(pushCommand.Configure());
        command.AddCommand(listCommand.Configure());
        command.AddCommand(removeCommand.Configure());
        return command;
    }
}