using ITBob_DiscordBot.Database;
using ITBob_DiscordBot.Database.Entitys;
using Microsoft.EntityFrameworkCore;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Services;

public class TempVoiceService
{
    public readonly DatabaseContext DatabaseContext;

    public TempVoiceService(DatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
    }

    public async Task<TempVoiceEntity[]> PrepareTempVoiceChannelDeleteCheckerAsync()
    {
        var tempVoiceChannels = await DatabaseContext.TempVoiceChannels
            .ToArrayAsync();

        return tempVoiceChannels.Length == 0 ? null : tempVoiceChannels;
    }

    public async Task<bool> DeleteTempVoiceChannelAsync(ulong channelId)
    {
        var tempVoiceChannel = await DatabaseContext.TempVoiceChannels
            .FirstOrDefaultAsync(x => x.ChannelId == channelId);

        if (tempVoiceChannel == null)
            return false;

        DatabaseContext.TempVoiceChannels.Remove(tempVoiceChannel);
        await DatabaseContext.SaveChangesAsync();

        return true;
    }

    public async Task SetupTempVoiceChannel(ulong channelId, ulong guildId, ulong userId)
    {
        await DatabaseContext.TempVoiceChannels.AddAsync(
            new TempVoiceEntity()
            {
                ChannelId = channelId,
                GuildId = guildId,
                UserId = userId
            }
        );
        await DatabaseContext.SaveChangesAsync();
    }
}