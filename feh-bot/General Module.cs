using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Discord;
using Discord.Commands;

public class InfoModule : ModuleBase<SocketCommandContext>
{
    TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

    [Command("Help")]
    [Summary("Gives commands and how to use them.")]
    public async Task Help([Remainder] string command = "")
    {
        string returnString = "";
        if (command == "")
        {
            returnString += "```" + String.Join("``````", Program.GetCommands().Where(c => (!c.Name.StartsWith("Character.") && c.Module.Name != "Voting")).OrderBy(c => c.Name).Select(c => c.Name + ": " + c.Summary)) + "```";
            returnString += "```" + String.Join("``````", Program.GetModules().Where(c => c.Summary != null).OrderBy(c => c.Name).Select(c => c.Name + ": " + c.Summary)) + "```";
        }
        else
        {
            command = myTI.ToTitleCase(command);
            if (command.StartsWith("Character"))
            {
                command = "Character";
            }
            try
            {
                CommandInfo com = Program.GetCommands().Where(c => c.Name == command || c.Aliases.Any(a => a == command.ToLower())).First();
                if (com.Parameters[0].Summary == null)
                {
                    await ReplyAsync("No expanded help information for command: " + command);
                    return;
                }
                else
                {
                    returnString += "```" + com.Parameters[0].Summary + "```";
                }
            }
            catch
            {
                try
                {
                    ModuleInfo mod = Program.GetModules().Where(c => c.Name == command).First();
                    returnString += "```" + String.Join("``````", mod.Commands.OrderBy(c => c.Name).Select(c => c.Name + ": " + c.Summary)) + "```";
                }
                catch
                {
                    await ReplyAsync("The command " + command + " does not exist.");
                }
            }
        }
        if (returnString != "")
        {
            await ReplyAsync(returnString);
        }
    }

    [Command("Skill")]
    [Summary("Gives a list of people who learn a specific skill.")]
    public async Task Skill([Remainder]string name)
    {
        Queue<string> replies = Heroes.GetSkill(myTI.ToTitleCase(name));
        while (replies.Count > 0)
        {
            await ReplyAsync(replies.Dequeue());
        }
    }

    [Command("Character")]
    [Alias("Character.5*", "Character.5*.40", "Character.5*.Lv40", "Character.5*.Lvl40", "Character.5.40", "Character.5.Lv40", "Character.5.Lvl40")]
    [Summary("Gives you a character's stats. See Feh.Help Character for more information")]
    public async Task FiveStarLv40([Summary(
        "Input command in the following format: \n\n" +
        "Feh.Character.RARITY.LEVEL HERONAME BOON BANE. \n\n" +
        "RARITY: 3* - 5* \n\n" +
        "LEVEL: 1 or 40. \n\n" +
        "BOON and BANE can be entered as +ATK or -defense.\n\n" +
        "Only HERONAME is required and will give a neutral 5* level 40 character if no other parameters are provided.\n\n" +
        "IF HERONAME contains spaces, enter HERONAME within quotation marks\n\n" +
        "Example: Feh.Character.4*.1 \"Marth\" +SPD -DEF")]
    string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 5, 40, mod1.ToUpper(), mod2.ToUpper()));
    }

    [Command("Character.5*.1")]
    [Alias("Character.5*.Lv1", "Character.5*.Lvl1", "Character.5.Lv1", "Character.5.Lvl1")]
    [Summary("Tells you a character's 5* level 1 stats.")]
    public async Task FiveStarLv1([Summary("Character you want to check")]string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 5, 1, mod1.ToUpper(), mod2.ToUpper()));
    }

    [Command("Character.4*")]
    [Alias("Character.4*.40", "Character.4*.Lv40", "Character.4*.Lvl40", "Character.4.40", "Character.4.Lv40", "Character.4.Lvl40")]
    [Summary("Tells you a character's 4* level 40 stats.")]
    public async Task FourStarLv40([Summary("Character you want to check")]string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 4, 40, mod1.ToUpper(), mod2.ToUpper()));
    }

    [Command("Character.4*.1")]
    [Alias("Character.4*.Lv1", "Character.4*.Lvl1", "Character.4.Lv1", "Character.4.Lvl1")]
    [Summary("Tells you a character's 4* level 1 stats.")]
    public async Task FourStarLv1([Summary("Character you want to check")]string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 4, 1, mod1.ToUpper(), mod2.ToUpper()));
    }

    [Command("Character.3*")]
    [Alias("Character.3*.40", "Character.3*.Lv40", "Character.3*.Lvl40", "Character.3.40", "Character.3.Lv40", "Character.3.Lvl40")]
    [Summary("Tells you a character's 3* level 40 stats.")]
    public async Task ThreeStarLv40([Summary("Character you want to check")]string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 3, 40, mod1.ToUpper(), mod2.ToUpper()));
    }

    [Command("Character.3*.1")]
    [Alias("Character.3*.Lv1", "Character.3*.Lvl1", "Character.3.Lv1", "Character.3.Lvl1")]
    [Summary("Tells you a character's 3* level 1 stats.")]
    public async Task ThreeStarLv1([Summary("Character you want to check")]string name, string mod1 = "", string mod2 = "")
    {
        await ReplyAsync(Heroes.GetHero(myTI.ToTitleCase(name), 3, 1, mod1.ToUpper(), mod2.ToUpper()));
    }

    /*CURRENTLY BROKEN
    [Command("Event")]
    [Alias("Events")]
    [Summary("Gives a list of currently ongoing events.")]
    public async Task Events(int dummy = -1)
    {
        await ReplyAsync(await WebConnection.GetEventsAsync());
    }*/

    [Command("Focus")]
    [Alias("Banner", "Banners")]
    [Summary("Gives a list of currently ongoing and upcoming Summoning Focuses.")]
    public async Task Banners(int dummy = -1)
    {
        string reply = String.Join("", Summoner.BannersDictionary.OrderBy(kv => kv.Value.StartDate).Where(kv => kv.Value.EndDate >= DateTime.UtcNow).Select(kv => kv.Value.BannerToString()));
        await ReplyAsync(reply);
    }

    [Command("Summon")]
    [Summary("A command that simulates summoning on a Summoning Focus. See Feh.Help Summon for more information.")]
    public async Task Summon([Summary(
        "Input command in the following format: \n\n" +
        "Feh.Summon \"FOCUS NAME\" AMOUNT COLOR \n\n" +
        "AMOUNT: Number of Heroes to summon. Limit of 100 \n\n" +
        "COLOR: Optional parameter to summon only Heroes of a specific color. If no Heroes of matching colors are found in a single summon session, a random color will be picked.\n\n" +
        "If you summon 5 times in a row without getting a 5* Hero, the chances of a 5* Focus and 5* Hero will increase. The rates will return to normal if a 5* Hero is obtained or the Summon command ends.\n\n" +
        "Example: Feh.Summon \"Hero Fest\" 10 Red")
        ]string banner, int times = 1, string color = "")
    {
        if (times > 100 || times <= 0)
        {
            await ReplyAsync("Please enter a value between 1-100 for number of Heroes to summon.");
        }
        else
        {
            color = myTI.ToTitleCase(color);
            if (color == "Colorless" || color == "Grey" || color == "Gray")
            {
                color = "Neutral";
            }
            await ReplyAsync(Summoner.Summon(banner, times, color).Result);
        }
    }
}