namespace ITBob_DiscordBot.Database.Entitys;

public class ReactionRolesEntity
{
    public int Id { get; set; }
    public required ulong GuildId { get; set; }
    public required string GameName { get; set; }
    public ulong RoleId { get; set; }
    public required ulong ReactionMessageId { get; set; }
    public required ulong ReactionChannelId { get; set; }
    public ulong ForumThreadId { get; set; }
    public required bool IsApproved { get; set; }
    public required ulong CreatorId { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}