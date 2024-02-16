namespace tl2_tp10_2023_InakiPoch.Models;

public enum TasksState { Ideas, ToDo, Doing, Review, Done }
public enum Color { Green, Yellow, Red, Skyblue, Grey, White }

public class Tasks {
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string Name { get; set; }
    public TasksState State { get; set; }
    public string Description { get; set; }
    public Color Color { get; set; }
    public int? AssignedUserId { get; set; }
    public string BoardName { get; set; }
    public string AssignedUserName { get; set; }
}