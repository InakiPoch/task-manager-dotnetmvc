using System.Data.SQLite;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;

public interface IUserRepository {
    void Add(User user);
    void Update(int id, User user);
    List<User> GetAll();
    User GetById(int id);
    void Delete(int id);
    User FindAccount(string username, string password);
}

public class UserRepository : IUserRepository {
    readonly string connectionPath;

    public UserRepository(string connectionPath) {
        this.connectionPath = connectionPath;
    }

    public void Add(User user) {
        string queryText = "INSERT INTO user (username, role) VALUES (@username, @role)";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@username", user.Username));
            query.Parameters.Add(new SQLiteParameter("@role", user.Role));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(int id, User user) {
        string queryText = "UPDATE user SET username = @username WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            query.Parameters.Add(new SQLiteParameter("@username", user.Username));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<User> GetAll() {
        string queryText = "SELECT * FROM user";
        List<User> users = new List<User>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var user = new User() {
                        Id = Convert.ToInt32(reader["id"]),
                        Username = reader["username"].ToString(),
                        Role = (Role)Convert.ToInt32(reader["role"]),
                        Password = reader["password"].ToString()
                    };
                    users.Add(user);
                }
            }
            connection.Close();
        }
        return users;
    }

    public User GetById(int id) {
        string queryText = "SELECT * FROM user WHERE id = @id";
        User user = new User();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(!reader.Read()) throw new Exception("Usuario no encontrado");
                while(reader.Read()) {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = reader["username"].ToString();
                    user.Role = (Role)Convert.ToInt32(reader["role"]);
                    user.Password = reader["password"].ToString();
                }
            }
            connection.Close();
        }
        return user;
    }

    public void Delete(int id) {
        string queryText = "DELETE FROM user WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public User FindAccount(string username, string password) {
        string queryText = "SELECT * FROM user WHERE username = @username AND password = @password";
        User user = new User();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@username", username));
            query.Parameters.Add(new SQLiteParameter("@password", password));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(!reader.Read()) throw new Exception("Los campos no coinciden");
                while(reader.Read()) {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = reader["username"].ToString();
                    user.Role = (Role)Convert.ToInt32(reader["role"]);
                    user.Password = reader["password"].ToString();
                }
            }
            connection.Close();
        }
        return user;
    }
}