using System.CommandLine;
using Fermion.DevCli.Core.Abstracts;

namespace Fermion.DevCli.Commands.Password.Commands.Length;

public class LengthCommand : BaseCommand
{
    public override string Name => "length";
    public override string Description => "Check the length of a password";

    public override Command Configure()
    {
        var command = new Command(Name, Description);

        var passwordArgument = new Argument<string>(
            name: "password",
            description: "Password to evaluate (use quotes for special characters)"
        )
        {
            Arity = ArgumentArity.ExactlyOne,
        };

        passwordArgument.AddValidator(result =>
        {
            var token = result.Tokens.FirstOrDefault();

            if (token is null)
            {
                result.ErrorMessage = "Password cannot be empty";
                return;
            }

            if (token.Value.Contains(" ") && !(token.Value.StartsWith("\"") && token.Value.EndsWith("\"")))
            {
                result.ErrorMessage = "Passwords containing spaces must be entered in quotes. Example: \"my password\"";
            }
        });

        command.AddArgument(passwordArgument);

        command.SetHandler((password) =>
        {
            Console.WriteLine($"Password length: [{password.Length}] characters");

            if (password.Length < 8)
            {
                Console.WriteLine("Security Level: Weak - Your password is too short!");
            }
            else if (password.Length < 12)
            {
                Console.WriteLine("Security Level: Medium - Your password is not long enough!");
            }
            else if (password.Length < 16)
            {
                Console.WriteLine("Security Level: Strong - Your password looks secure!");
            }
            else
            {
                Console.WriteLine("Security Level: Perfect - Your password is very strong!");
            }
        }, passwordArgument);

        return command;
    }
}