using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;

// Global namespace

public class AppUser
{
    public int id = 0;
    public string SubjectId = string.Empty;
    public string Username = string.Empty;
    public string PasswordSalt = string.Empty;
    public string PasswordHash = string.Empty;
    public string ProviderName = string.Empty;
    public string ProviderSubjectId = string.Empty;

    public List<Claim> Claims = new List<Claim>();

    public static string PasswordSaltInBase64()
    {
        var salt = new byte[32]; // 256 bits
        using(var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    public static string PasswordToHashBase64(string plaintextPassword, string storedPasswordSaltBase64)
    {
        var salt = Convert.FromBase64String(storedPasswordSaltBase64);
        var bytearray = KeyDerivation.Pbkdf2(plaintextPassword, salt, KeyDerivationPrf.HMACSHA512, 50000, 24);
        return Convert.ToBase64String(bytearray);
    }

    public static bool PasswordValidation(string storedPasswordHashBase64, string storedPasswordSaltBase64, string plaintextToValidate)
    {
        return storedPasswordHashBase64.Equals(PasswordToHashBase64(plaintextToValidate, storedPasswordSaltBase64));
    }
}
