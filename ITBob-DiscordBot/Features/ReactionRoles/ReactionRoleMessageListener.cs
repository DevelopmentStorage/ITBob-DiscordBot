using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace ITBob_DiscordBot.Features.ReactionRoles;

public class ReactionRoleMessageListener(ILogger<ReactionRoleMessageListener> logger) : IMessageCreateGatewayHandler
{
    public ValueTask HandleAsync(Message message)
    {
        logger.LogInformation("{}", message.Content);
        return default;
    }
}