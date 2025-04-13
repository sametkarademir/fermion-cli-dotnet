using System.CommandLine;
using Fermion.DevCli.Commands.Password.Commands;
using Fermion.DevCli.Commands.Password.Commands.Generate;
using Fermion.DevCli.Commands.Password.Commands.Length;
using Fermion.DevCli.Commands.Password.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fermion.DevCli.Commands.Password.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPasswordCommandServices(this IServiceCollection services)
    {
        //Services
        services.AddSingleton<IPasswordGeneratorService, PasswordGeneratorService>();
        services.AddSingleton<IRandomProvider, RandomProvider>();

        //Commands
        services.AddSingleton<PasswordCommand>();
        services.AddSingleton<GenerateCommand>();
        services.AddSingleton<LengthCommand>();

        return services;
    }

    public static RootCommand AddPasswordCommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var passwordCommand = serviceProvider.GetRequiredService<PasswordCommand>();
        rootCommand.AddCommand(passwordCommand.Configure());
        return rootCommand;
    }
}