namespace tl2_tp10_2023_InakiPoch.Models;

public enum Role { Admin = 1, Op }

public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public string Password { get; set; }
}