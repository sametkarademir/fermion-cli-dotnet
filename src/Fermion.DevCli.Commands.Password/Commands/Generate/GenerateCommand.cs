using System.CommandLine;
using Fermion.DevCli.Commands.Password.Models;
using Fermion.DevCli.Commands.Password.Services;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Password.Commands.Generate;

public class GenerateCommand(IPasswordGeneratorService passwordService) : BaseCommand
{
    public override string Name => "generate";
    public override string Description => "Generates a random password";
    public override Command Configure()
    {
        var command = new Command(Name, Description);

        var lengthOption = new Option<int>(
            aliases: new[] { "--length", "-l" },
            description: "The length of the password",
            getDefaultValue: () => 16);

        lengthOption.AddValidator(result =>
        {
            if (result.GetValueForOption(lengthOption) < 4)
            {
                result.ErrorMessage = "Password length must be at least 4 characters";
            }

            if (result.GetValueForOption(lengthOption) > 128)
            {
                result.ErrorMessage = "Password length must be at most 128 characters";
            }
        });

        var uppercaseOption = new Option<bool>(
            aliases: new[] { "--uppercase", "-u" },
            description: "Include uppercase letters ?",
            getDefaultValue: () => true);

        var lowercaseOption = new Option<bool>(
            aliases: new[] { "--lowercase", "-w" },
            description: "Include lowercase letters ?",
            getDefaultValue: () => true);

        var numbersOption = new Option<bool>(
            aliases: new[] { "--numbers", "-n" },
            description: "Include numbers ?",
            getDefaultValue: () => true);

        var specialCharsOption = new Option<bool>(
            aliases: new[] { "--special", "-s" },
            description: "Include special characters ?",
            getDefaultValue: () => true);

        var countOption = new Option<int>(
            aliases: new[] { "--count", "-c" },
            description: "Number of passwords to generate",
            getDefaultValue: () => 1);

        countOption.AddValidator(result =>
        {
            if (result.GetValueForOption(countOption) < 1)
            {
                result.ErrorMessage = "Number of passwords to generate must be at least 1";
            }

            if (result.GetValueForOption(countOption) > 100)
            {
                result.ErrorMessage = "Number of passwords to generate must be at most 100";
            }
        });

        command.AddOption(lengthOption);
        command.AddOption(uppercaseOption);
        command.AddOption(lowercaseOption);
        command.AddOption(numbersOption);
        command.AddOption(specialCharsOption);
        command.AddOption(countOption);

        command.SetHandler((length, uppercase, lowercase, numbers, special, count) =>
        {
            var options = new PasswordOptions
            {
                Length = length,
                IncludeUppercase = uppercase,
                IncludeLowercase = lowercase,
                IncludeNumbers = numbers,
                IncludeSpecialCharacters = special
            };

            for (int i = 0; i < count; i++)
            {
                var password = passwordService.GeneratePassword(options);
                Console.WriteLine(password);
            }
        }, lengthOption, uppercaseOption, lowercaseOption, numbersOption, specialCharsOption, countOption);

        return command;
    }
}