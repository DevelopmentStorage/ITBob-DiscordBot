﻿FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ITBob-DiscordBot/ITBob-DiscordBot.csproj", "ITBob-DiscordBot/"]
RUN dotnet restore "ITBob-DiscordBot/ITBob-DiscordBot.csproj"
COPY . .
WORKDIR "/src/ITBob-DiscordBot"
RUN dotnet build "./ITBob-DiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ITBob-DiscordBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ITBob-DiscordBot.dll"]
