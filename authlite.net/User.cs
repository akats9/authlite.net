using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

namespace authlite.net;

public class User {
    public string Email {
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(0);
        }
        set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Users (Email) VALUES ('$email');";
            command.Parameters.AddWithValue("$email", value);
        }
    }
    public string Username { 
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(1);
        }
        set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Users (Username) VALUES ('$username');";
            command.Parameters.AddWithValue("$username", value);
        } 
    }

    public string Password {
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(2);
        }
        set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();

            using SHA256 hasher = SHA256.Create();
            byte[] valueToBytes = Encoding.ASCII.GetBytes(value);
            byte[] passwordHashValue = hasher.ComputeHash(valueToBytes);
                    
            var hashString = Encoding.ASCII.GetString(passwordHashValue);

            command.CommandText = @"INSERT INTO Users (Password) VALUES ('$password');";
            command.Parameters.AddWithValue("$password", hashString);
        }
    }

    private static Random random = new Random();

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    public string Id {
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(2);
        }

        set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                var otherID = reader.GetString(3);
                var thisID = RandomString(10);
                while (thisID == otherID) {
                    thisID = RandomString(10);
                } 
                command.CommandText = @"INSERT INTO Users (Id) VALUES ('$id');";
                command.Parameters.AddWithValue("$id", thisID);
                
            }
        }
    }
    public bool Confirmed { get; set; }
    public role Role { get; set; }
}