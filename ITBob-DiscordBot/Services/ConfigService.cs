using System.Text.Json;
using ITBob_DiscordBot.Configuration;
using NetCord;
using NetCord.Gateway;

namespace ITBob_DiscordBot.Services;

public class ConfigService
{
    private AppConfig Config { get; set; }

    public ConfigService()
    {
    }

    public async Task CreateConfig()
    {
        var configFile = File.Exists("config.json");
        if (!configFile)
        {
            var defaultConfig = JsonSerializer.Serialize(new AppConfig
            {
                DiscordBot =
                {
                    ApplicationId = 00000000000,
                    Token = "",
                },
                BotPresence =
                {
                    Name = "Arbeitet gern für seinen Konzern.",
                    Status = UserStatusType.DoNotDisturb,
                    Type = UserActivityType.Custom, Url = ""
                }
            }, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            });
            await File.WriteAllTextAsync("config.json", defaultConfig);
        }

        var jsontext = await File.ReadAllTextAsync("config.json");
        Config = JsonSerializer.Deserialize<AppConfig>(jsontext)!;
    }


    public AppConfig Get() => Config;
}