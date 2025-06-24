using ITBob_DiscordBot.Configuration;
using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;

namespace ITBob_DiscordBot;

public static class Program
{
    private static AppConfig Config;

    public static async Task Main(string[] args)
    {
        var configService = new ConfigService(new AppConfig());
        await configService.CreateConfig();
        Config = configService.Get();

        var builder = Host.CreateApplicationBuilder(args);

        builder.Services
            .AddApplicationCommands()
            .AddDiscordGateway(options =>
                {
                    options.Token = Config.DiscordBot.Token;
                    options.Presence = new PresenceProperties(Config.BotPresence.Status)
                    {
                        Activities =
                        [
                            new UserActivityProperties(Config.BotPresence.Name,
                                Config.BotPresence.Type)
                            {
                                Name = Config.BotPresence.Name,
                                Type = Config.BotPresence.Type,
                                Url = Config.BotPresence.Url,
                                ApplicationId = Config.DiscordBot.ApplicationId
                            }
                        ],
                        StatusType = Config.BotPresence.Status,
                        Afk = false
                    };
                    options.Intents = GatewayIntents.GuildMessages
                                      | GatewayIntents.DirectMessages
                                      | GatewayIntents.MessageContent
                                      | GatewayIntents.DirectMessageReactions
                                      | GatewayIntents.GuildMessageReactions;
                }
            ).AddGatewayHandlers(typeof(Program).Assembly);

        var host = builder.Build();

        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayHandlers();

        await host.RunAsync();
    }
}