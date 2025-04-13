using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Extensions;
using Fermion.DevCli.Commands.Password.Extensions;
using Fermion.DevCli.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Fermion.DevCli.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        try
        {
            var services =
                new ServiceCollection()
                .AddPasswordCommandServices()
                .AddNugetCommandServices()
                .BuildServiceProvider();

            var rootCommand = new RootCommand();
            rootCommand.AddPasswordCommand(services);
            rootCommand.AddNugetCommand(services);
            await rootCommand.InvokeAsync(args);
        }
        catch (Exception e)
        {
            ConsoleWriteExtensions.PrintMessage("An error occurred while executing the command.", MessageType.Error);
            ConsoleWriteExtensions.PrintMessage(e.Message, MessageType.Error);
        }
    }
}