# ITBob-DiscordBot

ITBob-DiscordBot is a small Discord bot written in C# using the [netCord.dev](netCord.dev) library and is used by a smaller Discord server.
It uses [EF Core SQLite](https://learn.microsoft.com/de-de/ef/core/) for [ReactionRoles](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/ReactionRoles), [Verify](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/Verify), [TempVoice](https://github.com/DevelopmentStorage/ITBob-DiscordBot/tree/master/ITBob-DiscordBot/Features/TempVoice)

Use the bot easily with Docker Compose 
```services:
  itbob-discordbot:
    image: “ghcr.io/developmentstorage/itbob-discordbot:latest”
    container_name: itbob-discordbot
    volumes:
      - itbob_data:/app

volumes:
  itbob_data:```

Later, just adjust the config.json or work with a bind volume.

Translated with DeepL.com (free version)
