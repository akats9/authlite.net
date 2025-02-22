namespace authlite.net;

public class User {
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Id { get; set; }
    public bool Confirmed { get; set; }
    public role Role { get; set; }
}