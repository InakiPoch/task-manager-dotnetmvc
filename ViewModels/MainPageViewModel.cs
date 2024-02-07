namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class MainPageViewModel {
    public int TotalTasks { get; set; }
    public int TotalBoards { get; set; }
    public int TotalUsers { get; set; }

    public MainPageViewModel() {}

    public MainPageViewModel(int totalTasks, int totalBoards, int totalUsers) {
        TotalTasks = totalTasks;
        TotalBoards = totalBoards;
        TotalUsers = totalUsers; 
    }
}