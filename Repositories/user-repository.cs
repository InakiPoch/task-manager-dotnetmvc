using System.Data.SQLite;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;

public interface IUserRepository {
    void Add(User user);
    void Update(int id, User user);
    List<User> GetAll();
    User GetById(int id);
    User GetByUsername(string username);
    void Delete(int id);
    User FindAccount(string username, string password);
    bool UserExists(User user);
}

public class UserRepository : IUserRepository {
    readonly string connectionPath;

    public UserRepository(string connectionPath) {
        this.connectionPath = connectionPath;
    }

    public void Add(User user) {
        string queryText = "INSERT INTO user (username, role, password) VALUES (@username, @role, @password)";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@username", user.Username));
            query.Parameters.Add(new SQLiteParameter("@role", user.Role));
            query.Parameters.Add(new SQLiteParameter("@password", user.Password));
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
                if(reader.Read()) {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = reader["username"].ToString();
                    user.Role = (Role)Convert.ToInt32(reader["role"]);
                    user.Password = reader["password"].ToString();
                } else {
                    throw new Exception("Usuario no encontrado");
                }
            }
            connection.Close();
        }
        return user;
    }

    public User GetByUsername(string username) {
        string queryText = "SELECT * FROM user WHERE username = @username";
        User user = new User();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@username", username));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(reader.Read()) {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = reader["username"].ToString();
                    user.Role = (Role)Convert.ToInt32(reader["role"]);
                    user.Password = reader["password"].ToString();
                } else {
                    throw new Exception("Usuario no encontrado");
                }
            }
            connection.Close();
        }
        return user;
    }

    public void Delete(int id) {
        string queryText = "DELETE FROM user WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand("PRAGMA foreign_keys = 1;", connection)) {
                command.ExecuteNonQuery();
            } //Enables FK options, otherwise it doesn't detect them
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
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
                if(reader.Read()) {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.Username = reader["username"].ToString();
                    user.Role = (Role)Convert.ToInt32(reader["role"]);
                    user.Password = reader["password"].ToString();
                } else {
                    throw new Exception("Los campos no coinciden");
                }
            }
            connection.Close();
        }
        return user;
    }

    public bool UserExists(User user) {
        string queryText = "SELECT * FROM user WHERE username = @username";
        bool exists = false;
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@username", user.Username));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(reader.Read()) {
                    exists = true;
                }
            }
            connection.Close();
        }
        return exists;
    }
}