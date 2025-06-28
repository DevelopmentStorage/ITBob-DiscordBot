using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace ITBob_DiscordBot.Commands;

public class VerifyMessageCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly ConfigService ConfigService;

    public VerifyMessageCommand(ConfigService configService)
    {
        ConfigService = configService;
    }

    [SlashCommand("verifystartmessage", "[Admin] Send the verify start message to the user.")]
    [RequireUserPermissions<ApplicationCommandContext>(Permissions.Administrator)]
    [RequireContext<ApplicationCommandContext>(RequiredContext.Guild)]
    public async Task<InteractionMessageProperties> Execute()
    {
        await Context.Channel.SendMessageAsync(new MessageProperties
        {
            Components =
            [
                new ComponentContainerProperties
                {
                    new TextDisplayProperties(ConfigService.Get().Messages.VerifyMessages.VerifyStartMessage),
                    new ActionRowProperties
                    {
                        new ButtonProperties("verify-start", new EmojiProperties(1288526802024792145),
                            ButtonStyle.Secondary)
                        {
                            Id = 2,
                            Emoji = new EmojiProperties(1288663952506621995),
                            Style = ButtonStyle.Primary,
                            CustomId = "verify-start",
                        }
                    }
                }
            ],
            Flags = MessageFlags.IsComponentsV2
        });

        return new InteractionMessageProperties().WithContent("Sent the verify start message successfully!")
            .WithFlags(MessageFlags.Ephemeral);
    }
}