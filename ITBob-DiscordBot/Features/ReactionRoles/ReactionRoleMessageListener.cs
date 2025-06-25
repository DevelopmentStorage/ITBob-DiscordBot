using ITBob_DiscordBot.Configuration;
using ITBob_DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Features.ReactionRoles;

public class ReactionRoleMessageListener : IMessageCreateGatewayHandler
{
    private readonly ILogger<ReactionRoleMessageListener> Logger;
    private readonly ConfigService CongigService;
    private readonly IServiceScopeFactory ServiceScopeFactory;

    public ReactionRoleMessageListener(ILogger<ReactionRoleMessageListener> logger, ConfigService configService,
        IServiceScopeFactory scopeFactory)
    {
        Logger = logger;
        CongigService = configService;
        ServiceScopeFactory = scopeFactory;
    }

    public async ValueTask HandleAsync(Message message)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var reactionRoleService = scope.ServiceProvider.GetRequiredService<ReactionRoleService>();
        var restClient = scope.ServiceProvider.GetRequiredService<RestClient>();
        var config = CongigService.Get();

        if (message.ChannelId != config.FeatureConfig.ReactionRoles.RoleCreationChannelId) return;
        if (message.Author.IsBot) return;

        if (message.Content.Length < 2)
        {
            await message.ReplyAsync(config.Messages.ReactionRoleRoleNameToShort);
            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        await reactionRoleService.CreateReactionRoleAsync(message.Content, (ulong)message.GuildId, message.Id,
            message.ChannelId, message.Author.Id);
        await HandleConfirmMessage(message, config);

        // Admin Message


        var channels = await restClient.GetGuildChannelsAsync((ulong)message.GuildId);

        var adminChannel = channels.FirstOrDefault(channel =>
            channel.Id == config.FeatureConfig.ReactionRoles.AdminRoleApproveChannelId);

        if (adminChannel == null) return;

        if (adminChannel is TextGuildChannel textChannel)
        {
            await textChannel.SendMessageAsync(new MessageProperties()
            {
                Components =
                [
                    new ComponentContainerProperties()
                    {
                        new TextDisplayProperties("adsfdsf"),
                        new ActionRowProperties
                        {
                            new ButtonProperties("FSD22", "adsf", ButtonStyle.Danger)
                            {
                                Disabled = false,
                                Emoji = new EmojiProperties("🎮"),
                                Id = 2
                            }
                        }
                    }
                ],
                Flags = MessageFlags.IsComponentsV2
            });
        }
        else
        {
            Logger.LogWarning("I cant send a Message in the Admin Channel {AdminChannelName}", adminChannel?.Name);
        }

        return;
    }

    private async Task HandleConfirmMessage(Message message, AppConfig config)
    {
        var msg = await message.ReplyAsync(config.FeatureConfig.ReactionRoles.UserInfoMessageAfterCreate);

        _ = Task.Run(async () =>
        {
            await Task.Delay(
                TimeSpan.FromSeconds(config.FeatureConfig.ReactionRoles.AutoDeleteUserInfoMessageAfterCreate));
            await msg?.DeleteAsync();
        });
    }
}