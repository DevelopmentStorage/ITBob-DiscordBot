namespace ITBob_DiscordBot.Database.Entitys;

public class TempVoiceEntity
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong GuildId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}