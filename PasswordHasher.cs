using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHasher
{
    public static Tuple<string, string> GenerateSaltAndHash(string password)
    {
        // Generate Salt
        string salt = GenerateSalt();
        string hashedPassword = HashPasswordWithSalt(password, salt);
        return Tuple.Create(hashedPassword, salt);
    }

    private static string GenerateSalt(int size = 32)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(salt);
    }

    private static string HashPasswordWithSalt(string password, string saltStr)
    {
        byte[] salt = Encoding.UTF8.GetBytes(saltStr);
        var sha256 = SHA256.Create();

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];

        Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, passwordBytes.Length, salt.Length);

        byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);

        return Convert.ToBase64String(hashBytes);
    }
    public static bool CheckPassword(string password, string salt, string trueHash)
    {
        return trueHash == HashPasswordWithSalt(password, salt);
    }
}