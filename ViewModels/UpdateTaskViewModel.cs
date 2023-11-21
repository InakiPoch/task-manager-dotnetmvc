using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class UpdateTaskViewModel {
    public int Id { get; set; }
    public string Name { get; set; }
    public TasksState State { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }

    public UpdateTaskViewModel(Tasks task) {
        Id = task.Id;
        Name = task.Name;
        State = task.State;
        Description = task.Description;
        Color = task.Color;
    }
}