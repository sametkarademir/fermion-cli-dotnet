using System.Text;
using Fermion.DevCli.Commands.Password.Models;

namespace Fermion.DevCli.Commands.Password.Services;

/// <summary>
/// Interface for password generation service.
/// </summary>
/// <remarks>
/// This interface defines a contract for generating passwords based on specified options.
///</remarks>
public interface IPasswordGeneratorService
{
    /// <summary>
    /// Generates a password based on the provided options.
    /// </summary>
    /// <param name="options">The options to use for generating the password.</param>
    /// <returns>A string representing the generated password.</returns>
    /// <remarks>
    /// This method takes a <see cref="PasswordOptions"/> object that specifies the criteria for the password,
    /// such as length and character types. It returns a string that meets the specified criteria.
    /// </remarks>
    string GeneratePassword(PasswordOptions options);
}

public class PasswordGeneratorService(IRandomProvider randomProvider) : IPasswordGeneratorService
{
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string NumberChars = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?/";

    public string GeneratePassword(PasswordOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        if (options.Length <= 0)
            throw new ArgumentException("Password length must be a positive number.", nameof(options.Length));

        if (!options.IncludeUppercase && !options.IncludeLowercase &&
            !options.IncludeNumbers && !options.IncludeSpecialCharacters)
        {
            throw new ArgumentException("At least one character set must be selected.");
        }

        var charSets = new List<string>();
        if (options.IncludeUppercase) charSets.Add(UppercaseChars);
        if (options.IncludeLowercase) charSets.Add(LowercaseChars);
        if (options.IncludeNumbers) charSets.Add(NumberChars);
        if (options.IncludeSpecialCharacters) charSets.Add(SpecialChars);

        string allChars = string.Join("", charSets);

        var password = new StringBuilder();
        foreach (var charSet in charSets)
        {
            password.Append(charSet[randomProvider.GetRandomInt(0, charSet.Length)]);
        }

        for (int i = password.Length; i < options.Length; i++)
        {
            password.Append(allChars[randomProvider.GetRandomInt(0, allChars.Length)]);
        }

        var passwordChars = password.ToString().ToCharArray();
        for (int i = passwordChars.Length - 1; i > 0; i--)
        {
            int j = randomProvider.GetRandomInt(0, i + 1);
            (passwordChars[i], passwordChars[j]) = (passwordChars[j], passwordChars[i]);
        }
        return new string(passwordChars);
    }
}