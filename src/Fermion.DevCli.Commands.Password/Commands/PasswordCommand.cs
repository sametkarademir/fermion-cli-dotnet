using System.CommandLine;
using Fermion.DevCli.Commands.Password.Commands.Generate;
using Fermion.DevCli.Commands.Password.Commands.Length;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Password.Commands;

public class PasswordCommand(GenerateCommand generateCommand, LengthCommand lengthCommand) : BaseCommand
{
    public override string Name => "password";
    public override string Description => "Password generator and length checker";

    public override Command Configure()
    {
        var command = new Command(Name, Description);
        
        command.AddCommand(generateCommand.Configure());
        command.AddCommand(lengthCommand.Configure());
            
        return command;
    }
}