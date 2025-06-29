using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApproveAddLayer8 : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ILogger<VerifyApproveAddLayer8> Logger;
    private readonly ConfigService ConfigService;
    private readonly VerifyService VerifyService;

    public VerifyApproveAddLayer8(ILogger<VerifyApproveAddLayer8> logger, ConfigService configService,
        VerifyService verifyService)
    {
        VerifyService = verifyService;
        ConfigService = configService;
        Logger = logger;
    }

    [ComponentInteraction("verify-approve-add-layer8")]
    public async Task<InteractionMessageProperties?> Button(ulong userId, string name, string className,
        ulong interactionMessageId)
    {
        var guild = await Context.Client.Rest.GetGuildAsync((ulong)Context.Interaction.GuildId);
        if (guild is null)
        {
            Logger.LogWarning("Guild not found for interaction {InteractionId}", Context.Interaction.Id);
            return new InteractionMessageProperties
            {
                Content = "Guild not found. Please try again later.",
                Flags = MessageFlags.Ephemeral
            };
        }

        var member = await guild.GetUserAsync(userId);

        await member.ModifyAsync(options =>
            options.Nickname = $"{name}");

        var role = await guild.GetRoleAsync(ConfigService.Get().FeatureConfig.Verify.Layer8RoleId);

        if (role == null)
            return new InteractionMessageProperties
            {
                Content = "Layer8 role not found. Please contact an admin.",
                Flags = MessageFlags.Ephemeral
            };

        if (member.RoleIds.Contains(role.Id))
            return new InteractionMessageProperties
            {
                Content = "User already has the Layer8 role.",
                Flags = MessageFlags.Ephemeral
            };

        await member.AddRoleAsync(role.Id);
        Logger.LogInformation("Added Layer8 role to user {UserId} ({UserName})", userId, member.Username);

        VerifyService.SendVerifyLogMessage(
            (TextChannel)(await guild.GetChannelsAsync()).FirstOrDefault(channel => channel.Id == ConfigService
                .Get().FeatureConfig.Verify
                .AdminVerifyChannelId
            ),
            role, userId, Context.Interaction.User.Id);

        var interactionMessage = await Context.Channel.GetMessageAsync(interactionMessageId);
        if (interactionMessage != null)
        {
            await interactionMessage.DeleteAsync();
        }
        else
        {
            Logger.LogWarning("Interaction message with ID {MessageId} not found.", interactionMessageId);
        }


        return new InteractionMessageProperties
        {
            Content = "Done",
            Flags = MessageFlags.Ephemeral
        };
    }
}