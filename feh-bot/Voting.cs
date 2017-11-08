using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

[Group("Voting")]
[Summary("Commands used for the Voting Gauntlet. See Feh.Help Voting for information on Voting commands.")]
public class VotingModule : ModuleBase<SocketCommandContext>
{
    private static TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
    private static ConcurrentDictionary<String, List<String>> subscribedUsers = new ConcurrentDictionary<string, List<String>>();
    /*
    (
        new Dictionary<String, List<IUser>>()
        {
            { "Ninian", new List<IUser>() },
            { "Corrin1", new List<IUser>() },
            { "Tiki1", new List<IUser>() },
            { "Sophia", new List<IUser>() },
            { "Fae", new List<IUser>() },
            { "Nowi", new List<IUser>() },
            { "Corrin2", null },
            { "Tiki2", new List<IUser>() }
        }
    );*/
    public static ConcurrentDictionary<String, int> gauntletCharacterNametoID = new ConcurrentDictionary<String, int>();
    private static Timer gauntletStartClock = new Timer(async (_) => await CheckOngoing());//, null, new TimeSpan(1, 7, 3, 30, 0) - DateTime.UtcNow.TimeOfDay, new TimeSpan(-1));//SET TO START NOW
    private static Timer gauntletAlertClock = new Timer(async (_) => await Alert());

    [Command("Bracket")]
    [Summary("Gives voting gauntlet statistics")]
    public async Task Bracket()
    {
        await ReplyAsync(await WebConnection.GetGauntletBracket());
    }

    [Command("Join")]
    [Alias("Subscribe")]
    [Summary("Subscribes you to a Voting Gauntlet team. Will give hourly alerts when your team has a bonus available. Joining multiple teams is possible.")]
    public async Task Subscribe([Remainder]string name)
    {
        name = myTI.ToTitleCase(name);
        try
        {
            await Postgres.AddUser(Context.User.Username, Context.User.Discriminator, name);
            await ReplyAsync("You have joined Team " + name + ".");
        }
        catch (KeyNotFoundException)
        {
            List<String> matchingNames = new List<string>();
            string[] splitName = name.Split(new char[] { ' ', '.', '-', '!' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> teams = Postgres.GetTeams();
            foreach (string heroName in teams)
            {
                foreach (string s in splitName)
                {
                    if (Regex.IsMatch(heroName, @"\b" + s + @"\b"))
                    {
                        matchingNames.Add(heroName);
                    }
                }
            }
            if (matchingNames.Count == 0)
            {
                await ReplyAsync("Team " + name + " does not exist. Please check feh.voting.bracket for the current bracket");
            }
            else
            {
                string replyString = "Did you mean:";
                for (int i = matchingNames.Count - 1; i >= 0; i--)
                {
                    int ID = gauntletCharacterNametoID[matchingNames[i]];
                    ID = (ID % 2 == 0 ? ID + 1 : ID - 1);
                    replyString += '\n' + matchingNames[i] + "(Opponent in round 1: " + teams[ID] + ")";
                }
                await ReplyAsync(replyString);
            }
        }
    }

    [Command("Subscribed")]
    [Summary("Displays what teams you are currently subscribed to.")]
    public async Task Subscribed(int i = -1)
    {
        await ReplyAsync("You have subscribed to the following teams:\n" + Postgres.GetTeams(Context.User.Discriminator));
    }

    [Command("Leave")]
    [Alias("Unsubscribe")]
    [Summary("Removes you from a Voting Gauntlet team.")]
    public async Task Unsubscribe(string name)
    {
        name = myTI.ToTitleCase(name);
        try
        {
            await Postgres.RemoveUser(Context.User.Discriminator, name);
            await ReplyAsync("You have left Team " + name + ".");
        }
        catch (KeyNotFoundException)
        {
            List<String> matchingNames = new List<string>();
            string[] splitName = name.Split(new char[] { ' ', '.', '-', '!' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> teams = Postgres.GetTeams();
            foreach (string heroName in teams)
            {
                foreach (string s in splitName)
                {
                    if (Regex.IsMatch(heroName, @"\b" + s + @"\b"))
                    {
                        matchingNames.Add(heroName);
                    }
                }
            }
            if (matchingNames.Count == 0)
            {
                await ReplyAsync("Team " + name + " does not exist. Please check feh.voting.bracket for the current bracket");
            }
            else
            {
                string replyString = "Did you mean:";
                for (int i = matchingNames.Count - 1; i >= 0; i--)
                {
                    int ID = gauntletCharacterNametoID[matchingNames[i]];
                    ID = (ID % 2 == 0 ? ID + 1 : ID - 1);
                    replyString += '\n' + matchingNames[i] + "(Opponent in round 1: " + teams[ID] + ")";
                }
                await ReplyAsync(replyString);
            }
        }
    }

    public void UnsubscribeTeam([Remainder]string name)
    {
        subscribedUsers[name] = null;
    }

    private static void SetupGauntlet()
    {
        if (gauntletCharacterNametoID.IsEmpty)
        {
            gauntletCharacterNametoID = new ConcurrentDictionary<String, int>(WebConnection.GetGauntletDictionary().Result);
        }
        //Check for Gauntlet status daily. If a new Gauntlet has occured, reset both dictionaries, and create new gauntletCharacterIDtoName entries.
        //If a round changes, unsubscribe all players of a losing team and remove that team's dictionary entry.
    }

    //Sends an alert to all users in Voting Gauntlet teams who have an active bonus
    public static async Task Alert()
    {
        Console.WriteLine(DateTime.Now.TimeOfDay + ": " + "Sending alerts");
        string latestRound = WebConnection.GetGauntletBracket(true).Result;
        if (latestRound == "END")
        {
            gauntletAlertClock.Change(Timeout.Infinite, Timeout.Infinite);
            gauntletStartClock.Change(new TimeSpan(20, 0, 0, 0), Timeout.InfiniteTimeSpan);
            return;
        }
        else if (latestRound == "Round has ended. Currently calculating results.")
        {
            TimeSpan oneHour = new TimeSpan(1 + DateTime.UtcNow.Hour, 4, 0) - DateTime.UtcNow.TimeOfDay;
            Console.WriteLine("Round ended. Next alert: " + oneHour);
            return;
        }
        Dictionary<int, bool> bonus = new Dictionary<int, bool>()
        {
            {0, false}, {1, false}, {2, false}, {3, false}, {4, false}, {5, false}, {6, false}, {7, false},
        };
        Console.WriteLine(latestRound);
        string[] combatants = latestRound.Split('|', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine(combatants.Length);
        for (int i = 0; i < combatants.Length; i++)
        {
            string[] combatantInfo = combatants[i].Split(':');
            bonus[Int32.Parse(combatantInfo[0][combatantInfo[0].Length - 1].ToString()) - 1] = combatantInfo[1] == " Behind ";
        }

        for (int i = 0; i < bonus.Count; i++)
        {
            if (bonus[i])
            {
                List<string> users = Postgres.GetUsersInTeam(i);
                for (int k = 0; k < users.Count; k++)
                {
                    string[] userInfo = users[k].Split("|sep|");
                    IUser user = Program.GetUser(userInfo[1], userInfo[0]);
                    await (user.SendMessageAsync("Bonus Alert: " + userInfo[2]));
                }
            }
        }

        TimeSpan interval = new TimeSpan(1 + DateTime.UtcNow.Hour, 4, 0) - DateTime.UtcNow.TimeOfDay;
        Console.WriteLine("Next alert: " + interval);
        gauntletAlertClock.Change(interval, new TimeSpan(-1));
    }

    /*
    [Command("testalert")]
    public async Task testalert(string asd = "asd")
    {
        Alert();
    }*/

    public static async Task CheckOngoing()
    {
        string result = WebConnection.CheckGauntletOngoing().Result;
        if (result.Contains("gold"))
        {
            TimeSpan interval = new TimeSpan(1, 7, 3, 30, 0) - DateTime.UtcNow.TimeOfDay;
            gauntletStartClock.Change(interval, new TimeSpan(-1));
        }
        else
        {
            SetupGauntlet();
            string date = result.Substring(result.IndexOf("tournaments-date") + 18, result.IndexOf('~') - (result.IndexOf("tournaments-date") + 18));
            string[] dateSplit = date.Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
            DateTime round1Start = new DateTime(DateTime.UtcNow.Year, Int32.Parse(dateSplit[0]), Int32.Parse(dateSplit[1]), 7, 0, 0);
            if (DateTime.UtcNow > round1Start)
            {
                Console.WriteLine("Starting alerts: " + (new TimeSpan(1 + DateTime.UtcNow.Hour, 3, 0) - DateTime.UtcNow.TimeOfDay));
                gauntletAlertClock.Change(new TimeSpan(1 + DateTime.UtcNow.Hour, 4, 0) - DateTime.UtcNow.TimeOfDay, new TimeSpan(-1));
            }
            else
            {
                gauntletStartClock.Change((round1Start.AddMinutes(4) - DateTime.UtcNow), new TimeSpan(-1));
                Console.WriteLine("Round 1 Starts in: " + (round1Start.AddMinutes(4) - DateTime.UtcNow).ToString());
            }
        }
    }

    public static string RoundToString(string round, string roundNumber, bool getIDs = false)
    {
        string returnString = "";
        string[] combatants = round.Split(new string[] { "tournaments-art-right", "tournaments-art-left" }, StringSplitOptions.None);
        int battles = 0;
        if (roundNumber == "03")
        {
            battles = 1;
        }
        else if (roundNumber == "02")
        {
            battles = 4;
        }
        else if (roundNumber == "01")
        {
            battles = 8;
        }
        for (int i = 1; i < (battles + 1); i += 2)
        {
            string leftID = combatants[i].Substring(7, 1);
            string leftStatus = combatants[i].Substring(9, 6);
            int nameIndex = combatants[i].IndexOf("\"name\"") + 7;
            int nameLength = combatants[i].IndexOf("</p") - nameIndex;
            string leftName = combatants[i].Substring(nameIndex, combatants[i].IndexOf("</p") - nameIndex);
            string leftScore = combatants[i].Substring(nameIndex + nameLength + 7, combatants[i].IndexOf("</p></div>") - (nameIndex + nameLength + 7));

            string rightID = combatants[i + 1].Substring(7, 1);
            string rightStatus = combatants[i + 1].Substring(9, 6);
            nameIndex = combatants[i + 1].IndexOf("\"name\"") + 7;
            nameLength = combatants[i + 1].IndexOf("</p") - nameIndex;
            string rightName = combatants[i + 1].Substring(nameIndex, combatants[i + 1].IndexOf("</p") - nameIndex);
            string rightScore = combatants[i + 1].Substring(nameIndex + nameLength + 7, combatants[i + 1].IndexOf("</p></div>") - (nameIndex + nameLength + 7));

            if (leftStatus == "behind")
            {
                leftStatus = " Behind ";
                rightStatus = " Ahead ";
            }
            else if (rightStatus == "behind")
            {
                leftStatus = " Ahead ";
                rightStatus = " Behind ";
            }
            else
            {
                leftStatus = "";
                rightStatus = "";
            }

            if (getIDs)
            {
                returnString += leftID + ":" + leftStatus + "|" + rightID + ":" + rightStatus + "|";
            }
            else
            {
                if (leftScore == "" && rightScore == "")
                {
                    returnString += "```" + leftName + " vs " + rightName + "```";
                }
                else
                {
                    returnString += "```" + leftName + leftStatus + leftScore + "\nvs\n" + rightName + rightStatus + rightScore + "```";
                }
            }
        }
        return returnString;
    }
}