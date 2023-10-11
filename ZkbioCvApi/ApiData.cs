using ZKBioClient;

namespace ZkbioCvApi;

public class ApiData : IApiClientData
{
    public ApiData(IConfiguration configuration)
    {
        this.Username = configuration["api:login"];
        this.Password = configuration["api:password"];
    }

    public string Username
    {
        get;
        set;
    }

    public string Password
    {
        get;
        set;
    }

    public string SessionId { get; set; }

    public string PasswordHash { get; set; }
}

public class ApiClientProvider
{
    private readonly IApiClientData data;
    private readonly IConfiguration configuration;

    public ApiClientProvider(IApiClientData data, IConfiguration configuration)
    {
        this.data = data;
        this.configuration = configuration;
    }

    public ApiClient Get()
    {
        var client =  new ApiClient(this.configuration["api:url"], this.data);
        client.LoginAsync(this.data.Username, this.data.Password).GetAwaiter().GetResult();
        return client;
    }
}