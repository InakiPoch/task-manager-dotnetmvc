using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class GetTasksViewModel {
    public List<Tasks> OwnedTasks { get; set; }
    public List<Tasks> AssignedTasks { get; set; }
    public List<Tasks> AllTasks { get; set; }

    public GetTasksViewModel() {}

    public GetTasksViewModel(List<Tasks> ownedTasks, List<Tasks> assignedTasks, List<Tasks> allTasks) {
        OwnedTasks = ownedTasks;
        AssignedTasks = assignedTasks;
        AllTasks = allTasks;
    }
}