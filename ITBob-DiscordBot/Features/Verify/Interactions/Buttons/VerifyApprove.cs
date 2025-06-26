using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApprove : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("verify-approve")]
    public async Task<InteractionMessageProperties> Button(ulong userId, string name, string className)
    {
        return new InteractionMessageProperties
        {
            Components =
            [
                new ActionRowProperties
                {
                    new ButtonProperties("verify-approve-add-layer8:" + userId + ":" + name + ":" + className,
                        "Verify als Layer8",
                        ButtonStyle.Secondary),
                    new ButtonProperties("verify-approve-add-fachkraft:" + userId + ":" + name + ":" + className,
                        "Verify als Fachkraft",
                        ButtonStyle.Secondary),
                    new ButtonProperties("verify-deny:" + Context.Message.Id, "Deny",
                        ButtonStyle.Danger)
                },
                new ComponentContainerProperties
                {
                    new TextDisplayProperties("Selcet a role for the user:"),
                    new RoleMenuProperties("verify-approve-role-select:" + userId + ":" + name + ":" + className)
                    {
                        Id = 2,
                        Placeholder = "Select a role",
                        MinValues = 1,
                        MaxValues = 1
                    }
                }
            ],
            Flags = MessageFlags.IsComponentsV2 | MessageFlags.Ephemeral,
        };
    }
}