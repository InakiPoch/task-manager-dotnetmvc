using tl2_tp10_2023_InakiPoch.Models;

namespace tl2_tp10_2023_InakiPoch.ViewModels;

public class GetBoardsViewModel {
    public List<Board> Boards { get; set; }

    public GetBoardsViewModel(List<Board> boards) {
        Boards = boards;
    }
}