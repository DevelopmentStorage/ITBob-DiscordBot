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

    public async Task CreateReactionRoleAsync(string gameName, ulong guildId, ulong reactionMessageId,
        ulong reactionChannelId, ulong creatorId)
    {
        var reactionRole = new ReactionRolesEntity()
        {
            GuildId = guildId,
            RoleId = 0,
            GameName = gameName,
            ReactionMessageId = reactionMessageId,
            ReactionChannelId = reactionChannelId,
            ForumThreadId = 0,
            IsApproved = false,
            CreatorId = creatorId,
            CreatedAt = DateTime.UtcNow
        };

        DatabaseContext.ReactionRoles.Add(reactionRole);
        await DatabaseContext.SaveChangesAsync();
    }
}