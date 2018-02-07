using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


public class WebConnection
{
    static HttpClient client = new HttpClient();

    static async Task MainAsync()
    {
        client.BaseAddress = new Uri("http://localhost:55268/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static async Task<String> GetRequest(string link)
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync(link);
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        return result;
    }

    public static async Task GetAllCharacters()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync(
        "https://feheroes.gamepedia.com/index.php?title=Special:CargoExport&tables=Heroes&&fields=_pageName=Name,WeaponType,MoveType,Rarity,SummonRarities__full=SummonRarities,ReleaseDate,PoolDate&&order+by=%60_pageName%60&limit=5000&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoHero(result);

        response = await client.GetAsync(
        "https://feheroes.gamepedia.com/index.php?title=Special:CargoExport&tables=HeroBaseStats&&fields=_pageName=Name,Rarity=Rarity,Variation=Variation,HP=HP,Atk=ATK,Spd=SPD,Def=DEF,Res=RES&&order+by=%60_pageName%60,+%60Rarity%60,+%60Variation%60&limit=5000&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoStats(result, false);

        response = await client.GetAsync(
        "https://feheroes.gamepedia.com/index.php?title=Special:CargoExport&tables=HeroMaxStats&&fields=_pageName=Name,Rarity=Rarity,Variation=Variation,HP=HP,Atk=ATK,Spd=SPD,Def=DEF,Res=RES&&order+by=%60_pageName%60,+%60Rarity%60,+%60Variation%60&limit=5000&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoStats(result, true);

        response = await client.GetAsync(
        "https://feheroes.gamepedia.com/index.php?title=Special:CargoExport&format=json&tables=HeroAssists%2C+HeroSpecials%2C+HeroPassives%2C+HeroWeapons&fields=HeroAssists._pageName%3DName%2C+assist1%2C+assist2%2C+assist3%2C+assist4%2C+assist1Unlock%2C+assist2Unlock%2C+assist3Unlock%2C+assist4Unlock%2C+assist1Default%2C+assist2Default%2C+assist3Default%2C+assist4Default%2C+special1%2C+special2%2C+special3%2C+special4%2C+special1Unlock%2C+special2Unlock%2C+special3Unlock%2C+special4Unlock%2C+special1Default%2C+special2Default%2C+special3Default%2C+special4Default%2C+passiveA1%2C+passiveA2%2C+passiveA3%2C+passiveB1%2C+passiveB2%2C+passiveB3%2C+passiveC1%2C+passiveC2%2C+passiveC3%2C+passiveA1Unlock%2C+passiveA2Unlock%2C+passiveA3Unlock%2C+passiveB1Unlock%2C+passiveB2Unlock%2C+passiveB3Unlock%2C+passiveC1Unlock%2C+passiveC2Unlock%2C+passiveC3Unlock%2C+weapon1%2C+weapon2%2C+weapon3%2C+weapon4%2C+weapon1Unlock%2C++weapon2Unlock%2C+weapon3Unlock%2C+weapon4Unlock%2C+weapon1Default%2C+weapon2Default%2C+weapon3Default%2C+weapon4Default&join_on=HeroAssists._pageName%3DHeroSpecials._pageName%2C+HeroAssists._pageName%3DHeroPassives._pageName%2C+HeroAssists._pageName%3DHeroWeapons._pageName&limit=5000"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoSkills(result);
    }

    public static async Task GetNewCharacters()
    {
        string result = null;
        string date = '\'' + DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + '\'';
        HttpResponseMessage response = await client.GetAsync(
        "https://feheroes.gamepedia.com/index.php?title=Special:CargoExport&tables=Heroes&&fields=_pageName=Name,WeaponType=WeaponType,MoveType=MoveType,SummonRarities__full=SummonRarities,ReleaseDate=ReleaseDate,PoolDate=PoolDate&&order+by=%60_pageName%60&&where=ReleaseDate%3E" + date + "&limit=5000&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        try
        {
            JSON.JSONtoHero(result);
        }
        catch (Exception e)
        {
            Console.WriteLine("No New Heroes Found");
        }
    }

    public static async Task<String> GetEventsAsync()
    {
        string result = null;
        string returnstring = null;
        HttpResponseMessage response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=expandtemplates&text={{Template:Current_Events}}&prop=wikitext&format=json");
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        string[] splits = result.Split(new string[] { "'''" }, StringSplitOptions.None);
        for (int i = 1; i < splits.Length; i += 2)
        {
            if (i % 4 == 1)
            {
                returnstring += "```";
                string[] eventStrings = splits[i].Split(new string[] { "]]:<br>[[" }, StringSplitOptions.None);
                string name = (eventStrings[1].Contains('|') ?
                    eventStrings[1].Substring(eventStrings[1].IndexOf('|') + 1, eventStrings[1].IndexOf("]]") - eventStrings[1].IndexOf('|') - 1) :
                    eventStrings[1].Substring(0, eventStrings[1].IndexOf("]]")));
                Console.WriteLine(eventStrings[1].IndexOf("]]") - eventStrings[1].IndexOf('|') + 1);
                returnstring += eventStrings[0].Substring(2) + ": " + name + '\n';
            }
            else
            {
                returnstring += "Time Remaining: " + splits[i] + '\n' + "```";
            }
        }
        return returnstring;
    }

    public static async Task GetAllBanners()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=query&format=json&prop=revisions&continue=%7C%7C&generator=categorymembers&rvprop=content&rvsection=0&gcmtitle=Category%3ASummoning+Focuses&gcmlimit=50");
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoBanner(result);
    }

    public static async Task GetNewBanners()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=query&format=json&prop=revisions&meta=&continue=gcmcontinue%7C%7C&generator=categorymembers&rvprop=content&rvsection=0&gcmtitle=Category%3ASummoning+Focuses&gcmlimit=5&gcmsort=timestamp&gcmdir=descending&format=json");
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JSON.JSONtoBanner(result);
    }

    

    public static async Task<String> GetGauntletBracket(bool latestRoundOnly = false)
    {
        string result = null;
        string bracket = null;
        HttpResponseMessage response = await client.GetAsync("https://support.fire-emblem-heroes.com/voting_gauntlet/current?locale=en-US");
        if (response.IsSuccessStatusCode)
        {
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[10];
        }
        if (result.Contains("gold") && latestRoundOnly)
        {
            return "END";
        }
        else if (result.Contains("-sub-text"))
        {
            return "Round has ended. Currently calculating results.";
        }
        string[] rounds = result.Split(new string[] { "<article class=\"body-section-tournament\">" }, StringSplitOptions.None);
        foreach (string round in rounds)
        {
            if (round.IndexOf("<h2 class=\"") != -1)
            {
                string roundNumber = round.Substring(round.IndexOf("title-tournament") + 17, 2);
                if (roundNumber == "03")
                {
                    try
                    {
                        bracket += "Final Round\n";
                        bracket += VotingModule.RoundToString(round, roundNumber, latestRoundOnly);
                    }
                    catch { }
                }
                if (roundNumber == "02")
                {
                    try
                    {
                        bracket += "Round 2\n";
                        bracket += VotingModule.RoundToString(round, roundNumber, latestRoundOnly);
                    }
                    catch { }
                }
                if (roundNumber == "01")
                {
                    try
                    {
                        bracket += "Round 1\n";
                        bracket += VotingModule.RoundToString(round, roundNumber, latestRoundOnly);
                    }
                    catch { }
                }
                if (latestRoundOnly)
                {
                    return bracket;
                }
            }
        }
        return bracket;
    }

    public static async Task<Dictionary<String, int>> GetGauntletDictionary()
    {
        string result = null;
        List<String> gauntletNames = new List<String>();
        HttpResponseMessage response = await client.GetAsync("https://support.fire-emblem-heroes.com/voting_gauntlet/current?locale=en-US");
        if (response.IsSuccessStatusCode)
        {
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[10];
        }
        result = result.Substring(result.IndexOf("tournament-01", 0, StringComparison.Ordinal));
        string[] names = result.Split(new string[] { "\"name\">" }, StringSplitOptions.None);
        for (int i = 1; i < names.Length; i++)
        {
            string name = names[i].Substring(0, names[i].IndexOf("</p"));
            if (gauntletNames.Contains(name))
            {
                gauntletNames[gauntletNames.IndexOf(name)] = name + " 1";
                name += " 2";
            }
            gauntletNames.Add(name);
        }
        await Postgres.AddGauntletHeroes(gauntletNames);
        return gauntletNames.Select((s, i) => new { s, i }).ToDictionary(x => x.s, x => x.i);
    }

    public static async Task<string> CheckGauntletOngoing()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync("https://support.fire-emblem-heroes.com/voting_gauntlet/current?locale=en-US");
        if (response.IsSuccessStatusCode)
        {
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[10];
        }
        return (result);
    }
}