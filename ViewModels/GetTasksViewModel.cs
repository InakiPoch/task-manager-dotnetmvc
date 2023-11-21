using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class GetTasksViewModel {
    public List<Tasks> Tasks { get; set; }

    public GetTasksViewModel(List<Tasks> tasks) {
        Tasks = tasks;
    }
}