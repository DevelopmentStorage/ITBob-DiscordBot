using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace ITBob_DiscordBot.Features.Verify.Interactions.Buttons;

public class VerifyStartButton : ComponentInteractionModule<ButtonInteractionContext>
{
    private readonly ConfigService ConfigService;


    public VerifyStartButton(ConfigService configService)
    {
        ConfigService = configService;
    }

    [ComponentInteraction("verify-start")]
    public async Task<InteractionCallback<ModalProperties>> Button()
    {
        var config = ConfigService.Get();
        return InteractionCallback.Modal(
            new ModalProperties("verify-start-userdata-modal", config.Messages.VerifyMessages.VerifyModalTitle)
            {
                new TextInputProperties("name", TextInputStyle.Short,
                    config.Messages.VerifyMessages.VerifyModalNameOption)
                {
                    Required = true,
                    MaxLength = 100,
                    MinLength = 2,
                },
                new TextInputProperties("class", TextInputStyle.Short,
                    config.Messages.VerifyMessages.VerifyModalClassOption)
                {
                    Required = false,
                    MaxLength = 7,
                    MinLength = 2,
                }
            });
    }
}