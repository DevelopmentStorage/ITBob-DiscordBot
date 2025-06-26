using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using NetCord;
using NetCord.Gateway;
using NetCord.Gateway.Voice;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace ITBob_DiscordBot.Commands;

[SlashCommand("music", "Plays music", Contexts = [InteractionContextType.Guild])]
public class MusicCommand : ApplicationCommandModule<ApplicationCommandContext>
{
    [SubSlashCommand("play", "Plays a song from YouTube or Spotify")]
    public async Task PlayAsync(string songUrl)
    {
        await RespondAsync(InteractionCallback.DeferredMessage(MessageFlags.Ephemeral));
        /*
        await ModifyResponseAsync(options =>
            options.Content =
                "Downloading setup files... This may take a while, please be patient!");

        await YoutubeDLSharp.Utils.DownloadYtDlp();
        await YoutubeDLSharp.Utils.DownloadFFmpeg();

        var ytdl = new YoutubeDL();
        if (!File.Exists("./musicQueue"))
        {
            Directory.CreateDirectory("./musicQueue");
        }

        ytdl.OutputFolder = "./musicQueue";
        await ModifyResponseAsync(options =>
            options.Content = "Setup files downloaded successfully! Starting the download...");

        var audioDownload = await ytdl.RunAudioDownload(songUrl, AudioConversionFormat.Mp3);
        var audioFileName = audioDownload.Data.Split('/').Last();
*/
        var guild = await Context.Client.Rest.GetGuildAsync((ulong)Context.Interaction.GuildId);

        if (guild == null)
        {
            await ModifyResponseAsync(options =>
                options.Content = "Failed to get the guild information. Please try again later.");
            return;
        }

        await ModifyResponseAsync(options =>
            options.Content =
                $"Now playing: test\nPlease wait while the song is being processed and sent to Discord.");

        await JoinVoiceChannelAsync(
            guild,
            "C:/Users/jespe/Documents/GitHub/ITBob-DiscordBot/ITBob-DiscordBot/bin/Debug/net9.0/musicQueue/Sean Paul - No Lie ft. Dua Lipa [GzU8KqOY8YA].mp3",
            "test");
    }

    private async Task JoinVoiceChannelAsync(RestGuild guild, string audioDownload, string audioFileName)
    {
        var voiceState = await guild.GetUserVoiceStateAsync(Context.User.Id);

        var client = Context.Client;

        var voiceClient = await client.JoinVoiceChannelAsync(
            guild.Id,
            voiceState.ChannelId.GetValueOrDefault(),
            new VoiceClientConfiguration
            {
                Logger = new ConsoleLogger(),
            });

        // Connect
        await voiceClient.StartAsync();

        // Enter speaking state, to be able to send voice
        await voiceClient.EnterSpeakingStateAsync(new SpeakingProperties(SpeakingFlags.Microphone));

        // Create a stream that sends voice to Discord
        var outStream = voiceClient.CreateOutputStream();

        // We create this stream to automatically convert the PCM data returned by FFmpeg to Opus data.
        // The Opus data is then written to 'outStream' that sends the data to Discord
        OpusEncodeStream stream = new(outStream, PcmFormat.Short, VoiceChannels.Stereo, OpusApplication.Audio);

        ProcessStartInfo startInfo = new("ffmpeg")
        {
            RedirectStandardOutput = true,
        };

        var arguments = startInfo.ArgumentList;

        // Set reconnect attempts in case of a lost connection to 1
        arguments.Add("-reconnect");
        arguments.Add("1");

        // Set reconnect attempts in case of a lost connection for streamed media to 1
        arguments.Add("-reconnect_streamed");
        arguments.Add("1");

        // Set the maximum delay between reconnection attempts to 5 seconds
        arguments.Add("-reconnect_delay_max");
        arguments.Add("5");

        // Specify the input
        arguments.Add("-i");
        arguments.Add(audioDownload);

        // Set the logging level to quiet mode
        arguments.Add("-loglevel");
        arguments.Add("-8");

        // Set the number of audio channels to 2 (stereo)
        arguments.Add("-ac");
        arguments.Add("2");

        // Set the output format to 16-bit signed little-endian
        arguments.Add("-f");
        arguments.Add("s16le");

        // Set the audio sampling rate to 48 kHz
        arguments.Add("-ar");
        arguments.Add("48000");

        // Direct the output to stdout
        arguments.Add("pipe:1");

        // Start the FFmpeg process
        var ffmpeg = Process.Start(startInfo)!;

        // Copy the FFmpeg stdout to 'stream', which encodes the voice using Opus and passes it to 'outStream'
        await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);

        // Flush 'stream' to make sure all the data has been sent and to indicate to Discord that we have finished sending
        await stream.FlushAsync();
    }
}