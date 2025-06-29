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
                    new ButtonProperties(
                        "verify-approve-add-fachkraft:" + userId + ":" + name + ":" + className + ":" +
                        Context.Interaction.Message.Id,
                        "Verifiziren als Fachkraft",
                        ButtonStyle.Success)
                    {
                        Emoji = new EmojiProperties("🟠")
                    },
                    new ButtonProperties("verify-approve-add-layer8:" + userId + ":" + name + ":" + className + ":" +
                                         Context.Interaction.Message.Id,
                        "Verifiziren als Layer8",
                        ButtonStyle.Success)
                    {
                        Emoji = new EmojiProperties("⚫")
                    },

                    new ButtonProperties("verify-deny:" + Context.Message.Id, "Ablehnen",
                        ButtonStyle.Secondary)
                    {
                        Emoji = new EmojiProperties("❌")
                    }
                },
            ],
            Flags = MessageFlags.IsComponentsV2 | MessageFlags.Ephemeral,
        };
    }
}