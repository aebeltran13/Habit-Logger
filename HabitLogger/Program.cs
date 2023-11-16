using System;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace HabitLogger
{
    class Program
    {
        private DatabaseHelper DatabaseHelper;
        private static string connectionString;

        static void Main(string[] args)
        {
            connectionString = @"Data Source=..\..\..\Files\habits.db; Version=3";
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            DatabaseHelper dbHelper = new DatabaseHelper();

            // Check for connection to Datbase
            try
            {
                conn.Open();
                dbHelper.TestConnection(conn);

                // TODO: Open connection to database from main and pass connection to database helper.
                dbHelper.CreateHabitTable(conn);

                // Wait for user to see message then continue to menu
                Console.WriteLine("Welcome to Habit Logger! The place where you come log your habits :)!\n" +
                    "Press Enter to start.");
                Console.ReadLine();
            
                while(true) 
                {
                    ShowMenu(conn, dbHelper);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ShowMenu(SQLiteConnection conn, DatabaseHelper dbHelper)
        {
            Console.Clear();
            Console.WriteLine("Choose Option");
            Console.WriteLine("C to create new log\n" +
                "R to read previous logs\n" +
                "U to edit a previous log\n" +
                "D to delete a previous log\n" +
                "0 to exit application");

            string menuChoice = Console.ReadLine().ToUpper();

            switch (menuChoice)
            {
                case "C":
                    // TODO
                    CreateLog(conn, dbHelper);
                    break;
                case "R":
                    // TODO
                    ReadLog(conn, dbHelper);
                    break;
                case "U":
                    // TODO
                    UpdateLog(conn, dbHelper);
                    break;
                case "D":
                    // TODO
                    DeleteLog(conn, dbHelper);
                    break;
                case "0":
                    // TODO
                    ExitApplication();
                    break;
                default:
                    Console.WriteLine("Unknown Option\n" +
                        "Press Enter to Try Again.");
                    Console.ReadLine();
                    return;
            }
            Console.WriteLine("Press Enter to Continue.");
            Console.ReadLine();

        }

        private static void CreateLog(SQLiteConnection conn, DatabaseHelper dbHelper)
        {
            string habitName;
            string habitDate;
            int habitQuantity = 0;
            bool isValidInt = false;
            
            Console.WriteLine("-- Create Log --");
            Console.WriteLine("Would you like to log a habit for: \n" +
                "T today\n" +
                "O other day");
            string userInput = Console.ReadLine().ToUpper();

            switch (userInput)
            {
                case "T":
                    DateTime today = DateTime.Today;
                    habitDate = today.ToString("yyyy-MM-dd");
                    Console.WriteLine(habitDate);
                    break;
                case "O":
                    Console.WriteLine("Enter date: (yyyy-mm-dd)");
                    habitDate = Console.ReadLine();
                    Console.WriteLine(habitDate);
                    break;
                default:
                    Console.WriteLine("Unknown opetion.");
                    return;
            }

            Console.WriteLine("Enter Habit:");
            habitName = Console.ReadLine();

            Console.WriteLine("Enter Quantity:");
            while (!isValidInt)
            {
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out habitQuantity))
                {
                    isValidInt = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input.");
                }
            }

            Console.WriteLine($"Habit: {habitName}\n" +
                $"Quantity: {habitQuantity}\n" +
                $"Date: {habitDate}");

            dbHelper.CreateLog(conn, habitName, habitQuantity, habitDate);

        }

        private static void ReadLog(SQLiteConnection conn, DatabaseHelper dbHelper)
        {
            string userInput;
            int dateRange;

            Console.WriteLine("-- Read Logs --");

            Console.WriteLine("Would you like to see your habits from:\n" +
                "'T' Today\n" +
                "'W' The last 7 days\n" +
                "'M' The last 30 days\n" +
                "'A' All habit logs");
            userInput = Console.ReadLine().ToUpper();

            switch (userInput)
            {
                case "T":
                    DisplayLogs(dbHelper.GetLogsForToday(conn));
                    break;
                case "W":
                    DisplayLogs(dbHelper.GetLogsForNDays(conn, 7));
                    break;
                case "M":
                    DisplayLogs(dbHelper.GetLogsForNDays(conn, 30));
                    break;
                case "A":
                    DisplayLogs(dbHelper.GetAllLogs(conn));
                    break;
                default : 
                    Console.WriteLine("Unknown Command. Returning to menu"); 
                    return;
            }
        }

        private static void DisplayLogs(List<LogEntry> logs)
        {
            if (logs.Count == 0)
            {
                Console.WriteLine("No logs found for selected time range.");
                return;
            }

            Console.WriteLine("\n -- Habit Logs -- ");

            // Sort logs by the most recent date
            logs.Sort((log1, log2) => log2.Date.CompareTo(log1.Date));

            foreach (var log in logs)
            {
                Console.WriteLine($"Habit: {log.Habit}, Quantity: {log.Quantity}, Date: {log.Date.ToString("yyyy-MM-dd")}");
            }
        }

        private static void UpdateLog(SQLiteConnection conn, DatabaseHelper dbHelper)
        {
            Console.WriteLine("-- Update Log --");

            // Step 1: Prompt the user for log information (e.g., habit name and date)
            Console.WriteLine("Enter the habit name to update:");
            string habitNameToUpdate = Console.ReadLine();

            Console.WriteLine("Enter the date of the log to update (yyyy-MM-dd):");
            string logDateToUpdate = Console.ReadLine();

            // Step 2: Fetch the log to update
            LogEntry logToUpdate = dbHelper.GetLogByNameAndDate(conn, habitNameToUpdate, logDateToUpdate);

            if (logToUpdate == null)
            {
                Console.WriteLine($"No log found for habit '{habitNameToUpdate}' on {logDateToUpdate}'. Returning to menu.");
                return;
            }

            // Step 3: Display current log information
            Console.WriteLine($"Current Log Information: Habit: {logToUpdate.Habit}, Quantity: {logToUpdate.Quantity}, Date: {logToUpdate.Date.ToString("yyyy-MM-dd")}");

            // Step 4: Prompt for updated quantity
            Console.WriteLine("Enter updated quantity:");
            int updatedQuantity = Convert.ToInt32(Console.ReadLine());

            // Step 5: Perform the update
            try
            {
                dbHelper.UpdateLogQuantity(conn, logToUpdate.Habit, logToUpdate.Date.ToString("yyyy-MM-dd"), updatedQuantity);
                Console.WriteLine("Quantity updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating quantity: {ex.Message}");
            }

        }

        private static void DeleteLog(SQLiteConnection conn, DatabaseHelper dbHelper)
        {
            Console.WriteLine("-- Delete Log --");

            // Step 1: Prompt the user for log information (e.g., habit name and date)
            Console.WriteLine("Enter the habit name to delete:");
            string habitNameToDelete = Console.ReadLine();

            Console.WriteLine("Enter the date of the log to delete (yyyy-MM-dd):");
            string logDateToDelete = Console.ReadLine();

            // Step 2: Fetch the log to delete
            LogEntry logToDelete = dbHelper.GetLogByNameAndDate(conn, habitNameToDelete, logDateToDelete);

            if (logToDelete == null)
            {
                Console.WriteLine($"No log found for habit '{habitNameToDelete}' on {logDateToDelete}'. Returning to menu.");
                return;
            }

            // Step 3: Display current log information
            Console.WriteLine($"Current Log Information: Habit: {logToDelete.Habit}, Quantity: {logToDelete.Quantity}, Date: {logToDelete.Date.ToString("yyyy-MM-dd")}");

            // Step 4: Confirm deletion with user
            Console.WriteLine("Are you sure you want to delete this log? (Y/N):");
            string confirmDelete = Console.ReadLine().ToUpper();

            if (confirmDelete == "Y")
            {
                // Step 5: Perform the delete
                try
                {
                    dbHelper.DeleteLogEntry(conn, logToDelete.Habit, logToDelete.Date.ToString("yyyy-MM-dd"));
                    Console.WriteLine("Log deleted successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting log: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Deletion cancelled. Returning to menu.");
            }
        }

        private static void ExitApplication()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("See you soon!");
            Environment.Exit(0);
        }
    }
}