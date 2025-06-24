namespace ITBob_DiscordBot.Database.Entitys;

public class ReactionRolesEntity
{
    public int Id { get; set; }
    public required string GuildId { get; set; }
    public required string RoleId { get; set; }
    public required string ReactionMessageId { get; set; }
    public required string ReactionChannelId { get; set; }
    public required string ForumThreadId { get; set; }
    public required bool IsApproved { get; set; }
    public required string CreatorId { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}