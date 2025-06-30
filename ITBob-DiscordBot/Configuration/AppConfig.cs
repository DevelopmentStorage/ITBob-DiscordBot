using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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
    public Verify Verify { get; set; } = new();
    public TempVoice TempVoice { get; set; } = new();
}

public class TempVoice
{
    public ulong TempVoiceChannelId { get; set; }
    public ulong TempVoiceCategoryId { get; set; }

    public string[] TempVoiceChannelNames { get; set; } = new[]
    {
        "HTTPS:443", "DNS:53", "HTTP:80", "SSH:22", "SMTP:25", "POP3:110", "IMAP:143", "RDP:3389",
        "MySQL:3306", "SMB:445", "LDAP:389", "FTPS:990", "NTP:123", "DHCP:67-68", "SNMP:161", "PostgreSQL:5432",
        "Git:9418", "Docker:2375", "SFTP:22", "LDAPS:636", "IMAPS:993", "POP3S:995", "SMTPS:465", "MQTT:1883",
        "MQTTS:8883", "SIP:5060", "SIPS:5061", "Rsync:873", "TFTP:69", "Radius:1812", "OpenVPN:1194", "IPsec:500",
        "IPsec-NAT-T:4500", "VNC:5900", "FTP:21", "FTPS:989", "Telnet:23"
    };

    public List<Permissions> TempVoiceChannelAllowedPermissions { get; set; } = new()
    {
        Permissions.CreateInstantInvite,
        Permissions.Connect,
        Permissions.Speak,
        Permissions.UseVoiceActivityDetection,
        Permissions.ViewChannel,
        Permissions.ManageChannels,
        Permissions.KickUsers,
        Permissions.MuteUsers,
        Permissions.DeafenUsers,
        Permissions.MoveUsers,
        Permissions.ManageMessages,
    };

    public List<Permissions> TempVoiceChannelDeniedPermissions { get; set; } = new()
    {
        Permissions.ManageRoles,
        Permissions.ManageWebhooks,
    };
}

public class Verify
{
    public ulong AdminVerifyChannelId { get; set; }
    public ulong Layer8RoleId { get; set; }
    public ulong FachkraftRoleId { get; set; }
}

public class ReactionRoles
{
    public ulong ForumChannelId { get; set; }

    public ulong RoleCreationChannelId { get; set; }

    public bool AllowOtherToPostInRoleSpecifyChannel { get; set; }
    public ulong AdminRoleApproveChannelId { get; set; }

    public string UserInfoMessageAfterCreate { get; set; } =
        "Deine Rolle wurde erstellt! Ein Admin wird diese rolle nun bestätigen.\n-# (Autodelete in 10 sekunden)";

    public int AutoDeleteUserInfoMessageAfterCreate { get; set; } = 10;
    public string ReactionEmoji { get; set; } = "👍";
    public string AdminDeleteReactionEmoji { get; set; } = "🗑️";
    public ulong AdminDeleteReactionRoleId { get; set; } = 1091020350889918496;
}