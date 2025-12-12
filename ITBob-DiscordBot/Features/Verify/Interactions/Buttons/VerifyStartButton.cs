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
    public async Task<InteractionCallbackProperties<ModalProperties>> Button()
    {
        var config = ConfigService.Get();
        return InteractionCallback.Modal(
            new ModalProperties("verify-start-userdata-modal", config.Messages.VerifyMessages.VerifyModalTitle)
            {
                new LabelProperties(config.Messages.VerifyMessages.VerifyModalNameOption,
                    new TextInputProperties("name", TextInputStyle.Short)
                    {
                        Required = true,
                        MaxLength = 100,
                        MinLength = 2,
                    }),

                new LabelProperties(config.Messages.VerifyMessages.VerifyModalClassOption,
                    new TextInputProperties("class", TextInputStyle.Short)
                    {
                        Required = false,
                        MaxLength = 7,
                        MinLength = 2,
                    })
            });
    }
}