using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Configuration;
<<<<<<< HEAD
using System.Reflection.Metadata.Ecma335;
=======
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

internal class Program
{
    // static string connectionString = @"Data Source=habit-Tracker.db";
    // Version 1.0
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
    static void Main(string[] args)
    {
<<<<<<< HEAD
        CreateOrSelectHabits();
    }

    static void CreateOrSelectHabits()
    {
        bool closeApp = false;

        do
        {
            Console.WriteLine("\nWhat would you like to do?\n");
            Console.WriteLine("0 - Close application");
            Console.WriteLine("1 - Create a new habit");
            Console.WriteLine("2 - Select an existing habit");
            Console.WriteLine("3 - Delete an habit\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    closeApp = true;
                    Console.WriteLine("\nGoodbye\n\n");
                    Environment.Exit(0);
                    break;

                case "1":
                    string habit = CreateHabit();
                    string unit = UnitOfMeasurement();

                    TableCreator(habit, unit);
                    break;

                case "2":
                    string selectedHabit = ShowHabits();

                    if (selectedHabit != null)
                    {
                        Console.WriteLine($"You selected {selectedHabit} habit");
                        Console.WriteLine("Press any key to continue\n");
                        Console.ReadLine();
                        GetUserInput(selectedHabit);
                    }

                    break;
                
                case "3":
                    Console.WriteLine("TBD");
                    break;

                default:
                    Console.WriteLine("\nInvalid command. Please try again.\n");
                    break;
            }

        } while (!closeApp);
    }

    public static string CreateHabit()
    {
        Console.WriteLine("Name your habit: ");

        string habit = Console.ReadLine().Replace(" ", "_");

        return habit;
    }

    public static string UnitOfMeasurement()
    {
        Console.WriteLine("Enter an unit of measurement (e.g., Steps, Pages, Kilometers, Hours, Minutes, Bottles, Cups, Liters, etc):");
        string unit = Console.ReadLine();

        return unit;
    }

    public static string ShowHabits()
    {
=======
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
<<<<<<< HEAD
                    $@"SELECT name
                        FROM sqlite_master
                        WHERE type = 'table'
                        ORDER BY name;";

                List<string> tables = new List<string>();

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Select an habit\n");

                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }

                        for (int i = 0; i < tables.Count -1; i++)
                        {
                            Console.WriteLine($"{i + 1} - {tables[i]}");
                        }

                        Console.WriteLine("");

                        string command = Console.ReadLine();

                        if (int.TryParse(command, out int habitIndex) && habitIndex >= 1 && habitIndex <= tables.Count)
                        {
                            string selectedHabit = tables[habitIndex - 1];

                            TableCreator(selectedHabit, GetUnitName(selectedHabit));

                            return selectedHabit;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please select a valid habit.\n");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No habits found.\n");
                        return null;
                    }
                }
            }
        }
    }

    static string GetUnitName(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    $@"SELECT * FROM {tableName};)";

                using (var reader = tableCmd.ExecuteReader())
                {
                    return reader.GetName(2);
                }
            }
        }
    }
    static void TableCreator(string tableName, string unit)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    $@"CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        {unit} DOUBLE
=======
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                    )";

                tableCmd.ExecuteNonQuery();
            }
        }
<<<<<<< HEAD
    }
    static void GetUserInput(string habitName)
=======

        GetUserInput();
    }

    static void GetUserInput()
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
    {
        Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
<<<<<<< HEAD
            Console.WriteLine($"\n\n{habitName.Replace("_", " ").ToUpper()} HABIT");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine($"\nWhat would you like to do for the {habitName} habit?");
=======
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
<<<<<<< HEAD
            Console.WriteLine("Type 5 to go back to create or select another habit");
=======
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
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
<<<<<<< HEAD
                    GetAllRecords(habitName);
                    break;
                case "2":
                    Insert(habitName);
                    break;
                case "3":
                    Delete(habitName);
                    break;
                case "4":
                    Update(habitName);
                    break;
                case "5":
                    CreateOrSelectHabits();
=======
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
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
            Console.WriteLine("\n\n\nPress any key to continue");
            Console.ReadLine();
        }
    }

<<<<<<< HEAD
    private static void GetAllRecords(string tableName)
=======
    private static void GetAllRecords()
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
<<<<<<< HEAD
                    $"SELECT * FROM {tableName}";

                List<GenericRecord> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                string unitName = "";

                if (reader.HasRows)
                {
                    unitName = reader.GetName(2);
                    
                    while (reader.Read())
                    {   
                        tableData.Add(
                        new GenericRecord
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Unit = reader.GetInt32(2)
                        });
                    }
                }

=======
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
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                else
                {
                    Console.WriteLine("No rows found");
                }

<<<<<<< HEAD
                Console.WriteLine("------------------------------");
                Console.WriteLine($"{tableName}\n");

                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - {unitName}: {dw.Unit}");
=======
                // connection.Close();

                Console.WriteLine("------------------------------\n");

                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                }
            }
        }
    }

<<<<<<< HEAD
    private static void Insert(string habitName)
    {
        string date = GetDateInput(habitName);

        string unitName = GetUnitName(habitName);

        int quantity = GetNumberInput($"\nPlease insert the number of {unitName}.\n", habitName);
=======
    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\nPlease insert the number of glasses of water or other measures of your choice (no decimals allowed).\n");
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
<<<<<<< HEAD
                    $"INSERT INTO {habitName}(date, {unitName}) VALUES ('{date}', {quantity})";
=======
                    $"INSERT INTO drinking_water(date, quantity) VALUES ('{date}', {quantity})";
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

                tableCmd.ExecuteNonQuery();
            }
        }
    }

<<<<<<< HEAD
    private static void Delete(string habitName)
    {
        Console.Clear();

        GetAllRecords(habitName);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete" +
            " or type 0 to go back to the main menu.\n", habitName);
=======
    private static void Delete()
    {
        Console.Clear();

        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete" +
            " or type 0 to go back to the main menu.\n");
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

<<<<<<< HEAD
                tableCmd.CommandText = $"DELETE FROM {habitName} WHERE Id = {recordId}";
=======
                tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {recordId}";
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist\n");
<<<<<<< HEAD
                    Delete(habitName);
=======
                    Delete();
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                }
            }
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was deleted.\n");

    }

<<<<<<< HEAD
    private static void Update(string habitName)
    {
        GetAllRecords(habitName);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update" +
            " or type 0 to go back to the main menu.\n", habitName);
=======
    private static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update" +
            " or type 0 to go back to the main menu.\n");
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var checkCmd = connection.CreateCommand())
<<<<<<< HEAD
            {
                connection.Open();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE Id = {recordId})";
=======
            {   
                connection.Open();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();

                    connection.Close();
<<<<<<< HEAD
                    Update(habitName);
                }

                string date = GetDateInput(habitName);

                string unitName = GetUnitName(habitName);

                int quantity = GetNumberInput($"\nPlease insert the number of {unitName}.\n", habitName);

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE {habitName} SET date = '{date}', {unitName} = '{quantity}' " +
                $"WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
            }
=======
                    Update();
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\nPlease insert the number of glasses of water or other measures of your choice (no decimals allowed).\n");

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = '{quantity}' " +
                $"WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
            }   
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was updated.\n");

    }

<<<<<<< HEAD
    internal static string GetDateInput(string habitName)
=======
    internal static string GetDateInput()
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
<<<<<<< HEAD
            GetUserInput(habitName);
=======
            GetUserInput();
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date. Format: (dd-MM-yy). Type 0 to return to main menu or try again: \n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

<<<<<<< HEAD
    internal static int GetNumberInput(string input, string habitName)
=======
    internal static int GetNumberInput(string input)
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
    {
        Console.WriteLine(input);

        string numberInput = Console.ReadLine();

        if (numberInput == "0")
<<<<<<< HEAD
            GetUserInput(habitName);
=======
            GetUserInput();
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number, try again.");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

}

<<<<<<< HEAD
public class GenericRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Unit { get; set; }
}
=======
public class DrinkingWater
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }
}
>>>>>>> 478e287a8ab9409e0517e2952808649b4c123eb1
