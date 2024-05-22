using System.Text;

namespace AuthService.Application.Utils;

static class PasswordGenerator
{
    private static readonly Random Random = new Random();

    public static string GeneratePassword(int length)
    {
        if (length < 4)
        {
            throw new ArgumentException("Password length must be at least 4 characters.");
        }

        const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        StringBuilder password = new StringBuilder();
        password.Append(lowerChars[Random.Next(lowerChars.Length)]);
        password.Append(upperChars[Random.Next(upperChars.Length)]);
        password.Append(digits[Random.Next(digits.Length)]);
        password.Append(specialChars[Random.Next(specialChars.Length)]);

        string allChars = lowerChars + upperChars + digits + specialChars;
        while (password.Length < length)
        {
            password.Append(allChars[Random.Next(allChars.Length)]);
        }

        return new string(password.ToString().ToCharArray().OrderBy(x => Random.Next()).ToArray());
    }
}