using NetCord;
using NetCord.Gateway;

namespace ITBob_DiscordBot.Configuration;

public class AppConfig
{
    public DiscordBot DiscordBot { get; set; } = new();
    public Presence BotPresence { get; set; } = new();
}

public class DiscordBot
{
    public string Token { get; set; }
    public ulong ApplicationId { get; set; }
    public string GuildId { get; set; }
    public string BotAdmin { get; set; }
}

public class Presence
{
    public string Name { get; set; }
    public UserStatusType Status { get; set; }
    public UserActivityType Type { get; set; }
    public string? Url { get; set; }
}