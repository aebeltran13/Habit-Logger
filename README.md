# Habit Logger
This is an application in C# utilizing SQLite for data storage. The program allows users to log, read, update, and delete habit entries. The application features a user-friendly console interface with a main menu, where users can seamlessly interact with the SQLite database through CRUD operations. The database-related functionalities are encapsulated in a separate class, promoting clean code practices. The application employs error handling and user confirmation for data modifications, enhancing reliability and user experience. This project showcases proficiency in C#, database integration, and software design principles.

## Program Class

### Main method
- **Initialization:**
    - Creates a connection string for an SQLite database (`connectionString`).
    - Initializes an `SQLiteConnection` (`conn`) and a `DatabaseHelper` instance (`dbHelper`).
- **Database Connection:**
    - Attempts to open the database connection (`conn`).
    - Calls the `TestConnection` method of the `DatabaseHelper` class to check the connection and display the SQLite version.
    - Calls the `CreateHabitTable` method of the `DatabaseHelper` class to ensure that the habit table exists in the database.
- **User Interface:**
    - Displays a welcome message and prompts the user to press Enter to start the application.
- **Menu Loop:**
    - Enters an infinite loop (`while(true)`) to keep the application running.
    - Calls the `ShowMenu` method, passing the database connection and `DatabaseHelper` instance.
- **Exception Handling:**
    - Catches and handles any exceptions that might occur during the execution of the application, displaying an error message.

In summary, the `Main` method initializes the database connection, checks the connection, ensures the existence of the habit table, and enters a menu loop for user interaction. It also provides error handling to gracefully handle any exceptions that may arise during execution.
### ShowMenu
- **Inputs:**
    - Takes an SQLite connection (`conn`) and an instance of `DatabaseHelper` (`dbHelper`) as parameters.
- **User Interface:**
    - Clears the console and presents a menu to the user with the following options:
        - `"C"`: Create a new log
        - `"R"`: Read previous logs
        - `"U"`: Edit a previous log
        - `"D"`: Delete a previous log
        - `"0"`: Exit the application
    - Prompts the user to enter a choice.
- **Menu Options:**
    - Uses a `switch` statement to handle the user's choice and call the appropriate method based on the menu option.
    - If the user enters an unknown option, it prints an error message and returns to the main menu.
- **Continuation:**
    - After processing the user's choice, it prompts the user to press Enter to continue, providing a pause in the interface.

In summary, the `ShowMenu` method serves as the central hub for user interaction, directing the flow of the application based on the user's menu choice. It calls other methods such as `CreateLog`, `ReadLog`, `UpdateLog`, and `DeleteLog` to perform specific actions, and it provides a user-friendly interface for navigating through the application.
### CreateLog
- Inputs:
	- Takes an SQLite connection ('conn') and an instance of `DatabseHelper` ('dbHelper') as parameters.
- User Inputs:
	- Prompts the user to choose whether to log a habit for today or another day ('T' for today, 'O' for another day).
	- If the user chooses today ('T'), it automatically sets the habit date to the current date in the "yyyy-MM-dd" format.
	- If the user chooses another day ('O'), it prompts the user to enter a date in the "yyyy-MM-dd" format.
	- Validates the user input to handle unknown options.
- Habit Information:
	- Asks the user to enter the habit name and stores it in the `'habitName' `variable.
	- Asks the user to enter the quantity and validate it using a loop until a valid integer is provided, storing it in the `'habitQuantity'` variable.
	- Prints the entered habit name, quantity, and date to the console.
- Database Interaction:
	- Calls the 'CreateLog' method of the` 'DatabaseHelper' `class, passing the SQLite connection, habit name, quantity, and date as arguments.

Overall, the method provided a user-friendly interface to log habits, handles user input validation, and interacts with the database through the` 'DatabaseHelper'` class to create a new log entry.
### ReadLog
- Inputs:
	- Takes an SQLite connection(`'conn'`) and an instance of `DatabaseHelper` ('`dbHelper`') as parameters.
- **Purpose:**
	- Allows the user to select a time range to view habit logs and invokes the `DisplayLogs` method accordingly.
- **Steps:** 
    1. Displays a menu for the user to choose a time range ('T' for today, 'W' for the last 7 days, 'M' for the last 30 days, 'A' for all logs).
    2. Reads the user's input and converts it to uppercase for case-insensitive comparison.
    3. Uses a switch statement to call the appropriate `DisplayLogs` method based on the user's choice.
    4. Displays an error message and returns to the menu if an unknown command is entered.
### DisplayLogs
- **Purpose:**    
    - Displays habit logs to the console, sorted by the most recent date.
- **Steps:**
    1. Checks if there are logs to display; if not, prints a message and returns.
    2. Sorts the list of `LogEntry` objects in descending order based on the `Date` property.
    3. Prints a header indicating that habit logs are being displayed.
    4. Iterates through the sorted list and prints each log's habit, quantity, and date in the specified format.
### UpdateLog
1. **Prompt User:**    
    - The method prompts the user to enter the habit name and date of the log they want to update.
2. **Fetch Log:**    
    - It uses the `GetLogByNameAndDate` method in the `DatabaseHelper` class to fetch the log based on the entered habit name and date.
3. **Display Current Log:**    
    - Displays the current information of the log to be updated, including habit name, quantity, and date.
4. **Get Updated Quantity:**    
    - Prompts the user to enter the updated quantity for the habit.
5. **Update Log:**    
    - Calls the `UpdateLogQuantity` method in the `DatabaseHelper` class to update the quantity of the log.
6. **Error Handling:**    
    - Catches and handles any exceptions that might occur during the update process.
7. **Feedback to User:**    
    - Provides feedback to the user about the success or failure of the update.
### DeleteLog
1. **Prompt User:**    
    - The method prompts the user to enter the habit name and date of the log they want to delete.
2. **Fetch Log:**    
    - It uses the `GetLogByNameAndDate` method in the `DatabaseHelper` class to fetch the log based on the entered habit name and date.
3. **Display Current Log:**    
    - Displays the current information of the log to be deleted, including habit name, quantity, and date.
4. **Confirm Deletion:**    
    - Asks the user to confirm if they want to delete the displayed log.
5. **Perform Deletion:**    
    - If the user confirms, calls the `DeleteLogEntry` method in the `DatabaseHelper` class to delete the log.
6. **Error Handling:**    
    - Catches and handles any exceptions that might occur during the deletion process.
7. **Feedback to User:**    
    - Provides feedback to the user about the success or failure of the deletion.
### ExitApplication()
- This method simply closes the application with the following command:
```C#
Environment.Exit(0);
```
## Database Helper Class
### TestConnection
- **Purpose:**
    - Tests the connection to the SQLite database and prints information about the connection.
- **Steps:**
    1. Prints a message to the console indicating that the application is connected to the database.
    2. Retrieves and prints the SQLite server version using `connection.ServerVersion`.

This method serves as a simple test to check the connection status and provide information about the SQLite server version. It is useful for verifying that the application can connect to the database successfully.
### CreateHabitTable
- **Purpose:**
    - Creates the "HabitTable" in the database if it doesn't already exist.
- **Steps:**
    1. Constructs an SQL query string (`createTableQuery`) for creating the "HabitTable" with columns for habit (TEXT), quantity (INTEGER), and date (DATE).
    2. Uses a `using` statement to create an `SQLiteCommand` with the provided connection and the constructed query.
    3. Executes the query using `command.ExecuteNonQuery()` to create the table in the database if it doesn't exist.
    4. Prints a message to the console indicating whether the table was created.

This method encapsulates the logic for creating the "HabitTable" in the database, ensuring that the table is only created if it doesn't already exist. It uses the `CREATE TABLE IF NOT EXISTS` SQL statement to handle both table creation and checking for existing tables.
### CreateLog
- **Purpose:**
    - Inserts a new habit log entry into the "HabitTable" in the database.
- **Steps:**
    1. Uses a `using` statement to create an `SQLiteCommand` with the provided connection (`con`).
    2. Sets the command's text to an SQL query for inserting a new row into the "HabitTable" with placeholders for habit, quantity, and date.
    3. Adds parameters to the command corresponding to the placeholders, setting their values based on the provided habit, quantity, and date.
    4. Executes the query using `cmd.ExecuteNonQuery()` to insert the new log entry into the database.
    5. Prints a success message to the console.

This method encapsulates the process of creating and executing an SQL command to insert a new habit log entry into the database. The use of parameters helps prevent SQL injection and ensures proper handling of different data types.
### List LogEntry GetLogsForToday
- **Purpose:**
    - Retrieves habit logs from the database for the current date.
- **Steps:**
    1. Gets the current date (`DateTime.Today`) and formats it as a string in the "yyyy-MM-dd" format.
    2. Constructs an SQL query string to select all columns from the "HabitTable" where the date is equal to the formatted current date.
    3. Calls the `GetLogsFromQuery` method with the provided connection (`conn`) and the constructed query.
    4. Returns the list of `LogEntry` objects representing habit logs for the current date.

This method provides a convenient way to retrieve habit logs specifically for the present day. It leverages the `GetLogsFromQuery` method to handle the database query and log retrieval.
### List `LogEntry` GetLogsForNDays
- **Purpose:**
    - Retrieves habit logs from the database for a specified number of past days.
- **Steps:**
    1. Calculates the start date by subtracting the specified number of days from the current date (`DateTime.Today`).
    2. Formats the start date as a string in the "yyyy-MM-dd" format.
    3. Constructs an SQL query string to select all columns from the "HabitTable" where the date is greater than or equal to the calculated start date.
    4. Calls the `GetLogsFromQuery` method with the provided connection (`conn`) and the constructed query.
    5. Returns the list of `LogEntry` objects representing habit logs for the specified past days.

This method abstracts the logic for querying habit logs for a specific time range, making the code more modular and readable. It utilizes the `GetLogsFromQuery` method to handle the database query and log retrieval.
### List `LogEntry` GetAllLogs
- **Purpose:**
    - Retrieves all habit logs from the database.
- **Steps:**
    1. Constructs an SQL query string to select all columns from the "HabitTable" in the database.
    2. Calls the `GetLogsFromQuery` method with the provided connection (`conn`) and the constructed query.
    3. Returns the list of `LogEntry` objects representing all habit logs retrieved from the database.

This method serves as a high-level abstraction for getting all habit logs, making the code more readable and modular. It leverages the `GetLogsFromQuery` method, which encapsulates the logic for executing a query and populating a list of `LogEntry` objects.
### List LogEntry GetLogsFromQuery
- **Purpose:**
    - Retrieves habit logs from the database based on a given SQL query.
- **Steps:**
    1. Initializes an empty list to store `LogEntry` objects representing habit logs.
    2. Uses a `using` statement to create an `SQLiteCommand` with the provided SQL query and the specified connection (`conn`).
    3. Within the command context, creates an `SQLiteDataReader` to execute the query and read the results.
    4. Iterates through the reader's results using a `while` loop.
    5. For each row in the result set, creates a new `LogEntry` object and populates its properties (`Habit`, `Quantity`, and `Date`) from the corresponding columns in the result set.
    6. Adds each `LogEntry` object to the list of logs.
    7. Returns the populated list of `LogEntry` objects representing the habit logs retrieved from the database.
This method encapsulates the logic for querying the database, creating `LogEntry` objects, and building a list of habit logs based on the results. It is a reusable component for retrieving habit logs from the database.
