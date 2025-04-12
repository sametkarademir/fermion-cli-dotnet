using System.Security.Cryptography;

namespace Fermion.DevCli.Commands.Password.Services;

/// <summary>
/// Interface for random number and byte generation.
/// </summary>
/// <remarks>
/// This interface defines methods for generating random integers and byte arrays.
/// </remarks>
public interface IRandomProvider
{
    /// <summary>
    /// Generates a random integer within the specified range.
    /// </summary>
    /// <param name="minInclusive">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxExclusive">The exclusive upper bound of the random number returned.</param>
    /// <returns>A random integer between minInclusive and maxExclusive.</returns>
    /// <remarks>
    /// This method uses a cryptographically secure random number generator to produce a random integer.
    /// </remarks>
    int GetRandomInt(int minInclusive, int maxExclusive);
    
    /// <summary>
    /// Generates a random byte array of the specified length.
    ///</summary>
    ///<param name="length">The length of the byte array to generate.</param>
    ///<returns>A byte array filled with random bytes.</returns>
    ///<remarks>
    /// This method uses a cryptographically secure random number generator to produce a byte array.
    ///</remarks>
    byte[] GetRandomBytes(int length);
}

public class RandomProvider : IRandomProvider, IDisposable
{
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public int GetRandomInt(int minInclusive, int maxExclusive)
    {
        if (minInclusive >= maxExclusive)
        {
            throw new ArgumentOutOfRangeException(nameof(minInclusive), 
                $"The minimum value must be less than the maximum value. Min: [{minInclusive}], Max: [{maxExclusive}]");
        }
        
        var buffer = new byte[4];
        _rng.GetBytes(buffer);
        
        var randomInt = BitConverter.ToInt32(buffer, 0);
        var result = Math.Abs(randomInt == int.MinValue ? randomInt + 1 : randomInt);
            
        return minInclusive + result % (maxExclusive - minInclusive);
    }

    public byte[] GetRandomBytes(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), 
                "The length must be a positive integer.");
        }

        var buffer = new byte[length];
        _rng.GetBytes(buffer);
        return buffer;
    }

    public void Dispose()
    {
        _rng.Dispose();
    }
}