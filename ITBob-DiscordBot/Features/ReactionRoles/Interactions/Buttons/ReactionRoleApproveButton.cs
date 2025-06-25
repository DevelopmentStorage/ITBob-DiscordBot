using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.ReactionRoles.Interactions.Buttons;

public class ReactionRoleApproveButton : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ReactionRoleService ReactionRoleService;
    private readonly ConfigService ConfigService;
    private readonly RestClient Client;


    public ReactionRoleApproveButton(ReactionRoleService reactionRoleService, ConfigService configService,
        RestClient client)
    {
        Client = client;
        ReactionRoleService = reactionRoleService;
        ConfigService = configService;
    }

    [ComponentInteraction("reactionrole-approve")]
    public async Task<InteractionMessageProperties> Button(ulong messageId, ulong guildId)
    {
        var config = ConfigService.Get();

        var guild = await Client.GetGuildAsync(guildId);
        if (guild == null)
            return new InteractionMessageProperties()
            {
                Content = config.Messages.ReactionRoleMessages.Errors.ForumThreadCreatedFailed,
                Flags = MessageFlags.Ephemeral
            };

        var request = await ReactionRoleService.ApproveReactionRoleAsync(
            messageId,
            guild
        );

        if (!request)
        {
            return new InteractionMessageProperties
            {
                Content = config.Messages.ReactionRoleMessages.Errors.ReactionRoleCreatedFailed,
                Flags = MessageFlags.Ephemeral
            };
        }

        return new InteractionMessageProperties
        {
            Content = config.Messages.ReactionRoleMessages.ReactionRoleSuccessfullyCreated,
            Flags = MessageFlags.Ephemeral
        };
    }
}