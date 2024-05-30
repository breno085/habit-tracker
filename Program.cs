using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Reflection.Metadata.Ecma335;

internal class Program
{
    // static string connectionString = @"Data Source=habit-Tracker.db";
    // Version 1.0
    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    static void Main(string[] args)
    {
        GenerateHabits();

        CreateOrSelectHabits();
    }

    static void GenerateHabits()
    {
        //Generate multiple habits when the database gets created for the first time
        string[,] habits =
        {
            {"Coding", "Hours"},
            {"Reading", "Pages"},
            {"Guitar Practise", "Hours"},
            {"Exercise", "Hours"},
            {"Drinking Water", "Cups"},
            {"Meditaton", "Minutes"}
        };

        for (int i = 0; i < habits.GetLength(0); i++)
        {
            string habitName = habits[i, 0].Replace(" ", "_");
            string unit = habits[i, 1];
            TableCreator(habitName, unit);
            PopulateTableWithRandomData(habitName, unit, 100); // Insert 100 random records
        }
    }

    static void PopulateTableWithRandomData(string habitName, string unitName, int recordCount)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var checkCmd = connection.CreateCommand())
            {
                checkCmd.CommandText = $"SELECT COUNT(*) FROM {habitName}";

                long count = (long)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    List<GenericRecord> tableData = new List<GenericRecord>();

                    Random random = new Random();

                    for (int i = 0; i < recordCount; i++)
                    {
                        GenericRecord record = new GenericRecord();

                        record.Id = i + 1;
                        record.Date = RandomDay(random);
                        record.Unit = random.Next(12);

                        tableData.Add(record);
                    }

                    foreach (var record in tableData)
                    {
                        using (var tableCmd = connection.CreateCommand())
                        {
                            //String Concatenation Approach
                            //Drawbacks: Vulnerable to SQL injection, potential type conversion issues.
                            // tableCmd.CommandText =
                            // $"INSERT INTO {habitName} (Date, {unitName}) VALUES ('{record.Date.ToString("dd-MM-yy")}', {record.Unit.ToString()})";

                            // tableCmd.ExecuteNonQuery();

                            //Parameterized Queries Approach
                            //Benefits: Safe from SQL injection, clear and maintainable, ensures type safety.
                            tableCmd.CommandText = $"INSERT INTO {habitName} (Date, {unitName}) VALUES (@Date, @Unit)";
                            tableCmd.Parameters.AddWithValue("@Date", record.Date.ToString("dd-MM-yy"));
                            tableCmd.Parameters.AddWithValue("@Unit", record.Unit);

                            tableCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }

    private static DateTime RandomDay(Random random)
    {
        DateTime start = new DateTime(2024, 1, 1);

        int range = (DateTime.Today - start).Days;

        return start.AddDays(random.Next(range));
    }

    static void CreateOrSelectHabits()
    {
        bool closeApp = false;

        do
        {   Console.WriteLine("\nHabits Menu");
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
        Console.WriteLine("Enter an unit of measurement (e.g., Steps, Pages, Kilometers, Hours, Minutes, Bottles, Cups, Liters, etc): ");
        string unit = Console.ReadLine();

        return unit;
    }

    public static string ShowHabits()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
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

                        for (int i = 0; i < tables.Count - 1; i++)
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
                        {unit} INT
                    )";

                tableCmd.ExecuteNonQuery();
            }
        }
    }
    static void GetUserInput(string habitName)
    {
        Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine($"\n\n{habitName.Replace("_", " ").ToUpper()} HABIT");
            Console.WriteLine("\nMAIN MENU");
            Console.WriteLine($"\nWhat would you like to do for the {habitName} habit?");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View All Records");
            Console.WriteLine("Type 2 to Insert Record");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            Console.WriteLine("Type 5 to View Habit Reports (i.e. how many times the user ran in a year? how many kms?)");
            Console.WriteLine("Type 6 to go back to the habits menu");
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
                    Console.WriteLine("TBD");
                    break;
                case "6":
                    CreateOrSelectHabits();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
            Console.WriteLine("\n\n\nPress any key to continue");
            Console.ReadLine();
        }
    }

    private static void GetAllRecords(string tableName)
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
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

                else
                {
                    Console.WriteLine("No rows found");
                }

                Console.WriteLine("------------------------------");
                Console.WriteLine($"{tableName}\n");

                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - {unitName}: {dw.Unit}");
                }
            }
        }
    }

    private static void Insert(string habitName)
    {
        string date = GetDateInput(habitName);

        string unitName = GetUnitName(habitName);

        int quantity = GetNumberInput($"\nPlease insert the number of {unitName} (no decimals allowed).\n", habitName);

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText =
                    $"INSERT INTO {habitName}(date, {unitName}) VALUES ('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    private static void Delete(string habitName)
    {
        Console.Clear();

        GetAllRecords(habitName);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete" +
            " or type 0 to go back to the main menu.\n", habitName);

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = $"DELETE FROM {habitName} WHERE Id = {recordId}";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist\n");
                    Delete(habitName);
                }
            }
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was deleted.\n");

    }

    private static void Update(string habitName)
    {
        GetAllRecords(habitName);

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to update" +
            " or type 0 to go back to the main menu.\n", habitName);

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var checkCmd = connection.CreateCommand())
            {
                connection.Open();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id = {recordId} doesn't exist.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();

                    connection.Close();
                    Update(habitName);
                }

                string date = GetDateInput(habitName);

                string unitName = GetUnitName(habitName);

                int quantity = GetNumberInput($"\nPlease insert the number of {unitName} (no decimals allowed).\n", habitName);

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE {habitName} SET date = '{date}', {unitName} = '{quantity}' " +
                $"WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine($"\nRecord with Id = {recordId} was updated.\n");

    }

    internal static string GetDateInput(string habitName)
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
            GetUserInput(habitName);

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Invalid date. Format: (dd-MM-yy). Type 0 to return to main menu or try again: \n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    internal static int GetNumberInput(string input, string habitName)
    {
        Console.WriteLine(input);

        string numberInput = Console.ReadLine();

        if (numberInput == "0")
            GetUserInput(habitName);

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number, try again.");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

}

public class GenericRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Unit { get; set; }
}
