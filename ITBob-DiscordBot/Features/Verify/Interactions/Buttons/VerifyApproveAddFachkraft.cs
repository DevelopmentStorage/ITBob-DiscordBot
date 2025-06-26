using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApproveAddFachkraft : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ILogger<VerifyApproveAddFachkraft> Logger;

    public VerifyApproveAddFachkraft(ILogger<VerifyApproveAddFachkraft> logger)
    {
        Logger = logger;
    }

    [ComponentInteraction("verify-approve-add-fachkraft")]
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
            options.Nickname = $"{name} - {className}");

        // TODO: Add Config Layber8 Role here and add Logic for adding.

        return await RespondAsync(InteractionCallback.ModifyMessage(options =>
        {
            options.Content = "Done";
            options.Components = [];
        }));
    }
}