using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApproveAddLayer8 : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ILogger<VerifyApproveAddLayer8> Logger;

    public VerifyApproveAddLayer8(ILogger<VerifyApproveAddLayer8> logger)
    {
        Logger = logger;
    }

    [ComponentInteraction("verify-approve-add-layer8")]
    public async Task<InteractionCallbackResponse?> Button(ulong userId, string name, string className)
    {
        var guild = await Context.Client.Rest.GetGuildAsync((ulong)Context.Interaction.GuildId);
        if (guild is null)
        {
            Logger.LogWarning("Guild not found for interaction {InteractionId}", Context.Interaction.Id);
            return await RespondAsync(InteractionCallback.ModifyMessage(options =>
            {
                options.Content = "No Guild Found";
                options.Components = [];
            }));
        }

        var member = await guild.GetUserAsync(userId);

        await member.ModifyAsync(options =>
            options.Nickname = $"{name}");

        // TODO: Add Config Layber8 Role here and add Logic for adding.
        
        return await RespondAsync(InteractionCallback.ModifyMessage(options =>
        {
            options.Content = "Done";
            options.Components = [];
        }));
    }
}