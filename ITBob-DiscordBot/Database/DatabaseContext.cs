using ITBob_DiscordBot.Database.Entitys;
using Microsoft.EntityFrameworkCore;

namespace ITBob_DiscordBot.Database;

public class DatabaseContext : DbContext
{
    public DbSet<ReactionRolesEntity> ReactionRoles { get; set; }
    public DbSet<TempVoiceEntity> TempVoiceChannels { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.UseSqlite(
            "Data Source=ITBob_DiscordBot.db;Cache=Shared;Pooling=true;"
        );
    }
}