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

        if (arg.Emoji.Name == config.FeatureConfig.ReactionRoles.ReactionEmoji)
        {
            var reactionRole = await reactionRoleService.GetReactionRoleByMessageIdAsync(arg.MessageId);


            if (arg.ChannelId != config.FeatureConfig.ReactionRoles.RoleCreationChannelId) return;
            var reactionChannel = guildChannels.FirstOrDefault(c => c.Id == arg.ChannelId) as TextChannel;
            if (reactionChannel == null)
            {
                Logger.LogWarning("Reaction channel with ID {ChannelId} not found", arg.ChannelId);
                return;
            }

            if (reactionRole == null)
            {
                Logger.LogWarning("Reaction role not found for message ID {MessageId}", arg.MessageId);
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

            await member.AddRoleAsync(reactionRole.RoleId);
            Logger.LogInformation(
                "Role {RoleName} added to user {UserId} in guild {GuildId} for reaction role {ReactionRoleId}",
                role.Name, arg.UserId, arg.GuildId, reactionRole.Id);
        }
        else if (arg.Emoji.Name == config.FeatureConfig.ReactionRoles.AdminDeleteReactionEmoji)
        {
            var reactionRole = await reactionRoleService.GetReactionRoleByMessageIdAsync(arg.MessageId);

            if (reactionRole == null)
            {
                Logger.LogWarning("Reaction role not found for message ID {MessageId}", arg.MessageId);
                return;
            }

            var channel = await client.GetChannelAsync(arg.ChannelId) as TextChannel;
            if (channel == null)
            {
                Logger.LogWarning("Channel with ID {ChannelId} not found", arg.ChannelId);
                return;
            }

            var member = await guild.GetUserAsync(arg.UserId);
            if (!member.RoleIds.Contains(config.FeatureConfig.ReactionRoles.AdminDeleteReactionRoleId))
            {
                var error = await channel.SendMessageAsync(new MessageProperties
                {
                    Content = "You do not have the required role to delete reaction roles.",
                });

                await Task.Delay(2000);
                await error.DeleteAsync();

                Logger.LogWarning("User {UserId} does not have the required role to delete reaction roles",
                    arg.UserId);
                return;
            }


            var message = await channel.GetMessageAsync(arg.MessageId);

            if (message == null)
            {
                Logger.LogWarning("Message with ID {MessageId} not found in channel {ChannelId}", arg.MessageId,
                    arg.ChannelId);
                return;
            }

            var threadChannel = await client.GetChannelAsync(reactionRole.ForumThreadId);
            if (threadChannel != null) await threadChannel.DeleteAsync();

            var adminChannel =
                await client.GetChannelAsync(config.FeatureConfig.ReactionRoles.AdminRoleApproveChannelId) as
                    TextChannel;
            if (adminChannel != null)
            {
                // var adminMessage = await adminChannel.GetMessageAsync(reactionRole.AdminMessageId);
                // if (adminMessage != null) await adminMessage.DeleteAsync();

                await adminChannel.SendMessageAsync(new MessageProperties
                {
                    Content =
                        config.Messages.ReactionRoleMessages.ReactionRoleDeleted.Replace("{0}", reactionRole.GameName),
                });
            }

            var reactionRoleGuildRole = await guild.GetRoleAsync(reactionRole.RoleId);
            if (reactionRoleGuildRole != null)
            {
                await reactionRoleGuildRole.DeleteAsync();
            }

            await reactionRoleService.DeleteReactionRoleByMessageIdAsync(arg.MessageId);
            await message.DeleteAsync();

            var info = await channel.SendMessageAsync(new MessageProperties
            {
                Content = "Reaction role message deleted successfully.",
            });
            await Task.Delay(2000);
            await info.DeleteAsync();
        }
        else
        {
            var reactionMessage = await client.GetMessageAsync(arg.ChannelId, arg.MessageId);
            if (reactionMessage == null)
            {
                Logger.LogWarning("Reaction message with ID {MessageId} not found in channel {ChannelId}",
                    arg.MessageId, arg.ChannelId);
                return;
            }

            await reactionMessage.DeleteAllReactionsAsync();
        }
    }
}