using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebConnection
{
    static HttpClient client = new HttpClient();

    static async Task MainAsync()
    {
        client.BaseAddress = new Uri("http://localhost:55268/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public static async Task GetAllCharacters()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync(
        "https://feheroes.gamepedia.com/api.php?action=ask&query=[[Category:Heroes]]|?Has_Lv1_R5_ATK_Bane=Lv1_R5_ATK_Bane|?Has_Lv1_R5_DEF_Bane=Lv1_R5_DEF_Bane|?Has_Lv1_R5_HP_Bane=Lv1_R5_HP_Bane|?Has_Lv1_R5_RES_Bane=Lv1_R5_RES_Bane|?Has_Lv1_R5_SPD_Bane=Lv1_R5_SPD_Bane|?Has_Lv1_R5_ATK_Neut=Lv1_R5_ATK_Neut|?Has_Lv1_R5_DEF_Neut=Lv1_R5_DEF_Neut|?Has_Lv1_R5_HP_Neut=Lv1_R5_HP_Neut|?Has_Lv1_R5_RES_Neut=Lv1_R5_RES_Neut|?Has_Lv1_R5_SPD_Neut=Lv1_R5_SPD_Neut|?Has_Lv1_R5_ATK_Boon=Lv1_R5_ATK_Boon|?Has_Lv1_R5_DEF_Boon=Lv1_R5_DEF_Boon|?Has_Lv1_R5_HP_Boon=Lv1_R5_HP_Boon|?Has_Lv1_R5_RES_Boon=Lv1_R5_RES_Boon|?Has_Lv1_R5_SPD_Boon=Lv1_R5_SPD_Boon|?Has_Lv1_R4_ATK_Bane=Lv1_R4_ATK_Bane|?Has_Lv1_R4_DEF_Bane=Lv1_R4_DEF_Bane|?Has_Lv1_R4_HP_Bane=Lv1_R4_HP_Bane|?Has_Lv1_R4_RES_Bane=Lv1_R4_RES_Bane|?Has_Lv1_R4_SPD_Bane=Lv1_R4_SPD_Bane|?Has_Lv1_R4_ATK_Neut=Lv1_R4_ATK_Neut|?Has_Lv1_R4_DEF_Neut=Lv1_R4_DEF_Neut|?Has_Lv1_R4_HP_Neut=Lv1_R4_HP_Neut|?Has_Lv1_R4_RES_Neut=Lv1_R4_RES_Neut|?Has_Lv1_R4_SPD_Neut=Lv1_R4_SPD_Neut|?Has_Lv1_R4_ATK_Boon=Lv1_R4_ATK_Boon|?Has_Lv1_R4_DEF_Boon=Lv1_R4_DEF_Boon|?Has_Lv1_R4_HP_Boon=Lv1_R4_HP_Boon|?Has_Lv1_R4_RES_Boon=Lv1_R4_RES_Boon|?Has_Lv1_R4_SPD_Boon=Lv1_R4_SPD_Boon|?Has_Lv1_R3_ATK_Bane=Lv1_R3_ATK_Bane|?Has_Lv1_R3_DEF_Bane=Lv1_R3_DEF_Bane|?Has_Lv1_R3_HP_Bane=Lv1_R3_HP_Bane|?Has_Lv1_R3_RES_Bane=Lv1_R3_RES_Bane|?Has_Lv1_R3_SPD_Bane=Lv1_R3_SPD_Bane|?Has_Lv1_R3_ATK_Neut=Lv1_R3_ATK_Neut|?Has_Lv1_R3_DEF_Neut=Lv1_R3_DEF_Neut|?Has_Lv1_R3_HP_Neut=Lv1_R3_HP_Neut|?Has_Lv1_R3_RES_Neut=Lv1_R3_RES_Neut|?Has_Lv1_R3_SPD_Neut=Lv1_R3_SPD_Neut|?Has_Lv1_R3_ATK_Boon=Lv1_R3_ATK_Boon|?Has_Lv1_R3_DEF_Boon=Lv1_R3_DEF_Boon|?Has_Lv1_R3_HP_Boon=Lv1_R3_HP_Boon|?Has_Lv1_R3_RES_Boon=Lv1_R3_RES_Boon|?Has_Lv1_R3_SPD_Boon=Lv1_R3_SPD_Boon|?Has_Lv40_R5_ATK_Bane=Lv40_R5_ATK_Bane|?Has_Lv40_R5_DEF_Bane=Lv40_R5_DEF_Bane|?Has_Lv40_R5_HP_Bane=Lv40_R5_HP_Bane|?Has_Lv40_R5_RES_Bane=Lv40_R5_RES_Bane|?Has_Lv40_R5_SPD_Bane=Lv40_R5_SPD_Bane|?Has_Lv40_R5_ATK_Neut=Lv40_R5_ATK_Neut|?Has_Lv40_R5_DEF_Neut=Lv40_R5_DEF_Neut|?Has_Lv40_R5_HP_Neut=Lv40_R5_HP_Neut|?Has_Lv40_R5_RES_Neut=Lv40_R5_RES_Neut|?Has_Lv40_R5_SPD_Neut=Lv40_R5_SPD_Neut|?Has_Lv40_R5_ATK_Boon=Lv40_R5_ATK_Boon|?Has_Lv40_R5_DEF_Boon=Lv40_R5_DEF_Boon|?Has_Lv40_R5_HP_Boon=Lv40_R5_HP_Boon|?Has_Lv40_R5_RES_Boon=Lv40_R5_RES_Boon|?Has_Lv40_R5_SPD_Boon=Lv40_R5_SPD_Boon|?Has_Lv40_R4_ATK_Bane=Lv40_R4_ATK_Bane|?Has_Lv40_R4_DEF_Bane=Lv40_R4_DEF_Bane|?Has_Lv40_R4_HP_Bane=Lv40_R4_HP_Bane|?Has_Lv40_R4_RES_Bane=Lv40_R4_RES_Bane|?Has_Lv40_R4_SPD_Bane=Lv40_R4_SPD_Bane|?Has_Lv40_R4_ATK_Neut=Lv40_R4_ATK_Neut|?Has_Lv40_R4_DEF_Neut=Lv40_R4_DEF_Neut|?Has_Lv40_R4_HP_Neut=Lv40_R4_HP_Neut|?Has_Lv40_R4_RES_Neut=Lv40_R4_RES_Neut|?Has_Lv40_R4_SPD_Neut=Lv40_R4_SPD_Neut|?Has_Lv40_R4_ATK_Boon=Lv40_R4_ATK_Boon|?Has_Lv40_R4_DEF_Boon=Lv40_R4_DEF_Boon|?Has_Lv40_R4_HP_Boon=Lv40_R4_HP_Boon|?Has_Lv40_R4_RES_Boon=Lv40_R4_RES_Boon|?Has_Lv40_R4_SPD_Boon=Lv40_R4_SPD_Boon|?Has_Lv40_R3_ATK_Bane=Lv40_R3_ATK_Bane|?Has_Lv40_R3_DEF_Bane=Lv40_R3_DEF_Bane|?Has_Lv40_R3_HP_Bane=Lv40_R3_HP_Bane|?Has_Lv40_R3_RES_Bane=Lv40_R3_RES_Bane|?Has_Lv40_R3_SPD_Bane=Lv40_R3_SPD_Bane|?Has_Lv40_R3_ATK_Neut=Lv40_R3_ATK_Neut|?Has_Lv40_R3_DEF_Neut=Lv40_R3_DEF_Neut|?Has_Lv40_R3_HP_Neut=Lv40_R3_HP_Neut|?Has_Lv40_R3_RES_Neut=Lv40_R3_RES_Neut|?Has_Lv40_R3_SPD_Neut=Lv40_R3_SPD_Neut|?Has_Lv40_R3_ATK_Boon=Lv40_R3_ATK_Boon|?Has_Lv40_R3_DEF_Boon=Lv40_R3_DEF_Boon|?Has_Lv40_R3_HP_Boon=Lv40_R3_HP_Boon|?Has_Lv40_R3_RES_Boon=Lv40_R3_RES_Boon|?Has_Lv40_R3_SPD_Boon=Lv40_R3_SPD_Boon|?Has_weapon1=weapon1|?Has_weapon2=weapon2|?Has_weapon3=weapon3|?Has_weapon4=weapon4|?Has_weapon1_unlock=weapon1_unlock|?Has_weapon2_unlock=weapon2_unlock|?Has_weapon3_unlock=weapon3_unlock|?Has_weapon4_unlock=weapon4_unlock|?Has_weapon1_default=weapon1_default|?Has_weapon2_default=weapon2_default|?Has_weapon3_default=weapon3_default|?Has_weapon4_default=weapon4_default|?Has_special1=special1|?Has_special2=special2|?Has_special3=special3|?Has_special1_unlock=special1_unlock|?Has_special2_unlock=special2_unlock|?Has_special3_unlock=special3_unlock|?Has_special1_default=special1_default|?Has_special2_default=special2_default|?Has_special3_default=special3_default|?Has_assist1=assist1|?Has_assist2=assist2|?Has_assist3=assist3|?Has_assist1_unlock=assist1_unlock|?Has_assist2_unlock=assist2_unlock|?Has_assist3_unlock=assist3_unlock|?Has_assist1_default=assist1_default|?Has_assist2_default=assist2_default|?Has_assist3_default=assist3_default|?Has_passiveA1=passiveA1|?Has_passiveA2=passiveA2|?Has_passiveA3=passiveA3|?Has_passiveA1_unlock=passiveA1_unlock|?Has_passiveA2_unlock=passiveA2_unlock|?Has_passiveA3_unlock=passiveA3_unlock|?Has_passiveB1=passiveB1|?Has_passiveB2=passiveB2|?Has_passiveB3=passiveB3|?Has_passiveB1_unlock=passiveB1_unlock|?Has_passiveB2_unlock=passiveB2_unlock|?Has_passiveB3_unlock=passiveB3_unlock|?Has_passiveC1=passiveC1|?Has_passiveC2=passiveC2|?Has_passiveC3=passiveC3|?Has_passiveC1_unlock=passiveC1_unlock|?Has_passiveC2_unlock=passiveC2_unlock|?Has_passiveC3_unlock=passiveC3_unlock|?Is_Special_Hero=Is_Special_Hero|?SummonRarities=SummonRarities|?WeaponType=WeaponType|?MoveType=MoveType|?Has_release_date=releaseDate|limit=500&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JObject data = JObject.Parse(result);
        JSONtoHero(data);

        while ((data.SelectToken("query-continue-offset")) != null)
        {
            response = await client.GetAsync(
            "https://feheroes.gamepedia.com/api.php?action=ask&query=[[Category:Heroes]]|?Has_Lv1_R5_ATK_Bane=Lv1_R5_ATK_Bane|?Has_Lv1_R5_DEF_Bane=Lv1_R5_DEF_Bane|?Has_Lv1_R5_HP_Bane=Lv1_R5_HP_Bane|?Has_Lv1_R5_RES_Bane=Lv1_R5_RES_Bane|?Has_Lv1_R5_SPD_Bane=Lv1_R5_SPD_Bane|?Has_Lv1_R5_ATK_Neut=Lv1_R5_ATK_Neut|?Has_Lv1_R5_DEF_Neut=Lv1_R5_DEF_Neut|?Has_Lv1_R5_HP_Neut=Lv1_R5_HP_Neut|?Has_Lv1_R5_RES_Neut=Lv1_R5_RES_Neut|?Has_Lv1_R5_SPD_Neut=Lv1_R5_SPD_Neut|?Has_Lv1_R5_ATK_Boon=Lv1_R5_ATK_Boon|?Has_Lv1_R5_DEF_Boon=Lv1_R5_DEF_Boon|?Has_Lv1_R5_HP_Boon=Lv1_R5_HP_Boon|?Has_Lv1_R5_RES_Boon=Lv1_R5_RES_Boon|?Has_Lv1_R5_SPD_Boon=Lv1_R5_SPD_Boon|?Has_Lv1_R4_ATK_Bane=Lv1_R4_ATK_Bane|?Has_Lv1_R4_DEF_Bane=Lv1_R4_DEF_Bane|?Has_Lv1_R4_HP_Bane=Lv1_R4_HP_Bane|?Has_Lv1_R4_RES_Bane=Lv1_R4_RES_Bane|?Has_Lv1_R4_SPD_Bane=Lv1_R4_SPD_Bane|?Has_Lv1_R4_ATK_Neut=Lv1_R4_ATK_Neut|?Has_Lv1_R4_DEF_Neut=Lv1_R4_DEF_Neut|?Has_Lv1_R4_HP_Neut=Lv1_R4_HP_Neut|?Has_Lv1_R4_RES_Neut=Lv1_R4_RES_Neut|?Has_Lv1_R4_SPD_Neut=Lv1_R4_SPD_Neut|?Has_Lv1_R4_ATK_Boon=Lv1_R4_ATK_Boon|?Has_Lv1_R4_DEF_Boon=Lv1_R4_DEF_Boon|?Has_Lv1_R4_HP_Boon=Lv1_R4_HP_Boon|?Has_Lv1_R4_RES_Boon=Lv1_R4_RES_Boon|?Has_Lv1_R4_SPD_Boon=Lv1_R4_SPD_Boon|?Has_Lv1_R3_ATK_Bane=Lv1_R3_ATK_Bane|?Has_Lv1_R3_DEF_Bane=Lv1_R3_DEF_Bane|?Has_Lv1_R3_HP_Bane=Lv1_R3_HP_Bane|?Has_Lv1_R3_RES_Bane=Lv1_R3_RES_Bane|?Has_Lv1_R3_SPD_Bane=Lv1_R3_SPD_Bane|?Has_Lv1_R3_ATK_Neut=Lv1_R3_ATK_Neut|?Has_Lv1_R3_DEF_Neut=Lv1_R3_DEF_Neut|?Has_Lv1_R3_HP_Neut=Lv1_R3_HP_Neut|?Has_Lv1_R3_RES_Neut=Lv1_R3_RES_Neut|?Has_Lv1_R3_SPD_Neut=Lv1_R3_SPD_Neut|?Has_Lv1_R3_ATK_Boon=Lv1_R3_ATK_Boon|?Has_Lv1_R3_DEF_Boon=Lv1_R3_DEF_Boon|?Has_Lv1_R3_HP_Boon=Lv1_R3_HP_Boon|?Has_Lv1_R3_RES_Boon=Lv1_R3_RES_Boon|?Has_Lv1_R3_SPD_Boon=Lv1_R3_SPD_Boon|?Has_Lv40_R5_ATK_Bane=Lv40_R5_ATK_Bane|?Has_Lv40_R5_DEF_Bane=Lv40_R5_DEF_Bane|?Has_Lv40_R5_HP_Bane=Lv40_R5_HP_Bane|?Has_Lv40_R5_RES_Bane=Lv40_R5_RES_Bane|?Has_Lv40_R5_SPD_Bane=Lv40_R5_SPD_Bane|?Has_Lv40_R5_ATK_Neut=Lv40_R5_ATK_Neut|?Has_Lv40_R5_DEF_Neut=Lv40_R5_DEF_Neut|?Has_Lv40_R5_HP_Neut=Lv40_R5_HP_Neut|?Has_Lv40_R5_RES_Neut=Lv40_R5_RES_Neut|?Has_Lv40_R5_SPD_Neut=Lv40_R5_SPD_Neut|?Has_Lv40_R5_ATK_Boon=Lv40_R5_ATK_Boon|?Has_Lv40_R5_DEF_Boon=Lv40_R5_DEF_Boon|?Has_Lv40_R5_HP_Boon=Lv40_R5_HP_Boon|?Has_Lv40_R5_RES_Boon=Lv40_R5_RES_Boon|?Has_Lv40_R5_SPD_Boon=Lv40_R5_SPD_Boon|?Has_Lv40_R4_ATK_Bane=Lv40_R4_ATK_Bane|?Has_Lv40_R4_DEF_Bane=Lv40_R4_DEF_Bane|?Has_Lv40_R4_HP_Bane=Lv40_R4_HP_Bane|?Has_Lv40_R4_RES_Bane=Lv40_R4_RES_Bane|?Has_Lv40_R4_SPD_Bane=Lv40_R4_SPD_Bane|?Has_Lv40_R4_ATK_Neut=Lv40_R4_ATK_Neut|?Has_Lv40_R4_DEF_Neut=Lv40_R4_DEF_Neut|?Has_Lv40_R4_HP_Neut=Lv40_R4_HP_Neut|?Has_Lv40_R4_RES_Neut=Lv40_R4_RES_Neut|?Has_Lv40_R4_SPD_Neut=Lv40_R4_SPD_Neut|?Has_Lv40_R4_ATK_Boon=Lv40_R4_ATK_Boon|?Has_Lv40_R4_DEF_Boon=Lv40_R4_DEF_Boon|?Has_Lv40_R4_HP_Boon=Lv40_R4_HP_Boon|?Has_Lv40_R4_RES_Boon=Lv40_R4_RES_Boon|?Has_Lv40_R4_SPD_Boon=Lv40_R4_SPD_Boon|?Has_Lv40_R3_ATK_Bane=Lv40_R3_ATK_Bane|?Has_Lv40_R3_DEF_Bane=Lv40_R3_DEF_Bane|?Has_Lv40_R3_HP_Bane=Lv40_R3_HP_Bane|?Has_Lv40_R3_RES_Bane=Lv40_R3_RES_Bane|?Has_Lv40_R3_SPD_Bane=Lv40_R3_SPD_Bane|?Has_Lv40_R3_ATK_Neut=Lv40_R3_ATK_Neut|?Has_Lv40_R3_DEF_Neut=Lv40_R3_DEF_Neut|?Has_Lv40_R3_HP_Neut=Lv40_R3_HP_Neut|?Has_Lv40_R3_RES_Neut=Lv40_R3_RES_Neut|?Has_Lv40_R3_SPD_Neut=Lv40_R3_SPD_Neut|?Has_Lv40_R3_ATK_Boon=Lv40_R3_ATK_Boon|?Has_Lv40_R3_DEF_Boon=Lv40_R3_DEF_Boon|?Has_Lv40_R3_HP_Boon=Lv40_R3_HP_Boon|?Has_Lv40_R3_RES_Boon=Lv40_R3_RES_Boon|?Has_Lv40_R3_SPD_Boon=Lv40_R3_SPD_Boon|?Has_weapon1=weapon1|?Has_weapon2=weapon2|?Has_weapon3=weapon3|?Has_weapon4=weapon4|?Has_weapon1_unlock=weapon1_unlock|?Has_weapon2_unlock=weapon2_unlock|?Has_weapon3_unlock=weapon3_unlock|?Has_weapon4_unlock=weapon4_unlock|?Has_weapon1_default=weapon1_default|?Has_weapon2_default=weapon2_default|?Has_weapon3_default=weapon3_default|?Has_weapon4_default=weapon4_default|?Has_special1=special1|?Has_special2=special2|?Has_special3=special3|?Has_special1_unlock=special1_unlock|?Has_special2_unlock=special2_unlock|?Has_special3_unlock=special3_unlock|?Has_special1_default=special1_default|?Has_special2_default=special2_default|?Has_special3_default=special3_default|?Has_assist1=assist1|?Has_assist2=assist2|?Has_assist3=assist3|?Has_assist1_unlock=assist1_unlock|?Has_assist2_unlock=assist2_unlock|?Has_assist3_unlock=assist3_unlock|?Has_assist1_default=assist1_default|?Has_assist2_default=assist2_default|?Has_assist3_default=assist3_default|?Has_passiveA1=passiveA1|?Has_passiveA2=passiveA2|?Has_passiveA3=passiveA3|?Has_passiveA1_unlock=passiveA1_unlock|?Has_passiveA2_unlock=passiveA2_unlock|?Has_passiveA3_unlock=passiveA3_unlock|?Has_passiveB1=passiveB1|?Has_passiveB2=passiveB2|?Has_passiveB3=passiveB3|?Has_passiveB1_unlock=passiveB1_unlock|?Has_passiveB2_unlock=passiveB2_unlock|?Has_passiveB3_unlock=passiveB3_unlock|?Has_passiveC1=passiveC1|?Has_passiveC2=passiveC2|?Has_passiveC3=passiveC3|?Has_passiveC1_unlock=passiveC1_unlock|?Has_passiveC2_unlock=passiveC2_unlock|?Has_passiveC3_unlock=passiveC3_unlock|?Is_Special_Hero=Is_Special_Hero|?SummonRarities=SummonRarities|?WeaponType=WeaponType|?MoveType=MoveType|?Has_release_date=releaseDate|limit=500|offset= " + (data.SelectToken("query-continue-offset")) + "&format=json"
            );
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            data = JObject.Parse(result);
            JSONtoHero(data);
        }
    }

    public static async Task GetNewCharacters()
    {
        string result = null;
        string date = DateTime.UtcNow.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
        HttpResponseMessage response = await client.GetAsync(
        "https://feheroes.gamepedia.com/api.php?action=ask&query=[[Category:Heroes]][[Has_release_date::>" + date + "]]|?Has_Lv1_R5_ATK_Bane=Lv1_R5_ATK_Bane|?Has_Lv1_R5_DEF_Bane=Lv1_R5_DEF_Bane|?Has_Lv1_R5_HP_Bane=Lv1_R5_HP_Bane|?Has_Lv1_R5_RES_Bane=Lv1_R5_RES_Bane|?Has_Lv1_R5_SPD_Bane=Lv1_R5_SPD_Bane|?Has_Lv1_R5_ATK_Neut=Lv1_R5_ATK_Neut|?Has_Lv1_R5_DEF_Neut=Lv1_R5_DEF_Neut|?Has_Lv1_R5_HP_Neut=Lv1_R5_HP_Neut|?Has_Lv1_R5_RES_Neut=Lv1_R5_RES_Neut|?Has_Lv1_R5_SPD_Neut=Lv1_R5_SPD_Neut|?Has_Lv1_R5_ATK_Boon=Lv1_R5_ATK_Boon|?Has_Lv1_R5_DEF_Boon=Lv1_R5_DEF_Boon|?Has_Lv1_R5_HP_Boon=Lv1_R5_HP_Boon|?Has_Lv1_R5_RES_Boon=Lv1_R5_RES_Boon|?Has_Lv1_R5_SPD_Boon=Lv1_R5_SPD_Boon|?Has_Lv1_R4_ATK_Bane=Lv1_R4_ATK_Bane|?Has_Lv1_R4_DEF_Bane=Lv1_R4_DEF_Bane|?Has_Lv1_R4_HP_Bane=Lv1_R4_HP_Bane|?Has_Lv1_R4_RES_Bane=Lv1_R4_RES_Bane|?Has_Lv1_R4_SPD_Bane=Lv1_R4_SPD_Bane|?Has_Lv1_R4_ATK_Neut=Lv1_R4_ATK_Neut|?Has_Lv1_R4_DEF_Neut=Lv1_R4_DEF_Neut|?Has_Lv1_R4_HP_Neut=Lv1_R4_HP_Neut|?Has_Lv1_R4_RES_Neut=Lv1_R4_RES_Neut|?Has_Lv1_R4_SPD_Neut=Lv1_R4_SPD_Neut|?Has_Lv1_R4_ATK_Boon=Lv1_R4_ATK_Boon|?Has_Lv1_R4_DEF_Boon=Lv1_R4_DEF_Boon|?Has_Lv1_R4_HP_Boon=Lv1_R4_HP_Boon|?Has_Lv1_R4_RES_Boon=Lv1_R4_RES_Boon|?Has_Lv1_R4_SPD_Boon=Lv1_R4_SPD_Boon|?Has_Lv1_R3_ATK_Bane=Lv1_R3_ATK_Bane|?Has_Lv1_R3_DEF_Bane=Lv1_R3_DEF_Bane|?Has_Lv1_R3_HP_Bane=Lv1_R3_HP_Bane|?Has_Lv1_R3_RES_Bane=Lv1_R3_RES_Bane|?Has_Lv1_R3_SPD_Bane=Lv1_R3_SPD_Bane|?Has_Lv1_R3_ATK_Neut=Lv1_R3_ATK_Neut|?Has_Lv1_R3_DEF_Neut=Lv1_R3_DEF_Neut|?Has_Lv1_R3_HP_Neut=Lv1_R3_HP_Neut|?Has_Lv1_R3_RES_Neut=Lv1_R3_RES_Neut|?Has_Lv1_R3_SPD_Neut=Lv1_R3_SPD_Neut|?Has_Lv1_R3_ATK_Boon=Lv1_R3_ATK_Boon|?Has_Lv1_R3_DEF_Boon=Lv1_R3_DEF_Boon|?Has_Lv1_R3_HP_Boon=Lv1_R3_HP_Boon|?Has_Lv1_R3_RES_Boon=Lv1_R3_RES_Boon|?Has_Lv1_R3_SPD_Boon=Lv1_R3_SPD_Boon|?Has_Lv40_R5_ATK_Bane=Lv40_R5_ATK_Bane|?Has_Lv40_R5_DEF_Bane=Lv40_R5_DEF_Bane|?Has_Lv40_R5_HP_Bane=Lv40_R5_HP_Bane|?Has_Lv40_R5_RES_Bane=Lv40_R5_RES_Bane|?Has_Lv40_R5_SPD_Bane=Lv40_R5_SPD_Bane|?Has_Lv40_R5_ATK_Neut=Lv40_R5_ATK_Neut|?Has_Lv40_R5_DEF_Neut=Lv40_R5_DEF_Neut|?Has_Lv40_R5_HP_Neut=Lv40_R5_HP_Neut|?Has_Lv40_R5_RES_Neut=Lv40_R5_RES_Neut|?Has_Lv40_R5_SPD_Neut=Lv40_R5_SPD_Neut|?Has_Lv40_R5_ATK_Boon=Lv40_R5_ATK_Boon|?Has_Lv40_R5_DEF_Boon=Lv40_R5_DEF_Boon|?Has_Lv40_R5_HP_Boon=Lv40_R5_HP_Boon|?Has_Lv40_R5_RES_Boon=Lv40_R5_RES_Boon|?Has_Lv40_R5_SPD_Boon=Lv40_R5_SPD_Boon|?Has_Lv40_R4_ATK_Bane=Lv40_R4_ATK_Bane|?Has_Lv40_R4_DEF_Bane=Lv40_R4_DEF_Bane|?Has_Lv40_R4_HP_Bane=Lv40_R4_HP_Bane|?Has_Lv40_R4_RES_Bane=Lv40_R4_RES_Bane|?Has_Lv40_R4_SPD_Bane=Lv40_R4_SPD_Bane|?Has_Lv40_R4_ATK_Neut=Lv40_R4_ATK_Neut|?Has_Lv40_R4_DEF_Neut=Lv40_R4_DEF_Neut|?Has_Lv40_R4_HP_Neut=Lv40_R4_HP_Neut|?Has_Lv40_R4_RES_Neut=Lv40_R4_RES_Neut|?Has_Lv40_R4_SPD_Neut=Lv40_R4_SPD_Neut|?Has_Lv40_R4_ATK_Boon=Lv40_R4_ATK_Boon|?Has_Lv40_R4_DEF_Boon=Lv40_R4_DEF_Boon|?Has_Lv40_R4_HP_Boon=Lv40_R4_HP_Boon|?Has_Lv40_R4_RES_Boon=Lv40_R4_RES_Boon|?Has_Lv40_R4_SPD_Boon=Lv40_R4_SPD_Boon|?Has_Lv40_R3_ATK_Bane=Lv40_R3_ATK_Bane|?Has_Lv40_R3_DEF_Bane=Lv40_R3_DEF_Bane|?Has_Lv40_R3_HP_Bane=Lv40_R3_HP_Bane|?Has_Lv40_R3_RES_Bane=Lv40_R3_RES_Bane|?Has_Lv40_R3_SPD_Bane=Lv40_R3_SPD_Bane|?Has_Lv40_R3_ATK_Neut=Lv40_R3_ATK_Neut|?Has_Lv40_R3_DEF_Neut=Lv40_R3_DEF_Neut|?Has_Lv40_R3_HP_Neut=Lv40_R3_HP_Neut|?Has_Lv40_R3_RES_Neut=Lv40_R3_RES_Neut|?Has_Lv40_R3_SPD_Neut=Lv40_R3_SPD_Neut|?Has_Lv40_R3_ATK_Boon=Lv40_R3_ATK_Boon|?Has_Lv40_R3_DEF_Boon=Lv40_R3_DEF_Boon|?Has_Lv40_R3_HP_Boon=Lv40_R3_HP_Boon|?Has_Lv40_R3_RES_Boon=Lv40_R3_RES_Boon|?Has_Lv40_R3_SPD_Boon=Lv40_R3_SPD_Boon|?Has_weapon1=weapon1|?Has_weapon2=weapon2|?Has_weapon3=weapon3|?Has_weapon4=weapon4|?Has_weapon1_unlock=weapon1_unlock|?Has_weapon2_unlock=weapon2_unlock|?Has_weapon3_unlock=weapon3_unlock|?Has_weapon4_unlock=weapon4_unlock|?Has_weapon1_default=weapon1_default|?Has_weapon2_default=weapon2_default|?Has_weapon3_default=weapon3_default|?Has_weapon4_default=weapon4_default|?Has_special1=special1|?Has_special2=special2|?Has_special3=special3|?Has_special1_unlock=special1_unlock|?Has_special2_unlock=special2_unlock|?Has_special3_unlock=special3_unlock|?Has_special1_default=special1_default|?Has_special2_default=special2_default|?Has_special3_default=special3_default|?Has_assist1=assist1|?Has_assist2=assist2|?Has_assist3=assist3|?Has_assist1_unlock=assist1_unlock|?Has_assist2_unlock=assist2_unlock|?Has_assist3_unlock=assist3_unlock|?Has_assist1_default=assist1_default|?Has_assist2_default=assist2_default|?Has_assist3_default=assist3_default|?Has_passiveA1=passiveA1|?Has_passiveA2=passiveA2|?Has_passiveA3=passiveA3|?Has_passiveA1_unlock=passiveA1_unlock|?Has_passiveA2_unlock=passiveA2_unlock|?Has_passiveA3_unlock=passiveA3_unlock|?Has_passiveB1=passiveB1|?Has_passiveB2=passiveB2|?Has_passiveB3=passiveB3|?Has_passiveB1_unlock=passiveB1_unlock|?Has_passiveB2_unlock=passiveB2_unlock|?Has_passiveB3_unlock=passiveB3_unlock|?Has_passiveC1=passiveC1|?Has_passiveC2=passiveC2|?Has_passiveC3=passiveC3|?Has_passiveC1_unlock=passiveC1_unlock|?Has_passiveC2_unlock=passiveC2_unlock|?Has_passiveC3_unlock=passiveC3_unlock|?Is_Special_Hero=Is_Special_Hero|?SummonRarities=SummonRarities|?WeaponType=WeaponType|?MoveType=MoveType|?Has_release_date=releaseDate|limit=500&format=json"
        );
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JObject data = JObject.Parse(result);
        JSONtoHero(data);
    }

    private static void JSONtoHero(JObject data)
    {
        foreach (JProperty j in data.SelectToken("query.results"))
        {
            if (Heroes.HeroDictionary.ContainsKey(j.Name))
            {
                Console.WriteLine("Hero already exists");
                continue;
            }
            Hero hero = new Hero();
            hero.InitializeArrays();
            JToken token = j.First.SelectToken("printouts");
            if (!token.SelectToken("releaseDate").HasValues)
            {
                continue;
            }

            hero.name = j.Name;
            hero.rarity = (token.SelectToken("SummonRarities").HasValues ? token.SelectToken("SummonRarities").Select(t => t.ToString()).ToArray() : new string[3]);
            hero.weapontype = token.SelectToken("WeaponType[0]").ToString();
            hero.color = hero.weapontype.Substring(0, hero.weapontype.IndexOf(" "));
            hero.movetype = token.SelectToken("MoveType[0]").ToString();
            hero.isSpecialHero = (token.SelectToken("Is_Special_Hero[0]").ToString() == "t" ? true : false);
            string[] date = token.SelectToken("releaseDate[0].raw").ToString().Split('/');
            hero.releaseDate = new DateTime(Int32.Parse(date[1]), Int32.Parse(date[2]), Int32.Parse(date[3]), 7, 0, 0);
            for (int i = 0; i < 5; i++)
            {
                string stat = Heroes.statIndex[i];
                hero.R3Lv1Bane[i] = (token.SelectToken("Lv1_R3_" + stat + "_Bane").HasValues ? token.SelectToken("Lv1_R3_" + stat + "_Bane[0]").ToString() : "");
                hero.R3Lv1Neut[i] = (token.SelectToken("Lv1_R3_" + stat + "_Neut").HasValues ? token.SelectToken("Lv1_R3_" + stat + "_Neut[0]").ToString() : "");
                hero.R3Lv1Boon[i] = (token.SelectToken("Lv1_R3_" + stat + "_Boon").HasValues ? token.SelectToken("Lv1_R3_" + stat + "_Boon[0]").ToString() : "");
                hero.R3Lv40Bane[i] = (token.SelectToken("Lv40_R3_" + stat + "_Bane").HasValues ? token.SelectToken("Lv40_R3_" + stat + "_Bane[0]").ToString() : "");
                hero.R3Lv40Neut[i] = (token.SelectToken("Lv40_R3_" + stat + "_Neut").HasValues ? token.SelectToken("Lv40_R3_" + stat + "_Neut[0]").ToString() : "");
                hero.R3Lv40Boon[i] = (token.SelectToken("Lv40_R3_" + stat + "_Boon").HasValues ? token.SelectToken("Lv40_R3_" + stat + "_Boon[0]").ToString() : "");

                hero.R4Lv1Bane[i] = (token.SelectToken("Lv1_R4_" + stat + "_Bane").HasValues ? token.SelectToken("Lv1_R4_" + stat + "_Bane[0]").ToString() : "");
                hero.R4Lv1Neut[i] = (token.SelectToken("Lv1_R4_" + stat + "_Neut").HasValues ? token.SelectToken("Lv1_R4_" + stat + "_Neut[0]").ToString() : "");
                hero.R4Lv1Boon[i] = (token.SelectToken("Lv1_R4_" + stat + "_Boon").HasValues ? token.SelectToken("Lv1_R4_" + stat + "_Boon[0]").ToString() : "");
                hero.R4Lv40Bane[i] = (token.SelectToken("Lv40_R4_" + stat + "_Bane").HasValues ? token.SelectToken("Lv40_R4_" + stat + "_Bane[0]").ToString() : "");
                hero.R4Lv40Neut[i] = (token.SelectToken("Lv40_R4_" + stat + "_Neut").HasValues ? token.SelectToken("Lv40_R4_" + stat + "_Neut[0]").ToString() : "");
                hero.R4Lv40Boon[i] = (token.SelectToken("Lv40_R4_" + stat + "_Boon").HasValues ? token.SelectToken("Lv40_R4_" + stat + "_Boon[0]").ToString() : "");

                hero.R5Lv1Bane[i] = (token.SelectToken("Lv1_R5_" + stat + "_Bane").HasValues ? token.SelectToken("Lv1_R5_" + stat + "_Bane[0]").ToString() : "");
                hero.R5Lv1Neut[i] = (token.SelectToken("Lv1_R5_" + stat + "_Neut").HasValues ? token.SelectToken("Lv1_R5_" + stat + "_Neut[0]").ToString() : "");
                hero.R5Lv1Boon[i] = (token.SelectToken("Lv1_R5_" + stat + "_Boon").HasValues ? token.SelectToken("Lv1_R5_" + stat + "_Boon[0]").ToString() : "");
                hero.R5Lv40Bane[i] = (token.SelectToken("Lv40_R5_" + stat + "_Bane").HasValues ? token.SelectToken("Lv40_R5_" + stat + "_Bane[0]").ToString() : "");
                hero.R5Lv40Neut[i] = (token.SelectToken("Lv40_R5_" + stat + "_Neut").HasValues ? token.SelectToken("Lv40_R5_" + stat + "_Neut[0]").ToString() : "");
                hero.R5Lv40Boon[i] = (token.SelectToken("Lv40_R5_" + stat + "_Boon").HasValues ? token.SelectToken("Lv40_R5_" + stat + "_Boon[0]").ToString() : "");
            }
            for (int i = 1; i < 4; i++)
            {
                hero.weapon[i - 1] = (token.SelectToken("weapon" + i).HasValues ? token.SelectToken("weapon" + i + "[0].fulltext").ToString() : "");
                hero.special[i - 1] = (token.SelectToken("special" + i).HasValues ? token.SelectToken("special" + i + "[0].fulltext").ToString() : "");
                hero.assist[i - 1] = (token.SelectToken("assist" + i).HasValues ? token.SelectToken("assist" + i + "[0].fulltext").ToString() : "");
                hero.passiveA[i - 1] = (token.SelectToken("passiveA" + i).HasValues ? token.SelectToken("passiveA" + i + "[0]").ToString() : "");
                hero.passiveB[i - 1] = (token.SelectToken("passiveB" + i).HasValues ? token.SelectToken("passiveB" + i + "[0]").ToString() : "");
                hero.passiveC[i - 1] = (token.SelectToken("passiveC" + i).HasValues ? token.SelectToken("passiveC" + i + "[0]").ToString() : "");

                hero.weaponRarity[i - 1] += (token.SelectToken("weapon" + i + "_unlock").HasValues ? token.SelectToken("weapon" + i + "_unlock[0]").ToString() : "");
                hero.specialRarity[i - 1] += (token.SelectToken("special" + i + "_unlock").HasValues ? token.SelectToken("special" + i + "_unlock[0]").ToString() : "");
                hero.assistRarity[i - 1] += (token.SelectToken("assist" + i + "_unlock").HasValues ? token.SelectToken("assist" + i + "_unlock[0]").ToString() : "");
                hero.passiveARarity[i - 1] = (token.SelectToken("passiveA" + i + "_unlock").HasValues ? token.SelectToken("passiveA" + i + "_unlock[0]").ToString() : "");
                hero.passiveBRarity[i - 1] = (token.SelectToken("passiveB" + i + "_unlock").HasValues ? token.SelectToken("passiveB" + i + "_unlock[0]").ToString() : "");
                hero.passiveCRarity[i - 1] = (token.SelectToken("passiveC" + i + "_unlock").HasValues ? token.SelectToken("passiveC" + i + "_unlock[0]").ToString() : "");

                hero.weaponRarity[i - 1] += (hero.weaponRarity[i - 1] == "" ? (token.SelectToken("weapon" + i + "_default").HasValues ? token.SelectToken("weapon" + i + "_default[0]").ToString() : "") : "");
                hero.specialRarity[i - 1] += (hero.specialRarity[i - 1] == "" ? (token.SelectToken("special" + i + "_default").HasValues ? token.SelectToken("special" + i + "_default[0]").ToString() : "") : "");
                hero.assistRarity[i - 1] += (hero.assistRarity[i - 1] == "" ? (token.SelectToken("assist" + i + "_default").HasValues ? token.SelectToken("assist" + i + "_default[0]").ToString() : "") : "");
            }

            hero.weapon[3] = (token.SelectToken("weapon4").HasValues ? token.SelectToken("weapon4[0].fulltext").ToString() : "");
            hero.weaponRarity[3] += (token.SelectToken("weapon4_unlock").HasValues ? token.SelectToken("weapon4_unlock[0]").ToString() : "");
            hero.weaponRarity[3] += (hero.weaponRarity[3] == "" ? (token.SelectToken("weapon4_default").HasValues ? token.SelectToken("weapon4_default[0]").ToString() : "") : "");

            Heroes.HeroDictionary.TryAdd(hero.name, hero);
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
        HttpResponseMessage response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=query&prop=revisions&rvprop=content&rvsection=0&generator=categorymembers&gcmtitle=Category:Summoning%20Focuses&gcmlimit=50&format=json");
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JObject data = JObject.Parse(result);
        JSONtoBanner(data);

        while (data.SelectToken("continue") != null)
        {
            string continueString = data.SelectToken("continue").SelectToken("gcmcontinue").ToString();
            response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=query&prop=revisions&rvprop=content&rvsection=0&generator=categorymembers&gcmtitle=Category:Summoning%20Focuses&gcmlimit=50&format=json&continue=gcmcontinue||&gcmcontinue=" + continueString);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            data = JObject.Parse(result);
            JSONtoBanner(data);
        }
    }

    public static async Task GetNewBanners()
    {
        string result = null;
        HttpResponseMessage response = await client.GetAsync("https://feheroes.gamepedia.com/api.php?action=query&format=json&prop=revisions&meta=&continue=gcmcontinue%7C%7C&generator=categorymembers&rvprop=content&rvsection=0&gcmtitle=Category%3ASummoning+Focuses&gcmlimit=5&gcmsort=timestamp&gcmdir=descending&format=json");
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadAsStringAsync();
        }
        JObject data = JObject.Parse(result);
        JSONtoBanner(data);
    }

    private static void JSONtoBanner(JObject data)
    {
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
                        Console.WriteLine("Banner already in dictionary.");
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
                    DateTime dt = new DateTime(year, month, day);
                    b.StartDate = dt;
                }
                else if (bannerInfo[i].StartsWith("end"))
                {
                    int month = (Array.IndexOf(DateTimeFormatInfo.InvariantInfo.MonthNames, bannerInfo[i].Substring(4, bannerInfo[i].IndexOf(' ') - 4)) + 1);
                    int day = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].IndexOf(' ') + 1, bannerInfo[i].IndexOf(',') - (bannerInfo[i].IndexOf(' ') + 1))));
                    int year = (Int32.Parse(bannerInfo[i].Substring(bannerInfo[i].IndexOf(',') + 2, 4)));
                    DateTime dt = new DateTime(year, month, day);
                    b.EndDate = dt;
                }
            }
            Summoner.BannersDictionary.TryAdd(b.Title, b);
        }
    }

    public static async Task<String> GetGauntletBracket(bool latestRoundOnly = false)
    {
        string result = null;
        string bracket = null;
        HttpResponseMessage response = await client.GetAsync("https://support.fire-emblem-heroes.com/voting_gauntlet/current?locale=en-US");
        if (response.IsSuccessStatusCode)
        {
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[9];
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
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[9];
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
            result = (await response.Content.ReadAsStringAsync()).Split('\n')[9];
        }
        return (result);
    }
}