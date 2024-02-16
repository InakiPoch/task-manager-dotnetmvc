using System.Data.SQLite;
using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.Repositories;

public interface IBoardRepository {
    void Add(Board board);
    void Update(int id, Board board);
    Board GetById(int id);
    List<Board> GetAll();
    List<Board> GetByUser(int userId);
    List<Board> GetByTask(int userId);
    bool BoardExists(Board board);
    void Delete(int id);
}

public class BoardRepository : IBoardRepository {
    readonly string connectionPath;

    public BoardRepository(string connectionPath) {
        this.connectionPath = connectionPath;
    }

    public void Add(Board board) {
        string queryText = "INSERT INTO board (name, description, board_owner_id) " + 
                            "VALUES (@name, @description, @board_owner_id)";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@name", board.Name));
            query.Parameters.Add(new SQLiteParameter("@description", board.Description));
            query.Parameters.Add(new SQLiteParameter("@board_owner_id", board.OwnerId));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Update(int id, Board board) {
        string queryText = "UPDATE board SET name = @name, description = @description " + 
                            "WHERE id = @id";
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            query.Parameters.Add(new SQLiteParameter("@name", board.Name));
            query.Parameters.Add(new SQLiteParameter("@description", board.Description));
            connection.Open();
            query.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Board GetById(int id) {
        string queryText = "SELECT * FROM board WHERE id = @id";
        Board board = new Board();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                if(reader.Read()) {
                    board.Id = Convert.ToInt32(reader["id"]);
                    board.Name = reader["name"].ToString();
                    board.Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString();
                    board.OwnerId = Convert.ToInt32(reader["board_owner_id"]);
                    board.OwnerName = GetUserName(board.OwnerId);
                } else {
                    throw new Exception("Tablero no encontrado");
                }
            }
            connection.Close();
        }
        return board;
    }

    public List<Board> GetAll() {
        string queryText = "SELECT * FROM board";
        List<Board> boards = new List<Board>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var board = new Board() {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                        OwnerId = Convert.ToInt32(reader["board_owner_id"]),
                    };
                    board.OwnerName = GetUserName(board.OwnerId);
                    boards.Add(board);
                }
            }
            connection.Close();
        }
        return boards;
    }

    public List<Board> GetByUser(int userId) {
        string queryText = "SELECT * FROM board WHERE board_owner_id = @id";
        List<Board> boards = new List<Board>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", userId));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var board = new Board() {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                        OwnerId = Convert.ToInt32(reader["board_owner_id"]),
                    };
                    board.OwnerName = GetUserName(board.OwnerId);
                    boards.Add(board);
                }
            }
            connection.Close();
        }
        return boards;
    }

    public List<Board> GetByTask(int userId) {
        string queryText = "SELECT b.id, b.name, b.description, board_owner_id FROM task INNER JOIN board b " +
                            "ON board_id = b.id WHERE assigned_user_id = @id";
        List<Board> boards = new List<Board>();
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
            SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@id", userId));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                while(reader.Read()) {
                    var board = new Board() {
                        Id = Convert.ToInt32(reader["id"]),
                        Name = reader["name"].ToString(),
                        Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                        OwnerId = Convert.ToInt32(reader["board_owner_id"]),
                    };
                    board.OwnerName = GetUserName(board.OwnerId);
                    boards.Add(board);
                }
            }
            connection.Close();
        }
        return boards;
    }

    public bool BoardExists(Board board) {
        string queryText = "SELECT name FROM board WHERE name = @name";
        bool exists;
        using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
        SQLiteCommand query = new SQLiteCommand(queryText, connection);
            query.Parameters.Add(new SQLiteParameter("@name", board.Name));
            connection.Open();
            using(SQLiteDataReader reader = query.ExecuteReader()) {
                exists = reader.Read();
            }
            connection.Close();
        }
        return exists;
    }

    public void Delete(int id) {
        string queryText = "DELETE FROM board WHERE id = @id";
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

    private string GetUserName(int id) {
        UserRepository userRepository = new UserRepository(connectionPath);
        User user = userRepository.GetById(id);
        return user.Username;
    }
}