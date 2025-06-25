using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.ReactionRoles.Interactions.Buttons;

public class ReactionRoleDenyButton : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ConfigService ConfigService;
    private readonly ILogger<ReactionRoleDenyButton> Logger;
    private readonly ReactionRoleService ReactionRoleService;

    public ReactionRoleDenyButton(ConfigService configService, ILogger<ReactionRoleDenyButton> logger,
        ReactionRoleService reactionRoleService)
    {
        Logger = logger;
        ConfigService = configService;
        ReactionRoleService = reactionRoleService;
    }

    [ComponentInteraction("reactionrole-deny")]
    public async Task<InteractionMessageProperties> Button(ulong messageId)
    {
        var config = ConfigService.Get();

        var delete = await ReactionRoleService.DeleteReactionRoleByMessageIdAsync(messageId);

        if (delete > 0)
        {
            var adminMessage = await Context.Channel.GetMessageAsync(delete);
            await adminMessage.DeleteAsync();

            return new InteractionMessageProperties
            {
                Content = config.Messages.ReactionRoleMessages.Errors.AdminReactionRoleDeny,
                Flags = MessageFlags.Ephemeral
            };
        }


        Logger.LogError("Failed to delete reaction role with message ID {MessageId}.", messageId);
        return new InteractionMessageProperties
        {
            Content = config.Messages.ReactionRoleMessages.Errors.AdminReactionRoleDenyFailed,
            Flags = MessageFlags.Ephemeral
        };
    }
}