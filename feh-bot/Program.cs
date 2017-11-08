using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private CommandService _commands;
    private static IEnumerable<CommandInfo> allCommands;
    private static IEnumerable<ModuleInfo> allModules;
    private DiscordSocketClient _client;
    private static DiscordSocketClient _staticClient;
    private IServiceProvider _services;
    private static Timer dailyUpdate = new Timer(async (_) => await DailyUpdate());

    public static void Main(string[] args)
    {
        new Program().MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {
        Postgres.OpenConnection();
        await Initialize();

        _client = new DiscordSocketClient();
        _commands = new CommandService();

        _client.Log += Log;

        string token = Environment.GetEnvironmentVariable("TOKEN");//Discord Bot token hidden.

        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

        await _client.SetGameAsync("Fire Emblem Heroes");

        await InstallCommandsAsync();

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _staticClient = _client;

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived Event into our Command Handler
        _client.MessageReceived += HandleCommandAsync;
        // Discover all of the commands in this assembly and load them.
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        allCommands = _commands.Commands;
        allModules = _commands.Modules;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a System Message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        // Determine if the message is a command, based on if it starts with '!' or a mention prefix
        if (!(message.HasStringPrefix("feh.", ref argPos) || message.HasStringPrefix("Feh.", ref argPos) || message.HasStringPrefix("FEH.", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
        // Create a Command Context
        var context = new SocketCommandContext(_client, message);
        // Execute the command. (result does not indicate a return value, 
        // rather an object stating if the command executed successfully)
        var result = await _commands.ExecuteAsync(context, argPos, _services);
        if (!result.IsSuccess)
            await context.Channel.SendMessageAsync(result.ErrorReason);
    }

    public static IEnumerable<CommandInfo> GetCommands()
    {
        return allCommands;
    }

    public static IEnumerable<ModuleInfo> GetModules()
    {
        return allModules;
    }

    public static IUser GetUser(string name, string discriminator)
    {
        return _staticClient.GetUser(name, discriminator);
    }

    private static async Task Initialize()
    {
        try
        {
            await WebConnection.GetAllCharacters();
            await WebConnection.GetAllBanners();
            Heroes.GetHeroRarityLists();
            await VotingModule.CheckOngoing();
            TimeSpan interval = ((new TimeSpan(0, 8, 0, 0) - DateTime.UtcNow.TimeOfDay) < DateTime.UtcNow.TimeOfDay ? new TimeSpan(1, 8, 0, 0) - DateTime.UtcNow.TimeOfDay : new TimeSpan(0, 8, 0, 0) - DateTime.UtcNow.TimeOfDay);
            Console.WriteLine("Daily Check in: " + interval);
            dailyUpdate.Change(interval, Timeout.InfiniteTimeSpan);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static async Task DailyUpdate()
    {
        Console.WriteLine("Daily Update");
        await WebConnection.GetNewCharacters();
        await WebConnection.GetNewBanners();
        Heroes.GetHeroRarityLists();
        TimeSpan interval = ((new TimeSpan(0, 8, 0, 0) - DateTime.UtcNow.TimeOfDay) < DateTime.UtcNow.TimeOfDay ? new TimeSpan(1, 8, 0, 0) - DateTime.UtcNow.TimeOfDay : new TimeSpan(0, 8, 0, 0) - DateTime.UtcNow.TimeOfDay);
        Console.WriteLine("Next Daily Check: " + interval);
        dailyUpdate.Change(interval, Timeout.InfiniteTimeSpan);
    }
}