using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

public static class JSON
{

    public static void JSONtoHero(string text)
    {
        JArray data = JArray.Parse(text);
        foreach (JObject j in data)
        {
            if (Heroes.HeroDictionary.ContainsKey(j.SelectToken("Name").ToString()))
            {
                continue;
            }
            Hero hero = new Hero();
            hero.InitializeArrays();
            if (j.SelectToken("ReleaseDate").ToString() == String.Empty)
            {
                continue;
            }
            hero.name = j.SelectToken("Name").ToString();
            hero.rarity = (j.SelectToken("SummonRarities").HasValues ? j.SelectToken("SummonRarities").Select(t => t.ToString()).ToArray() : new string[3]);
            hero.weapontype = j.SelectToken("WeaponType").ToString();
            hero.color = hero.weapontype.Substring(0, hero.weapontype.IndexOf(" "));
            hero.movetype = j.SelectToken("MoveType").ToString();
            string[] date = j.SelectToken("ReleaseDate").ToString().Split('-');
            hero.releaseDate = new DateTime(Int32.Parse(date[0]), Int32.Parse(date[1]), Int32.Parse(date[2]), 7, 0, 0);
            if (j.SelectToken("PoolDate").ToString() != String.Empty)
            {
                date = j.SelectToken("PoolDate").ToString().Split('-');
                hero.poolDate = new DateTime(Int32.Parse(date[0]), Int32.Parse(date[1]), Int32.Parse(date[2]), 7, 0, 0);
            }
            else
            {
                hero.poolDate = DateTime.UtcNow;
            }
            Heroes.HeroDictionary.TryAdd(hero.name, hero);
        }
    }

    public static void JSONtoStats(string text, bool lv40)
    {
        JArray data = JArray.Parse(text);
        foreach (JObject j in data)
        {
            if (Int32.Parse(j.SelectToken("Rarity").ToString()) <= 2)
            {
                continue;
            }
            string[] stats = new string[5]
            {
                j.SelectToken("ATK").ToString(), j.SelectToken("DEF").ToString(), j.SelectToken("HP").ToString(), j.SelectToken("RES").ToString(), j.SelectToken("SPD").ToString()
            };
            Heroes.HeroDictionary[j.SelectToken("Name").ToString()].AddStats(Int32.Parse(j.SelectToken("Rarity").ToString()), lv40, j.SelectToken("Variation").ToString(),stats);
        }
    }

    public static void JSONtoSkills(string text)
    {
        string[] weapon = new string[4] { "", "", "", "" };
        string[] special = new string[3] { "", "", "" };
        string[] assist = new string[3] { "", "", "" };
        string[] passiveA = new string[3];
        string[] passiveB = new string[3];
        string[] passiveC = new string[3];

        string[] weaponRarity = new string[4] { "", "", "", "" };
        string[] specialRarity = new string[3] { "", "", "" };
        string[] assistRarity = new string[3] { "", "", "" };
        string[] passiveARarity = new string[3];
        string[] passiveBRarity = new string[3];
        string[] passiveCRarity = new string[3];

        string[][] skills = new string[6][]
        {
            weapon, special, assist, passiveA, passiveB, passiveC
        };

        string[][] skillUnlocks = new string[6][]
        {
            weaponRarity, specialRarity, assistRarity, passiveARarity, passiveBRarity, passiveCRarity
        };

        JArray data = JArray.Parse(text);
        foreach (JObject j in data)
        {
            if (!Heroes.HeroDictionary.ContainsKey(j.SelectToken("Name").ToString()))
            {
                continue;
            }
            for (int i = 1; i < 4; i++)
            {
                weapon[i - 1] = (j.SelectToken("weapon" + i).HasValues ? j.SelectToken("weapon" + i + "[0].fulltext").ToString() : "");
                special[i - 1] = (j.SelectToken("special" + i).HasValues ? j.SelectToken("special" + i + "[0].fulltext").ToString() : "");
                assist[i - 1] = (j.SelectToken("assist" + i).HasValues ? j.SelectToken("assist" + i + "[0].fulltext").ToString() : "");
                passiveA[i - 1] = (j.SelectToken("passiveA" + i).HasValues ? j.SelectToken("passiveA" + i + "[0]").ToString() : "");
                passiveB[i - 1] = (j.SelectToken("passiveB" + i).HasValues ? j.SelectToken("passiveB" + i + "[0]").ToString() : "");
                passiveC[i - 1] = (j.SelectToken("passiveC" + i).HasValues ? j.SelectToken("passiveC" + i + "[0]").ToString() : "");

                weaponRarity[i - 1] += (j.SelectToken("weapon" + i + "Unlock").HasValues ? j.SelectToken("weapon" + i + "Unlock[0]").ToString() : "");
                specialRarity[i - 1] += (j.SelectToken("special" + i + "Unlock").HasValues ? j.SelectToken("special" + i + "Unlock[0]").ToString() : "");
                assistRarity[i - 1] += (j.SelectToken("assist" + i + "Unlock").HasValues ? j.SelectToken("assist" + i + "Unlock[0]").ToString() : "");
                passiveARarity[i - 1] = (j.SelectToken("passiveA" + i + "Unlock").HasValues ? j.SelectToken("passiveA" + i + "Unlock[0]").ToString() : "");
                passiveBRarity[i - 1] = (j.SelectToken("passiveB" + i + "Unlock").HasValues ? j.SelectToken("passiveB" + i + "Unlock[0]").ToString() : "");
                passiveCRarity[i - 1] = (j.SelectToken("passiveC" + i + "Unlock").HasValues ? j.SelectToken("passiveC" + i + "Unlock[0]").ToString() : "");

                weaponRarity[i - 1] += (weaponRarity[i - 1] == "" ? (j.SelectToken("weapon" + i + "Default").HasValues ? j.SelectToken("weapon" + i + "Default[0]").ToString() : "") : "");
                specialRarity[i - 1] += (specialRarity[i - 1] == "" ? (j.SelectToken("special" + i + "Default").HasValues ? j.SelectToken("special" + i + "Default[0]").ToString() : "") : "");
                assistRarity[i - 1] += (assistRarity[i - 1] == "" ? (j.SelectToken("assist" + i + "Default").HasValues ? j.SelectToken("assist" + i + "Default[0]").ToString() : "") : "");
            }

            weapon[3] = (j.SelectToken("weapon4").HasValues ? j.SelectToken("weapon4[0].fulltext").ToString() : "");
            weaponRarity[3] += (j.SelectToken("weapon4Unlock").HasValues ? j.SelectToken("weapon4Unlock[0]").ToString() : "");
            weaponRarity[3] += (weaponRarity[3] == "" ? (j.SelectToken("weapon4Default").HasValues ? j.SelectToken("weapon4Default[0]").ToString() : "") : "");
            Heroes.HeroDictionary[j.SelectToken("Name").ToString()].AddSkills(skills, skillUnlocks);
        }
    }

    public static void JSONtoBanner(string text)
    {
        JObject data = JObject.Parse(text);
        foreach (JToken j in data.SelectToken("query.pages"))
        {
            Banner b = new Banner();
            if (!j.First.SelectToken("title").ToString().EndsWith("(Focus)"))
            {
                continue;
            }
            string info = j.First.SelectToken("revisions[0]").Last.ToString();
            string[] bannerInfo = info.Split(new string[] { "\\n|" }, StringSplitOptions.None);
            for (int i = 0; i < bannerInfo.Length; i++)
            {
                if (bannerInfo[i].StartsWith("name="))
                {
                    b.Title = bannerInfo[i].Substring(bannerInfo[i].IndexOf('=') + 1);
                    if (Summoner.BannersDictionary.ContainsKey(b.Title))
                    {
                        continue;
                    }
                }
                if (bannerInfo[i].StartsWith("rarity"))
                {
                    b.rates[i - 4] = Convert.ToDouble(bannerInfo[i].Remove(bannerInfo[i].Length - 1).Substring(bannerInfo[i].IndexOf('=') + 1)) / 100;
                }
                else if (bannerInfo[i].StartsWith("hero"))
                {
                    b.Heroes.Add(Heroes.HeroDictionary[bannerInfo[i].Substring(bannerInfo[i].IndexOf('=') + 1)]);
                }
                else if (bannerInfo[i].StartsWith("start"))
                {
                    int month = (Array.IndexOf(DateTimeFormatInfo.InvariantInfo.MonthNames, bannerInfo[i].Substring(6, bannerInfo[i].IndexOf(' ') - 6)) + 1);
                    int day = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].IndexOf(' ') + 1, bannerInfo[i].IndexOf(',') - (bannerInfo[i].IndexOf(' ') + 1))));
                    int year = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].Length - 4)));
                    DateTime dt = new DateTime(year, month, day, 7, 0, 0);
                    b.StartDate = dt;
                }
                else if (bannerInfo[i].StartsWith("end"))
                {
                    int month = (Array.IndexOf(DateTimeFormatInfo.InvariantInfo.MonthNames, bannerInfo[i].Substring(4, bannerInfo[i].IndexOf(' ') - 4)) + 1);
                    int day = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].IndexOf(' ') + 1, bannerInfo[i].IndexOf(',') - (bannerInfo[i].IndexOf(' ') + 1))));
                    int year = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].IndexOf(',') + 2, 4)));
                    DateTime dt = new DateTime(year, month, day, 7, 0, 0);
                    b.EndDate = dt;
                }
            }
            Summoner.BannersDictionary.TryAdd(b.Title, b);
        }
        if (data.SelectToken("continue") != null)
        {
            string continueString = data.SelectToken("continue").SelectToken("gcmcontinue").ToString();
            string result = WebConnection.GetRequest("https://feheroes.gamepedia.com/api.php?action=query&prop=revisions&rvprop=content&rvsection=0&generator=categorymembers&gcmtitle=Category:Summoning%20Focuses&gcmlimit=50&format=json&continue=gcmcontinue||&gcmcontinue=" + continueString).Result;
            JSONtoBanner(result);
        }
    }
}