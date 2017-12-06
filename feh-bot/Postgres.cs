using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

public class Postgres
{
    static string host = Environment.GetEnvironmentVariable("DATABASE_HOST");
    static string database = Environment.GetEnvironmentVariable("DATABASE_DATABASE");
    static string user = Environment.GetEnvironmentVariable("DATABASE_USER");
    static string port = Environment.GetEnvironmentVariable("DATABASE_PORT");
    static string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
    static NpgsqlCommand com;

    static NpgsqlConnection connection = new NpgsqlConnection(
            "Server=" + host + ";" +
            "Port=" + port + ";" +
            "User Id=" + user + ";" +
            "Password=" + password + ";" +
            "Database=" + database + ";"
            );

    public static void OpenConnection()
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection Open");
            com = connection.CreateCommand();
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void CloseConnection()
    {
        try
        {
            connection.Close();
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static NpgsqlDataReader ExecuteCommand(string command)
    {
        return com.ExecuteReader();
    }

    public static async Task AddGauntletHeroes(List<string> heroes)
    {
        string command =
            "SELECT Name FROM GauntletHero WHERE ID=0";
        com.CommandText = command;
        string firstHero = (string)(await com.ExecuteScalarAsync());
        if (firstHero == heroes[0])
        {
            return;
        }
        else if (firstHero == "")
        { }
        else if (firstHero != heroes[0])
        {
            await ClearTables();
        }
        Console.WriteLine("ADDING GAUNTLETHEROES TO DATABASE");
        command =
            "INSERT INTO GauntletHero(ID, Name)\n" +
            "VALUES";
        for (int i = 0; i < heroes.Count; i++)
        {
            string h = "(" + i + ", '" + heroes[i] + "'),";
            Console.WriteLine(h);
            command += h;
        }
        command = command.Remove(command.Length - 1, 1);
        Console.WriteLine(command);
        com.CommandText = command;
        try
        {
            await com.ExecuteReaderAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static async Task AddUser(string username, string discriminator, string heroname)
    {
        string command = 
            "SELECT ID FROM GauntletHero WHERE Name='" + heroname + "'";
        com.CommandText = command;
        int heroID = (int)(await com.ExecuteScalarAsync());

        try
        {
            command =
                "INSERT INTO Users(Discriminator, Name)\n" +
                "VALUES('" + discriminator + "', '" + username + "')";
            com.CommandText = command;
            await com.ExecuteScalarAsync();
        }
        catch
        {

        }

        try
        {
            command =
                "INSERT INTO HeroToUser(ID, Discriminator)\n" +
                "VALUES(" + heroID + ", '" + discriminator + "')";
            com.CommandText = command;
            await com.ExecuteScalarAsync();
        }
        catch
        {

        }
    }

    public static async Task RemoveUser(string discriminator, string heroname)
    {
        string command =
            "SELECT ID FROM GauntletHero WHERE Name='" + heroname + "'";
        com.CommandText = command;
        int heroID = (int)(await com.ExecuteScalarAsync());

        command =
            "DELETE FROM HeroToUser\n" +
            "WHERE ID=" + heroID + " AND Discriminator='" + discriminator + "'";
        com.CommandText = command;
        await com.ExecuteScalarAsync();
    }

    public static string GetTeams(string discriminator)
    {
        string command =
            "SELECT Name FROM GauntletHero WHERE ID IN" +
            "(" +
            "   SELECT ID FROM HeroToUser WHERE discriminator ='" + discriminator + "'" +
            ")";
        com.CommandText = command;
        NpgsqlDataReader reader = com.ExecuteReader();

        string Heroes = "";
        while (reader.Read())
        {
            Heroes += reader[0] + ", ";
        }
        reader.Close();
        return (Heroes == "" ? "" : Heroes.Remove(Heroes.Length - 2, 2));
    }

    public static List<string> GetUsersInTeam(int heroID)
    {
        List<string> users = new List<string>();
        string command =
            "SELECT Discriminator, Name , (SELECT Name FROM GauntletHero WHERE ID = " + heroID +")" +
            "FROM Users\n" +
            "WHERE Discriminator IN" +
            "(" +
            "SELECT Discriminator FROM HeroToUser WHERE ID =" + heroID +
            ")";
        com.CommandText = command;
        NpgsqlDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            users.Add(reader[0] + "|sep|" + reader[1] + "|sep|" + reader[2]);
        }
        reader.Close();
        return users;
    }

    public static List<string> GetTeams()
    {
        List<string> teams = new List<string>();
        string command = "SELECT Name FROM GauntletHero";
        com.CommandText = command;
        NpgsqlDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            teams.Add(reader[0].ToString());
        }
        return teams;
    }

    public static async Task ClearTables()
    {
        string command = "DELETE * FROM Users";
        com.CommandText = command;
        await com.ExecuteScalarAsync();

        command = "DELETE * FROM HeroToUser";
        com.CommandText = command;
        await com.ExecuteScalarAsync();

        command = "DELETE * FROM GauntletHero";
        com.CommandText = command;
        await com.ExecuteScalarAsync();
    }
}
