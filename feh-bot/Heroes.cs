using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public struct Hero
{
    public string name;
    public string[] rarity;
    public string color;
    public string weapontype;
    public string movetype;
    public bool isSpecialHero;
    public DateTime releaseDate;

    //string array of stats. [0] = ATK, [1] = DEF, [2] = HP, [3] = RES, [4] = SPD
    public string[] R3Lv1Bane;
    public string[] R3Lv1Neut;
    public string[] R3Lv1Boon;
    public string[] R3Lv40Bane;
    public string[] R3Lv40Neut;
    public string[] R3Lv40Boon;

    public string[] R4Lv1Bane;
    public string[] R4Lv1Neut;
    public string[] R4Lv1Boon;
    public string[] R4Lv40Bane;
    public string[] R4Lv40Neut;
    public string[] R4Lv40Boon;

    public string[] R5Lv1Bane;
    public string[] R5Lv1Neut;
    public string[] R5Lv1Boon;
    public string[] R5Lv40Bane;
    public string[] R5Lv40Neut;
    public string[] R5Lv40Boon;

    //A jagged array that references the 3 rarities, the 6 level and Boon/Bane combination array, and the 5 stats within those array.
    public string[][][] stats;

    public string[] weapon;
    public string[] special;
    public string[] assist;
    public string[] passiveA;
    public string[] passiveB;
    public string[] passiveC;

    //A jagged array that references the 6 skill categories, and the skills a Hero learns within those categories.
    public string[][] skills;

    public string[] weaponRarity;
    public string[] specialRarity;
    public string[] assistRarity;
    public string[] passiveARarity;
    public string[] passiveBRarity;
    public string[] passiveCRarity;

    //A jagged array that references the 6 skill categories, and what rarity is required for a hero to learn the skills within those categories.
    public string[][] skillUnlocks;

    public void InitializeArrays()
    {
        R3Lv1Bane = new string[5];
        R3Lv1Neut = new string[5];
        R3Lv1Boon = new string[5];
        R3Lv40Bane = new string[5];
        R3Lv40Neut = new string[5];
        R3Lv40Boon = new string[5];

        R4Lv1Bane = new string[5];
        R4Lv1Neut = new string[5];
        R4Lv1Boon = new string[5];
        R4Lv40Bane = new string[5];
        R4Lv40Neut = new string[5];
        R4Lv40Boon = new string[5];

        R5Lv1Bane = new string[5];
        R5Lv1Neut = new string[5];
        R5Lv1Boon = new string[5];
        R5Lv40Bane = new string[5];
        R5Lv40Neut = new string[5];
        R5Lv40Boon = new string[5];

        stats = new string[3][][]
        {
            new string[6][] 
            {
                R3Lv1Bane , R3Lv1Neut, R3Lv1Boon, R3Lv40Bane, R3Lv40Neut, R3Lv40Boon
            },
            new string[6][]
            {
                R4Lv1Bane , R4Lv1Neut, R4Lv1Boon, R4Lv40Bane, R4Lv40Neut, R4Lv40Boon
            },
            new string[6][]
            {
                R5Lv1Bane , R5Lv1Neut, R5Lv1Boon, R5Lv40Bane, R5Lv40Neut, R5Lv40Boon
            },
        };

        weapon = new string[4] { "", "", "", "" };
        special = new string[3] { "", "", "" };
        assist = new string[3] { "", "", "" };
        passiveA = new string[3];
        passiveB = new string[3];
        passiveC = new string[3];

        skills = new string[6][]
        {
            weapon, special, assist, passiveA, passiveB, passiveC
        };

        weaponRarity = new string[4] { "", "", "", "" };
        specialRarity = new string[3] { "", "", "" };
        assistRarity = new string[3] { "", "", "" };
        passiveARarity = new string[3];
        passiveBRarity = new string[3];
        passiveCRarity = new string[3];

        skillUnlocks = new string[6][]
        {
            weaponRarity, specialRarity, assistRarity, passiveARarity, passiveBRarity, passiveCRarity
        };
    }

    public string GetStatString(int rarity = 5, int level = 40, string mod1 = "", string mod2 = "")
    {
        int rarityIndex = rarity - 3; //index of rarities 3, 4, 5 are 0, 1, 2
        int levelIndex = (level == 40 ? 4 : 1); //index of neutral level 1 and level 40 stats are 1 and 4 respectively
        if (stats[rarityIndex][levelIndex][0] == "")
        {
            return name + " cannot be of rarity " + rarity + "*.";
        }
        string modifierString = (mod1 == "" && mod2 == "" ? "neutral" : mod1 + " " + mod2);
        string ATK = stats[rarityIndex][levelIndex][0];
        string DEF = stats[rarityIndex][levelIndex][1];
        string HP = stats[rarityIndex][levelIndex][2];
        string RES = stats[rarityIndex][levelIndex][3];
        string SPD = stats[rarityIndex][levelIndex][4];
        string[] statline = new string[5] { ATK, DEF, HP, RES, SPD };
        if (mod1 != "")
        {
            try
            {
                int mod = 0;
                if (mod1[0] == '+')
                {
                    mod = 1;
                }
                else if (mod1[0] == '-')
                {
                    mod = -1;
                }
                int modIndex = Heroes.statIndex.IndexOf(mod1.Substring(1));
                if (stats[rarityIndex][levelIndex + mod][modIndex] == "")
                {
                    return name + " cannot have boons and banes.";
                }
                statline[modIndex] = stats[rarityIndex][levelIndex + mod][modIndex];
            }
            catch
            {
                return mod1 + " is an invalid input. Example: +ATK -SPD or +hp -def";
            }
        }
        if (mod2 != "")
        {
            try
            {
                int mod = 0;
                if (mod2[0] == '+')
                {
                    mod = 1;
                }
                else if (mod2[0] == '-')
                {
                    mod = -1;
                }
                int modIndex = Heroes.statIndex.IndexOf(mod2.Substring(1));
                if (stats[rarityIndex][levelIndex + mod][modIndex] == "")
                {
                    return name + " cannot have boons and banes.";
                }
                statline[modIndex] = stats[rarityIndex][levelIndex + mod][modIndex];
            }
            catch
            {
                return mod2 + " is an invalid input. Example: +ATK -SPD or +hp -def";
            }
        }
        string result = name + "'s " + rarity + "* level " + level + " " + modifierString + " stats are\n" + "HP: " + statline[2] + " ATK: " + statline[0] + " SPD: " + statline[4] + " DEF: " + statline[1] + " RES: " + statline[3];
        return result;
    }
}

public static class Heroes
{
    public static List<string> statIndex = new List<string>()
    {
        "ATK", "DEF", "HP", "RES", "SPD"
    };
    public static ConcurrentDictionary<string, Hero> HeroDictionary;
    public static List<Hero> threeStarHeroes;
    public static List<Hero> fourStarHeroes;
    public static List<Hero> fiveStarHeroes;

    static Heroes()
    {
        HeroDictionary = new ConcurrentDictionary<string, Hero>();
    }

    public static void GetHeroRarityLists()
    {
        threeStarHeroes = HeroDictionary.Where(kv => kv.Value.rarity.Contains("3")).Select(kv => kv.Value).ToList();
        fourStarHeroes = HeroDictionary.Where(kv => kv.Value.rarity.Contains("4")).Select(kv => kv.Value).ToList();
        fiveStarHeroes = HeroDictionary.Where(kv => kv.Value.rarity.Contains("5") && !kv.Value.isSpecialHero).Select(kv => kv.Value).ToList();
    }

    public static string GetHero(string name, int rarity, int level, string mod1, string mod2)
    {
        if (HeroDictionary.ContainsKey(name))
        {
            return HeroDictionary[name].GetStatString(rarity, level, mod1, mod2);
        }
        else
        {
            //If a directly matching Hero name is not found, attempt to split the entered name and search for Heroes that contain part of the entered name.
            string returnString = "Did you mean:\n";
            List<String> matchingNames = new List<string>();
            string[] splitName = name.Split(new char[] { ' ', '.', '-', '!' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (KeyValuePair<string, Hero> kv in HeroDictionary)
            {
                foreach (string s in splitName)
                {
                    if (Regex.IsMatch(kv.Key, @"\b" + s + @"\b"))
                    {
                        matchingNames.Add(kv.Key);
                    }
                }
            }
            if (matchingNames.Count == 0)
            {
                return "No character named " + name + " exists.";
            }
            if (matchingNames.Count != matchingNames.Distinct().Count())
            {
                string[] doubleMatch = matchingNames.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
                if (doubleMatch.Length == 1)
                {
                    return GetHero(doubleMatch[0], rarity, level, mod1, mod2);
                }
                return returnString += String.Join('\n', matchingNames.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key));
            }
            else
            {
                return returnString += String.Join('\n', matchingNames);
            }
        }
    }

    public static Queue<string> GetSkill(string name)
    {
        Queue<string> returnStrings = new Queue<string>();
        string returnString = "";
        var heroes = HeroDictionary.Select(kv => kv.Value).OrderBy(h => h.name);
        foreach (Hero h in heroes)
        {
            foreach (var si in (h.skills.Select((skilltype, index) => new { skilltype, index }).Where(s => s.skilltype.Any(c => c.Contains(name)))))
            {
                returnString += h.name + ":\n```";
                for (int i = 0; i < si.skilltype.Length; i++)
                {
                    if (si.skilltype[i] == "")
                    {
                        continue;
                    }
                    string skillUnlock;
                    if (h.skillUnlocks[si.index][i] == "")
                    {
                        skillUnlock = "-";
                    }
                    else
                    {
                        skillUnlock = (Int32.Parse(h.skillUnlocks[si.index][i]) < 3 ? "3*" : h.skillUnlocks[si.index][i] + "*");
                    }
                    returnString += (si.skilltype[i] + ": " + skillUnlock + '\n');
                }
                returnString += "```";
                if (returnString.Length >= 1800)
                {
                    returnStrings.Enqueue(returnString);
                    returnString = "";
                }
            }
        }
        if (returnString != "")
        {
            returnStrings.Enqueue(returnString);
        }
        return returnStrings;
    }
}