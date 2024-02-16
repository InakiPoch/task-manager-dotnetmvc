using System.Data.SQLite;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;

public interface ITasksRepository {
    List<Tasks> GetAll();
    void Add(int boardId, Tasks task);
    void Update(int id, Tasks task);
    Tasks GetById(int id);
    List<Tasks> GetByAssigned(int userId);
    List<Tasks> GetByUser(int userId);
    List<Tasks> GetByBoard(int boardId);
    bool TaskExists(Tasks task);
    void Delete(int id);
    void AssignTask(int usarId, int taskId);
}

public class TasksRepository : ITasksRepository {
    readonly string connectionPath;

    public TasksRepository(string connectionPath) {
        this.connectionPath = connectionPath;
    }

    public void Add(int boardId, Tasks task) {
        string queryText = "INSERT INTO task (board_id, name, state, description, color, assigned_user_id) " +  
                            "VALUES (@board_id, @name, @state, @description, @color, @assigned_user_id)";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@board_id", boardId));
            query.Parameters.Add(new SQLiteParameter("@name", task.Name));
            query.Parameters.Add(new SQLiteParameter("@state", task.State));
            query.Parameters.Add(new SQLiteParameter("@description", task.Description));
            query.Parameters.Add(new SQLiteParameter("@color", task.Color));
            query.Parameters.Add(new SQLiteParameter("@assigned_user_id", task.AssignedUserId));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(int id, Tasks task) {
        string queryText = "UPDATE task SET name = @name, state = @state, description = @description, " +  
                            "color = @color, assigned_user_id = @assigned_user_id WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            query.Parameters.Add(new SQLiteParameter("@name", task.Name));
            query.Parameters.Add(new SQLiteParameter("@state", task.State));
            query.Parameters.Add(new SQLiteParameter("@description", task.Description));
            query.Parameters.Add(new SQLiteParameter("@color", task.Color));
            query.Parameters.Add(new SQLiteParameter("@assigned_user_id", task.AssignedUserId));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<Tasks> GetAll() {
        string queryText = "SELECT * FROM task";
        List<Tasks> tasks = new List<Tasks>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var task = new Tasks() {
                        Id = Convert.ToInt32(reader["id"]),
                        BoardId = Convert.ToInt32(reader["board_id"]),
                        Name = reader["name"].ToString(),
                        State = (TasksState)Convert.ToInt32(reader["state"]),
                        Description = reader["description"].ToString(),
                        Color = (Color)Convert.ToInt32(reader["color"]),
                        AssignedUserId = reader["assigned_user_id"] == DBNull.Value ? null : Convert.ToInt32(reader["assigned_user_id"]) 
                    };
                    if(task.AssignedUserId != null) {
                        task.AssignedUserName = GetAssignedUserName((int)task.AssignedUserId);
                    }
                    task.BoardName = GetBoardName(task.BoardId);
                    tasks.Add(task);
                }
            }
            connection.Close();
        }
        return tasks;
    }

    public Tasks GetById(int id) {
        string queryText = "SELECT * FROM task WHERE id = @id";
        Tasks task = new Tasks();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(reader.Read()) {
                    task.Id = Convert.ToInt32(reader["id"]);
                    task.BoardId = Convert.ToInt32(reader["board_id"]);
                    task.Name = reader["name"].ToString();
                    task.State = (TasksState)Convert.ToInt32(reader["state"]);
                    task.Description = reader["description"].ToString();
                    task.Color = (Color)Convert.ToInt32(reader["color"]);
                    task.BoardName = GetBoardName(task.BoardId);
                    if(reader["assigned_user_id"] != DBNull.Value) {
                        task.AssignedUserId = Convert.ToInt32(reader["assigned_user_id"]);
                    } else {
                        task.AssignedUserId = null;
                    }
                } else {
                    throw new Exception("Tarea no encontrada");
                }
            }
            connection.Close();
        }
        return task;
    }

    public List<Tasks> GetByAssigned(int userId) {
        string queryText = "SELECT * FROM task WHERE assigned_user_id = @id";
        List<Tasks> tasks = new List<Tasks>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", userId));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var task = new Tasks() {
                        Id = Convert.ToInt32(reader["id"]),
                        BoardId = Convert.ToInt32(reader["board_id"]),
                        Name = reader["name"].ToString(),
                        State = (TasksState)Convert.ToInt32(reader["state"]),
                        Description = reader["description"].ToString(),
                        Color = (Color)Convert.ToInt32(reader["color"]),
                        AssignedUserId = reader["assigned_user_id"] == DBNull.Value ? null : Convert.ToInt32(reader["assigned_user_id"]) 
                    };
                    if(task.AssignedUserId != null) {
                        task.AssignedUserName = GetAssignedUserName((int)task.AssignedUserId);
                    }
                    task.BoardName = GetBoardName(task.BoardId);
                    tasks.Add(task);
                }
            }
            connection.Close();
        }
        return tasks;
    }

    public List<Tasks> GetByUser(int userId) {
        string queryText = "SELECT t.id, t.name, t.description, color, state, board_id, assigned_user_id FROM task t " +
                            "INNER JOIN board b ON board_id = b.id WHERE board_owner_id = @id";
        List<Tasks> tasks = new List<Tasks>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", userId));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var task = new Tasks() {
                        Id = Convert.ToInt32(reader["id"]),
                        BoardId = Convert.ToInt32(reader["board_id"]),
                        Name = reader["name"].ToString(),
                        State = (TasksState)Convert.ToInt32(reader["state"]),
                        Description = reader["description"].ToString(),
                        Color = (Color)Convert.ToInt32(reader["color"]),
                        AssignedUserId = reader["assigned_user_id"] == DBNull.Value ? null : Convert.ToInt32(reader["assigned_user_id"]) 
                    };
                    if(task.AssignedUserId != null) {
                        task.AssignedUserName = GetAssignedUserName((int)task.AssignedUserId);
                    }
                    task.BoardName = GetBoardName(task.BoardId);
                    tasks.Add(task);
                }
            }
            connection.Close();
        }
        return tasks;
    }

    public List<Tasks> GetByBoard(int boardId) {
        string queryText = "SELECT * FROM task WHERE board_id = @id";
        List<Tasks> tasks = new List<Tasks>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", boardId));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var task = new Tasks() {
                        Id = Convert.ToInt32(reader["id"]),
                        BoardId = Convert.ToInt32(reader["board_id"]),
                        Name = reader["name"].ToString(),
                        State = (TasksState)Convert.ToInt32(reader["state"]),
                        Description = reader["description"].ToString(),
                        Color = (Color)Convert.ToInt32(reader["color"]),
                        AssignedUserId = reader["assigned_user_id"] == DBNull.Value ? null : Convert.ToInt32(reader["assigned_user_id"])
                    };
                    task.BoardName = GetBoardName(task.BoardId);
                    if(task.AssignedUserId != null) {
                        task.AssignedUserName = GetAssignedUserName((int)task.AssignedUserId);
                    }
                    tasks.Add(task);
                }
            }
            connection.Close();
        }
        if(tasks.Count == 0)
            throw new Exception("La tarea no pertenece a ningun tablero"); 
        return tasks;
    }

    public bool TaskExists(Tasks task) {
        string queryText = "SELECT name FROM task WHERE name = @name";
        bool exists;
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
        SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@name", task.Name));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                exists = reader.Read();
            }
            connection.Close();
        }
        return exists;
    }

    public void Delete(int id) {
        string queryText = "DELETE FROM task WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void AssignTask(int userId, int taskId) {
        string queryText = "UPDATE task SET assigned_user_id = @assigned_user_id WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@assigned_user_id", userId));
            query.Parameters.Add(new SQLiteParameter("@id", taskId));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    private string GetBoardName(int id) {
        BoardRepository boardRepository = new BoardRepository(connectionPath);
        Board board = boardRepository.GetById(id);
        return board.Name;
    }

    private string GetAssignedUserName(int id) {
        UserRepository userRepository = new UserRepository(connectionPath);
        User user = userRepository.GetById(id);
        return user.Username;
    }
}