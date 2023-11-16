using System.Data.SQLite;
using System.IO;

namespace HabitLogger
{
    public class DatabaseHelper
    {
        
        public DatabaseHelper()
        {
            
        }

        public void TestConnection(SQLiteConnection connection)
        {
            Console.WriteLine("Connected to the database");

            Console.WriteLine($"SQLite Version: {connection.ServerVersion}");
        }

        public void CreateHabitTable(SQLiteConnection connection)
        {
            string createTableQuery = "CREATE TABLE IF NOT EXISTS HabitTable (" +
                "Habit TEXT NOT NULL, " +
                "Number INTEGER, " +
                "Date DATE);";

            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Habit Table Created (if it didnt already)");
            }
        }

        public void CreateLog(SQLiteConnection con, string habit, int quantity, string date)
        {
            using (var cmd = new SQLiteCommand(con))
            {
                cmd.CommandText = "INSERT INTO HabitTable(Habit, Number, Date) VALUES(@Habit, @Quantity, @Date)";
                cmd.Parameters.AddWithValue("@Habit", habit);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Date", date);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Successfully Created New Entry");
            }
        }

        public List<LogEntry> GetLogsForToday(SQLiteConnection conn)
        {
            string today = DateTime.Today.ToString("yyyy-MM-dd");

            string query = $"SELECT * FROM HabitTable WHERE Date = '{today}'";
            return GetLogsFromQuery(conn, query);
        }

        public List<LogEntry> GetLogsForNDays(SQLiteConnection conn, int days)
        {
            string startDate = DateTime.Today.AddDays(-days).ToString("yyyy-MM-dd");

            string query = $"SELECT * FROM HabitTable WHERE Date >= '{startDate}'";
            return GetLogsFromQuery(conn, query);
        }

        public List<LogEntry> GetAllLogs(SQLiteConnection conn)
        {
            string query = "SELECT * FROM HabitTable";
            return GetLogsFromQuery(conn, query);
        }

        private List<LogEntry> GetLogsFromQuery(SQLiteConnection conn, string query)
        {
            List<LogEntry> logs = new List<LogEntry>();

            using (SQLiteCommand command = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LogEntry log = new LogEntry
                        {
                            Habit = reader["Habit"].ToString(),
                            Quantity = Convert.ToInt32(reader["Number"]),
                            Date = Convert.ToDateTime(reader["Date"]).Date
                        };

                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

        public void UpdateLogQuantity(SQLiteConnection conn, string habitName, string logDate, int updatedQuantity)
        {
            string updateQuery = $"UPDATE HabitTable SET Number = {updatedQuantity} WHERE Habit = '{habitName}' AND Date = '{logDate}'";

            using (SQLiteCommand command = new SQLiteCommand(updateQuery, conn))
            {
                command.ExecuteNonQuery();
            }
        }
        
        public LogEntry GetLogByNameAndDate(SQLiteConnection conn, string habitName, string date)
        {
            string query = $"SELECT * FROM HabitTable WHERE Habit = '{habitName}' AND Date = '{date}'";

            List<LogEntry> logs = GetLogsFromQuery(conn, query);

            // Assuming there should be at most one log entry for a habit on a specific date
            return logs.FirstOrDefault();
        }

        public void DeleteLogEntry(SQLiteConnection connection, string habitName, string logDate)
        {
            string deleteQuery = $"DELETE FROM HabitTable WHERE Habit = '{habitName}' AND Date = '{logDate}'";

            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
