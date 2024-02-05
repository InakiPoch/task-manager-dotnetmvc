using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class GetBoardsViewModel {
    public List<Board> AssignedBoards { get; set; }
    public List<Board> AssignedTasksBoards { get; set; }
    public List<Board> AllBoards { get; set; }

    public GetBoardsViewModel() {}

    public GetBoardsViewModel(List<Board> boards, List<Board> tasksBoards, List<Board> allBoards) {
        AssignedBoards = boards;
        AssignedTasksBoards = tasksBoards;
        AllBoards = allBoards;
    }
}