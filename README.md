# ITBob Discord Bot

**ITBob Discord Bot** is a small Discord bot written in **C#** using the [netCord.dev](https://netcord.dev) library, and is used on a smaller Discord server.

It uses **[EF Core SQLite](https://learn.microsoft.com/de-de/ef/core/)** for various features such as:

- [ReactionRoles](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/ReactionRoles)  
- [Verify](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/Verify)
- [TempVoice](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/TempVoice)

You can easily start the bot with Docker Compose:

```yaml
services:
  itbob-discordbot:
    image: ‘ghcr.io/developmentstorage/itbob-discordbot:latest’
    container_name: itbob-discordbot
    volumes:
      - /docker/bobbot/config.json:/app/config.json
```

You can then customise the `config.json` in the volume or alternatively use a bind volume.
