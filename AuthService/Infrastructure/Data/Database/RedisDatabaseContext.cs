using StackExchange.Redis;

namespace AuthService.Infrastructure.Data.Database;

public class RedisDatabaseContext
{
    private readonly IDatabase _database;

    public RedisDatabaseContext(string connectionString)
    {
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _database = connection.GetDatabase();
    }

    public async Task DeleteRefreshToken(string token)
    {
        await _database.KeyDeleteAsync(token);
    }

    public async Task AddRefreshToken(string token, string userId)
    {
        await _database.StringSetAsync(token, userId, TimeSpan.FromDays(1));
    }

    public async Task<string?> GetUserIdByRefreshToken(string token)
    {
        return await _database.StringGetAsync(token);
    }
}