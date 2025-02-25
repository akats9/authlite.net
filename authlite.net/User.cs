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
        internal set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
    
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Users SET Email='$email' WHERE Id='$id';";
            command.Parameters.AddWithValue("$email", value);
            command.Parameters.AddWithValue("$id", this.Id);
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
        internal set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
    
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Users SET Username='$username' WHERE Id='$id';";
            command.Parameters.AddWithValue("$username", value);
            command.Parameters.AddWithValue("$id", this.Id);
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
        internal set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
    
            using SHA256 hasher = SHA256.Create();
            byte[] valueToBytes = Encoding.ASCII.GetBytes(value);
            byte[] passwordHashValue = hasher.ComputeHash(valueToBytes);
                    
            var hashString = Encoding.ASCII.GetString(passwordHashValue);
    
            command.CommandText = @"UPDATE Users SET Email='$pass' WHERE Id='$id';";
            command.Parameters.AddWithValue("$pass", hashString);
            command.Parameters.AddWithValue("$id", this.Id);
        }
    }
    
    public string Id {
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(2);
        }
    
        internal set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                var thisID = value;
                command.CommandText = @"INSERT INTO Users (Id) VALUES ('$id');";
                command.Parameters.AddWithValue("$id", thisID);
                
            }
        }
    }

    public role Role {
        get {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
            var command = connection.CreateCommand();
            using var reader = command.ExecuteReader();
            return reader.GetString(4) switch {
                "Admin" => role.Admin,
                "Manager" => role.Manager,
                "Employee" => role.Employee,
                _ => role.None
            };
        }
        internal set {
            using var connection = new SqliteConnection("Data Source=Data/auth.db");
            connection.Open();
    
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Users SET Role='$role' WHERE Id='$id';";
            command.Parameters.AddWithValue("$role", value);
            command.Parameters.AddWithValue("$id", this.Id);
        } 
    }

    internal User(string email, string username, string password, string id, role role) {
        this.Email = email;
        this.Username = username;
        this.Password = password;
        this.Role = role;
        this.Id = id;
    }
}