using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyDeny : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("verify-deny")]
    public async Task<InteractionCallbackResponse?> Button(ulong messageId)
    {
        var message = await Context.Interaction.Channel.GetMessageAsync(messageId);
        await message.DeleteAsync();
        return await RespondAsync(InteractionCallback.ModifyMessage(options =>
        {
            options.Content = "Done";
            options.Components = [];
        }));
    }
}