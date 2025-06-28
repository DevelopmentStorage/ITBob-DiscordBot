using ITBob_DiscordBot.Services;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Gateway.Voice;
using NetCord.Hosting.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Features.TempVoice.Events;

public class TempVoiceChannelCreate : IVoiceStateUpdateGatewayHandler
{
    private readonly ILogger<TempVoiceChannelCreate> Logger;
    private readonly ConfigService ConfigService;
    private readonly RestClient Client;
    private readonly GatewayClient GatewayClient;
    private readonly TempVoiceService TempVoiceService;

    public TempVoiceChannelCreate(ILogger<TempVoiceChannelCreate> logger, ConfigService configService,
        RestClient client, TempVoiceService tempVoiceService, GatewayClient gatewayClient)
    {
        ConfigService = configService;
        Logger = logger;
        Client = client;
        TempVoiceService = tempVoiceService;
        GatewayClient = gatewayClient;
    }

    public async ValueTask HandleAsync(VoiceState arg)
    {
        var guild = await Client.GetGuildAsync(arg.GuildId);
        if (guild == null)
        {
            Logger.LogError("Guild with ID {ArgGuildId} not found.", arg.GuildId);
            return;
        }

        var config = ConfigService.Get();
        if (arg.ChannelId != config.FeatureConfig.TempVoice.TempVoiceChannelId)
        {
            var allTempVoiceChannels = await TempVoiceService.PrepareTempVoiceChannelDeleteCheckerAsync();
            if (allTempVoiceChannels == null || allTempVoiceChannels.Length == 0)
            {
                Logger.LogInformation("No temporary voice channels to check for deletion.");
                return;
            }

            if (!GatewayClient.Cache.Guilds.TryGetValue(arg.GuildId, out var voiceGuild) || voiceGuild == null)
            {
                Logger.LogWarning("Guild not found in cache for ID {GuildId}.", arg.GuildId);
                return;
            }

            foreach (var tempVoiceChannel in allTempVoiceChannels)
            {
                var channelVoiceStates = voiceGuild.VoiceStates.Values
                    .Where(s => s.ChannelId.GetValueOrDefault() == tempVoiceChannel.ChannelId);


                if (channelVoiceStates.Any()) continue;
                var pendingChannelDeletion =
                    (await guild.GetChannelsAsync()).FirstOrDefault(guildChannel =>
                        guildChannel.Id == tempVoiceChannel.ChannelId) as VoiceGuildChannel;
                if (pendingChannelDeletion == null)
                {
                    await TempVoiceService.DeleteTempVoiceChannelAsync(tempVoiceChannel.ChannelId);
                    continue;
                }

                await pendingChannelDeletion.DeleteAsync();
                await TempVoiceService.DeleteTempVoiceChannelAsync(tempVoiceChannel.ChannelId);
                Logger.LogInformation("Temporary voice channel with ID {ChannelId} deleted for user {UserId}.",
                    tempVoiceChannel.ChannelId, tempVoiceChannel.UserId);
            }

            return;
        }

        if (arg.ChannelId is null)
        {
            Logger.LogWarning("VoiceStateUpdate received without ChannelId.");
            return;
        }


        var categoryId = config.FeatureConfig.TempVoice.TempVoiceCategoryId;


        var allowedPermissions = config.FeatureConfig.TempVoice.TempVoiceChannelAllowedPermissions
            .Aggregate(Permissions.Connect, (current, p) => current | p);

        var deniedPermissions = config.FeatureConfig.TempVoice.TempVoiceChannelDeniedPermissions
            .Aggregate(Permissions.Connect, (current, p) => current | p);

        var randomChannelNames = config.FeatureConfig.TempVoice.TempVoiceChannelNames;


        var channelName = "";
        var random = new Random();
        if (string.IsNullOrEmpty(channelName))
        {
            channelName = randomChannelNames[random.Next(randomChannelNames.Length)];
        }


        var channel = await guild.CreateChannelAsync(
            new GuildChannelProperties(
                channelName,
                ChannelType.VoiceGuildChannel)
            {
                Name = channelName,
                Type = ChannelType.VoiceGuildChannel,
                ParentId = categoryId,
                PermissionOverwrites = new[]
                {
                    new PermissionOverwriteProperties(arg.UserId, PermissionOverwriteType.User)
                    {
                        Allowed = allowedPermissions,
                        Denied = deniedPermissions
                    }
                }
            }
        );

        var user = await guild.GetUserAsync(arg.UserId);

        await TempVoiceService.SetupTempVoiceChannel(channel.Id, arg.GuildId, arg.UserId);
        if (user == null)
        {
            Logger.LogError($"User with ID {arg.UserId} not found in guild {arg.GuildId}.");
            return;
        }

        await user.ModifyAsync(x => x.ChannelId = channel.Id);

        Logger.LogInformation($"Temporary voice channel '{channelName}' created for user {arg.UserId}.");
    }
}