using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace ITBob_DiscordBot.Features.ReactionRoles;

public class ReactionRoleMessageListener : IMessageCreateGatewayHandler
{
    private readonly ILogger<ReactionRoleMessageListener> Logger;
    private readonly ConfigService CongigService;

    public ReactionRoleMessageListener(ILogger<ReactionRoleMessageListener> logger, ConfigService configService)
    {
        Logger = logger;
        CongigService = configService;
    }

    public async ValueTask HandleAsync(Message message)
    {
        var config = CongigService.Get();

        if (message.ChannelId != config.FeatureConfig.ReactionRoles.RoleCreationChannelId) return;
        if (message.Author.IsBot) return;


        if (message.Content.Length < 2)
        {
            await message.ReplyAsync(config.Messages.ReactionRoleRoleNameToShort);
        }


        return;
    }
}