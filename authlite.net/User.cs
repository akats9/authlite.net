using Microsoft.Data.Sqlite;

namespace authlite.net;

public class User {
    public string Email {
        get {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                var command = connection.CreateCommand();
                
                connection.Open();
                using (var reader = command.ExecuteReader()) {
                    return reader.GetString(0);
                }
            }
        }
        set {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Users (Email) VALUES ('$email');";
                command.Parameters.AddWithValue("$email", value);
            }  
        }
    }
    public string Username { 
        get {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                var command = connection.CreateCommand();
                
                connection.Open();
                using (var reader = command.ExecuteReader()) {
                    return reader.GetString(1);
                }
            }
        }
        set {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Users (Username) VALUES ('username');";
                command.Parameters.AddWithValue("username", value);
            }  
        } 
    }
    public string Password { get; set; }
    public string Id { get; set; }
    public bool Confirmed { get; set; }
    public role Role { get; set; }
}