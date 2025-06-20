using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace ITBob_DiscordBot.Commands;

public class PingCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("ping", "Pong!")]
    public async Task<InteractionMessageProperties> Execute()
    {
        return new InteractionMessageProperties().WithContent(
                $"OoO... Pong to {Context!.User.Username} \n-# I'm Connected with {Context!.Client.Latency.Milliseconds}ms")
            .WithFlags(MessageFlags.Ephemeral);
    }
}