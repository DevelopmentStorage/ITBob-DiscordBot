using ITBob_DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Features.ReactionRoles.Events;

public class ReactionRoleReactionAdd : IMessageReactionAddGatewayHandler
{
    private readonly ILogger<ReactionRoleReactionAdd> Logger;
    private readonly ConfigService CongigService;
    private readonly IServiceScopeFactory ServiceScopeFactory;

    public ReactionRoleReactionAdd(ILogger<ReactionRoleReactionAdd> logger, IServiceScopeFactory serviceScopeFactory,
        ConfigService configService)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Logger = logger;
        CongigService = configService;
    }

    public async ValueTask HandleAsync(MessageReactionAddEventArgs arg)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var reactionRoleService = scope.ServiceProvider.GetRequiredService<ReactionRoleService>();
        var config = CongigService.Get();
        var client = scope.ServiceProvider.GetRequiredService<RestClient>();

        var guild = await client.GetGuildAsync((ulong)arg.GuildId);
        var guildChannels = await guild.GetChannelsAsync();
        var reactionRole = await reactionRoleService.GetReactionRoleByMessageIdAsync(arg.MessageId);

        if (reactionRole == null)
        {
            Logger.LogWarning("Reaction role not found for message ID {MessageId}", arg.MessageId);
            return;
        }

        if (arg.ChannelId != config.FeatureConfig.ReactionRoles.RoleCreationChannelId) return;
        var reactionChannel = guildChannels.FirstOrDefault(c => c.Id == arg.ChannelId) as TextChannel;
        if (reactionChannel == null)
        {
            Logger.LogWarning("Reaction channel with ID {ChannelId} not found", arg.ChannelId);
            return;
        }

        var role = await guild.GetRoleAsync(reactionRole.RoleId);
        var member = await guild.GetUserAsync(arg.UserId);

        if (!reactionRole.IsApproved)
        {
            Logger.LogWarning("Reaction role with message ID {MessageId} is not approved", arg.MessageId);
            return;
        }

        if (reactionRole.RoleId == 0)
        {
            Logger.LogWarning("Reaction role with message ID {MessageId} has no role assigned", arg.MessageId);
            return;
        }


        if (arg.Emoji.Name != config.FeatureConfig.ReactionRoles.ReactionEmoji)
        {
            Logger.LogWarning("Reaction emoji {Emoji} does not match configured emoji {ConfiguredEmoji}",
                arg.Emoji.Name, config.FeatureConfig.ReactionRoles.ReactionEmoji);
            return;
        }

        await member.AddRoleAsync(reactionRole.RoleId);
        Logger.LogInformation(
            "Role {RoleName} added to user {UserId} in guild {GuildId} for reaction role {ReactionRoleId}",
            role.Name, arg.UserId, arg.GuildId, reactionRole.Id);
    }
}