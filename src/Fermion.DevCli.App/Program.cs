

using System.CommandLine;
using Fermion.DevCli.Commands.Password.Extensions;
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
                .BuildServiceProvider();

            var rootCommand = new RootCommand();
            rootCommand.AddPasswordCommand(services);
            await rootCommand.InvokeAsync(args);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}