using System.CommandLine;
using Fermion.DevCli.Commands.Nuget.Commands;
using Fermion.DevCli.Commands.Nuget.Commands.List;
using Fermion.DevCli.Commands.Nuget.Commands.Push;
using Fermion.DevCli.Commands.Nuget.Commands.Remove;
using Fermion.DevCli.Commands.Nuget.Commands.SetToken;
using Fermion.DevCli.Commands.Nuget.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fermion.DevCli.Commands.Nuget.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNugetCommandServices(this IServiceCollection services)
    {
        //Services
        services.AddSingleton<INugetService, NugetService>();

        //Commands
        services.AddSingleton<NugetCommand>();
        services.AddSingleton<SetTokenCommand>();
        services.AddSingleton<PushCommand>();
        services.AddSingleton<ListCommand>();
        services.AddSingleton<RemoveCommand>();

        return services;
    }

    public static RootCommand AddNugetCommand(this RootCommand rootCommand, IServiceProvider serviceProvider)
    {
        var nugetCommand = serviceProvider.GetRequiredService<NugetCommand>();
        rootCommand.AddCommand(nugetCommand.Configure());
        return rootCommand;
    }
}