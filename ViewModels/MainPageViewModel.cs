namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class MainPageViewModel {
    public int PersonalBoards { get; set; }
    public int PersonalTasks { get; set; }
    public int AssignedTasks { get; set; }
    public int TotalTasks { get; set; }
    public int TotalBoards { get; set; }
    public int TotalUsers { get; set; }

    public MainPageViewModel() {}

    public MainPageViewModel(int personalBoards, int personalTasks, int assignedTasks, int totalTasks, int totalBoards, int totalUsers) {
        PersonalBoards = personalBoards;
        PersonalTasks = personalTasks;
        AssignedTasks = assignedTasks;
        TotalTasks = totalTasks;
        TotalBoards = totalBoards;
        TotalUsers = totalUsers; 
    }
}