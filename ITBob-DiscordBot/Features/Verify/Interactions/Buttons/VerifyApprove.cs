using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApprove : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("verify-approve")]
    public async Task<InteractionMessageProperties> Button(ulong userId)
    {
        return new InteractionMessageProperties
        {
            Components =
            [
                new ComponentContainerProperties
                {
                    new TextDisplayProperties("Selcet a role for the user:"),
                    new RoleMenuProperties("verify-approve-role-select:" + userId)
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