using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Configuration;

internal class Program
{
    // static string connectionString = @"Data Source=habit-Tracker.db";
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                    )";

                tableCmd.ExecuteNonQuery();
            }
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("--------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye\n\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
            Console.WriteLine("\n\n\nPress any key to continue");
            Console.ReadLine();
        }
    }
    
    private static void GetAllRecords()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                // connection.Close();

                Console.WriteLine("------------------------------\n");

                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
            }
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\nPlease insert the number of glasses of water or other measures of your choice (no decimals allowed).\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    private static void Delete()
    {
        Console.Clear();

        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete" +
            " or type 0 to go back to the main menu.\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {recordId}";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist\n");
                    Delete();
                }
            }
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was deleted.\n");

    }

    private static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update" +
            " or type 0 to go back to the main menu.\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var checkCmd = connection.CreateCommand())
            {   
                connection.Open();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();

                    connection.Close();
                    Update();
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\nPlease insert the number of glasses of water or other measures of your choice (no decimals allowed).\n");

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = '{quantity}' " +
                $"WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
            }   
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was updated.\n");

    }

    internal static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
            GetUserInput();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date. Format: (dd-MM-yy). Type 0 to return to main menu or try again: \n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string input)
    {
        Console.WriteLine(input);

        string numberInput = Console.ReadLine();

        if (numberInput == "0")
            GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number, try again.");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

}

public class DrinkingWater
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }
}