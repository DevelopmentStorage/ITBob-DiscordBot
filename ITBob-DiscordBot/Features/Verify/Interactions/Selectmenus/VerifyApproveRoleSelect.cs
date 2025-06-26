using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Selectmenus;

public class VerifyApproveRoleSelect : ComponentInteractionModule<RoleMenuInteractionContext>
{
    private readonly ILogger<VerifyApproveRoleSelect> Logger;
    private readonly VerifyService VerfyService;
    private readonly ConfigService ConfigService;

    public VerifyApproveRoleSelect(ILogger<VerifyApproveRoleSelect> logger, VerifyService verfyService,
        ConfigService configService)
    {
        ConfigService = configService;
        VerfyService = verfyService;
        Logger = logger;
    }

    [ComponentInteraction("verify-approve-role-select")]
    public async Task<InteractionMessageProperties> SelectMenu(ulong userId, string name, string className)
    {
        try
        {
            var role = Context.SelectedRoles[0];
            var guild = await Context.Client.Rest.GetGuildAsync((ulong)Context.Interaction.GuildId);
            if (guild is null)
            {
                Logger.LogWarning("Guild not found for interaction {InteractionId}", Context.Interaction.Id);
                return new InteractionMessageProperties
                {
                    Content = "Guild not found!",
                    Flags = MessageFlags.Ephemeral
                };
            }

            var member = await guild.GetUserAsync(userId);

            if (member.RoleIds.Contains(role.Id))
            {
                return new InteractionMessageProperties
                {
                    Content = "Role already assigned to user!",
                    Flags = MessageFlags.Ephemeral
                };
            }

            await member.AddRoleAsync(role.Id);

            await VerfyService.SendVerifyLogMessage(
                (TextChannel)(await guild.GetChannelsAsync()).FirstOrDefault(channel => channel.Id == ConfigService
                    .Get().FeatureConfig.Verify
                    .AdminVerifyChannelId
                ),
                role, userId, Context.Interaction.User.Id);

            return new InteractionMessageProperties
            {
                Content = "Role updated successfully!",
                Flags = MessageFlags.Ephemeral
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while processing role select interaction for user {UserId}", userId);
            return new InteractionMessageProperties
            {
                Content = "An error occurred while processing your request. Please try again later.",
                Flags = MessageFlags.Ephemeral
            };
        }
    }
}