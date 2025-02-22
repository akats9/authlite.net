using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

namespace authlite.net;

public class User {
    public string Email {
        get {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();
                var command = connection.CreateCommand();
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
                connection.Open();
                var command = connection.CreateCommand();
                using (var reader = command.ExecuteReader()) {
                    return reader.GetString(1);
                }
            }
        }
        set {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Users (Username) VALUES ('$username');";
                command.Parameters.AddWithValue("$username", value);
            }  
        } 
    }

    public string Password {
        get {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();
                var command = connection.CreateCommand();
                using (var reader = command.ExecuteReader()) {
                    return reader.GetString(2);
                }
            }
        }
        set {
            using (var connection = new SqliteConnection("Data Source=Data/auth.db")) {
                connection.Open();
                var command = connection.CreateCommand();

                using (SHA256 hasher = SHA256.Create()) {
                    byte[] valueToBytes = Encoding.ASCII.GetBytes(value);
                    byte[] passwordHashValue = hasher.ComputeHash(valueToBytes);
                    
                    var hashString = Encoding.ASCII.GetString(passwordHashValue);

                    command.CommandText = @"INSERT INTO Users (Password) VALUES ('$password');";
                    command.Parameters.AddWithValue("$password", hashString);
                }
            }
        }
    }
    public string Id { get; set; }
    public bool Confirmed { get; set; }
    public role Role { get; set; }
}