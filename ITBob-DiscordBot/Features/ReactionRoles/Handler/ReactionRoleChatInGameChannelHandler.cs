using ITBob_DiscordBot.Configuration;
using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Features.ReactionRoles.Handler;

public class ReactionRoleChatInGameChannelHandler
{
    public async Task Handle(Message message, AppConfig config, RestClient client, ILogger logger,
        ReactionRoleService reactionRoleService)
    {
        var reactionRole = await reactionRoleService.GetReactionRoleByMessageIdAsync(message.Id);
        var isSendMessageFromNotGameUsersAllowed =
            config.FeatureConfig.ReactionRoles.AllowOtherToPostInRoleSpecifyChannel;
        var member = message.Author as GuildUser;
        if (member == null)
        {
            logger.LogWarning("ReactionRoleChatInGameChannelHandler: Message author is not a guild user");
            return;
        }

        if (message.GuildId is 0 or null)
        {
            logger.LogWarning("ReactionRoleChatInGameChannelHandler: Message is not from a guild");
            return;
        }

        var guild = await client.GetGuildAsync((ulong)message.GuildId);
        var guildChannels = await guild.GetActiveThreadsAsync();
        var forumThreadChannel =
            guildChannels.FirstOrDefault(guildChannel => guildChannel.Id == message.ChannelId);

        if (isSendMessageFromNotGameUsersAllowed ||
            member.GetRoles(guild).Any(r => r.Id == reactionRole?.RoleId)) return;

        var errorMessage =
            await forumThreadChannel?.SendMessageAsync(config.Messages.ReactionRoleMessages.IsNotAllowedToSendMessagesInGameThread);

        _ = Task.Run(async () =>
        {
            await Task.Delay(5000);
            await message.DeleteAsync();
            await errorMessage.DeleteAsync();
        });
        return;
    }
}