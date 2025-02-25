using Microsoft.Data.Sqlite;

namespace authlite.net;

public class UserManager {
    private static Random random = new Random();

    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    public void createUser(string email, string username, string password, role role) {
        var id = RandomString(20);
        var user = new User(email, username, password, id, role);
    }

    public void deleteUser(string id) {
        using var connection = new SqliteConnection("Data Source=Data/auth.db");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM Users WHERE Id='$ID';";
        command.Parameters.AddWithValue("$id", id);
    }

    public void initUsersFromDb() {
        using var connection = new SqliteConnection("Data Source=Data/auth.db");
        connection.Open();
        var command = connection.CreateCommand();
        using var reader = command.ExecuteReader();
        while (reader.Read()) {
            var email = reader.GetString(0);
            var usename = reader.GetString(1);
            var password = reader.GetString(2);
            var id = reader.GetString(3);
            var role = reader.GetString(4);
        }
    }
}