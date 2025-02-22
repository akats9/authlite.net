using Microsoft.Data.Sqlite;

internal class Program {
    public static void Main(string[] args) {
        using (var connection = new SqliteConnection()) {
            connection.Open();
            
            //INIT TABLE
            var command = connection.CreateCommand();
            command.CommandText =
                @"
                    CREATE TABLE Users(
                        Email TEXT,
                        Username TEXT,
                        Password TEXT,
                        Id TEXT,
                        Confirmed INTEGER,
                        Role TEXT
                    );
                ";
            
            //command.Parameters.AddWithValue("$id",id);

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var name = reader.GetString(0);
                    Console.WriteLine($"Hello {name}");
                }
            }
        }
    }
}
