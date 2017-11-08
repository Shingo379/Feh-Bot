using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Banner
{
    public string Title;
    public List<Hero> Heroes;
    public DateTime StartDate;
    public DateTime EndDate;
    public double[] rates;

    public Banner()
    {
        Heroes = new List<Hero>();
        rates = new double[4];
    }

    public string BannerToString()
    {
        string timeString = (StartDate < DateTime.UtcNow ? "Time Remaining: " + TimeRemaining(EndDate) : "Banner Begins in: " + TimeRemaining(StartDate));
        return "```" + Title + "\n" + "Focus Heroes: " + String.Join(", ", Heroes.Select(h => h.name)) + '\n' + timeString + "```";
    }

    public string TimeRemaining(DateTime dt)
    {
        TimeSpan span = dt.AddHours(7) - DateTime.UtcNow;
        string remaining = "";
        if (span.Days >= 1)
        {
            remaining += span.Days + "d ";
        }
        if (span.Hours >= 1)
        {
            remaining += span.Hours + "h ";
        }
        if (span.Days == 0 && span.Hours == 0)
        {
            remaining += span.Minutes + "m ";
        }
        return remaining;
    }
}

public static class Summoner
{
    public static ConcurrentDictionary<string, Banner> BannersDictionary;

    static Summoner()
    {
        BannersDictionary = new ConcurrentDictionary<string, Banner>();
    }

    public static async Task<string> Summon(string banner, int times, string color = "")
    {
        if (!BannersDictionary.ContainsKey(banner))
        {
            return "The Summoning Focus \"" + banner + "\" does not exist.";
        }

        int summonedHeroes = 0;
        int summonedColor = 0;
        string summonResultsString = "```";

        Queue<Tuple<string, Hero>> summonResults = new Queue<Tuple<string, Hero>>();
        Banner focusBanner = BannersDictionary[banner];

        double focusBase = focusBanner.rates[0];
        double fiveStarBase = focusBanner.rates[1];
        double fourStarBase = focusBanner.rates[2];
        double threeStarBase = focusBanner.rates[3];
        double focus = focusBase;
        double fiveStar = fiveStarBase;
        double fourStar = fourStarBase;
        double threeStar = threeStarBase;

        Random rng = new Random();
        bool[] raritiesObtained = new bool[4]; //[0] = 3*, [1] = 4*, [2] = 5*, [3] = 5* Focus 
        bool reset = false;
        int rounds = 1 + (times - 1) / 5;

        while (summonedHeroes < times)
        {
            //First calculates the rarity summoned, then picks a random hero from that rarity list.
            double outcome = rng.NextDouble();
            if (outcome <= focus)
            {
                int r = rng.Next(0, focusBanner.Heroes.Count);
                summonResults.Enqueue(new Tuple<string, Hero>("5* Focus: ", focusBanner.Heroes[r]));
                raritiesObtained[3] = true;
                reset = true;
            }
            else if (outcome <= focus + fiveStar)
            {
                int r = rng.Next(0, Heroes.fiveStarHeroes.Count - 1);
                summonResults.Enqueue(new Tuple<string, Hero>("5*: ", Heroes.fiveStarHeroes[r]));
                raritiesObtained[2] = true;
                reset = true;
            }
            else if (outcome <= focus + fiveStar + fourStar)
            {
                int r = rng.Next(0, Heroes.fourStarHeroes.Count - 1);
                summonResults.Enqueue(new Tuple<string, Hero>("4*: ", Heroes.fourStarHeroes[r]));
                raritiesObtained[1] = true;
            }
            else
            {
                int r = rng.Next(0, Heroes.threeStarHeroes.Count - 1);
                summonResults.Enqueue(new Tuple<string, Hero>("3*: ", Heroes.threeStarHeroes[r]));
                raritiesObtained[0] = true;
            }
            summonedHeroes += 1;

            if (summonedHeroes % 5 == 0 || summonedHeroes >= times)
            {
                if (color != "")
                {
                    //If a color is specified, try to pick only that color from a summoning session consisting of 5 Heroes.
                    var matchingHeroes = summonResults.Where(c => c.Item2.color == color);
                    summonedColor += matchingHeroes.Count();
                    summonedHeroes -= (summonedHeroes - summonedColor);
                    string summonSession = String.Join(", ", matchingHeroes.Select(s => s.Item1 + s.Item2.name));
                    if (summonSession == "")
                    {
                        //If the specified color is not summoned within the summoning session, only pick one Hero of a different color.
                        Tuple<string, Hero> h = summonResults.Dequeue();
                        summonSession = h.Item1 + h.Item2.name;
                        summonedColor += 1;
                        summonedHeroes += 1;
                    }
                    summonResultsString += summonSession + "``````";
                }
                else
                {
                    summonResultsString += String.Join(", ", summonResults.Select(s => s.Item1 + s.Item2.name)) + "``````";
                }
                summonResults.Clear();
                if (summonedHeroes % 5 == 0)
                {
                    if (reset)
                    {
                        //If a 5* or 5* Focus character is summoned, the summoning rates return to their base values
                        focus = focusBase;
                        fiveStar = fiveStarBase;
                        fourStar = fourStarBase;
                        reset = false;
                    }
                    else
                    {
                        //If no 5* or 5* Focus Heroes are summoned five times in a row, the rates of getting a 5* and 5* Focus is increased by .5% split between the two.
                        int pityRounds = Convert.ToInt32(((focus - focusBase) + (fiveStar - fiveStarBase)) / .005) + 1;
                        focus = Math.Round(focusBase + (.005 * pityRounds * (focusBase / (focusBase + fiveStarBase))), 4);
                        fiveStar = Math.Round(fiveStarBase + (.005 * pityRounds * (fiveStarBase / (focusBase + fiveStarBase))), 4);
                        fourStar = Math.Round(fourStarBase - (.005 * pityRounds * (fourStarBase / (fourStarBase + threeStarBase))), 4);
                        threeStar = Math.Round(threeStarBase - (.005 * pityRounds * (threeStarBase / (fourStarBase + threeStarBase))), 4);
                    }
                }
            }
        }
        summonResultsString = (summonResults.Count == 0 ? summonResultsString.Remove(summonResultsString.Length - 3) : summonResultsString + String.Join(", ", summonResults.Select(s => s.Item1 + s.Item2.name)) + "```");
        return summonResultsString;
    }
}