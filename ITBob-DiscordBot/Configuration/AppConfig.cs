using NetCord;
using NetCord.Gateway;

namespace ITBob_DiscordBot.Configuration;

public class AppConfig
{
    public DiscordBot DiscordBot { get; set; } = new();
    public Presence BotPresence { get; set; } = new();
    public FeatureConfig FeatureConfig { get; set; } = new();
    public Messages Messages { get; set; } = new();
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

public class FeatureConfig
{
    public ReactionRoles ReactionRoles { get; set; } = new();
}

public class ReactionRoles
{
    public ulong ForumChannelId { get; set; }
    public ulong RoleCreationChannelId { get; set; }
    public bool AllowOtherToPostInRoleSpecifyChannel { get; set; }
    public ulong AdminRoleApproveChannelId { get; set; }

    public string UserInfoMessageAfterCreate { get; set; } =
        "Deine Roe wurde erstellt! Ein Admin wird diese rolle nun bestätigen.\n-# (Autodelete in 10 sekunden)";

    public int AutoDeleteUserInfoMessageAfterCreate { get; set; } = 10;
    public string ReactionEmoji { get; set; } = "🎮";
}