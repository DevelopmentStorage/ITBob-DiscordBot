using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyApproveAddFachkraft : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ILogger<VerifyApproveAddFachkraft> Logger;
    private readonly ConfigService ConfigService;
    private readonly VerifyService VerifyService;

    public VerifyApproveAddFachkraft(ILogger<VerifyApproveAddFachkraft> logger, ConfigService configService,
        VerifyService verifyService)
    {
        Logger = logger;
        ConfigService = configService;
        VerifyService = verifyService;
    }

    [ComponentInteraction("verify-approve-add-fachkraft")]
    public async Task<InteractionMessageProperties> Button(ulong userId, string name, string className)
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
            options.Nickname = $"{name} - {className}");

        var role = await guild.GetRoleAsync(ConfigService.Get().FeatureConfig.Verify.FachkraftRoleId);

        if (role == null)
            return new InteractionMessageProperties
            {
                Content = "Fachkraft role not found. Please contact an admin.",
                Flags = MessageFlags.Ephemeral
            };

        if (member.RoleIds.Contains(role.Id))
            return new InteractionMessageProperties
            {
                Content = "User already has the Fachkraft role.",
                Flags = MessageFlags.Ephemeral
            };

        await member.AddRoleAsync(role.Id);

        VerifyService.SendVerifyLogMessage(
            (TextChannel)(await guild.GetChannelsAsync()).FirstOrDefault(channel => channel.Id == ConfigService
                .Get().FeatureConfig.Verify
                .AdminVerifyChannelId
            ),
            role, userId, Context.Interaction.User.Id);

        return new InteractionMessageProperties
        {
            Content = "Done",
            Components = [],
            Flags = MessageFlags.Ephemeral
        };
    }
}