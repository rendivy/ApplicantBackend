using Microsoft.Extensions.Options;

namespace AuthService.Configuration;

public class EasyNetQConfiguration
{
    public string ConnectionString { get; set; } = null;
}

public class EasyNetQConfigureOption(IConfiguration configuration) : IConfigureOptions<EasyNetQConfiguration>
{
    public void Configure(EasyNetQConfiguration options)
    {
        configuration.GetSection("EasyNetQ").Bind(options);
    }
}