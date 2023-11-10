using System.Data.SQLite;
using tl2_tp10_2023_InakiPoch.Models;

public interface IBoardRepository {
    void Add(Board board);
    void Update(int id, Board board);
    Board GetById(int id);
    List<Board> GetAll();
    List<Board> GetByUser(int userId);
    void Delete(int id);
}

namespace tl2_tp09_2023_InakiPoch.Repositories {
    public class BoardRepository : IBoardRepository {
        readonly string connectionPath = "Data Source=DataBase/board.db;Cache=Shared";

        public void Add(Board board) {
            string queryText = "INSERT INTO board (id, name, description, board_owner_id) " + 
                                "VALUES (@id, @name, @description, @board_owner_id)";
            using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
                SQLiteCommand query = new SQLiteCommand(queryText, connection);
                query.Parameters.Add(new SQLiteParameter("@id", board.Id));
                query.Parameters.Add(new SQLiteParameter("@name", board.Name));
                query.Parameters.Add(new SQLiteParameter("@description", board.Description));
                query.Parameters.Add(new SQLiteParameter("@board_owner_id", board.OwnerId));
                connection.Open();
                query.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(int id, Board board) {
            string queryText = "UPDATE board SET name = @name, description = @description, board_owner_id = @board_owner_id " + 
                                "WHERE id = @id";
            using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
                SQLiteCommand query = new SQLiteCommand(queryText, connection);
                query.Parameters.Add(new SQLiteParameter("@id", id));
                query.Parameters.Add(new SQLiteParameter("@name", board.Name));
                query.Parameters.Add(new SQLiteParameter("@description", board.Description));
                query.Parameters.Add(new SQLiteParameter("@board_owner_id", board.OwnerId));
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
                    while(reader.Read()) {
                        board.Id = Convert.ToInt32(reader["id"]);
                        board.Name = reader["name"].ToString();
                        board.Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString();
                        board.OwnerId = Convert.ToInt32(reader["board_owner_id"]);
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
                            OwnerId = Convert.ToInt32(reader["board_owner_id"])
                        };
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
                        boards.Add(board);
                    }
                }
                connection.Close();
            }
            return boards;
        }

        public void Delete(int id) {
            string queryText = "DELETE FROM board WHERE id = @id";
            using(SQLiteConnection connection = new SQLiteConnection(connectionPath)) {
                SQLiteCommand query = new SQLiteCommand(queryText, connection);
                query.Parameters.Add(new SQLiteParameter("@id", id));
                connection.Open();
                query.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}