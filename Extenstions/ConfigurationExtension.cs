namespace TinyURL.Extenstions;

public static class ConfigurationExtension
{
    public static string MongoConnectionString(this IConfiguration configuration)
    {
        return configuration["MONGO_DB_CONNECTION"];
    }
    
    public static string MongoDbName(this IConfiguration configuration)
    {
        return configuration["MONGO_DB_NAME"];
    }
    
    public static string HostUrl(this IConfiguration configuration)
    {
        return configuration["HostUrl"];
    }
}