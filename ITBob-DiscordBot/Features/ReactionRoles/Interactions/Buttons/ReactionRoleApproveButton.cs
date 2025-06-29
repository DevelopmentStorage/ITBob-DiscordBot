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

        var reactionRole = await ReactionRoleService.GetReactionRoleByMessageIdAsync(messageId);
        await Context.Interaction.Message.DeleteAsync();
        await Context.Channel.SendMessageAsync(
            new MessageProperties
            {
                Components =
                [
                    new ComponentContainerProperties
                    {
                        new TextDisplayProperties(
                            config.Messages.ReactionRoleMessages.AdminReactionRoleLog.Replace("{0}",
                                    Context.User.Id.ToString())
                                .Replace("{1}", reactionRole?.GameName ?? "Unbekannt")
                                .Replace("{2}",
                                    $"https://discord.com/channels/{guildId}/{reactionRole?.ForumThreadId ?? 0}"))
                    }
                ],
                Flags = MessageFlags.IsComponentsV2
            }
        );

        return new InteractionMessageProperties
        {
            Content = config.Messages.ReactionRoleMessages.ReactionRoleSuccessfullyCreated,
            Flags = MessageFlags.Ephemeral
        };
    }
}