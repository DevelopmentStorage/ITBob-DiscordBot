using ITBob_DiscordBot.Database;
using ITBob_DiscordBot.Database.Entitys;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace ITBob_DiscordBot.Services;

public class ReactionRoleService
{
    private readonly DatabaseContext DatabaseContext;

    public ReactionRoleService(DatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
    }

    public async Task<Role> CreateRoleAsync(Guild guild, string? roleName)
    {
        var role = await guild.CreateRoleAsync(
            new RoleProperties()
            {
                Name = roleName ?? "Default Role",
                Color = new Color(32, 32, 23),
                Mentionable = false,
                Permissions = Permissions.SendMessages
            }
        );

        return role;
    }

    public async Task CreateReactionRoleAsync(ulong guildId, ulong roleId, ulong reactionMessageId,
        ulong reactionChannelId, ulong forumThreadId, string creatorId)
    {
        var reactionRole = new ReactionRolesEntity()
        {
            GuildId = guildId.ToString(),
            RoleId = roleId.ToString(),
            ReactionMessageId = reactionMessageId.ToString(),
            ReactionChannelId = reactionChannelId.ToString(),
            ForumThreadId = forumThreadId.ToString(),
            IsApproved = false,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        DatabaseContext.ReactionRoles.Add(reactionRole);
        await DatabaseContext.SaveChangesAsync();
    }
}